USE MASTER; 
--DDL --El Orden de ejecucion de scripts es: 
/*
  1. Biblioteca-DDL Y triGGERS 
  2. Vistas
  3. Procedimientos Almacenados
  4. Inserciones  
*/
CREATE DATABASE BibliotecaWeb;
go
USE BibliotecaWeb;
go
--Creacion de tablas y relaciones

-- CREATE TABLE Categoria(*
--     IDCategoria varchar(10)  not null CONSTRAINT PK_Categoria PRIMARY KEY,--Tiene un trigger para autogenerar codigo
--     Categoria nvarchar(60) not null,--Tiene indice unico
--     );--Esta referenciado con libro (No se permite eliminar)
-- go
CREATE TABLE Categoria(--Tiene un trigger para autogenerar codigo
	IdCategoria  nvarchar(10)  not null CONSTRAINT PK_Categoria PRIMARY KEY,--Esta referenciado con libro (No se permite eliminar)
	Descripcion nvarchar(100) not null,--Tiene un indice único
	Activo bit default 1,
	FechaRegistro datetime default getdate()
)
go
CREATE TABLE Sala(--Tiene un trigger para autogenerar codigo
	IdSala  nvarchar(10)  not null CONSTRAINT PK_Sala PRIMARY KEY,--Esta referenciado con libro (No se permite eliminar)
	Descripcion nvarchar(100) not null,--Tiene un indice único
	Activo bit default 1,--Esta referenciado con libro (No se permite eliminar)
	FechaRegistro datetime default getdate()
)
go
CREATE TABLE Editorial(--Tiene un trigger para autogenerar codigo
	IdEditorial  nvarchar(10)  not null CONSTRAINT PK_Editorial PRIMARY KEY,--Esta referenciado con libro (No se permite eliminar)
	Descripcion nvarchar(100) not null,--Tiene un indice único
	Activo bit default 1,--Esta referenciado con libro (No se permite eliminar) (ESTOS  3 a menos que no esten referenciados si se pueden eliminar)
	FechaRegistro datetime default getdate()
)
go
-- CREATE TABLE Sala(
--     IDSala varchar(10)  not null  CONSTRAINT PK_Sala PRIMARY KEY,--Tiene un trigger para autogenerar codigo
--     Sala varchar(40) not null,--Tiene indica unico
--     );--Esta referenciado con libro (No se permite eliminar)
-- go
-- CREATE TABLE Editorial(
--     IDEditorial varchar(10)  not null CONSTRAINT PK_Editorial PRIMARY KEY,--Tiene un trigger para autogenerar codigo
--     Editorial nvarchar(60) not null, --Tiene indice unico
--     );--Esta referenciado con libro (No se permite eliminar) (ESTOS  3 a menos que no esten referenciados si se pueden eliminar)
-- go
CREATE TABLE Autor(--Tiene un trigger para autogenerar codigo
	IdAutor  nvarchar(10)  not null CONSTRAINT PK_Autor PRIMARY KEY,--Esta referenciado con libro (No se permite eliminar)
	Nombres nvarchar(100) not null,--Tiene indice compuesto con Apellidos
  Apellidos varchar(100) not null,--Tiene indice compuesto con Nombre
	Activo bit default 1,
	FechaRegistro datetime default getdate()
)----Esta referenciado con Libro autor (No se puede eliminar, o afectará a todos)
 --LA UNICA FORMA DE ELIMINAR A UN AUTOR, UNA SALA, UNA CATEGORIA, UN EDITORIAL ES QUE NO ESTEN REFERENCIADAS A NINGUNA TABLA
 --Por ejemplo si solo creamos una autor como prueba, pues mientras no lo referenciemos, si se puede eliminar
go
CREATE TABLE LibroAutor(
    IDLibroAutor varchar(10)  not null CONSTRAINT PK_LibroAutor PRIMARY KEY,--Tiene un trigger para autogenerar codigo
    --Llaves foraneas
    ID_Libro varchar(25) not null CONSTRAINT FK_Libro FOREIGN KEY(ID_Libro) 
    REFERENCES Libro(IDLibro) ON DELETE CASCADE ON UPDATE CASCADE,
    ID_Autor nvarchar(10) not null CONSTRAINT FK_Autor FOREIGN KEY(ID_Autor) 
    REFERENCES Autor(IDAutor),
    Activo bit default 1,
	  FechaRegistro datetime default getdate() 
);--LibroAutor no esta referenciado a otra tabla (al igual que prestamo se puede eliminar)
    go
    /*
      La tabla LibroAutor 
      Tiene como foreig key a ID_Libro: 
      En este caso como a libro si lo podemos eliminar y actualizar, pues esta
      foreign key va a ser de tipo delete cascade y update cascade por si se
      hace algun cambio en libro o se elimina (ya que no tendria sentido tener un LibroAutor,
      estando referenciando a un libro que no existe)
    */
