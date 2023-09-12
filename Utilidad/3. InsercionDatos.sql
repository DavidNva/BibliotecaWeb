use BibliotecaWeb4;
--El Orden de ejecucion de scripts es: 
/*
  1. Biblioteca-DDL Y triGGERS 
  2. Vistas
  3. Procedimientos Almacenados
  4. Inserciones  
*/
go
--Insertarción de  2 registros a cada tabla

INSERT INTO TipoPersona(Descripcion)--Es id es identity
VALUES ('Administrador'),
       ('Empleado');
go
sp_RegistrarUsuario 'David','Nava Garcia','Zacatlan','Luis Cabrera','7649726682','david.nava.garcia4@gmail.com','ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae',1,1,'',1
go 
exec sp_RegistrarSala 'GENERAL',1,'',1
exec sp_RegistrarSala 'INFANTIL',1,'',1
go

