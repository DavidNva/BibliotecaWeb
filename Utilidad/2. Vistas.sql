use Biblioteca;
--El Orden de ejecucion de scripts es: 
/*
  1. Biblioteca-DDL Y triGGERS 
  2. Vistas
  3. Procedimientos Almacenados
  4. Inserciones  
*/
go
CREATE VIEW V_Sala with encryption
AS
SELECT * FROM SALA;
go
----------------------------------------
CREATE VIEW V_Categoria with encryption
AS
SELECT * FROM Categoria;
go
----------------------------------------
CREATE VIEW V_Editorial with encryption
AS
SELECT * FROM Editorial;
go
----------------------------------------
CREATE VIEW V_Ejemplar with encryption
AS
SELECT * FROM Ejemplar;
go
----------------------------------------
CREATE VIEW V_Libro with encryption
AS
SELECT * FROM Libro;
go
----------------------------------------
CREATE VIEW V_LibroAutor with encryption
AS
SELECT * FROM LibroAutor;
go
---------------------------------------
CREATE VIEW V_HLibrosActualizados with encryption --Tabla de reslpaldo automatizada con trigger 
AS
SELECT * FROM HLibrosActualizados;
go
----------------------------------------
CREATE VIEW V_Prestamo with encryption
AS
SELECT * FROM Prestamo;
go
----------------------------------------
CREATE VIEW V_UsuarioAdmin with encryption
AS
SELECT * FROM Usuario WHERE ID_TipoPersona = 2 OR ID_TipoPersona = 3;
go
CREATE VIEW V_TodosUsuarioAdmin with encryption
AS
SELECT TOP (5000 ) IDUsuario as Codigo, Nombre, A_Paterno as Paterno, A_Materno as Materno, Edad, EscuelaProcedencia AS Escuela, Ciudad, Calle, Telefono AS 'Teléfono',
Email, Observaciones,ID_TipoPersona AS Tipo, Contrasenia AS 'Contraseña', FechaCreacion AS 'Fecha de Registro' FROM Usuario ORDER BY IDUsuario DESC;
go
CREATE VIEW V_Lector with encryption
AS
SELECT * FROM Usuario WHERE ID_TipoPersona = 1;
go
----------------------------------------
CREATE VIEW V_TipoPersona with encryption
AS
SELECT * FROM TipoPersona;
go

----------------------------------------
CREATE VIEW V_LibroCompleto with encryption
AS
SELECT IDLibro,Titulo,CONCAT (Nombre,' ',APELLIDOS) AS Autor , Categoria, Ubicacion AS Ubicación, NumPaginas AS Páginas,NumEjemplar AS Ejemplares,
Volumen AS Vol, Editorial,NumEdicion,AñoEdicion,Sala,Observaciones FROM Libro
INNER JOIN Sala ON Sala.IDSala = Libro.ID_Sala
INNER JOIN Categoria ON Categoria.IDCategoria = Libro.ID_Categoria
INNER JOIN Editorial ON Editorial.IDEditorial = Libro.ID_Editorial
INNER JOIN LibroAutor ON  Libro.IDLibro = LibroAutor.ID_Libro
INNER JOIN Autor ON Autor.IDAutor = LibroAutor.ID_Autor
INNER JOIN Ejemplar ON Ejemplar.ID_Libro = Libro.IDLibro;
go
select * from libro;
----------------------------------------
go
CREATE VIEW V_LibroColumPrincipalesAdmin with encryption
AS
SELECT TOP (5000 ) IDLibro,Titulo,CONCAT (Nombre,' ',APELLIDOS) AS Autor , Categoria, Ubicacion AS Ubicación, NumPaginas AS Páginas,NumEjemplar AS Ejemplares,
Editorial,Sala,Observaciones FROM Libro 
INNER JOIN Sala ON Sala.IDSala = Libro.ID_Sala
INNER JOIN Categoria ON Categoria.IDCategoria = Libro.ID_Categoria
INNER JOIN Editorial ON Editorial.IDEditorial = Libro.ID_Editorial
INNER JOIN LibroAutor ON  Libro.IDLibro = LibroAutor.ID_Libro
INNER JOIN Autor ON Autor.IDAutor = LibroAutor.ID_Autor
INNER JOIN Ejemplar ON Ejemplar.ID_Libro = Libro.IDLibro ORDER BY IDLibro;
go
CREATE VIEW V_LibroColumPrincipales with encryption
AS
SELECT TOP (5000 ) IDLibro as Codigo, Titulo, Ubicacion, NumEdicion as Edición,AñoEdicion as 'Año-Edición',Volumen, NumPaginas as Páginas, Observaciones FROM Libro ORDER BY IDLibro;
go