go

CREATE TABLE Usuario(
	IdUsuario int not null CONSTRAINT PK_Usuario PRIMARY KEY identity,
  Nombres nvarchar(100) not null,
  Apellidos varchar(100) not null,
  Ciudad nvarchar(60) not null, --Default como --"Zacatlán"
  Calle nvarchar(100) not null,
  Telefono varchar(20) not null, 
	Correo nvarchar(100) not null,
	Clave nvarchar(150) not null, --Contrase�as encriptadas
  Tipo int not null CONSTRAINT FK_TipoPersona FOREIGN KEY(Tipo)
  REFERENCES TipoPersona(IdTipoPersona) DEFAULT 1,--El DEFAULT id 1, es para tipo lectores:
	Reestablecer bit default 1, -- Por default 1
	Activo bit default 1,
	FechaRegistro datetime default getdate()
)
select * from lector  
select IDLector,Nombres,Apellidos,Edad, Genero, Escuela, GradoGrupo, Ciudad, Calle, Telefono, Correo,Clave,Reestablecer from Lector

insert into Lector(Nombres, Apellidos, Edad, Genero,Escuela,GradoGrupo, Ciudad, Calle, Telefono, Correo, Clave, Activo)
values ('Angel David','Nava Garcia Lector', 21, 1,'Instituto Tecnologico Superior de la Sierra Norte de Puebla','Sexto Semestre','Zacatlan', 'Josefa Ortiz', '768273817','david@gmail.com','test123',0)

insert into Lector(Nombres, Apellidos, Edad, Genero,Escuela,GradoGrupo, Ciudad, Calle, Telefono, Correo, Clave)
values ('Guadalupe','Garcia Lector', 21, 0,'Secundaria Benito Juarez','3ro de Secundaria','Puebla', 'Josefa Ortiz', '2229879873','guadalupe@gmail.com','test123')

--RECORDAR QUE DEBEMOS CREAR DE NUEVO ESTA TABLA CON EL REESTABLECER EN DEFAULT 0
CREATE  TABLE Lector(
	IdLector int not null CONSTRAINT PK_Lector PRIMARY KEY identity,
  Nombres nvarchar(100) not null,
  Apellidos varchar(100) not null,
  Edad tinyint not null,--Tiene un check (0-125),
  Genero bit not null, --H = 0 o M = 1
  Escuela nvarchar(100) null, --Si usamos el SP: Su default es: NINGUNA
  GradoGrupo nvarchar(100) null, --3ro de secundaria
  Ciudad nvarchar(60) not null, --Default como --"Zacatlán"
  Calle nvarchar(100) not null,
  Telefono varchar(20) not null, 
	Correo nvarchar(100) not null,--No puede ser null porque es con el que se van a registrar
	Clave nvarchar(150) not null, --Contrase�as encriptadas
	Reestablecer bit default 0, -- Por default 0
	Activo bit default 1,
	FechaRegistro datetime default getdate()
)--Esta referenciado con Prestamo (Si se puede eliminar, para ello su foreign key en prestamo tiene un delete en cascade)

-- CREATE TABLE Usuario(*
--     IDUsuario int identity (1,1)  not null CONSTRAINT PK_Usuario PRIMARY KEY,
--     Nombre nvarchar(40) not null,
--     A_Paterno varchar(30) not null,
--     A_Materno varchar(30) not null,
--     Edad tinyint not null,--Tiene un check (0-125)
--     EscuelaProcedencia nvarchar(100) null, --Si usamos el SP: Su default es: NINGUNA
--     --Grado varchar (10),
--     Ciudad nvarchar(60) not null, --Default como --"Zacatlán"
--     Calle nvarchar(100) not null,
--     Telefono varchar(20) not null, 
--     Email nvarchar(100) null,
--     Observaciones varchar(500) null, --Estará definida como default, como "NINGUNA"
--     ID_TipoPersona int not null CONSTRAINT FK_TipoPersona FOREIGN KEY(ID_TipoPersona) 
--     REFERENCES TipoPersona(IdTipoPersona) DEFAULT 1,--El DEFAULT id 1, es para tipo lectores:
--     Contrasenia varchar(30) DEFAULT '321',
--     FechaCreacion date not null DEFAULT GETDATE()
--     );--Esta referenciado con Prestamo (Si se puede eliminar, para ello su foreign key en prestamo tiene un delete en cascade)
-- go
CREATE TABLE TipoPersona(
    IdTipoPersona  int identity (1,1)  CONSTRAINT PK_TipoPersona PRIMARY KEY,
    Descripcion varchar(50)--Solo hay tres tipos: Lector, Empleado y Administrador
)--Esta referenciado con usuario (no haemos un delete cascade porque no tendria sentido eliminar uno de estos tres tipos de persona)
go

