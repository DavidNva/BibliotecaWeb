Use Biblioteca;
--El Orden de ejecucion de scripts es: 
/*
  1. Biblioteca-DDL Y triGGERS 
  2. Vistas
  3. Procedimientos Almacenados
  4. Inserciones  
*/
-- --Para cada procedimiento se debe
-- --Categoria: Procedimiento para generar codigo al insertar
-- --      Procedimiento para actualizar
-- --      Procedimiento para eliminar - (para el que lo requiera)
-- go
go
-- ---------------------------------------------------CATEGORIA-------------------------------------------------------------------
-- --************************* Autogenerar Codigo Categoria*******************************
--EJemplo en categoria es C0001,C0002,C0003,C0004, sucesivamente
--Esta referenciado con libro  (No se permite eliminar)
--Categoria tiene un indice unico
--Tiene un trigger para autogenerar codigo(aunque en el procedimiento lo automaticemos)
create procedure sp_RegistrarCategoria ( 
    @Descripcion varchar(100), 
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
) --Generar codigo autoomaticamente e hacer demas inserciones -Hay un indice unico para categoria
as
begin
    SET @Resultado = 0 --No permite repetir una misma descripcion, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Categoria WHERE Descripcion = @Descripcion)
    begin 
        DECLARE @CodCategoria VARCHAR(10), @Cod int
        SELECT @Cod = RIGHT(MAX(IDCategoria),4 ) + 1 FROM Categoria;--Estamos seleccionando los numeros
        
        IF @Cod IS NULL --Pero si en inicio no hay ningun dato
        BEGIN  
            SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
        END
        
        SELECT @CodCategoria = CONCAT('C',RIGHT(CONCAT('0000',@Cod),4));
         insert into CATEGORIA(IDCategoria, Descripcion, Activo) values 
        (@CodCategoria,@Descripcion, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity()
    end
    else 
        SET @Mensaje = 'La categoria ya existe'
end
go

create  proc sp_EditarCategoria( --Trabajo como un booleano
    @IdCategoria nvarchar(10),
    @Descripcion varchar(100),--Tiene índice único
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion and IDCategoria != @IdCategoria)
    begin 
        update top(1) CATEGORIA set 
        Descripcion = @Descripcion,
        Activo = @Activo
        where IDCategoria = @IdCategoria

        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La categoria ya existe'
end
go

create proc sp_EliminarCategoria( --Trabajo como un booleano
    @IdCategoria nvarchar(10),
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM Libro p --validacion de que la categoria no este relacionada con un producto
    inner join CATEGORIA c on c.IDCategoria = p.Id_Categoria WHERE p.Id_Categoria= @IdCategoria)
    begin 
        delete top(1) from CATEGORIA where IDCategoria = @IdCategoria
        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La categoria se encuentra relacionada con un libro'
end
go
-- ----------------------------------------------------SALA------------------------------------------------------------------------
-- --************************* Autogenerar Codigo Sala***********************************
-- --EJemplo EN SALA es S0001,S0002,S0003,S0004, sucesivamente
-- --Sala tiene un indice unico para no repetirlas 
-- --Esta referenciado con libro (No se permite eliminar)
-- --Tiene un trigger para autogenerar codigo(aunque en el procedimiento lo automaticemos)

create procedure sp_RegistrarSala ( 
    @Descripcion varchar(100), 
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
) --Generar codigo autoomaticamente e hacer demas inserciones -Hay un indice unico para Sala
as
begin
    SET @Resultado = 0 --No permite repetir una misma descripcion, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Sala WHERE Descripcion = @Descripcion)
    begin 
        DECLARE @CodSala VARCHAR(10), @Cod int
        SELECT @Cod = RIGHT(MAX(IDSala),4 ) + 1 FROM Sala;--Estamos seleccionando los numeros
        
        IF @Cod IS NULL --Pero si en inicio no hay ningun dato
        BEGIN  
            SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
        END
        
        SELECT @CodSala = CONCAT('S',RIGHT(CONCAT('0000',@Cod),4));
         insert into Sala(IDSala, Descripcion, Activo) values 
        (@CodSala,@Descripcion, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity()
    end
    else 
        SET @Mensaje = 'La sala ya existe'
end
go

create  proc sp_EditarSala( --Trabajo como un booleano
    @IdSala nvarchar(10),
    @Descripcion varchar(100),--Tiene índice único
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM Sala WHERE Descripcion = @Descripcion and IDSala != @IdSala)
    begin 
        update top(1) Sala set 
        Descripcion = @Descripcion,
        Activo = @Activo
        where IDSala = @IdSala

        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La sala ya existe'
end
go

create proc sp_EliminarSala( --Trabajo como un booleano
    @IdSala nvarchar(10),
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM Libro l --validacion de que la Sala no este relacionada con un producto
    inner join Sala s on s.IDSala = l.Id_Sala WHERE l.Id_Sala= @IdSala)
    begin 
        delete top(1) from Sala where IDSala = @IdSala
        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La sala se encuentra relacionada con un libro'
end
GO


-- CREATE PROCEDURE sp_RegistrarSala ( @NombreSala varchar(30)) --Generar codigo autoomaticamente e hacer demas inserciones --Hay un indice unico para sala
-- AS
-- BEGIN
--   DECLARE @CodSala VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDSala),4 ) + 1 FROM Sala;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodSala = CONCAT('S',RIGHT(CONCAT('0000',@Cod),4));
--         INSERT INTO Sala VALUES (@CodSala,@NombreSala)
-- END
-- go
-- --*************************** Actualizar Sala *********************************** 
-- CREATE PROCEDURE sp_ActualizarSala
-- @IDSalaSP varchar(10),
-- @SalaSP varchar(30)--Tiene indice unico
-- AS
-- BEGIN
-- IF NOT EXISTS (SELECT * FROM Sala WHERE Sala =@SalaSP and IdSala != @IdSalaSP)
-- 	UPDATE Sala SET Sala = @SalaSP WHERE IDSala = @IDSalaSP;
-- END;
-- go
-- --*************************** Eliminar Sala *********************************** 
-- CREATE PROCEDURE sp_EliminarSala
-- @IDSalaSP varchar(10)
-- AS
-- DELETE Sala WHERE IDSala = @IDSalaSP;
 go