CREATE VIEW V_UsuarioInicio with encryption
AS
SELECT TOP (5000 ) IDUsuario as Codigo, (Nombre + ' ' + A_Paterno + ' ' + A_Materno )AS Nombre, Edad, EscuelaProcedencia AS Escuela, 
CONCAT(Ciudad, '/',Calle) as Dirección, Telefono AS 'Teléfono', Email, Observaciones, Descripcion AS Cargo, FechaCreacion AS 'Fecha de Registro' FROM Usuario
INNER JOIN TipoPersona ON
TipoPersona.IdTipoPersona = Usuario.ID_TipoPersona WHERE Descripcion = 'Lector' ORDER BY IDUsuario DESC;
go
----------------------------------------
CREATE VIEW V_PrestamoInicio with encryption
AS
SELECT  TOP (5000 ) IDPrestamo as Codigo,CONCAT (Nombre,' ',A_Paterno,' ',A_Materno) AS Usuario, Titulo as 'Titulo del libro',FechaPrestamo  AS 'Fecha Préstamo',
FechaMaxDev AS 'Fecha-Max Devolver', Devuelto as Estado, FechaDevolucion as 'Fecha Devolución',/*
Telefono, CONCAT(Ciudad, '/',Calle) as Dirección, */Prestamo. Observaciones AS 'Observaciones de Préstamo'/*, Descripcion  AS 'Tipo Persona'*/ FROM Prestamo 
INNER JOIN Usuario ON Usuario.IDUsuario = Prestamo.ID_Usuario 
INNER JOIN Ejemplar ON  Prestamo.ID_Ejemplar = Ejemplar.IDEjemplar 
--INNER JOIN TipoPersona ON  TipoPersona.IdTipoPersona = Usuario.ID_TipoPersona 
INNER JOIN Libro ON  Libro.IDLibro = Ejemplar.ID_Libro ORDER BY IDPrestamo DESC;
go

CREATE VIEW V_PrestamoCompleto with encryption
AS
SELECT  TOP (20000) IDPrestamo as Codigo,CONCAT (Nombre,' ',A_Paterno,' ',A_Materno) AS 'Nombre de Usuario', Titulo as 'Libro en Préstamo',FechaPrestamo  AS 'Fecha Préstamo',
Devuelto as Estado, FechaDevolucion as 'Fecha Devolución',FechaMaxDev AS 'Fecha-Max Devolver', Prestamo. Observaciones AS 'Observaciones de Préstamo', ID_Usuario AS Usuario, ID_Ejemplar as Ejemplar,NumEjemplar as Stock/*, Descripcion  AS 'Tipo Persona'*/ FROM Prestamo 
INNER JOIN Usuario ON Usuario.IDUsuario = Prestamo.ID_Usuario 
INNER JOIN Ejemplar ON  Prestamo.ID_Ejemplar = Ejemplar.IDEjemplar 
--INNER JOIN TipoPersona ON  TipoPersona.IdTipoPersona = Usuario.ID_TipoPersona 
INNER JOIN Libro ON  Libro.IDLibro = Ejemplar.ID_Libro ORDER BY IDPrestamo DESC;
go
SELECT * from V_PrestamoCompleto;

go
CREATE  VIEW V_TablaLogin 
AS
SELECT Descripcion,CONCAT(Nombre, ' ',A_Paterno,' ', A_Materno) AS 'NombreUsuario',
CONCAT(A_Paterno,IDUsuario) AS 'Usuario', Contrasenia AS Pass  from Usuario 
INNER JOIN TipoPersona ON TipoPersona.IdTipoPersona = Usuario.ID_TipoPersona;
GO
select * from Usuario
SELECT * FROM V_TablaLogin;
SELECT * FROM V_PrestamoCompleto;
select * from Prestamo;
select * FROM V_LibroColumPrincipalesAdmin;
select * from Prestamo;
SELECT * FROM Libro;

GO
--
CREATE VIEW V_ListadoUsuarios
AS
SELECT TOP(20000) IDUsuario as Codigo, CONCAT(Nombre,' ',A_Paterno,' ',A_Materno) AS [Nombre de Usuario]
FROM Usuario ORDER BY IDUsuario DESC;
--select * from V_Usuario;
GO
CREATE VIEW V_ListadoLibrosEjemplares
AS
SELECT TOP(20000) NumEjemplar as Stock, Titulo , IDEjemplar as Codigo FROM Ejemplar 
INNER JOIN Libro ON Libro.IDLibro = Ejemplar.ID_Libro ORDER BY IDEjemplar DESC;
GO

CREATE VIEW V_ListadoEditoriales
AS
SELECT TOP(5000)  IDEditorial AS Codigo, Editorial FROM Editorial ORDER BY IDEditorial DESC;


go
SELECT  IDUsuario, Nombre,A_Paterno,A_Paterno,Descripcion from Usuario inner join TipoPersona on TipoPersona.IdTipoPersona = Usuario.ID_TipoPersona;