CREATE TABLE Libro(
    IDLibro int not null identity,
    Codigo varchar(25)  not null CONSTRAINT PK_CodigoLibro PRIMARY KEY,
    Titulo nvarchar(130) not null,
    -- Ubicacion varchar(10) not null,--Ejemplo EN
    Paginas int not null,
    --Llaves foraneas
    ID_Categoria nvarchar(10) not null CONSTRAINT FK_Categoria FOREIGN KEY(ID_Categoria) 
    REFERENCES Categoria(IDCategoria),
    ID_Editorial nvarchar(10) not null CONSTRAINT FK_Editorial FOREIGN KEY(ID_Editorial) 
    REFERENCES Editorial(IDEditorial),
    ID_Sala nvarchar(10) not null CONSTRAINT FK_Sala FOREIGN KEY(ID_Sala) 
    REFERENCES Sala(IDSala) DEFAULT 'S0001',--Sala 
    Ejemplares int,--En producto seria el stock
    -- NumEdicion varchar(60) not null,
    AñoEdicion varchar(5) not null,
    Volumen tinyint not null DEFAULT 1,
	  RutaImagen varchar(100),
  	NombreImagen varchar(100),
    Observaciones varchar(500) not null default 'NINGUNA',--Tiene un DEFAULT en Observaciones = EN PERFECTO ESTADO
	  Activo bit default 1,
	  FechaRegistro datetime default getdate()
    );--Esta referenciado con ejemplar y libroLutor (si se puede eliminar ya que estos como foreign key son delete y update cascade)
go

CREATE TABLE Prestamo(/*Arrelgar este prestamo, ya que es de venta (Del curso anterior)*/
	IdPrestamo int identity (1,1)  not null CONSTRAINT PK_Prestamo primary key, --Es  IDENTITY
	ID_Lector int not null CONSTRAINT FK_Lector FOREIGN KEY(ID_Lector) 
  REFERENCES Lector(IDLector) ON DELETE CASCADE,
  -- ID_Ejemplar varchar(10) not null CONSTRAINT FK_Ejemplar FOREIGN KEY(ID_Ejemplar) 
  -- REFERENCES Ejemplar(IDEjemplar) ON DELETE CASCADE,
  TotalLibro int,--Total de libros, o total de ejemplares de ese libro --El lector pudo haber solicitado el prestamo de 3 libros
	-- MontoTotal decimal(10,2),--La suma total del precio de todos los productos
  Estado bit default 0,--Devuelto o no devuelto --1 es igual a si, y 0 es igual a no . Asigando por default = 0, 
	FechaPrestamo datetime default getdate(),
  FechaDevolucion datetime null,--No especificaremos nada para que por default sea null (tiene un default null)
  DiasDePrestamo int not null default 7,--Regularmente una semana 7 dias
  Observaciones varchar(500) not null
)--No tiene referencia (Se puede eliminar)
go
select * from prestamo
CREATE TABLE DetallePrestamo(--Areglar este detallePrestamo (Ya que este es detalle venta del curso anterior)
	IdDetallePrestamo int primary key identity,
	IdPrestamo int references Prestamo(IdPrestamo),
	IDLibro varchar(25) references Libro(Codigo),--Posiblemente sea mejor el ejemplar id de ejemplar
	CantidadEjemplares int,--Cuantos ejemplares de un libro se prestaron (Regularmente solo 1)
	Total decimal(10,2)--total del precio del producto
)
go
-- /*ALTER TABLE libro
-- ADD 
--     RutaPortada nvarchar(200)  null,
--     NombrePortada nvarchar(200)  null,
--     Imagen binary  null;*/
-- go
-- CREATE TABLE Autor(
--     IDAutor varchar(10)  not null CONSTRAINT PK_Autor PRIMARY KEY,--Tiene un trigger para autogenerar codigo
--     Nombre nvarchar(40) not null,--Tiene indice compuesto con Apellidos
--     Apellidos nvarchar(40) not null --Tiene indice compuesto con Nombre
--     );----Esta referenciado con Libro autor (No se puede eliminar, o afectará a todos)
--  --LA UNICA FORMA DE ELIMINAR A UN AUTOR, UNA SALA, UNA CATEGORIA, UN EDITORIAL ES QUE NO ESTEN REFERENCIADAS A NINGUNA TABLA
--  --Por ejemplo si solo creamos una autor como prueba, pues mientras no lo referenciemos, si se puede eliminar
-- go
-- CREATE TABLE LibroAutor(
--     IDLibroAutor varchar(10)  not null CONSTRAINT PK_LibroAutor PRIMARY KEY,--Tiene un trigger para autogenerar codigo
--     --Llaves foraneas
--     ID_Libro varchar(25) not null CONSTRAINT FK_Libro FOREIGN KEY(ID_Libro) 
--     REFERENCES Libro(IDLibro) ON DELETE CASCADE ON UPDATE CASCADE,
--     ID_Autor varchar(10) not null CONSTRAINT FK_Autor FOREIGN KEY(ID_Autor) 
--     REFERENCES Autor(IDAutor) 
--     );--LibroAutor no esta referenciado a otra tabla (al igual que prestamo se puede eliminar)
--     /*
--       La tabla LibroAutor 
--       Tiene como foreig key a ID_Libro: 
--       En este caso como a libro si lo podemos eliminar y actualizar, pues esta
--       foreign key va a ser de tipo delete cascade y update cascade por si se
--       hace algun cambio en libro o se elimina (ya que no tendria sentido tener un LibroAutor,
--       estando referenciando a un libro que no existe)
--     */
-- go