-- ---------------------------------------------------Editorial-------------------------------------------------------------------
-- --************************* Autogenerar Codigo Editorial*******************************
-- --EJemplo EN editorial es ED0001,ED0002,ED0003,ED0004, sucesivamente
-- --Editorial tiene un índice único
-- --Esta referenciado con libro  (No se permite eliminar)
-- --Tiene un trigger para autogenerar codigo(aunque en el procedimiento lo automaticemos)
create procedure sp_RegistrarEditorial ( 
    @Descripcion varchar(100),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
) --Generar codigo autoomaticamente e hacer demas inserciones -Hay un indice unico para Editorial
as
begin
    SET @Resultado = 0 --No permite repetir una misma descripcion, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Editorial WHERE Descripcion = @Descripcion)
    begin 
        DECLARE @CodEditorial VARCHAR(10), @Cod int
        SELECT @Cod = RIGHT(MAX(IDEditorial),4 ) + 1 FROM Editorial;--Estamos seleccionando los numeros
        
        IF @Cod IS NULL --Pero si en inicio no hay ningun dato
        BEGIN  
            SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
        END
        
        SELECT @CodEditorial = CONCAT('ED',RIGHT(CONCAT('0000',@Cod),4));
         insert into Editorial(IDEditorial, Descripcion, Activo) values 
        (@CodEditorial,@Descripcion, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity()
    end
    else 
        SET @Mensaje = 'La editorial ya existe'
end
go

create  proc sp_EditarEditorial( --Trabajo como un booleano
    @IdEditorial nvarchar(10),
    @Descripcion varchar(100),--Tiene índice único
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM Editorial WHERE Descripcion = @Descripcion and IDEditorial != @IdEditorial)
    begin 
        update top(1) Editorial set 
        Descripcion = @Descripcion,
        Activo = @Activo
        where IDEditorial = @IdEditorial

        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La editorial ya existe'
end
go

create proc sp_EliminarEditorial( --Trabajo como un booleano
    @IdEditorial nvarchar(10),
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM Libro l --validacion de que la Editorial no este relacionada con un producto
    inner join Editorial e on e.IDEditorial = l.Id_Editorial WHERE l.Id_Editorial= @IdEditorial)
    begin 
        delete top(1) from Editorial where IDEditorial = @IdEditorial
        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La editorial se encuentra relacionada con un libro'
end
go




-- CREATE PROCEDURE sp_RegistrarEditorial ( @NombreEditorial varchar(60)) --Generar codigo autoomaticamente e hacer demas inserciones -Hay un indice unico para editorial
-- AS
-- BEGIN
--   DECLARE @CodEditorial VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDEditorial),4 ) + 1 FROM Editorial;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodEditorial = CONCAT('ED',RIGHT(CONCAT('0000',@Cod),4));--Al tener dos letras, cambia el numero a recorrer a 3
--         INSERT INTO Editorial VALUES (@CodEditorial,@NombreEditorial)
-- END
-- go
-- --*************************** Actualizar Editorial*********************************** 
-- CREATE PROCEDURE sp_ActualizarEditorial
-- @IDEditorialSP varchar(10),
-- @EditorialSP varchar(60)--Tiene índice único
-- AS
-- BEGIN
-- IF NOT EXISTS (SELECT * FROM Editorial WHERE Editorial =@EditorialSP and IdEditorial != @IdEditorialSP)
-- 	UPDATE Editorial SET Editorial = @EditorialSP WHERE IDEditorial = @IDEditorialSP;
-- END;
-- go
-- --*************************** Eliminar Editorial *********************************** 
-- CREATE PROCEDURE sp_EliminarEditorial
-- @IDEditorialSP varchar(10)
-- AS
-- DELETE Editorial WHERE IDEditorial = @IDEditorialSP;
-- go
-- ---------------------------------------------------Autor------------------------------------------------------------------------
-- --************************* Autogenerar Codigo Autor *******************************
-- --Ejemplo EN autor es  A0001,A0002,A0003,A0004, sucesivamente
-- --Autor tiene un índice compuesto unico con (Nombre, Apellidos)
-- ----Esta referenciado con LibroAutor (No se puede eliminar, o afectará a todos)
-- --Tiene un trigger para autogenerar codigo(aunque en el procedimiento lo automaticemos)
use BibliotecaWeb
go
create procedure sp_RegistrarAutor ( 
    @Nombres nvarchar(100),
    @Apellidos nvarchar(100),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
) --Generar codigo autoomaticamente e hacer demas inserciones -Hay un indice unico para Autor
as
begin
    SET @Resultado = 0 --No permite repetir una misma descripcion, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Autor WHERE CONCAT(Nombres,Apellidos) = CONCAT(@Nombres,@Apellidos))
    begin 
        DECLARE @CodAutor VARCHAR(10), @Cod int
        SELECT @Cod = RIGHT(MAX(IdAutor),4 ) + 1 FROM Autor;--Estamos seleccionando los numeros
        
        IF @Cod IS NULL --Pero si en inicio no hay ningun dato
        BEGIN  
            SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
        END
        
        SELECT @CodAutor = CONCAT('A',RIGHT(CONCAT('0000',@Cod),4));
        insert into Autor(IDAutor, Nombres, Apellidos, Activo) values 
        (@CodAutor,@Nombres, @Apellidos, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity()
    end
    else 
        SET @Mensaje = 'La autor ya existe'
end
go

create  proc sp_EditarAutor( --Trabajo como un booleano
    @IdAutor nvarchar(10),
    @Nombres nvarchar(100),--Tiene indice compuesto con Apellidos
    @Apellidos nvarchar(100),--Tiene indice compuesto con Nombre
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM Autor WHERE CONCAT(Nombres,Apellidos) = CONCAT(@Nombres,@Apellidos) and IDAutor != @IdAutor)
    begin 
        update top(1) Autor set 
        Nombres = @Nombres,
        Apellidos = @Apellidos,
        Activo = @Activo
        where IDAutor = @IdAutor

        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La autor ya existe'
end
go

create proc sp_EliminarAutor( --Trabajo como un booleano
    @IdAutor nvarchar(10),
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM LibroAutor la --validacion de que la Autor no este relacionada con un producto
    inner join Autor a on a.IdAutor = la.Id_Autor WHERE la.Id_Autor = @IdAutor)
    begin 
        delete top(1) from Autor where IDAutor = @IdAutor
        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La autor se encuentra relacionada con un libro'
end
go








-- CREATE PROCEDURE sp_RegistrarAutor ( @NombreAutor varchar(40), @ApellidosAutor varchar(40)) --Hay un indice unico para el nombre completo de autor
-- AS
-- BEGIN
--   DECLARE @CodAutor VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDAutor),4 ) + 1 FROM Autor;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodAutor = CONCAT('A',RIGHT(CONCAT('0000',@Cod),4));
--         INSERT INTO Autor VALUES (@CodAutor,@NombreAutor,@ApellidosAutor)
-- END
-- go
-- --*************************** Actualizar Autor*********************************** 
-- CREATE PROCEDURE sp_ActualizarAutor
-- @IDAutorSP varchar(10),
-- @NombreAutorSP varchar(40),--Tiene indice compuesto con Apellidos
-- @ApellidosSP varchar(40)--Tiene indice compuesto con Nombre
-- AS
-- BEGIN--EJemplo anterior
-- --IF NOT EXISTS (SELECT * FROM Editorial WHERE Editorial =@EditorialSP and IdEditorial != @IdEditorialSP)
-- IF EXISTS (SELECT *  FROM Autor WHERE IDAutor = @IdAutorSP )
--   BEGIN
-- 	UPDATE Autor SET 
--   Nombre = @NombreAutorSP,
--   Apellidos = @ApellidosSP
--    WHERE IDAutor = @IDAutorSP;
--   END
-- END;
-- go
-- --*************************** Eliminar Autor *********************************** 
-- CREATE PROCEDURE sp_EliminarAutor
-- @IDAutorSP varchar(10)
-- AS
-- DELETE Autor WHERE IDAutor = @IDAutorSP;
-- go
-- ------------------------------------------------------TipoPersona----------------------------------------------------------
-- --************************* Actualizar TipoPersona ********************************
-- --Es identity
-- --Tiene un indice unico para Descripcion
-- --Solo hay tres tipos
-- --Esta referenciado a Usuario, pero no tiene sentido que usuario tenga un delete cascade EN SU Foreign key, porque no eliminaremos un tipo
-- --quizas la actualizaremos
-- --ademas si se puede elimianr mientras hay referencia (pero estos tres no tiene sentido eliminarlos)
-- CREATE PROCEDURE sp_ActualizarTipoPersona(--Actualiza todos los campos, menos el ID
--     @IDTipoPersonaSP varchar(25),
--     @DescripcionSP varchar(25) --En este se indica la nueva descripcion para el IDTipoPersona
--   ) 
-- AS
-- BEGIN 
--   IF EXISTS (SELECT *  FROM TipoPersona WHERE IdTipoPersona = @IDTipoPersonaSP )
--     BEGIN
-- 	    UPDATE TipoPersona SET Descripcion = @DescripcionSP WHERE IdTipoPersona = @IDTipoPersonaSP;
--     END
-- END;
-- ---------------------------------------------------Usuario------------------------------------------------------------
use BibliotecaWeb
go
-- --************************* Registrar Usuario *******************************
--Depende de tipo persona, pero no es necesario hacer un delete cascade, porque no tendria sentido eliminar un tipo de persona (solo tenemos 3)
--Tiene indice compuesto UNICO con (Nombre, A_Paterno, A_Materno)
--Tiene un Check para la edad
--Tiene un DEFAULT en Ciudad = ZACATLÁN 
--Tiene un DEFAULT en Observaciones = Ninguna 
--Tiene un DEFAULT en TipoPersona = 1
--Tiene un DEFAULT en FechaCreacion = GETDATE();
--Es identity
--Escuela de procedencia e EMAIL pueden ser NULL
--Esta referenciado con Prestamo (Si se puede eliminar, para ello su foreign key con prestamo tiene un delete en cascade)
--PROCEDIMIENTOS PARA USUARIO
create procedure sp_RegistrarUsuario(--Hay un indice unico para el nombre completo del usuario 
    --@IDUsuario int,---El id es Identity
    @Nombres varchar(100),--Tiene indice compuesto con Apellidos
    @Apellidos varchar(100),--Tiene indice compuesto con Nombre
    @Ciudad varchar(100),
    @Calle varchar(100),
    @Telefono varchar(20),
    @Correo varchar(100),--Puede ser null
    @Clave varchar(150),
    @Tipo int,
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
    --@ID_TipoPersona int --ESTARÁ COMO DEFAULT = 1, ES DECIR, COMO LECTOR
    --FechaCreacion date --Esta como default DEFAULT GETDATE()
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo)
    begin 
        insert into USUARIO(Nombres, Apellidos, Ciudad, Calle, Telefono, Correo, Clave,Tipo, Activo) values 
        (@Nombres, @Apellidos,@Ciudad,@Calle,@Telefono, @Correo, @Clave,@Tipo, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity()
    end 
    else 
     SET @Mensaje = 'El correo del usuario ya existe'
end 
go 
-- sp_RegistrarUsuario 'Zaid','test3','Tepeixco','Centro','18277373','angel@gmail.com','test123',1,1,'',1
create proc sp_EditarUsuario(
    @IdUsuario int,
    @Nombres varchar(100),--Tiene indice compuesto con Apellidos
    @Apellidos varchar(100),--Tiene indice compuesto con Nombre
    @Ciudad varchar(100),
    @Calle varchar(100),
    @Telefono varchar(20),
    @Correo varchar(100),--Puede ser null
    @Tipo int,
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
)
as
begin 
    SET @Resultado = 0
    IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo and IdUsuario != @IdUsuario)
    begin 
        update top(1) USUARIO set 
        Nombres = @Nombres,
        Apellidos  = @Apellidos,
        Ciudad = @Ciudad,
        Calle = @Calle,
        Telefono = @Telefono,
        Correo = @Correo,
        Tipo = @Tipo,
        Activo = @Activo
        where IdUsuario = @IdUsuario
        set @Resultado = 1
    end 
    else 
        set @Mensaje = 'El correo del usuario ya existe'
