Use BibliotecaWeb2;
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
--Tiene un trigger para autogenerar codigo(aunque en el procedimiento lo automaticemos) ahora en sp
create procedure sp_RegistrarCategoria ( --Generar codigo autoomaticamente e hacer demas inserciones --Hay un indice unico para categoria
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
--POSIBLE MODIFICACION DE sp EditarCategoria: Para no actualizar a inactivo si la categoria esta relacionada con un libro
--O quizas la mejr opcion sea inabilitar la seleccion de activo para no dejar al usuario admin actualizar ese valo a inactivo
--y no generar problemas de logico relacionando un libro con categoria que quizas este inactiva
--aunque es listado de libro tambien tiene la validacion de solo listar categorias que estan si o si activas
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
-- ----------------------------------------------------SALA------------------------------------------------------------------------
-- --************************* Autogenerar Codigo Sala***********************************
-- --EJemplo EN SALA es S0001,S0002,S0003,S0004, sucesivamente
-- --Sala tiene un indice unico para no repetirlas 
-- --Esta referenciado con libro (No se permite eliminar)
-- --Tiene un trigger para autogenerar codigo(aunque en el procedimiento lo automaticemos)
go
create procedure sp_RegistrarSala ( --Generar codigo autoomaticamente e hacer demas inserciones --Hay un indice unico para sala
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
go
-- ---------------------------------------------------Editorial-------------------------------------------------------------------
-- --************************* Autogenerar Codigo Editorial*******************************
-- --EJemplo EN editorial es ED0001,ED0002,ED0003,ED0004, sucesivamente
-- --Editorial tiene un índice único
-- --Esta referenciado con libro  (No se permite eliminar)
-- --Tiene un trigger para autogenerar codigo(aunque en el procedimiento lo automaticemos)
create procedure sp_RegistrarEditorial ( --Generar codigo autoomaticamente e hacer demas inserciones --Hay un indice unico para editorial
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
create procedure sp_RegistrarAutor (--Generar codigo autoomaticamente e hacer demas inserciones -Hay un indice unico para Autor
    @Nombres nvarchar(100),
    @Apellidos nvarchar(100),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
) 
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
create  proc sp_EditarAutor(--Trabajo como un booleano
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
-----------------------------------------------------Usuario------------------------------------------------------------
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
go
create procedure sp_RegistrarUsuario(--Hay un indice unico para el nombre completo del usuario 
    --@IDUsuario int,---El id es Identity
    @Nombres varchar(100),--Tiene indice compuesto con Apellidos
    @Apellidos varchar(100),--Tiene indice compuesto con Nombre
    @Ciudad varchar(100),
    @Calle varchar(100),
    @Telefono varchar(20),
    @Correo varchar(100),--Puede ser null
    @Clave varchar(150),
    @Tipo int,--Tipo Persona
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
GO
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
        set @Mensaje = 'Error: No se pudo eliminar el usuario. Intentelo de nuevo'
end

go  
create procedure sp_RegistrarLector(--Hay un indice unico para el nombre completo del usuario 
    --@IDUsuario int,---El id es Identity
    @Nombres varchar(100),--Tiene indice compuesto con Apellidos
    @Apellidos varchar(100),--Tiene indice compuesto con Nombre
    @Edad tinyint, --Tiene un check (0 - 125)
    @Genero bit, --H = 1 o M = 0
    @Escuela nvarchar(100) null,
    @GradoGrupo nvarchar(100) null,
    @Ciudad nvarchar(100),
    @Calle nvarchar(100),
    @Telefono varchar(20),
    @Correo nvarchar(100),--Puede ser null
    @Clave nvarchar(150),
    -- @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
    --@ID_TipoPersona int --ESTARÁ COMO DEFAULT = 1, ES DECIR, COMO LECTOR
    --FechaCreacion date --Esta como default DEFAULT GETDATE()
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Lector WHERE Correo = @Correo)
    begin 
        insert into Lector(Nombres, Apellidos, Edad, Genero, Escuela, GradoGrupo, Ciudad, Calle, Telefono, Correo, Clave, Reestablecer) values 
        (@Nombres, @Apellidos,@Edad, @Genero, @Escuela, @GradoGrupo, @Ciudad, @Calle, @Telefono, @Correo, @Clave,0)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity()
    end 
    else 
     SET @Mensaje = 'El correo del lector ya existe'
end     
go 
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

GO
create proc sp_EditarLector(
    @IdLector int,
    @Nombres varchar(100),--Tiene indice compuesto con Apellidos
    @Apellidos varchar(100),--Tiene indice compuesto con Nombre
    @Edad tinyint, --Tiene un check (0 - 125)
    @Genero bit, --H = 1 o M = 0
    @Escuela nvarchar(100) null,
    @GradoGrupo nvarchar(100) null,
    @Ciudad nvarchar(100),
    @Calle nvarchar(100),
    @Telefono varchar(20),
    @Correo nvarchar(100),--Puede ser null
    --@Clave nvarchar(150),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output

)
as
begin 
    SET @Resultado = 0
    IF NOT EXISTS (SELECT * FROM Lector WHERE Correo = @Correo and IdLector != @IdLector)
    begin 
        update top(1) Lector set 
        Nombres = @Nombres,
        Apellidos  = @Apellidos,
        Edad  = @Edad,
        Genero  = @Genero,
        Escuela  = @Escuela,
        GradoGrupo  = @GradoGrupo,
        Ciudad = @Ciudad,
        Calle = @Calle,
        Telefono = @Telefono,
        Correo = @Correo,
      
        Activo = @Activo
        where IdLector = @IdLector
        set @Resultado = 1
    end 
    else 
        set @Mensaje = 'El correo del lector ya existe'
end
go
create proc sp_EliminarLector( --Trabajo como un booleano
    @IdLector int,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    begin
        delete top(1) from Lector where IDLector = @IdLector
        set @Resultado = 1 --true
    end 
    if(@Resultado != 1)
        set @Mensaje = 'Error: No se pudo eliminar el lector. Intentelo de nuevo'
end

go
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
--               Se podria decir que 
--               Libro esta referenciado a ejemplar, y ejemplar esta referenciado a prestamo
--               Por eso para poder eliminar libro, las foreign key de estos debe ser cascade
-- */
go
create procedure sp_RegistrarLibro(--Nuevo sp que registra igual el libro con su ejemplar  la vez
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
    @Activo bit,--Mejor al registrarlo darlo por default como 1, no tiene sentido regitrar y darlo como inactivo
    @Mensaje varchar(500) output,
    @Resultado int output
    )
as
begin
    begin try 
        declare @idLibro int = 0
        SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
        IF NOT EXISTS (SELECT * FROM Libro WHERE Codigo = @Codigo)
        begin
            begin transaction registrolibro
            insert into Libro(Codigo,Titulo,Paginas,Id_Categoria, Id_Editorial,Id_Sala, Ejemplares, AñoEdicion,Volumen,Observaciones, Activo) values 
            (@Codigo, @Titulo,@Paginas, @IdCategoria, @IdEditorial, @IdSala, @Ejemplares, @AñoEdicion,@Volumen, @Observaciones, 1)
            --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
            SET @Resultado = scope_identity() 
            set @idLibro = SCOPE_IDENTITY()--obtiene el ultimo id que se esta registrando
            
            insert into Ejemplar(ID_Libro, Activo)
            values(@idLibro,1)
            commit transaction registrolibro
        end
        else 
        SET @Mensaje = 'El código del libro ya existe*'
    end try
    begin catch
        set @Resultado = 0
        set @Mensaje = ERROR_MESSAGE()
        rollback transaction registrolibro 
    end catch
    
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
--PARA QUE ESTO FUNCIONE BIEN, ENTONCES EL DETALLEPRESTAMO EN SU COLUMNA ACTIVO
    --CUANDO EL ADMIN ACTUALICE UN PRESTAMO DE ACTIVO A INACTIVO (ES DECIR QUE VA A DEVOLVER EL LIBRO Y YA NO VA ESTAR EN PRESTAMO)
    --EL ACTIVO DE PRESTAMO SE ACTUALIZA A 0 Y ENTONCES AUTOMAICAMENTE TAMBIEN EL ACTIVO DE DETALLE PRESTAMO DEBE SER 0
    --CON ESO VALIDAREMOS ESTA SELECCION PARA ELIMINAR UN LIBRO NO DEBE HABER UN LIBRO RELACIONADO CON EJEMPLAR CUYO EJEMPLAR ESTE RELACIONADO 
    --A UN DETALLEPRESTAMO CUYO A SU VEZ ESTA RELACIONADO CON PRESTAMO Y ESTE ACTIVO DICHO PRESTAMO. ENTONCES PARA PODER ELMINAR
    --NO DEBE ESTAR UN ID CON UN EJEMPLAR EN DETALLE PRESTAMO QUE AUN ESTE ACTIVO.
go
create procedure sp_EliminarLibro(
    @IdLibro int,
    @Mensaje varchar(500) output,
    @Resultado int output
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (select * from Ejemplar ej
    inner join Libro l on l.IDLibro = ej.ID_Libro
    inner join DetallePrestamo dp on dp.IDEjemplar = ej.IDEjemplarLibro
    inner join Prestamo p on p.IdPrestamo = dp.IdPrestamo and p.Activo = 1
    where l.IdLibro = @IdLibro)--No podemos eliminar un Libro si ya esta incluido en una venta
    begin 
        delete top(1) from Libro where IdLibro = @IdLibro

        -- delete top(1) from Prestamo where IdPrestamo = @IdPrestamo
        --Como el ejemplar tiene una relacion con idLibro y un deletecascade se eliminará automaticamente al eliminar el libro
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
     SET @Mensaje = 'El libro se encuentra relacionado a un ejemplar en préstamo'
end 
go
create proc sp_OperacionEjemplarLibro( --servirá para validar que vamos a agregar un ejemplar mas a un Libro registrado
    --@IdLector int,
    @IdLibro int, 
	--@IdEjemplar int,
    @Sumar bit, --si sumar aplica, recibe el valor de 1 y si no aplica recibe el valor de 0
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    set @Resultado = 1
    set @Mensaje = '' 
    --obtener el Ejemplares actual  del Libro de acuerdo al que estamos solicitando
    declare @EjemplaresLibro int = (select Ejemplares from Libro where IdLibro =  @IdLibro)
    BEGIN TRY --capturador de errores
        BEGIN TRANSACTION OPERACION 
        if(@Sumar = 1)
        begin 
            update Libro set Ejemplares = Ejemplares + 1 where IdLibro = @IdLibro
            insert into Ejemplar(ID_Libro, Activo)
            values(@IdLibro,1)
        end 
        else --si la suma no es igual a 1, entonces se va a restar
        --es decir que si es diferente de 1, el Lector lo que esta haciendo es eliminar de la bandeja de carrito este Libro
        begin --resta 1 a la cantidad de carrito de acuerdo al idLector y el idLibro 
       
            update Libro set Ejemplares = Ejemplares - 1 where IdLibro = @IdLibro --y actualiza el Ejemplares con un - 1 
            --Elimina el primer ejemplar de la siguiente consulta siempre y cuando el ejemplar este activo, es decir no este en prestamo

            --antes de eliminar y restar a ejemplares, quizas primero debemos comprobar que exista registro a eliminar, es decir, 
            -- comprobar la siguiente consulta  y ya comprobada de que si hay registro, entonces pasar a eliminar
            DELETE top(1) ej FROM Ejemplar ej
            INNER JOIN Libro l ON ej.ID_Libro = l.IDLibro AND ej.Activo = 1
            WHERE l.IDLibro = @IdLibro
        end 
        --todo lo anterior lo ejecuta temporalmente, pero cuando llega a esta linea lo qu hace es guardar los cambios ya definitivos
        COMMIT TRANSACTION OPERACION --indica que toda operacion que se haya realizado se va a guardar los cambios
    END TRY 
    BEGIN CATCH --en el casi de que exista un error en el proceso
        set @Resultado = 0 --manda un result de 0, envia el mensaje
        set @Mensaje = ERROR_MESSAGE()
        ROLLBACK TRANSACTION OPERCION -- entonces regresa a como estaba antes, como si no hubieramos hecho nada
    END CATCH

end 

go

-- ----------------------------------------------------Busqueda de libro ----------------------------------
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


-------------------------------------- PROCEDIMIENTOS ALMACENADOS PARA EL DASHBOARD -----------------------------------
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
create  proc sp_ReportePrestamos(
    @fechaInicio varchar(40),
    @fechaFin varchar(40),
    @codigo varchar(50)
)
as
begin
    set dateformat dmy; /*Indicamos el formato que queremos si o si*/
    --el formato 103, muestra solo la fecha
    select CONVERT(char(10), p.FechaPrestamo,103) [FechaPrestamo] , CONCAT(lc.Nombres,' ', lc.Apellidos)[Lector],
    l.Titulo[Libro], dp.CantidadEjemplares, p.Activo, dp.Total,l.Codigo
    from Libro l
    inner join Ejemplar ej on ej.ID_Libro = l.IDLibro
    --inner join Libro l on l.IDLibro = ej.ID_Libro
    inner join DetallePrestamo dp on dp.IDEjemplar = ej.IDEjemplarLibro
    inner join Prestamo p on p.IdPrestamo = dp.IdPrestamo
    inner join Lector lc on lc.IdLector = p.Id_Lector
    where CONVERT(date, p.FechaPrestamo) BETWEEN @fechaInicio and @fechaFin 
    and l.Codigo = iif(@codigo = '', l.Codigo, @codigo)
    /*Si el usuario no esta indicando ningun id de transaccion, le decimos que use ese mismo id transaccion del where, pero
    si lo esta indicando, lo use con el @idTransaccion*/
end
go
--------------------------------- Agregar al carrito -----------------------------------------------
go
create proc sp_ExisteCarrito( --devuelve si existe ya un Libro dentro del carrito, validando que no se repita un Libro ya agregado
    @IdLector int, 
    @IdLibro int, 
    @Resultado bit output
)
as 
begin 
    if exists(select * from carrito where IdLector = @IdLector and IdLibro = @IdLibro)
        set @Resultado = 1
    else 
        set @Resultado = 0
end 
go 
create proc sp_OperacionCarrito( --servirá para validar que vamos a agregar un Libro a un carrito
    @IdLector int,
    @IdLibro int, 
	--@IdEjemplar int,
    @Sumar bit, --si sumar aplica, recibe el valor de 1 y si no aplica recibe el valor de 0
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    set @Resultado = 1
    set @Mensaje = '' 
    --si en verdad existe una relacion con id Lector y idLibro en la tabla carrito
    declare @existeCarrito bit =  iif(exists(select * from carrito where IdLector = @IdLector and IdLibro = @IdLibro),1,0)
    declare @EjemplaresLibro int = (select Ejemplares from Libro where IdLibro =  @IdLibro)--obtener el Ejemplares actual  del Libro de acuerdo al que estamos solicitando

    BEGIN TRY --capturador de errores
        BEGIN TRANSACTION OPERACION 
        if(@Sumar = 1)
        begin 
            if(@EjemplaresLibro > 0)--validamos que el Ejemplares del Libro sea mayor a 0 (que si haya Libros disponibles)
            begin 
                if(@existeCarrito = 1) --verificamos entonces que si ya existe en el carrito
                --en el caso de que ya existe, actualiza la cantidad con un mas 1
                    update Carrito set Cantidad = Cantidad + 1 where IdLector = @IdLector and IdLibro = @IdLibro
                else --y si todavia no exista en el carrito, insertamos un nuevo valor a carrito
                    insert into Carrito(IdLector, IdLibro, Cantidad) values (@IdLector, @IdLibro, 1)
                
					update Libro set Ejemplares = Ejemplares - 1 where IdLibro = @IdLibro --si lo anterior fue bien, resta 1 a Ejemplares de la tabla Libro
					--update Ejemplar set Activo = 0 where IDEjemplarLibro = @IdEjemplar --actualiza el activo a 0
            end 
            else --en el caso de que el Ejemplares del Libro no se mayor a 0
            begin --envia el siguiente error
                set @Resultado = 0
                set @Mensaje = 'El libro no cuenta con otro ejemplar disponible'
            end 
        end 
        else --si la suma no es igual a 1
        --es decir que si es diferente de 1, el Lector lo que esta haciendo es eliminar de la bandeja de carrito este Libro
        begin --resta 1 a la cantidad de carrito de acuerdo al idLector y el idLibro 
            update Carrito set Cantidad =  Cantidad - 1 where IdLector = @IdLector  and IdLibro = @IdLibro
            update Libro set Ejemplares = Ejemplares + 1 where IdLibro = @IdLibro --y actualiza el Ejemplares con un + 1 
        end 
        --todo lo anterior lo ejecuta temporalmente, pero cuando llega a esta linea lo qu hace es guardar los cambios ya definitivos
        COMMIT TRANSACTION OPERACION --indica que toda operacion que se haya realizado se va a guardar los cambios
    END TRY 
    BEGIN CATCH --en el casi de que exista un error en el proceso
        set @Resultado = 0 --manda un result de 0, envia el mensaje
        set @Mensaje = ERROR_MESSAGE()
        ROLLBACK TRANSACTION OPERCION -- entonces regresa a como estaba antes, como si no hubieramos hecho nada
    END CATCH

end 
go
---------------------------------------FUNCION PARA OBTENER CARRITO CLIENTE ---------------------------
create function fn_obtenerCarritoLector(
    @idLector int 
)
returns table --una funcion de tipo tabla
as 
return (
    
	SELECT IdLibro, Codigo, DesEjemplar, Titulo, Ejemplares, Cantidad, RutaImagen, NombreImagen
	FROM (
		SELECT l.IdLibro, l.Codigo, ej.IdEjemplarLibro DesEjemplar, l.Titulo, l.Ejemplares, c.Cantidad, l.RutaImagen, l.NombreImagen,
		ROW_NUMBER() OVER (PARTITION BY l.IdLibro ORDER BY ej.IdEjemplarLibro) AS RowNum
		FROM carrito c
		INNER JOIN Libro l ON l.IdLibro = c.IdLibro
		INNER JOIN Ejemplar ej ON ej.Id_libro = l.IdLibro and ej.Activo = 1
		WHERE c.IdLector = @idLector
		) AS tbl
	WHERE tbl.RowNum = 1
	
)

go
---Procedimiento almacenado para eliminar del carrito
create proc sp_EliminarCarrito(
    @IdLector int, 
    @IdLibro int, 
    @Resultado bit output
)
as 
begin 
    set @Resultado = 1 --obtiene la cantidad de este Libro en carrito
    declare @cantidadLibro int  = (select Cantidad from Carrito where IdLector = @IdLector and IdLibro = @IdLibro)

    BEGIN TRY --inicia una transaccion
        BEGIN TRANSACTION OPERACION 
        update Libro set Ejemplares = Ejemplares + @cantidadLibro where IdLibro = @IdLibro --actualiza el stock del Libro + 1
        delete top(1) from Carrito where IdLector = @IdLector and IdLibro = @IdLibro --y eliminamos ese Libro en la tabla carrito
        COMMIT TRANSACTION OPERACION 
    END TRY 
    BEGIN CATCH --si existe un error
        set @Resultado = 0
        ROLLBACK TRANSACTION OPERACION  --reestablece todo lo que hayamos hecho antes
    END CATCH
end

go
------------------------------------------------------- Para Prestamos -------------------------------------------------
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

--creacion de estructura tipo tabla de detallePrestamo
CREATE TYPE [dbo].[EDetalle_Prestamo] AS TABLE(
    [IdEjemplar] int null,
    [CantidadEjemplares] int null,
    [Total] decimal (18, 2) null
)
go
CREATE TYPE [dbo].[Ejemplar_Activo] AS TABLE(
    [IdEjemplar] int null
)
go
create procedure usp_RegistrarPrestamo(
    @Id_Lector int,
    @IdLibro int,
    @TotalLibro int, 
    --@MontoTotal decimal(18,2),
    @DiasDePrestamo int, 
    --@Estado bit,--Es como si dijeramos activo(El 0 significa no Prestamo activo o "DEVUELTO" y
    -- el 1 significa prestamo activo o "No devuelto")
    @Observaciones nvarchar(500),
    @Id_Ejemplar int,--SE AGREGO ESTA COLUMNA PARA PLICAR LA ELIMINACION EN CASCADA EN CASO DE QUE SE ELIMINE EL LIBRO
    @DetallePrestamo [EDetalle_Prestamo] READONLY,--SE USA LA ESTRUCTURA CREADA ANTERIORMENTE
	--@EjemplarActivo [Ejemplar_Activo] READONLY,
    @Resultado bit output,
    @Mensaje varchar(500) output
)
as 
begin 
    begin try 
        declare @idPrestamo int = 0
        set @Resultado = 1
        set @Mensaje = ''
        begin transaction registro
        insert into Prestamo(Id_Lector, TotalLibro,DiasDePrestamo,Observaciones, IDEjemplar )--COLUMNA NUEVA PARA PLICAR EL DELETE CASCADE EN CASO DE QUE SE ELIMINE UN LIBRO
        values(@Id_Lector, @TotalLibro,@DiasDePrestamo, @Observaciones, @Id_Ejemplar )

        set @idPrestamo = SCOPE_IDENTITY()--obtiene el ultimo id que se esta registrando

        insert into DetallePrestamo(IdPrestamo, IDEjemplar, CantidadEjemplares, Total)
        select @idPrestamo, IdEjemplar, CantidadEjemplares, Total from @DetallePrestamo

		--update Ejemplar set Activo = 0 where IDEjemplarLibro = (select IdEjemplar from @EjemplarActivo)
        update Ejemplar set Activo = 0 where IDEjemplarLibro = (select IdEjemplar from @DetallePrestamo)
        update Libro set Ejemplares = Ejemplares - 1 where IdLibro = @IdLibro 

        DELETE FROM CARRITO WHERE IdLector = @Id_Lector
        commit transaction registro 
    end try 
    begin catch --en el caso de algun error, reestablece todo
        set @Resultado = 0
        set @Mensaje = ERROR_MESSAGE()
        rollback transaction registro 
    end catch 
end

go
create procedure sp_RegistrarPrestamo2(--Hay un indice unico para el nombre completo del usuario 
    --@IDUsuario int,---El id es Identity
    @Id_Lector int,
    @TotalLibro int, 
    --@MontoTotal decimal(18,2),
    @DiasDePrestamo int, 
    --@Estado bit,--Es como si dijeramos activo(El 0 significa no Prestamo activo o "DEVUELTO" y
    -- el 1 significa prestamo activo o "No devuelto")
    @Observaciones nvarchar(500),
    @Id_Ejemplar int,--SE AGREGO ESTA LINEA COMO PRUEBA
    --@DetallePrestamo [EDetalle_Prestamo] READONLY,--SE USA LA ESTRUCTURA CREADA ANTERIORMENTE
	--@EjemplarActivo [Ejemplar_Activo] READONLY,
    @Resultado bit output,
    @Mensaje varchar(500) output
    --@ID_TipoPersona int --ESTARÁ COMO DEFAULT = 1, ES DECIR, COMO LECTOR
    --FechaCreacion date --Esta como default DEFAULT GETDATE()
    )
as
begin
    begin try 
        declare @idPrestamo int = 0
        set @Resultado = 1
        set @Mensaje = ''
        begin transaction registro

        insert into Prestamo(Id_Lector, TotalLibro ,DiasDePrestamo,Observaciones )
        values(@Id_Lector, @TotalLibro,@DiasDePrestamo, @Observaciones )

        set @idPrestamo = SCOPE_IDENTITY()--obtiene el ultimo id que se esta registrando

        commit transaction registro 
    end try 
    begin catch --en el caso de algun error, reestablece todo
        set @Resultado = 0  
        set @Mensaje = ERROR_MESSAGE()
        rollback transaction registro 
    end catch 
end 
--EDITAR PRESTAMO 2
go
create procedure sp_EditarPrestamo
(
    @IdPrestamo int,
    @Id_Lector int,
    @TotalLibro int,
    --@Activo bit,
    @FechaPrestamo date,
    --@FechaDevolucion date,
    @DiasDePrestamo int,
    @Observaciones varchar(500),
    --@DetallePrestamo [EDetalle_Prestamo] READONLY,--SE USA LA ESTRUCTURA CREADA ANTERIORMENTE
	--@EjemplarActivo [Ejemplar_Activo] READONLY,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    SET @Resultado = 1 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    SET @Mensaje = '' -- Asignar un valor vacío a la variable @Mensaje

    IF EXISTS (SELECT * FROM Prestamo WHERE IdPrestamo = @IdPrestamo)
    begin 

        -- Convert the input date string to datetime
        DECLARE @FechaPrestamoDatetime datetime
        SET @FechaPrestamoDatetime = CONVERT(datetime, @FechaPrestamo, 3)

        -- DECLARE @FechaDevolucionDatetime datetime
        -- SET @FechaDevolucionDatetime = CONVERT(datetime, @FechaDevolucion, 3)

        update Prestamo set
        ID_Lector = @Id_Lector,
        TotalLibro = @TotalLibro,
        --Activo = @Activo, 
        FechaPrestamo = @FechaPrestamoDatetime,
        --FechaDevolucion = @FechaDevolucionDatetime,
        DiasDePrestamo = @DiasDePrestamo, 
        Observaciones = @Observaciones
        where IdPrestamo = @IdPrestamo
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
        SET @Mensaje = 'El préstamo no pudo ser actualizado'
end 

go
create procedure sp_FinalizarPrestamo --EditarPrestamo2
(
    @IdPrestamo int,
    --@FechaPrestamo date,
    @FechaDevolucion date,
    @Observaciones varchar(500),
    --@DetallePrestamo [EDetalle_Prestamo] READONLY,--SE USA LA ESTRUCTURA CREADA ANTERIORMENTE
	--@EjemplarActivo [Ejemplar_Activo] READONLY,
    @IdEjemplarLibro int,--Este y el siguiente será para actualizar a activo el jemplar y el stock sumar1 al libro correspondiente
    @IdLibro int,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    SET @Resultado = 1 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    SET @Mensaje = '' -- Asignar un valor vacío a la variable @Mensaje

    IF EXISTS (SELECT * FROM Prestamo WHERE IdPrestamo = @IdPrestamo)
    begin 

        -- Convert the input date string to datetime
        --DECLARE @FechaPrestamoDatetime datetime
        --SET @FechaPrestamoDatetime = CONVERT(datetime, @FechaPrestamo, 3)

        DECLARE @FechaDevolucionDatetime datetime
        SET @FechaDevolucionDatetime = CONVERT(datetime, @FechaDevolucion, 3)

        update Prestamo set
        Activo = 0,--Es decir prestamo devuelto 
        --FechaPrestamo = @FechaPrestamoDatetime,
        FechaDevolucion = @FechaDevolucionDatetime,
        Observaciones = @Observaciones
        where IdPrestamo = @IdPrestamo
        update Ejemplar set Activo = 1 where IDEjemplarLibro = @IdEjemplarLibro
        update Libro set Ejemplares = Ejemplares + 1 where IdLibro = @IdLibro
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
        SET @Mensaje = 'Error: No se pudo finalizar el préstamo. Intentelo otra vez.'
end 

go
create proc sp_EliminarPrestamo( --Trabajo como un booleano
    @IdPrestamo int,
    @IdEjemplarLibro int,
    @IdLibro int, 
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    begin
        delete top(1) from Prestamo where IdPrestamo = @IdPrestamo
        update Ejemplar set Activo = 1 where IDEjemplarLibro = @IdEjemplarLibro
        update Libro set Ejemplares = Ejemplares + 1 where IdLibro = @IdLibro

        set @Resultado = 1 --true
        
    end 
    if(@Resultado != 1)
        set @Mensaje = 'Error: No se pudo elimnar el préstamo. Intentelo de nuevo'
end

go
create FUNCTION fn_ListarPrestamos(
    @idLector int
)
RETURNS TABLE 
AS 
RETURN 
(
    SELECT TOP(5000) l.RutaImagen, l.NombreImagen,l.Codigo,l.Titulo, DP.CantidadEjemplares, DP.Total, DP.IdDetallePrestamo FROM DETALLEPrestamo DP
    INNER JOIN Ejemplar ej ON ej.IDEjemplarLibro = DP.IDEjemplar
	INNER JOIN Libro l ON l.IdLibro = ej.ID_Libro
    INNER JOIN Prestamo p ON p.IdPrestamo = DP.IdPrestamo 
    where p.Id_Lector = @idLector order by DP.IdDetallePrestamo DESC
)

go
create proc sp_ActualizarEjemplarActivo(
    @IdLector int, 
    @IdEjemplar int, 
    @Resultado bit output
)
as 
begin 
    set @Resultado = 1 
    BEGIN TRY --inicia una transaccion
        BEGIN TRANSACTION OPERACION 
        update Ejemplar set Activo = 0 where IDEjemplarLibro = @IdEjemplar --actualiza el activo a 0
        --delete top(1) from Carrito where IdLector = @IdLector and IdLibro = @IdLibro --y eliminamos ese Libro en la tabla carrito
        COMMIT TRANSACTION OPERACION 
    END TRY 
    BEGIN CATCH --si existe un error
        set @Resultado = 0
        ROLLBACK TRANSACTION OPERACION  --reestablece todo lo que hayamos hecho antes
    END CATCH
end
go


select * from usuario
select * from prestamo
select * from DetallePrestamo
select * from libro