CREATE TABLE Ejemplar(
    IDEjemplar varchar(10)  not null CONSTRAINT PK_Ejemplar PRIMARY KEY,--Tiene un trigger para autogenerar codigo
    -- NumEjemplar int not null,
    --Llave foranea
    ID_Libro varchar(25) not null CONSTRAINT FK_Ejemplar_Libro FOREIGN KEY(ID_Libro) 
    REFERENCES Libro(Codigo) ON DELETE CASCADE ON UPDATE CASCADE --Tiene un indice unico: ID_Libro
    );--Esta referenciado con Prestamo (Si se puede eliminar, para ello su foreign key en prestamo tiene un delete en cascade)
    -- /*
--       Ocurre lo mismo con Ejemplar
--       La tabla Ejemplar
--       Tiene como foreig key a ID_Libro: 
--       En este caso como a libro si lo podemos eliminar y actualizar, pues esta
--       foreign key va a ser de tipo delete cascade y update cascade por si se
--       hace algun cambio en libro o se elimina (Ya que no tiene sentido tener un ejemplar
--       de un libro que no existe)
--     */
-- go
-- CREATE TABLE Prestamo(
    
--     IDPrestamo int identity (1,1)  not null CONSTRAINT PK_Prestamo PRIMARY KEY, --Es  IDENTITY
--     --Llaves foraneas
--     ID_Usuario int not null CONSTRAINT FK_Usuario FOREIGN KEY(ID_Usuario) 
--     REFERENCES Usuario(IDUsuario) ON DELETE CASCADE,
--     ID_Ejemplar varchar(10) not null CONSTRAINT FK_Ejemplar FOREIGN KEY(ID_Ejemplar) 
--     REFERENCES Ejemplar(IDEjemplar) ON DELETE CASCADE,
--     -------------------------------------------------
--     --Cantidad tinyint not null,
--     FechaPrestamo date not null,--Tiene un DEFAULT: GetDate();
--     FechaMaxDev date not null,
--     Devuelto bit not null,--1 es igual a si, y 0 es igual a no . Asigando por default = 0, 
--     FechaDevolucion date null,--No especificaremos nada para que por default sea null (tiene un default null)
--     Observaciones varchar(500) not null
--     );--No tiene referencia (Se puede eliminar)
--     /*
--       La tabla Prestamo
--       Tiene como foreig key a ID_Usuario: 
--       En este caso como al ID usuario si lo podemos eliminar  pues esta
--       foreign key va a ser de tipo delete cascade solamente por si
--       se elimina un usuario (ya que no tendria sentido estar haciendo un prestamo
--       a un usuario que no existe)
--     */
-- go
-- CREATE TABLE HLibrosActualizados(
--     IDLibrosActualizados int identity(1,1) CONSTRAINT PK_LibrosActualizados PRIMARY KEY,
--     TipoAccion varchar(20) not null,
--     Usuario VARCHAR(30) not null,
--     FechaModif date not null,
--     IDLibro varchar(25)  not null ,
--     IDLibroAnterior varchar(25)  not null ,
--     Titulo nvarchar(130) not null,
--     TituloAnterior nvarchar(130) not null,
--     Ubicacion varchar(10) not null,
--     UbicacionAnterior varchar(10) not null,
--     NumEdicion varchar(60) not null,
--     NumEdicionAnterior varchar(60) not null,
--     AñoEdicion varchar(5) not null,
--     AñoEdicionAnterior varchar(5) not null,
--     Volumen tinyint not null,
--     VolumenAnterior tinyint not null,
--     NumPaginas int not null,
--     NumPaginasAnterior int not null,
--     Observaciones varchar(500) not null,
--     ObservacionesAnterior varchar(500) not null,
--     --Llaves foraneas
--     ID_Sala varchar(10) not null,
--     ID_SalaAnterior varchar(10) not null,
--     ID_Categoria varchar(10) not null,
--     ID_CategoriaAnterior varchar(10) not null,
--     ID_Editorial varchar(10) not null,
--     ID_EditorialAnterior varchar(10) not null
--     );
--     go
-- ----------------------------------------------------------Indices, constraint, checks, default------------------------------------------------------------------------------------
-- --Creacion de índices
-- CREATE UNIQUE INDEX Idx_Editorial ON Editorial(Editorial);--Para no repetir editoriales
-- go
-- CREATE UNIQUE INDEX Idx_Categoria ON Categoria(Categoria);
-- go
-- CREATE UNIQUE INDEX Idx_Sala ON Sala(Sala);
-- go
-- CREATE UNIQUE INDEX Idx_TipoPersona ON TipoPersona(Descripcion);
-- go
-- CREATE UNIQUE INDEX Idx_Usuario_NombreCompleto ON Usuario(Nombre, A_Paterno, A_Materno);
-- go
-- CREATE UNIQUE INDEX Idx_Autor_NombreCompleto ON Autor(Nombre, Apellidos);
-- go
-- CREATE UNIQUE INDEX Idx_Ejemplar_ID_Libro ON Ejemplar(ID_Libro); 
-- go
-- CREATE UNIQUE INDEX Idx_LibroAUtor ON LibroAutor(ID_Libro,ID_Autor); 
-- go