end
GO
select * from usuario
go
create proc sp_EliminarUsuario( --Trabajo como un booleano
    @IdUsuario int,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    begin
        delete top(1) from Usuario where IDUsuario = @IdUsuario
        set @Resultado = 1 --true
    end 
    if(@Resultado != 1)
        set @Mensaje = 'Error: No se pudo elimnar el usuario. Intentelo de nuevo'
end
GO
-- --************************* Registrar Usuario *******************************
--Depende de tipo persona, pero no es necesario hacer un delete cascade, porque no tendria sentido eliminar un tipo de persona (solo tenemos 3)
--Tiene indice compuesto UNICO con (Nombre, A_Paterno, A_Materno)
--Tiene un Check para la edad
--Tiene un DEFAULT en Ciudad = ZACATLÁN 
--Tiene un DEFAULT en Observaciones = Ninguna 
--Tiene un DEFAULT en TipoPersona = 1
--Tiene un DEFAULT en FechaCreacion = GETDATE();
--Es identity
--Escuela de procedencia e EMAIL pueden ser NULL
--Esta referenciado con Prestamo (Si se puede eliminar, para ello su foreign key con prestamo tiene un delete en cascade)
-- CREATE PROCEDURE sp_RegistrarUsuario(--Hay un indice unico para el nombre completo del usuario 
--     --@IDUsuario int,---El id es Identity
--     @Nombre nvarchar(40), --Tiene indice compuesto con A_Paterno y A_Materno
--     @A_Paterno varchar(20),--Tiene indice compuesto con Nombre y A_Materno
--     @A_Materno varchar(20),--Tiene indice compuesto con A_Paterno y Nombre
--     @Edad tinyint, --Tiene un check para una edad entre 0 y 125
--     @EscuelaProcedencia nvarchar(100),--Puede ser null --Aunque para este SP SI ES NULL se agrega un TIPO DEFAULT: NINGUNA
--     --Grado varchar (10), --No agregado en DDL
--     --/* @Ciudad*/ --Está Default como --"Zacatlán"
--     @Calle nvarchar(100),
--     @Telefono varchar(20), 
--     @Email nvarchar(100) --Puede ser null
--    -- @Observaciones varchar(500) --Estará definida como default, como "Ninguna"
--     --@ID_TipoPersona int --ESTARÁ COMO DEFAULT = 1, ES DECIR, COMO LECTOR
--     --FechaCreacion date --Esta como default DEFAULT GETDATE()
--   )--TERMINAN LOS PARAMETROS
-- AS
-- BEGIN
--     IF @EscuelaProcedencia IS NULL
--       BEGIN
--         SET @EscuelaProcedencia = 'NINGUNA';
--       END
--       INSERT INTO Usuario (Nombre, A_Paterno, A_Materno, Edad, EscuelaProcedencia, Calle, Telefono, Email)
--                    VALUES (@Nombre, @A_Paterno, @A_Materno, @Edad, @EscuelaProcedencia,@Calle, @Telefono, @Email)--/* @Ciudad*/ --Posible Default como --"Zacatlán"
-- END
-- go
-- --************************* Registrar Usuario Indicando TipoPersona*******************************
-- CREATE PROCEDURE sp_RegistrarUsuarioAdmin(--Hay un indice unico para el nombre completo del usuario 
--     --@IDUsuario int,
--     @Nombre nvarchar(40),--Hay un indce compueto con los apellidos
--     @A_Paterno varchar(20),
--     @A_Materno varchar(20),
--     @Edad tinyint,--Tiene un check para una edad entre 0 y 125
--     @EscuelaProcedencia nvarchar(100),--Puede ser null --Aunque para este SP SI ES NULL se agrega un TIPO DEFAULT: NINGUNA
--     --Grado varchar (10), No agregado en DDL
--     @Ciudad nvarchar(60), --Estará Default como --"Zacatlán"
--     @Calle nvarchar(100),
--     @Telefono varchar(20), 
--     @Email nvarchar(100),--PUEDE SER NULL 
--     @Observaciones varchar(500), --Estará definida como default, como "Ninguna"
--     @ID_TipoPersona int,
--     @Contrasenia VARCHAR(100)
--     --FechaCreacion date --Esta como default DEFAULT GETDATE()
--   )--TERMINAN LOS PARAMETROS
-- AS
-- BEGIN
--     IF @EscuelaProcedencia IS NULL
--       BEGIN
--        SET @EscuelaProcedencia = 'NINGUNA';
--       END
--     INSERT INTO Usuario (Nombre, A_Paterno, A_Materno, Edad, EscuelaProcedencia, Ciudad, Calle, Telefono, Email,Observaciones,ID_TipoPersona,Contrasenia)
--                  VALUES (@Nombre, @A_Paterno, @A_Materno, @Edad, @EscuelaProcedencia,@Ciudad,@Calle, @Telefono, @Email,@Observaciones,@ID_TipoPersona,@Contrasenia)--/* @Ciudad*/ --Posible Default como --"Zacatlán"
-- END
-- go


-- --************************* Registrar Usuario Indicando TipoPersona*******************************
-- CREATE PROCEDURE sp_RegistrarUsuarioConTipoPersona(--Hay un indice unico para el nombre completo del usuario 
--     --@IDUsuario int,
--     @Nombre nvarchar(40),--Hay un indce compueto con los apellidos
--     @A_Paterno varchar(20),
--     @A_Materno varchar(20),
--     @Edad tinyint,--Tiene un check para una edad entre 0 y 125
--     @EscuelaProcedencia nvarchar(100),--Puede ser null --Aunque para este SP SI ES NULL se agrega un TIPO DEFAULT: NINGUNA
--     --Grado varchar (10), No agregado en DDL
--     @Ciudad nvarchar(60), --Estará Default como --"Zacatlán"
--     @Calle nvarchar(100),
--     @Telefono varchar(20), 
--     @Email nvarchar(100),--PUEDE SER NULL 
--     @Observaciones varchar(500), --Estará definida como default, como "Ninguna"
--     @ID_TipoPersona int 
--     --FechaCreacion date --Esta como default DEFAULT GETDATE()
--   )--TERMINAN LOS PARAMETROS
-- AS
-- BEGIN
--     IF @EscuelaProcedencia IS NULL
--       BEGIN
--        SET @EscuelaProcedencia = 'NINGUNA';
--       END
--     INSERT INTO Usuario (Nombre, A_Paterno, A_Materno, Edad, EscuelaProcedencia, Ciudad, Calle, Telefono, Email,Observaciones,ID_TipoPersona)
--                  VALUES (@Nombre, @A_Paterno, @A_Materno, @Edad, @EscuelaProcedencia,@Ciudad,@Calle, @Telefono, @Email,@Observaciones,@ID_TipoPersona)--/* @Ciudad*/ --Posible Default como --"Zacatlán"
-- END
-- go

-- --*************************** Actualizar Usuario con tipo persona*********************************** 
-- CREATE PROCEDURE sp_ActualizarUsuario(
--     @IDUsuarioSP int,--No se va a actualizar el ID, esta variable se usará para identificar la actualizacion
--     @NombreUsuarioSP nvarchar(40),--Tiene indice compuesto con los apellidos
--     @A_PaternoSP varchar(20),
--     @A_MaternoSP varchar(20),
--     @EdadSP tinyint, --Tiene un check (0-125)
--     @EscuelaProcedenciaSP nvarchar(100),--Puede ser NULL
--     --Grado varchar (10),
--     @CiudadSP varchar(60), 
--     @CalleSP nvarchar(100),
--     @TelefonoSP varchar(20), 
--     @EmailSP nvarchar(100),--Puede ser NULL
--     @ObservacionesSP nvarchar(100),
--     @ID_TipoPersonaSP int, --1, 2 ó 3
--     @Contrasenia VARCHAR(100)
--     --FechaCreacion date --Esta como default DEFAULT GETDATE()
-- )
-- AS
-- BEGIN--EJemplo anterior
-- --IF NOT EXISTS (SELECT * FROM Usuario WHERE Email =@EmailSP and IdUsuario != @IdUsuarioSP)
-- IF EXISTS (SELECT *  FROM Usuario WHERE IDUsuario = @IdUsuarioSP )
--   BEGIN
-- 	UPDATE Usuario SET 
--   Nombre = @NombreUsuarioSP,
--   A_Paterno = @A_PaternoSP,
--   A_Materno = @A_MaternoSP,
--   Edad = @EdadSP,
--   EscuelaProcedencia = @EscuelaProcedenciaSP,
--     --Grado varchar (10),
--   Ciudad = @CiudadSP, 
--   Calle = @CalleSP,
--   Telefono = @TelefonoSP, 
--   Email = @EmailSP,
--   OBservaciones = @ObservacionesSP,
--   ID_TipoPersona = @ID_TipoPersonaSP,
--   Contrasenia = @Contrasenia
--   WHERE IDUsuario = @IDUsuarioSP;
--   END
-- END;
-- go
-- --*************************** Eliminar Usuario *********************************** 
-- CREATE PROCEDURE sp_EliminarUsuario
-- @IDUsuarioSP varchar(10)
-- AS
-- DELETE Usuario WHERE IDUsuario = @IDUsuarioSP;
-- go
-- ---------------------------------------------------Ejemplar------------------------------------------------------------
-- --EJemplo en Ejemplar es  EJ0001,EJ0002,EJ0003,EJ0004, sucesivamente
-- --Tiene un indice unico: ID_Libro
-- --Tiene un CHECK para NumEjemplar, donde no admita numeros menos a 0 ni mayores a 500 (0-500)
-- --Esta referenciado con Prestamo (Si se puede eliminar, para ello su foreign key con prestamo tiene un delete en cascade)
-- --Depende de libro, por eso tiene un delete cascade y update cascade en su foreign key (esto por si
-- --se elimina un libro o se actualiza su codigo)
-- --************************* Autogenerar Codigo Ejemplar y registrar*******************************
-- CREATE PROCEDURE sp_RegistrarEjemplar (--Hay un indice unico para validar el idlibro
--   @NumEjemplar int,
--   @ID_Libro_EJ varchar(25)
--   ) --Generar codigo autoomaticamente e hacer demas inserciones
-- AS
-- BEGIN
--   DECLARE @CodEjemplar VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDEjemplar),4 ) + 1 FROM Ejemplar;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--     IF EXISTS (SELECT * FROM Ejemplar WHERE NumEjemplar > 0)
--         SELECT @CodEjemplar = CONCAT('EJ',RIGHT(CONCAT('0000',@Cod),4));--Al tener dos letras, cambia el numero a recorrer a 3
--         INSERT INTO Ejemplar VALUES (@CodEjemplar,@NumEjemplar,@ID_Libro_EJ)
-- END
-- go
-- --*************************** Actualizar Ejemplar*********************************** 
-- CREATE PROCEDURE sp_ActualizarEjemplar(
-- @IDEjemplarSP varchar(10),
-- @NumEjemplarSP int,
-- @ID_Libro_EJSP varchar(25)
-- )
-- AS
-- BEGIN
-- IF NOT EXISTS (SELECT * FROM Ejemplar WHERE ID_Libro =@ID_Libro_EJSP and IdEJemplar != @IDEjemplarSP)
-- 	UPDATE Ejemplar SET
--   NumEjemplar = @NumEjemplarSP,
--   ID_Libro = @ID_Libro_EJSP
--   WHERE IDEjemplar = @IDEjemplarSP;
-- END;
-- go
-- --*************************** Eliminar Ejemplar *********************************** 
-- CREATE PROCEDURE sp_EliminarEjemplar
-- @IDEjemplarSP varchar(10)
-- AS
-- DELETE Ejemplar WHERE IDEjemplar = @IDEjemplarSP;
-- go