-- --Creacion de CONSTRAINT
-- ALTER TABLE Usuario
-- ADD CONSTRAINT CH_Usuario_Edad CHECK(Edad>0 and Edad <=125);
-- go
-- ALTER TABLE Ejemplar
-- ADD CONSTRAINT CH_Ejemplar_NumEjemplar CHECK(NumEjemplar>=0 and NumEjemplar <=500);
-- go
-- ALTER TABLE Libro
-- ADD CONSTRAINT DF_Libro_Observaciones DEFAULT 'EN PERFECTO ESTADO' FOR Observaciones
-- go
-- ALTER TABLE Usuario
-- ADD CONSTRAINT DF_Usuario_Observaciones DEFAULT 'NINGUNA' FOR Observaciones
-- go
-- ALTER TABLE Usuario
-- ADD CONSTRAINT DF_Usuario_Ciudad DEFAULT 'ZACATLÁN' FOR Ciudad
-- go
-- ALTER TABLE Prestamo
-- ADD CONSTRAINT DF_Prestamo_FechaPrestamo DEFAULT GETDATE() FOR FechaPrestamo;
-- go
-- ALTER TABLE Prestamo 
-- ADD CONSTRAINT DF_Prestamo_Devuelto DEFAULT 0 FOR Devuelto;
-- go
-- ALTER TABLE Prestamo 
-- ADD CONSTRAINT DF_Prestamo_FechaDevolucion DEFAULT  NULL FOR FechaDevolucion;
-- go
-- ---------------------------------------------------------
-- ---------------------------------------------------------------Triggers -----------------------------------------------------------------------------------------
-- --Triger 1
-- CREATE TRIGGER TR_Ejemplar_AI --Que al insertar obviamente se reste uno al numero de ejemplares de dicho id
-- ON Prestamo FOR INSERT
-- AS
-- SET NOCOUNT ON
-- UPDATE Ejemplar set Ejemplar.NumEjemplar = Ejemplar.NumEjemplar - 1 from inserted
-- INNER JOIN Ejemplar ON Ejemplar.IDEjemplar = inserted.ID_Ejemplar;
-- go
-- --Trigger 2
-- CREATE TRIGGER TR_Ejemplar_BD --Que al insertar obviamente se reste uno al numero de ejemplares de dicho id
-- ON Prestamo FOR DELETE
-- AS
-- SET NOCOUNT ON
-- UPDATE Ejemplar set Ejemplar.NumEjemplar = Ejemplar.NumEjemplar + 1 from deleted
-- INNER JOIN Ejemplar ON Ejemplar.IDEjemplar = deleted.ID_Ejemplar;
-- go
-- --Triger 2 --ESTE TRIGGER NO SIRVIO AL CAMBIAR LA FECHADEV COMO NULL, BUENO NO TIENE ESO IMPLEMENTADO
-- ---Actualizar numero de ejemplares cuando el libro se devuelva, es decir, sumar 1 (Agregar al script original)
-- /*CREATE TRIGGER TR_Actualizar_Ejemplar_AUPDATE
-- ON  Prestamo FOR UPDATE
-- AS
-- SET NOCOUNT ON
-- BEGIN
-- DECLARE @LibroDevuelto bit 
-- SELECT @LibroDevuelto = Devuelto from inserted;
-- IF @LibroDevuelto = 1 
--     UPDATE Ejemplar set Ejemplar.NumEjemplar = Ejemplar.NumEjemplar + 1 from inserted
--     INNER JOIN Ejemplar ON Ejemplar.IDEjemplar = inserted.ID_Ejemplar
-- ELSE 
--     UPDATE Ejemplar set Ejemplar.NumEjemplar = Ejemplar.NumEjemplar - 1 from inserted
--     INNER JOIN Ejemplar ON Ejemplar.IDEjemplar = inserted.ID_Ejemplar
-- END;*/
-- --------------------------------------------------------------------------------
-- --TRIGGER 3
-- --Respalda la información de los libros al actualizar algun dato de un libro colocandola en otra tabla
-- --llamada librosactualizados
-- --Aun falta especificar que lo haga en cada registro
-- go
-- CREATE TRIGGER TR_ACTUALIZA_LIBRO_AUPDATE --Respaldo al actualizar un libro
-- ON Libro AFTER UPDATE 
-- AS
-- SET NOCOUNT ON
-- DECLARE @IDLibroT varchar(25) 
-- DECLARE @TituloT nvarchar(130) 
-- DECLARE @UbicacionT varchar(10) 
-- DECLARE @NumEdicionT varchar(60) 
-- DECLARE @AñoEdicionT varchar(5) 
-- DECLARE @VolumenT tinyint
-- DECLARE @NumPaginasT int 
-- DECLARE @ObservacionesT varchar(500)
-- DECLARE @ID_SalaT varchar(10) 
-- DECLARE @ID_CategoriaT varchar(10) 
-- DECLARE @ID_EditorialT varchar(10) 
-- ---
-- DECLARE @IDLibroAnterior varchar(25) 
-- DECLARE @TituloTAnterior nvarchar(130) 
-- DECLARE @UbicacionTAnterior varchar(10) 
-- DECLARE @NumEdicionTAnterior varchar(60) 
-- DECLARE @AñoEdicionTAnterior varchar(5) 
-- DECLARE @VolumenTAnterior tinyint
-- DECLARE @NumPaginasTAnterior int  
-- DECLARE @ObservacionesTAnterior varchar(500) 
-- DECLARE @ID_SalaTAnterior varchar(10) 
-- DECLARE @ID_CategoriaTAnterior varchar(10)
-- DECLARE @ID_EditorialTAnterior varchar(10) 