-- --------------------------------------------------Prestamo------------------------------------------------------------
-- go
-- --************************* Registrar prestamo ********************************
-- --Es identity
-- --Depende de usuario  (foreign key: delete cascade)
-- --Depende de prestamo (foreign key: delete cascade)
-- --Como no tiene referencias (nadie depende de prestamo), pues se puede eliminar
-- --Tiene un DEFAULT  en FechaPrestamo = GetDate();
-- --Tiene un DEFAULT  en Devuelto = 0; --Es decir devuelte = false
-- --Tiene un DEFAULT  en FechaDevolucion = NULL; En teoria al insertar pues no puede haber un fecha en que ya lo devolvió
-- --Tiene un TRIGGER PARA reducir 1 a NumEjemplar de la tabla ejemplar: Esto para que al insertar 
-- --automaticamente se reste uno a numeros ejemplares, esto por cada ejemplar (se puede insertar y aplica el trigger para muchos valores
-- -- mientras no queramos insertar al mismo tiempo dos mismos ejemplares)
-- --Tiene otro TRIGGER para actualizar prestamo, si se actualizar (falta corregir errores)
-- CREATE  PROCEDURE sp_RegistrarPrestamo (
--    --@IDPrestamo int,--Es identity
--    @ID_Usuario int,
--    @ID_Ejemplar varchar(10),
--    --@FechaPrestamo date,--Estará asignada automáticamente con el CONSTRAINT DEFAULT -- DF_Prestamo_FechaPrestamo
--    @FechaMaxDev date,
--    --@Devuelto bit,--1 es igual a si, y 0 es igual a no . Asigando por default como 0, 
--    --@FechaDevolucion date,--No especificaremos nada para que por default sea null
--    @Observaciones varchar(500) 
--    ) 
-- AS
-- BEGIN
--   INSERT INTO Prestamo (ID_Usuario,ID_Ejemplar,FechaMaxDev, Observaciones) 
--                 VALUES (@ID_Usuario,@ID_Ejemplar,@FechaMaxDev,@Observaciones)
-- END
-- go
-- --*************************** Actualizar Prestamo Completo*********************************** 
-- CREATE PROCEDURE sp_ActualizarPrestamo(
--    @IDPrestamoSP varchar(50),--Variable base para actualizar
--    @ID_UsuarioSP int,
--    @ID_EjemplarSP varchar(10),
--    @FechaPrestamoSP date,--Estará asignada automáticamente con el CONSTRAINT DEFAULT -- DF_Prestamo_FechaPrestamo
--    @FechaMaxDevSP date,
--    @DevueltoSP bit,--1 es igual a si, y 0 es igual a no . Asigando por default como 0, 
--    @FechaDevolucionSP date,--No especificaremos nada para que por default sea null
--    @ObservacionesSP varchar(500) 
--   ) 
-- AS
-- BEGIN
--   IF EXISTS (SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoSP )
--     IF @DevueltoSP = 1
--     BEGIN
-- 	   UPDATE Prestamo SET 
--        ID_Usuario = @ID_UsuarioSP,
--        ID_Ejemplar = @ID_EjemplarSP,
--        FechaPrestamo = @FechaPrestamoSP,--Estará asignada automáticamente con el CONSTRAINT DEFAULT -- DF_Prestamo_FechaPrestamo
--        FechaMaxDev = @FechaMaxDevSP,
--        Devuelto = @DevueltoSP,--1 es igual a si, y 0 es igual a no . Asigando por default como 0, 
--        FechaDevolucion = @FechaDevolucionSP,--No especificaremos nada para que por default sea null
--        Observaciones = @ObservacionesSP 
--      WHERE IDPrestamo = @IDPrestamoSP;
--     END
--  ELSE --(SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoDSP)--(SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoDSP AND Devuelto = 1)
--     BEGIN
-- 	   UPDATE Prestamo SET 
--        ID_Usuario = @ID_UsuarioSP,
--        ID_Ejemplar = @ID_EjemplarSP,
--        FechaPrestamo = @FechaPrestamoSP,--Estará asignada automáticamente con el CONSTRAINT DEFAULT -- DF_Prestamo_FechaPrestamo
--        FechaMaxDev = @FechaMaxDevSP,
--        Devuelto = @DevueltoSP,--1 es igual a si, y 0 es igual a no . Asigando por default como 0, 
--        FechaDevolucion = NULL,--No especificaremos nada para que por default sea null
--        Observaciones = @ObservacionesSP  
--      WHERE IDPrestamo = @IDPrestamoSP;
--     END
-- END;
-- go
-- --actualizar prestamo completo sin ifs
-- CREATE PROCEDURE sp_ActualizarPrestamoCompleto(
--    @IDPrestamoSP int,--Variable base para actualizar
--    @ID_UsuarioSP int,
--    @ID_EjemplarSP varchar(10),
--    @FechaPrestamoSP date,--Estará asignada automáticamente con el CONSTRAINT DEFAULT -- DF_Prestamo_FechaPrestamo
--    @FechaMaxDevSP date,
--    @DevueltoSP bit,--1 es igual a si, y 0 es igual a no . Asigando por default como 0, 
--    @FechaDevolucionSP date,--No especificaremos nada para que por default sea null
--    @ObservacionesSP varchar(500) 
--   ) 
-- AS
-- BEGIN
--   IF EXISTS (SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoSP )
--     BEGIN
-- 	   UPDATE Prestamo SET 
--        ID_Usuario = @ID_UsuarioSP,
--        ID_Ejemplar = @ID_EjemplarSP,
--        FechaPrestamo = @FechaPrestamoSP,--Estará asignada automáticamente con el CONSTRAINT DEFAULT -- DF_Prestamo_FechaPrestamo
--        FechaMaxDev = @FechaMaxDevSP,
--        Devuelto = @DevueltoSP,--1 es igual a si, y 0 es igual a no . Asigando por default como 0, 
--        FechaDevolucion = @FechaDevolucionSP,--No especificaremos nada para que por default sea null
--        Observaciones = @ObservacionesSP 
--      WHERE IDPrestamo = @IDPrestamoSP;
--     END
-- END
-- go
-- --*************************** Actualizar Prestamo Devuelto con la fecha*********************************** 
-- CREATE PROCEDURE sp_ActualizarPrestamoDevYFecha( --Servira cuando se devuelva un libro, donde obviamente debemos ingresar la fechaDev
-- @IDPrestamoDSP varchar(10),
-- @DevueltoSiNoSP bit,--Por default tiene 0
-- @FechaDevolucionDSP date --Por default es null
-- )
-- AS
-- BEGIN
--   IF EXISTS (SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoDSP)
--     IF @DevueltoSiNoSP = 1
--     BEGIN
-- 	   UPDATE Prestamo SET 
--      Devuelto = @DevueltoSiNoSP,
--      FechaDevolucion = @FechaDevolucionDSP 
--      WHERE IDPrestamo = @IDPrestamoDSP;
--     END
--  ELSE --(SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoDSP)--(SELECT *  FROM Prestamo WHERE IDPrestamo = @IdPrestamoDSP AND Devuelto = 1)
--     BEGIN
-- 	   UPDATE Prestamo SET 
--      Devuelto = @DevueltoSiNoSP,
--      FechaDevolucion = NULL 
--      WHERE IDPrestamo = @IDPrestamoDSP;
--     END
-- END;
-- go