-- SELECT @IDLibroT = IDLibro from inserted
-- SELECT @IDLibroAnterior = IDLibro from deleted
-- SELECT @TituloT = Titulo from inserted
-- SELECT @TituloTAnterior =  Titulo from deleted
-- SELECT @UbicacionT = Ubicacion from inserted
-- SELECT @UbicacionTAnterior = Ubicacion from deleted
-- SELECT @NumEdicionT = NumEdicion from inserted
-- SELECT @NumEdicionTAnterior = NumEdicion from deleted
-- SELECT @AñoEdicionT= AñoEdicion from inserted
-- SELECT @AñoEdicionTAnterior =  AñoEdicion from deleted
-- SELECT @VolumenT = Volumen from inserted
-- SELECT @VolumenTAnterior = Volumen from deleted
-- SELECT @NumPaginasT = NumPaginas from inserted
-- SELECT @NumPaginasTAnterior = NumPaginas from deleted
-- SELECT @ObservacionesT = Observaciones from inserted
-- SELECT @ObservacionesTAnterior =  Observaciones from deleted
-- SELECT @ID_SalaT = ID_Sala from inserted
-- SELECT @ID_SalaTAnterior = ID_Sala from deleted
-- SELECT @ID_CategoriaT = ID_Categoria from inserted
-- SELECT @ID_CategoriaTAnterior = ID_Categoria from deleted
-- SELECT @ID_EditorialT = ID_Editorial from inserted
-- SELECT @ID_EditorialTAnterior = ID_Editorial from deleted