-- --*************************** Eliminar Prestamo *********************************** 
-- CREATE PROCEDURE sp_EliminarPrestamo
-- @IDPrestamoSP varchar(10)
-- AS
-- DELETE Prestamo WHERE IDPrestamo = @IDPrestamoSP;
-- go
-- ---------------------------------------------------Libro Autor------------------------------------------------------------
-- --************************* Autogenerar Codigo LibroAutor*******************************
-- --Tiene un trigger para autogenerar codigo
-- --EJemplo EN LibroAutor es LA0001,LA0002,LA0003,LA0004, sucesivamente
-- --Tiene un indice compuesto unico con ID_Libro + ID_Autor
-- --Depende de ID_Autor
-- --Depende de libro, por eso tiene un delete cascade y update cascade en su foreign key (esto por si
-- --se elimina un libro o se actualiza su codigo)
-- --LibroAutor no esta referenciado a otra tabla (al igual que prestamo se puede eliminar)

-- CREATE PROCEDURE sp_RegistrarLibroAutor ( --Hay un indice unico compuesto de los dos valores
--   @ID_Libro_LA varchar(25),
--   @ID_Autor varchar(10)
--   ) --Generar codigo autoomaticamente e hacer demas inserciones
-- AS
-- BEGIN
--   DECLARE @CodLibroAutor VARCHAR(10), @Cod int
--   SELECT @Cod = RIGHT(MAX(IDLibroAutor),4 ) + 1 FROM LibroAutor;--Estamos seleccionando los numeros
--     IF @Cod IS NULL --Pero si en inicio no hay ningun dato
--       BEGIN  
--         SElECT @Cod = 1; --Entonces asignamos como primer numero = 1
--       END
--         SELECT @CodLibroAutor = CONCAT('LA',RIGHT(CONCAT('0000',@Cod),4));--Al tener dos letras, cambia el numero a recorrer a 3
--         INSERT INTO LibroAutor VALUES (@CodLibroAutor, @ID_Libro_LA, @ID_Autor)
-- END
-- go
-- --*************************** Actualizar LibroAutor*********************************** 
-- CREATE PROCEDURE sp_ActualizarLibroAutor 
-- @IDLibroAutorSP varchar(10),
-- @ID_Libro_LASP varchar(60),
-- @ID_Autor_LASP varchar(60)
-- AS
-- BEGIN--EJemplo anterior
-- --IF NOT EXISTS (SELECT * FROM Editorial WHERE Editorial =@EditorialSP and IdEditorial != @IdEditorialSP)
-- IF EXISTS (SELECT *  FROM LibroAutor WHERE IDLibroAutor = @IdLibroAutorSP )
--   BEGIN
-- 	UPDATE LibroAutor SET 
--   ID_Libro = @ID_LIBRO_LASP,
--   ID_Autor = @ID_Autor_LASP
--    WHERE IDLibroAutor = @IDLibroAutorSP;
--   END
-- END;
-- go
-- --*************************** Eliminar LibroAutor *********************************** 
-- CREATE PROCEDURE sp_EliminarLibroAutor
-- @IDLibroAutorSP varchar(10)
-- AS
-- DELETE LibroAutor WHERE IDLibroAutor = @IDLibroAutorSP;
-- go
-- ------------------------------------------------------Libro----------------------------------------------------------
-- --************************* Registrar Libro ********************************
-- --Tiene un DEFAULT en Observaciones = EN PERFECTO ESTADO
-- --Tiene un DEFAULT en Volumen = 1
-- --Tiene un DEFAULT en Sala = S0001 (Sala General)
-- --Depende de Sala 
-- --Depende de Categoria
-- --Depende de Editorial
-- --Como estas tres no se permite eliminar, no es necesarios declararlas con delete cascade
-- --IMPORTANTE: Entender porque unas si se pueden eliminar y otras no:
-- /*            No podemos eliminar libro si el libroAutor no fuera cascade 
--               No podemos eliminar libro si el ejemplar no fuera cascade
--               Ahora, aunque el ejemplar fuera cascade (como esta referenciado a prestamo
--               pues tambien debe ser cascade en la foreign key de prestamo) de lo contrario
--               no podriamos eliminar el libro.
--           Se podria decir que 
--               Libro esta referenciado a ejemplar, y ejemplar esta referenciado a prestamo
--               Por eso para poder eliminar libro, las foreign key de estos debe ser cascade
-- */
--PROCEDIMIENTOS PARA LIBRO
create procedure sp_RegistrarLibro(
    @Codigo varchar(25),--Es asignado por administrador al insertar
    @Titulo nvarchar(130),
    @Paginas int,
    --Llaves foraneas
    @IDCategoria nvarchar(10),
    @IDEditorial nvarchar(10),
    @IDSala nvarchar(10),--Tiene un DEFAULT en Sala = S0001 (Sala General)
    @Ejemplares int,
    @AñoEdicion varchar(5),
    @Volumen int,
    @Observaciones varchar(500), --Definido como default: EN BUEN ESTADO
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Libro WHERE Codigo = @Codigo)
    begin 
        insert into Libro(Codigo,Titulo,Paginas,Id_Categoria, Id_Editorial,Id_Sala, Ejemplares, AñoEdicion,Volumen,Observaciones, Activo) values 
        (@Codigo, @Titulo,@Paginas, @IdCategoria, @IdEditorial, @IdSala, @Ejemplares, @AñoEdicion,@Volumen, @Observaciones, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity() 
    end 
    else 
     SET @Mensaje = 'El codigo del libro ya existe'
end 
go

create procedure sp_EditarLibro(
    @IDLibro int,
    @Codigo varchar(25),--Es asignado por administrador al insertar
    @Titulo nvarchar(130),
    @Paginas int,
    --Llaves foraneas
    @IDCategoria nvarchar(10),
    @IDEditorial nvarchar(10),
    @IDSala nvarchar(10),--Tiene un DEFAULT en Sala = S0001 (Sala General)
    @Ejemplares int,
    @AñoEdicion varchar(5),
    @Volumen int,
    @Observaciones varchar(500), --Definido como default: EN BUEN ESTADO
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Libro WHERE Codigo = @Codigo and IdLibro != @IdLibro)
    begin 
        update Libro set
        Codigo = @Codigo,
        Titulo = @Titulo,
        Paginas = @Paginas, 
        ID_Categoria = @IDCategoria, 
        ID_Editorial = @IDEditorial, 
        ID_Sala = @IDSala, 
        Ejemplares = @Ejemplares, 
        AñoEdicion = @AñoEdicion, 
        Volumen = @Volumen, 
        Observaciones = @Observaciones, 
        Activo = @Activo 
        where IdLibro = @IdLibro
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
     SET @Mensaje = 'El código del libro ya existe'
end 
go
create procedure sp_EliminarLibro(
    @IdLibro int,
    @Mensaje varchar(500) output,
    @Resultado int output
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (select * from DetallePrestamo dp
    inner join Libro l on l.Codigo = dp.Codigo
    where l.IdLibro = @IdLibro)--No podemos eliminar un Libro si ya esta incluido en una venta
    begin 
        delete top(1) from Libro where IdLibro = @IdLibro
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
     SET @Mensaje = 'El libro se encuentra relacionado a una préstamo'
end 
go
-- CREATE PROCEDURE sp_RegistrarLibro (
--     @IDLibro varchar(25),--Es asignado por administrador al insertar
--     @Titulo nvarchar(130),
--     @Ubicacion varchar(10),
--     @NumEdicion varchar(60),
--     @AñoEdicion varchar(5),
--     @Volumen tinyint,--Tiene un DEFAULT en Volumen = 1
--     @NumPaginas int,
--    -- @Observaciones varchar(500), --Definido como default: EN PERFECTO ESTADO
--     --Llaves foraneas
--     @ID_Sala varchar(10),--Tiene un DEFAULT en Sala = S0001 (Sala General)
--     @ID_Categoria varchar(10),
--     @ID_Editorial varchar(10)
--    ) 
-- AS
-- BEGIN
--   INSERT INTO Libro(IDLibro, Titulo, Ubicacion, NumEdicion, AñoEdicion, Volumen, NumPaginas,
--                         ID_Sala, ID_Categoria, ID_Editorial)
--                 VALUES (@IDLibro, @Titulo, @Ubicacion, @NumEdicion, @AñoEdicion, @Volumen, @NumPaginas,
--                         @ID_Sala, @ID_Categoria, @ID_Editorial)
-- END
-- go

-- --**************************Registrar libro con todos los campos **********************
-- CREATE PROCEDURE sp_RegistrarLibroCompleto (
--     @IDLibro varchar(25),--Es asignado por administrador al insertar
--     @Titulo nvarchar(130),
--     @Ubicacion varchar(10),
--     @NumEdicion varchar(60),
--     @AñoEdicion varchar(5),
--     @Volumen tinyint,--Tiene un DEFAULT en Volumen = 1
--     @NumPaginas int,
--     @Observaciones varchar(500), --Definido como default: EN PERFECTO ESTADO
--     --Llaves foraneas
--     @ID_Sala varchar(10),--Tiene un DEFAULT en Sala = S0001 (Sala General)
--     @ID_Categoria varchar(10),
--     @ID_Editorial varchar(10)
--    ) 
-- AS
-- BEGIN
--   INSERT INTO Libro(IDLibro, Titulo, Ubicacion, NumEdicion, AñoEdicion, Volumen, NumPaginas,Observaciones,
--                         ID_Sala, ID_Categoria, ID_Editorial)
--                 VALUES (@IDLibro, @Titulo, @Ubicacion, @NumEdicion, @AñoEdicion, @Volumen, @NumPaginas, @Observaciones,
--                         @ID_Sala, @ID_Categoria, @ID_Editorial)
-- END
-- go


-- --*************************** Actualizar Libro Completo*********************************** 
-- CREATE PROCEDURE sp_ActualizarLibro(--Actualiza todos los campos, menos el ID
--     @IDLibroSP varchar(25),
--     @TituloSP nvarchar(130),
--     @UbicacionSP varchar(10),
--     @NumEdicionSP varchar(60),
--     @AñoEdicionSP varchar(5),
--     @VolumenSP tinyint,--Tiene un DEFAULT en Volumen = 1
--     @NumPaginasSP int,
--     @Observaciones varchar(500), --Definido como default: EN PERFECTO ESTADO
--     --Llaves foraneas
--     @ID_SalaSP varchar(10),--Tiene un DEFAULT en Sala = S0001 (Sala General)--Aunque en procedimiento si es un parametro se debe colocar si  o si
--     @ID_Categoria varchar(10),
--     @ID_Editorial varchar(10) 
--   ) 
-- AS
-- BEGIN --EJemplo anterior
--  --IF NOT EXISTS (SELECT * FROM Editorial WHERE Editorial =@EditorialSP and IdEditorial != @IdEditorialSP)
--   IF EXISTS (SELECT *  FROM Libro WHERE IDLibro = @IdLibroSP )
--     BEGIN
-- 	    UPDATE Libro SET 
--        Titulo = @TituloSP,
--        Ubicacion = @UbicacionSP,
--        NumEdicion = @NumEdicionSP,
--        AñoEdicion = @AñoEdicionSP,
--        Volumen = @VolumenSP,
--        NumPaginas = @NumPaginasSP,
--        Observaciones = @Observaciones, --Definido como default: EN PERFECTO ESTADO
--     --Llaves foraneas
--        ID_Sala = @ID_SalaSP,
--        ID_Categoria = @ID_Categoria,
--        ID_Editorial = @ID_Editorial 
--       WHERE IDLibro = @IDLibroSP;
--     END
-- END;
-- go
-- --*************************** Actualizar CodigoLibro *********************************** 
-- CREATE PROCEDURE sp_ActualizarCodigoLibro(--Actualiza todos los campos, menos el ID
--     @IDLibro_CodigoSP varchar(25),--En este se indica el codigo actual del libro para identificar la actualizacion
--     @IDLibroActualizarSP varchar(25) --En este se indica el nuevo codigo o ID para el libro
--   ) 
-- AS
-- BEGIN 
--   IF EXISTS (SELECT *  FROM Libro WHERE IDLibro = @IdLibro_CodigoSP )
--     BEGIN
-- 	    UPDATE Libro SET IDLibro = @IDLibroActualizarSP WHERE IDLibro = @IDLibro_CodigoSP;
--     END
-- END;
-- go
-- --*************************** Eliminar Libro *********************************** 
-- CREATE PROCEDURE sp_EliminarLibro
-- @IDLibroSP varchar(10)
-- AS
-- DELETE Libro WHERE IDLibro = @IDLibroSP;
-- GO
-- -----------Procedimientos almacenados para vistas, busqueda, logins y demas en los formularios de Aplicacion---------------------------------------------------------
-- --Login
-- CREATE PROC sp_Logueo 
-- @Usuario varchar(50),
-- @pass varchar(40)
-- as  
-- SELECT * FROM V_TablaLogin WHERE Usuario = @Usuario and Pass = @pass; 
-- GO
-- -------------------------------------------Formulario Libro ---------------------------------------------------------------
-- -----Listas para comboBox de sistema: Form libro
-- CREATE PROC sp_ListarSalas
-- AS 
-- SELECT * FROM Sala ORDER BY IDSala asc
-- go

-- CREATE PROC sp_ListarCategorias 
-- AS 
-- SELECT * FROM Categoria ORDER BY IDCategoria asc

-- go
-- CREATE PROC sp_ListarEditorial
-- AS 
-- SELECT IDEditorial AS Codigo, Editorial FROM Editorial ORDER BY IDEditorial DESC;

-- go
-- CREATE PROC sp_BuscarListadoEditorial--PARA BUSCAR usuarios al registrar prestamos (new)
-- @ID VARCHAR(40),
-- @Editorial VARCHAR(40)
-- AS
-- SELECT * FROM V_ListadoEditoriales WHERE Codigo LIKE '%' + @ID + '%' 
--                               OR Editorial LIKE '%' + @Editorial + '%';
-- go
-- --Consultas para los 3 botones iniciales de formulario
-- CREATE PROC sp_ConsultarLibrosCompletos -- Para administrador
-- AS 
-- SELECT IDLibro as Codigo, Titulo,Ubicacion,NumEdicion AS Edicion, AñoEdicion as Año, Volumen as Vol, NumPaginas as Paginas,Observaciones,
-- ID_Sala AS Sala,ID_Categoria as Categoria,ID_Editorial as Editorial, Editorial as Editoriales FROM Libro 
-- INNER JOIN Editorial ON Editorial.IDEditorial = Libro.ID_Editorial
-- ORDER BY IDLibro;
-- go --SELECT TOP 15 * FROM Libro ORDER BY IDLibro ASC

-- CREATE PROC sp_ConsultarLibrosColumImportantes -- Para lector y demas
-- AS 
-- SELECT * FROM V_LibroColumPrincipales;
-- go
-- CREATE PROC sp_ConsultarUsuariosLibros -- Para administrador y bibliotecario
-- AS 
-- SELECT * FROM V_UsuarioInicio
-- GO

-- CREATE PROC sp_ConsultarPrestamosLibros --
-- AS 
-- SELECT * FROM V_PrestamoInicio; --Administrador
-- GO
-- CREATE PROC sp_BuscarPrestamosPeriodoInicio
-- @fromDate VARCHAR(40),
-- @toDate VARCHAR(40)
-- AS
-- SELECT  * FROM V_PrestamoInicio WHERE [Fecha Préstamo]  BETWEEN @fromDate and @toDate;
-- go
-- -----------------Busqueda de libro ---------
-- CREATE PROC sp_BuscarLibro
-- @Titulo VARCHAR(100)
-- AS
-- SELECT IDLibro as Codigo, Titulo, Ubicacion, NumEdicion AS Edicion, AñoEdicion as Año, Volumen as Vol, NumPaginas as Paginas,Observaciones,
-- ID_Sala AS Sala,ID_Categoria as Categoria,ID_Editorial as Editorial, Editorial as Editoriales FROM Libro 
-- INNER JOIN Editorial ON Editorial.IDEditorial = Libro.ID_Editorial
-- WHERE Titulo LIKE '%' + @Titulo + '%' ORDER BY IDLibro;
-- go

-- CREATE PROC sp_BuscarLibroLectores --Debido a que al lector se le muestra la vista de columnas principales, la busqueda tambien será de esta
-- @Titulo VARCHAR(100)
-- AS
-- SELECT * FROM V_LibroColumPrincipales WHERE Titulo LIKE '%' + @Titulo + '%'
-- go


-- --Cantidades para la presentacion de Inicio (Libros, Usuarios, Prestamos registrados, y Prestamos pendientes)
-- CREATE PROC sp_CantLibrosTotales
-- AS
-- SELECT COUNT(IDLibro) from Libro
-- go
-- update prestamo set devuelto = 1  where idPrestamo = 2;
-- go

-- ----------------------------------- Procedimientos almacenados para el fornulario usuario --------------------------------------
-- -----Listar para comboBox de sistema: Form Usuario
-- /*
-- CREATE PROC sp_ConsultarUsuariosAdmin -- Para administrador y bibliotecario
-- AS 
-- SELECT * FROM V_UsuarioAdmin;--Solo muestra Administradores y bibliotecarios
-- GO*/
-- CREATE PROC sp_ConsultarTodosUsuariosAdmin -- Para administrador y bibliotecario
-- AS 
-- SELECT * FROM V_TodosUsuarioAdmin;
-- GO
-- ----------------------cHECAR SI SE UTILIZA O NO
-- CREATE PROC sp_ConsultarUsuariosNormal -- Para administrador y bibliotecario
-- AS 
-- SELECT * FROM V_Lector;
-- -----------------
-- GO
-- CREATE PROC sp_ListarTipoPersona
-- AS 
-- SELECT * FROM TipoPersona ORDER BY Descripcion desc
-- go
-- CREATE PROC sp_ListarTipoPersonaBibliotecario
-- AS 
-- SELECT * FROM TipoPersona WHERE IdTipoPersona = 1 
-- go

-- CREATE PROC sp_BuscarUsuarioAdmin
-- @Nombre VARCHAR(40),
-- @A_Paterno VARCHAR(40),
-- @A_Materno VARCHAR(40)
-- AS
-- SELECT * FROM V_TodosUsuarioAdmin WHERE Nombre LIKE '%' + @Nombre + '%' 
--                           OR Paterno LIKE '%' + @A_Paterno + '%' 
--                           OR Materno LIKE '%' + @A_Materno + '%';
-- go
-- CREATE  PROC sp_BuscarUsuarioBibliotecario
-- @Nombre VARCHAR(40),
-- @A_Paterno VARCHAR(40),
-- @A_Materno VARCHAR(40)
-- AS
-- SELECT * FROM V_Lector WHERE Nombre LIKE '%' + @Nombre + '%' 
--                           OR A_Paterno LIKE '%' + @A_Paterno + '%' 
--                           OR A_Materno LIKE '%' + @A_Materno + '%'; 
-- go
-- CREATE PROC sp_BuscarUsuarioInicio--para busqueda d usuario en inicio y busqueda de usuario en prestamo
-- @Nombre VARCHAR(40)
-- AS
-- SELECT * FROM V_UsuarioInicio WHERE Nombre LIKE '%' + @Nombre + '%'
-- go

-- --------------------------------------Procedimientos almacenados para el formulario de prestamo-----------------------------------------------------------------
-- --Listas para llenar combo box del formulario usuario, listar el ejemplar y el usuario (con nombre completo)
-- CREATE PROC sp_ListarEjemplarLibro
-- AS 
-- SELECT NumEjemplar as Stock, Titulo , IDEjemplar as Codigo FROM Ejemplar 
-- INNER JOIN Libro ON Libro.IDLibro = Ejemplar.ID_Libro ORDER BY IDEjemplar DESC;
-- GO
-- CREATE PROC sp_ListarUsuarioLibro
-- AS 
-- SELECT IDUsuario as Codigo, CONCAT(Nombre,' ',A_Paterno,' ',A_Materno) AS Nombre
-- FROM Usuario ORDER BY IDUsuario DESC--SE PUEDE USAR LA VISTA CREADA
-- go  
-- ----Consultas de prestamo
-- CREATE PROC sp_ConsultarPrestamosCompleto --Para inserció, actualizacion, elimacion en un form
-- AS 
-- SELECT * FROM V_PrestamoCompleto
-- GO
-- /*
-- CREATE PROC sp_ConsultarPrestamosInicio --Para inserció, actualizacion, elimacion en un form
-- AS 
-- SELECT * FROM V_PrestamoInicio
-- GO
-- */
-- ----Busqueda de prestamo
-- GO        
-- CREATE PROC sp_BuscarPrestamoInicio
-- @Usuario VARCHAR(40),
-- @Titulo VARCHAR(40),
-- @Estado VARCHAR(10)
-- AS
-- SELECT  * FROM V_PrestamoInicio WHERE Usuario LIKE '%' + @Usuario + '%' 
--                           OR [Titulo del libro] LIKE '%' + @Titulo + '%'
--                           OR Estado LIKE '%' + @Estado + '%';
-- GO                  
-- CREATE PROC sp_BuscarPrestamos
-- @Usuario VARCHAR(40),
-- @Titulo VARCHAR(40),
-- @Estado VARCHAR(10)
-- AS
-- SELECT  * FROM V_PrestamoCompleto WHERE [Nombre de Usuario]  LIKE '%' + @Usuario + '%' 
--                           OR [Libro en Préstamo] LIKE '%' + @Titulo + '%'
--                           OR Estado LIKE '%' + @Estado + '%';
-- go
-- CREATE PROC sp_BuscarPrestamosPeriodo 
-- @fromDate VARCHAR(40),
-- @toDate VARCHAR(40)
-- AS
-- SELECT  * FROM V_PrestamoCompleto WHERE [Fecha Préstamo]  BETWEEN @fromDate and @toDate;
-- go
-- sp_BuscarPrestamosPeriodo '2022-11-01','2022-12-04';
-- go
-- CREATE PROC sp_BuscarUsuariosPrestamo--PARA BUSCAR usuarios al registrar prestamos (new)
-- @ID VARCHAR(40),
-- @Usuario VARCHAR(40)
-- AS
-- SELECT * FROM V_ListadoUsuarios WHERE Codigo LIKE '%' + @ID + '%' 
--                               OR [Nombre de Usuario] LIKE '%' + @Usuario + '%';
-- GO
-- CREATE PROC sp_BuscarLibrosPrestamo--PARA BUSCAR usuarios al registrar prestamos (new)
-- @ID VARCHAR(40),
-- @Libros VARCHAR(40)
-- AS
-- SELECT * FROM V_ListadoLibrosEjemplares WHERE Codigo LIKE '%' + @ID + '%' 
--                               OR Titulo LIKE '%' + @Libros + '%';
-- GO
-- ----------------------------- Procedimientos almacenados para el formulario categoria ------------------------------------------------------
-- CREATE PROC sp_ConsultarCategoria
-- AS 
-- SELECT * FROM CATEGORIA order by IDCategoria;
-- GO
-- CREATE PROC sp_BuscarCategoria
-- @NombreCategoria VARCHAR(100)
-- AS
-- SELECT * FROM Categoria WHERE Categoria LIKE '%' + @NombreCategoria + '%';               
-- GO  
-- ---------------------------------------- Procedimientos almacenados para el formulario editorial ---------------------------------------------
-- CREATE PROC sp_ConsultarEditoral
-- AS 
-- SELECT * FROM Editorial order by IDEditorial DESC;
-- GO
-- CREATE PROC sp_BuscarEditorial
-- @NombreEditorial VARCHAR(100)
-- AS
-- SELECT  * FROM Editorial WHERE Editorial LIKE '%' + @NombreEditorial + '%';
-- go
-- ---------------------------------------------------- Procedimientos almacenados para el formulario sala -----------------------------------------
-- CREATE PROC sp_ConsultarSala
-- AS 
-- SELECT * FROM Sala order by IDSala;
-- GO
-- CREATE PROC sp_BuscarSala
-- @NombreSala VARCHAR(100)
-- AS
-- SELECT  * FROM Sala WHERE Sala LIKE '%' + @NombreSala + '%';
-- go
-- -------------------------------------  Procedimientos almacenados para el formulario autor ---------------------------------------------------------------
-- CREATE PROC sp_ConsultarAutores
-- AS 
-- SELECT * FROM Autor order by IDAutor DESC;
-- GO
-- CREATE PROC sp_BuscarAutor
-- @NombreAutor VARCHAR(40),
-- @ApellidosAutor VARCHAR(40)
-- AS
-- SELECT  * FROM Autor WHERE Nombre LIKE '%' + @NombreAutor + '%'
--                         OR Apellidos LIKE '%' + @ApellidosAutor + '%';
-- go
-- SELECT * FROM TipoPersona;
-- SELECT * FROM Usuario;
-- SELECT * from Libro;
-- SELECT * FROM Prestamo;
-- SELECT * FROM Ejemplar;
-- SELECT * FROM Sala;
-- SELECT * FROM Editorial;
-- SELECT * FROM Categoria;
-- go
-- use Biblioteca;
-- go
-- CREATE PROC sp_ReportePrestamos 
-- @fromFecha date,
-- @toFecha date
-- AS
-- --ID, Usuario,Titulo, Estado, FechaPrestamo, FechaMaxDev, FechaDevolucion
-- SELECT  IDPrestamo AS ID,CONCAT (Nombre,' ',A_Paterno,' ',A_Materno) AS Usuario, Titulo, Devuelto as Estado, FechaPrestamo,
-- FechaMaxDev,  FechaDevolucion/*Telefono, CONCAT(Ciudad, '/',Calle) as Dirección, */, 1 as Num FROM Prestamo 
-- INNER JOIN Usuario ON Usuario.IDUsuario = Prestamo.ID_Usuario 
-- INNER JOIN Ejemplar ON  Prestamo.ID_Ejemplar = Ejemplar.IDEjemplar 
-- --INNER JOIN TipoPersona ON  TipoPersona.IdTipoPersona = Usuario.ID_TipoPersona 
-- INNER JOIN Libro ON  Libro.IDLibro = Ejemplar.ID_Libro
-- WHERE FechaPrestamo BETWEEN @fromFecha and @toFecha ORDER BY IDPrestamo DESC;
-- go
-- sp_ReportePrestamos '2022-11-01','2022-12-04';
-- GO


--PROCEDIMIENTOS ALMACENADOS PARA EL DASHBOARD
go
select * from detallePrestamo
go
create proc sp_ReporteDashboard --Para el reporte de dashboard
as 
begin
select
    (select count(*) from Lector) [TotalLector],
    (select count(*) from DetallePrestamo) [TotalPrestamo],
    (select count(*) from Libro)[TotalLibro],
    (select isnull(sum(Ejemplares), 0) from libro)[TotalEjemplares]
end
go


select * from Detalleprestamo

select * from prestamo
--
go
SELECT * FROM LIBRO
GO
sp_ReportePrestamos '01/06/2023', '27/06/2023','LOBVA'
GO  
create proc sp_ReportePrestamos(
    @fechaInicio varchar(40),
    @fechaFin varchar(40),
    @codigo varchar(50)
)
as
begin
    set dateformat dmy; /*Indicamos el formato que queremos si o si*/
    --el formato 103, muestra solo la fecha
    select CONVERT(char(10), p.FechaPrestamo,103) [FechaPrestamo] , CONCAT(lc.Nombres,' ', lc.Apellidos)[Lector],
    l.Titulo[Libro], dp.CantidadEjemplares, p.Estado, dp.Total,l.Codigo
    from DetallePrestamo dp
    inner join Libro l on l.Codigo = dp.IDLibro
    inner join Prestamo p on p.IdPrestamo = dp.IdPrestamo
    inner join Lector lc on lc.IdLector = p.Id_Lector
    where CONVERT(date, p.FechaPrestamo) BETWEEN @fechaInicio and @fechaFin 
    and l.Codigo = iif(@codigo = '', l.Codigo, @codigo)
    /*Si el usuario no esta indicando ningun id de transaccion, le decimos que use ese mismo id transaccion del where, pero
    si lo esta indicando, lo use con el @idTransaccion*/
end

select * from Prestamo

update prestamo set estado =  1

SELECT * FROM USUARIO