-- INSERT INTO HLibrosActualizados VALUES('Actualización',SYSTEM_USER,GETDATE(),@IDLibroT,@IDLibroAnterior,@TituloT,@TituloTAnterior,@UbicacionT,@UbicacionTAnterior,
--                                       @NumEdicionT,@NumEdicionTAnterior,@AñoEdicionT,@AñoEdicionTAnterior,@VolumenT,@VolumenTAnterior,
--                                       @NumPaginasT,@NumPaginasTAnterior,@ObservacionesT,@ObservacionesTAnterior,
--                                       @ID_SalaT,@ID_SalaTAnterior,@ID_CategoriaT,@ID_CategoriaTAnterior,@ID_EditorialT,@ID_EditorialTAnterior)
-- ----------------------------------------------------------------------------------
-- GO
-- --------------------------------Trigger nuevos para generar codigo automaticamente -----------------------------------
-- ---CODIGO SALA
-- CREATE TRIGGER TR_CodigoSala 
-- ON Sala instead of INSERT -- instead of: En vez de insertar como normalmente se haria, haz esto:
-- AS 
-- SET NOCOUNT ON
-- BEGIN
--   DECLARE @NombreSala VARCHAR(30) 
--   SELECT  @NombreSala = Sala from inserted
--   DECLARE @CodSala VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDSala),4 ) + 1 FROM Sala;--Estamos seleccionando los numeros //Esto da un numero exacto
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodSala = CONCAT('S',RIGHT(CONCAT('0000',@Cod),4));
--         INSERT INTO Sala VALUES (@CodSala,@NombreSala)
-- END
-- GO
-- ---CODIGO CATEGORIA
-- CREATE TRIGGER TR_CodigoCategoria 
-- ON Categoria instead of INSERT 
-- AS 
-- SET NOCOUNT ON
-- BEGIN
--   DECLARE @NombreCategoria VARCHAR(50) 
--   SELECT  @NombreCategoria = Categoria from inserted
--   DECLARE @CodCategoria VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDCategoria),4 ) + 1 FROM Categoria;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodCategoria = CONCAT('C',RIGHT(CONCAT('0000',@Cod),4));
--         INSERT INTO Categoria VALUES (@CodCategoria,@NombreCategoria)
-- END
-- ---CODIGO Editorial
-- GO
-- CREATE TRIGGER TR_CodigoEditorial
-- ON Editorial instead of INSERT 
-- AS 
-- SET NOCOUNT ON
-- BEGIN
--   DECLARE @NombreEditorial VARCHAR(60) 
--   SELECT  @NombreEditorial = Editorial from inserted
--   DECLARE @CodEditorial VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDEditorial),4 ) + 1 FROM Editorial;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodEditorial = CONCAT('ED',RIGHT(CONCAT('0000',@Cod),4));--Al tener dos letras, cambia el numero a recorrer a 3
--         INSERT INTO Editorial VALUES (@CodEditorial,@NombreEditorial)
-- END
-- GO
-- ---CODIGO Autor
-- CREATE TRIGGER TR_CodigoAutor
-- ON Autor instead of INSERT 
-- AS 
-- SET NOCOUNT ON
-- BEGIN
--   DECLARE @NombreAutor VARCHAR(40) 
--   SELECT  @NombreAutor =   Nombre from inserted
--   DECLARE @ApellidosAutor VARCHAR(40) 
--   SELECT  @ApellidosAutor =   Apellidos from inserted
--   DECLARE @CodAutor VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDAutor),4 ) + 1 FROM Autor;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodAutor = CONCAT('A',RIGHT(CONCAT('0000',@Cod),4));
--         INSERT INTO Autor VALUES (@CodAutor,@NombreAutor,@ApellidosAutor)
-- END
-- ---CODIGO lIBROAutor
-- GO
-- CREATE TRIGGER TR_CodigoLibroAutor
-- ON LibroAutor instead of INSERT 
-- AS 
-- SET NOCOUNT ON
-- BEGIN
--   DECLARE @ID_Libro_LA VARCHAR(40) 
--   SELECT  @ID_Libro_LA = ID_Libro from inserted
--   DECLARE @ID_Autor VARCHAR(40) 
--   SELECT  @ID_Autor = ID_Autor from inserted
--   BEGIN
--   DECLARE @CodLibroAutor VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDLibroAutor),4 ) + 1 FROM LibroAutor;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodLibroAutor = CONCAT('LA',RIGHT(CONCAT('0000',@Cod),4));--Al tener dos letras, cambia el numero a recorrer a 3
--         INSERT INTO LibroAutor VALUES (@CodLibroAutor, @ID_Libro_LA, @ID_Autor)
--   END
-- END
-- go
-- ---CODIGO Ejemplar
-- GO
-- CREATE TRIGGER TR_CodigoEjemplar
-- ON Ejemplar instead of INSERT 
-- AS 
-- SET NOCOUNT ON
-- BEGIN
--   DECLARE @NumEjemplar VARCHAR(40) 
--   SELECT  @NumEjemplar = NumEjemplar from inserted
--   DECLARE @ID_Libro_EJ VARCHAR(40) 
--   SELECT  @ID_Libro_EJ = ID_Libro from inserted
--   BEGIN
--   DECLARE @CodEjemplar VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDEjemplar),4 ) + 1 FROM Ejemplar;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodEjemplar = CONCAT('EJ',RIGHT(CONCAT('0000',@Cod),4));--Al tener dos letras, cambia el numero a recorrer a 3
--         INSERT INTO Ejemplar VALUES (@CodEjemplar,@NumEjemplar,@ID_Libro_EJ)
--   END
-- END


-- /*
-- UPDATE Prestamo SET DEVUELTO = 1 WHERE IDPrestamo = 2;
-- UPDATE Prestamo SET ID_Usuario=2 WHERE IDPrestamo=2;
-- UPDATE Prestamo SET FechaDevolucion='26-10-22' WHERE IDPrestamo=2;
-- go
-- UPDATE Prestamo SET Devuelto = 1 WHERE IDPrestamo=1;*/
-- ---
-- GO
-- ----------------------Intento trigger para actualizar correctamente un prestamo y afecte a ejemplar
-- --Deberiamos separar la parte de validacion null y no null en fecha devolucion y aparte lo de actualizar (quien sabe, jjaa)
-- CREATE TRIGGER TR_Prestamo_DevueltoNull --ESte trigger sustituye a los dos iniciales y al tercero sobre validar 
-- ON PRESTAMO FOR UPDATE --cuando la actualizacion devuelto es = 0, entonces la fechaDevolucion pasa a NULL
-- AS 
-- SET NOCOUNT ON
-- DECLARE @IDPrestamoDSP varchar(10), @DevueltoSiNoSP bit, @FechaDevolucionDSP date
-- SELECT @IDPrestamoDSP = IDPrestamo from inserted;
-- SELECT @DevueltoSiNoSP = Devuelto from inserted;
-- SELECT @FechaDevolucionDSP = FechaDevolucion from inserted;
-- BEGIN
--   IF EXISTS (SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoDSP )
--   BEGIN
--     IF EXISTS (SELECT *  FROM Prestamo WHERE Devuelto != @DevueltoSiNoSP AND @DevueltoSiNoSP = 1)
--     BEGIN
-- 	  UPDATE Prestamo SET 
--          Devuelto = @DevueltoSiNoSP,
--          FechaDevolucion = @FechaDevolucionDSP 
--       WHERE IDPrestamo = @IDPrestamoDSP;
--       UPDATE Ejemplar set Ejemplar.NumEjemplar = Ejemplar.NumEjemplar + 1 from inserted
--       INNER JOIN Ejemplar ON Ejemplar.IDEjemplar = inserted.ID_Ejemplar
--     END
--     ELSE--(SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoDSP)--(SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoDSP AND Devuelto = 1)
--       IF EXISTS (SELECT *  FROM Prestamo WHERE Devuelto != @DevueltoSiNoSP OR @DevueltoSiNoSP = 0)  
--     BEGIN
-- 	  UPDATE Prestamo SET 
--         Devuelto = @DevueltoSiNoSP,
--         FechaDevolucion = NULL 
--       WHERE IDPrestamo = @IDPrestamoDSP;
--       UPDATE Ejemplar set Ejemplar.NumEjemplar = Ejemplar.NumEjemplar - 1 from inserted
--       INNER JOIN Ejemplar ON Ejemplar.IDEjemplar = inserted.ID_Ejemplar
--      END
--   END
-- END;
-- GO

-- go
-- select * from sala;
-- SELECT  *from Categoria;
-- select  *from Autor;
-- SELECT * FROM LibroAutor;
-- select * from Libro;
-- SELECT * FROM Ejemplar;
-- SELECT * FROM Prestamo;
-- SELECT * FROM Categoria;
-- SELECT * FROM HLibrosActualizados; 