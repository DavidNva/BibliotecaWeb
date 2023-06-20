use Biblioteca;
--El Orden de ejecucion de scripts es: 
/*
  1. Biblioteca-DDL Y triGGERS 
  2. Vistas
  3. Procedimientos Almacenados
  4. Inserciones  
*/
go
--Insertarción de  2 registros a cada tabla

INSERT INTO Sala(Sala) VALUES('GENERAL');---Tiene trigger para autogenerar codigo
INSERT INTO Sala(IDSala,Sala) VALUES('S0005','INFANTIL');--Tambien podemos indicar el codigo pero no lo tomara en cuenta, sino al trigger
go 
--------------------------------------------------------------------------------
INSERT INTO Categoria(Categoria)VALUES('CONSULTA');--Tiene trigger para autogenerar codigo
INSERT INTO Categoria(Categoria)VALUES('GENERALIDADES');--0-99
INSERT INTO Categoria(Categoria)VALUES('FILOSOFÍA Y PSICOLOGÍA');--100-199
INSERT INTO Categoria(Categoria)VALUES('RELIGIONES');--200-299
INSERT INTO Categoria(Categoria)VALUES('CIENCIAS SOCIALES');--300-399
INSERT INTO Categoria(Categoria)VALUES('CIENCIAS PURAS');--500-599
INSERT INTO Categoria(Categoria)VALUES('CIENCIAS APLICADAS');--600-699
INSERT INTO Categoria(Categoria)VALUES('BELLAS ARTES');--700-799
INSERT INTO Categoria(IDCategoria,Categoria) VALUES('C0001','LITERATURA');---Aunque podemos declarar el codigo COMO EN EL CASO ANTERIOR: si este ya esta escrito
--no nos dara error, con el trigger automaticamente generamos un codigo. Es decir ese C0001, NO LO TOMA EN CUENTA, el trigger respeta el orden que lleva en codigo AUTOGENERADO
--y si esta en C0099 PUES a esta insercion le pondrá el codigo de C0100
INSERT INTO Categoria(Categoria)VALUES('GEOGRAFÍA E HISTORIA');--900-999
INSERT INTO Categoria(Categoria)VALUES('NOVELAS');--N
INSERT INTO Categoria(Categoria)VALUES('POESÍA');--POE
INSERT INTO Categoria(Categoria)VALUES('CUENTOS');--CUE
INSERT INTO Categoria(Categoria)VALUES('TEATRO');--TEA
INSERT INTO Categoria(Categoria)VALUES('BIOGRAFÍAS');--BIO
INSERT INTO Categoria(Categoria)VALUES('MÉXICO');--MEX
INSERT INTO Categoria(Categoria)VALUES('PUEBLA');--PUE
INSERT INTO Categoria(Categoria)VALUES('LIBROS DONADOS');--PD
INSERT INTO Categoria(Categoria)VALUES('LIBROS INFANTILES');--LIF
--------------------------------------------------------------------------------
go --Lo misma aplica para editorial
INSERT INTO Editorial(Editorial) VALUES('LAROUSSE');--Tiene trigger para autogenerar codigo
INSERT INTO Editorial(Editorial) VALUES('APOLO');
INSERT INTO Editorial(Editorial) VALUES('CULTURAL');
INSERT INTO Editorial(Editorial) VALUES('CONACULTA');
INSERT INTO Editorial(IDEditorial,Editorial)
VALUES('ED0001','BIBLIOTECA MEXICANA');--Aplica lo del comentario anterior (respeta el trigger y orden actual)
INSERT INTO Editorial(Editorial) VALUES('LIBROS DEL RINCÓN');
INSERT INTO Editorial(Editorial) VALUES('OCEANO');
INSERT INTO Editorial(Editorial) VALUES('MUNDI-PRENSE');
INSERT INTO Editorial(Editorial) VALUES('TURNER LIBROS');
INSERT INTO Editorial(Editorial) VALUES('CIEN DEL MUNDO');
INSERT INTO Editorial(Editorial) VALUES('NOSTRA EDICIONES');
INSERT INTO Editorial(Editorial) VALUES('ALAS Y RAÍCES');
INSERT INTO Editorial(Editorial) VALUES('ANAGRAMA');
INSERT INTO Editorial(Editorial) VALUES('AMAQUEMECAN');
INSERT INTO Editorial(Editorial) VALUES('ANAM');
INSERT INTO Editorial(Editorial) VALUES('CAL Y ARENA');
INSERT INTO Editorial(Editorial) VALUES('FONDO DE CULTURA ECONÓMICA');
INSERT INTO Editorial(Editorial) VALUES('OCEANO DE MÉXICO');
INSERT INTO Editorial(Editorial) VALUES('GRUPO EDITORIAL PATRIA');
INSERT INTO Editorial(Editorial) VALUES('EDICIONES DE LA U');

--------------------------------------------------------------------------------
go
INSERT INTO Libro(IDLibro,Titulo,Ubicacion,NumEdicion,AñoEdicion,Volumen,NumPaginas,Observaciones,ID_Sala,ID_Categoria,ID_Editorial)
VALUES('036C654_3', 'MATEMÁTICAS, FÍSICA Y COMPUTACIÓN','C','PRIMERA EDICIÓN','2011',1,102,'EN PERFECTO ESTADO','S0001','C0001','ED0002'),
      ('972003D52_1', 'DICCIONARIO ENCICLOPÉDICO DE MÉXICO A-B','C','PRIMERA EDICIÓN','2006',1,89,'MALTRATADO EN LA PORTADA','S0001','C0001','ED0001'),
      ('028.9P47', 'COMO UNA NOVELA','0-99','TERCERA EDICIÓN','1993',1,169,'EN PERFECTO ESTADO','S0001','C0002','ED0013'),
      ('027.009S42', 'EL LIBRO DE LAS BIBLIOTECAS','0-99','PRIMERA EDICIÓN','2011',1,69,'EN PERFECTO ESTADO','S0001','C0002','ED0014'),
      ('121.686R52', 'DEL TEXTO A LA EDICIÓN: ENSAYOS DE LA HERMENÉUTICA II','100-199','PRIMERA EDICIÓN','2000',1,376,'MALTRATADO EN LA PORTADA','S0001','C0003','ED0016'),
      ('144H86', 'HUMANISTAS MEXICANOS DEL SIGLO XVI','100-199','SEGUNDA EDICIÓN','1994',1,169,'EN BUEN ESTADO','S0001','C0003','ED0002'),
      ('155.33G65', 'NIÑO O NIÑA: LAS DIFERENCIAS SEXUALES','100-199','PRIMERA EDICIÓN','2006',1,176,'MALTRATADO EN LA PORTADA','S0001','C0003','ED0001'),
      ('296.162Z63', 'ZOHAR: LIBRO DEL ESPLENDOR','200-299','TERCERA EDICIÓN','2010',1,189,'EN PERFECTO ESTADO','S0001','C0004','ED0010'),
      ('299.79281P651', 'POPOL VUH: LAS ANTIGUAS HISTORIAS DEL QUICHÉ','200-299','TRIGESIMA TERCERA EDICIÓN','2008',1,169,'EN PERFECTO ESTADO','S0001','C0004','ED0016'),
      ('305.42P463', 'MI HISTORIA DE LAS MUJERES','300-399','PRIMERA EDICIÓN','2011',1,247,'EN BUEN ESTADO','S0001','C0005','ED0016'),
      ('305.56M57', 'LA MISERIA DEL MUNDO','300-399','PRIMERA EDICIÓN','1999',1,204,'EN PERFECTO ESTADO','S0001','C0005','ED0016'),
      ('577.O6229', 'EL ORIGEN DE LA VIDA','500-599','PRIMERA EDICIÓN','2004',1,158,'EN PERFECTO ESTADO','S0001','C0006','ED0017'),
      ('513.B336', 'ARITMÉTICA','500-599','PRIMERA EDICIÓN','2007',1,89,'LIGERAMENTE MALTRATADO EN LAS ESQUINAS','S0001','C0006','ED0018'),
      ('621.3A734', 'ELECTRICIDAD BÁSICA','600-699','PRIMERA EDICIÓN','2011',1,154,'EN PERFECTO ESTADO','S0001','C0007','ED0020'),
      ('629.130972R84', '100 AÑOS DE LA AVIACIÓN EN MÉXICO','600-699','PRIMERA EDICIÓN','2006',1,169,'EN PERFECTO ESTADO','S0001','C0007','ED0004');
/*INSERT INTO Libro(IDLibro,Titulo,Ubicacion,NumEdicion,AñoEdicion,NumPaginas,Observaciones,ID_Categoria,ID_Editorial)
VALUES('PruebaSV', 'Sala y Volumen','0-100','SEGUNDA EDICIÓN','2008',169,'EN PERFECTO ESTADO','C0003','ED0003');*/
go 

select * from Categoria order by IDCategoria;
select * from Editorial ORder by IDEditorial;
select * from Sala;
SELECT * from Autor order by IDAutor;
--------------------------------------------------------------------------------
INSERT INTO Autor(Nombre,Apellidos) VALUES('SECRETARIA DE CULTURA','');--Tiene trigger para autogenerar codigo
INSERT INTO Autor(Nombre,Apellidos) VALUES('HUMBERTO','MUSACCHIO');
INSERT INTO Autor(Nombre,Apellidos) VALUES('DANIEL','PENNAC');
INSERT INTO Autor(Nombre,Apellidos) VALUES('MAUREN','SAWA');
INSERT INTO Autor(IDAutor,Nombre,Apellidos) VALUES('A0001', 'PAUL','RICOEUR');--Podemo igual ingresar como normalmente lo haciamos, pero aplica lo mismo del trigger
INSERT INTO Autor(Nombre,Apellidos) VALUES('GABRIEL','MENDEZ PLANCARTE');
INSERT INTO Autor(Nombre,Apellidos) VALUES('LUIS','GONZÁLEZ DE ALBA');
INSERT INTO Autor(Nombre,Apellidos) VALUES('ESTHER','COHEN');
INSERT INTO Autor(Nombre,Apellidos) VALUES('ADRIÁN','RECINOS');
INSERT INTO Autor(Nombre,Apellidos) VALUES('MICHELLE','PERROT');
INSERT INTO Autor(Nombre,Apellidos) VALUES('PIERRE','BOURDIEU');
INSERT INTO Autor(Nombre,Apellidos) VALUES('ALEXANDER','IVÁNOVICH OPARIN');
INSERT INTO Autor(Nombre,Apellidos) VALUES('AURELIO','BALDOR');
INSERT INTO Autor(Nombre,Apellidos) VALUES('DAVID','ARBOLEDAS BRIGUEGA');
INSERT INTO Autor(Nombre,Apellidos) VALUES('MANUEL','RUIZ ROMERO');
INSERT INTO Autor(Nombre,Apellidos) VALUES('ALICIA','MONTEMAYOR GARCÍA');
INSERT INTO Autor(Nombre,Apellidos) VALUES('JOHANNES','NEURATH');
INSERT INTO Autor(Nombre,Apellidos) VALUES('EL REY LEAR','SHAKESPEARE');
INSERT INTO Autor(Nombre,Apellidos) VALUES('JOSEPH','CONRAD');
INSERT INTO Autor(Nombre,Apellidos) VALUES('FABIOLA','GARCÍA RUBIO');
INSERT INTO Autor(Nombre,Apellidos) VALUES('ROBERTO','GARCIA MOLL');
INSERT INTO Autor(Nombre,Apellidos) VALUES('RAFAEL','CABOS');
INSERT INTO Autor(Nombre,Apellidos) VALUES('GABRIEL','GARCÍA MÁRQUEZ');
INSERT INTO Autor(Nombre,Apellidos) VALUES('JAVIER','SIERRA');
INSERT INTO Autor(Nombre,Apellidos) VALUES('GERARDO','LINO');
INSERT INTO Autor(Nombre,Apellidos) VALUES('RAQUEL','HOYOS GÚZMAN');
INSERT INTO Autor(Nombre,Apellidos) VALUES('EMILIO','CARBALLIDO');
INSERT INTO Autor(Nombre,Apellidos) VALUES('JUAN','RUIZ DE ALARCÓN');
INSERT INTO Autor(Nombre,Apellidos) VALUES('ÉDGAR','ADRIÁN MORA');
INSERT INTO Autor(Nombre,Apellidos) VALUES('ALEJANDRO','TORRES');
INSERT INTO Autor(Nombre,Apellidos) VALUES('RODOLFO','STAVENHAGEN');
INSERT INTO Autor(Nombre,Apellidos) VALUES('ANA','MARTOS');
INSERT INTO Autor(Nombre,Apellidos) VALUES('IVAR','DA COLL');
INSERT INTO Autor(Nombre,Apellidos) VALUES('LAURA','VARELA');
--------------------------------------------------------------------------------
go 
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('027.009S42','A0004');--Tiene trigger para autogenerar codigo
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('028.9P47','A0003');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('036C654_3','A0001');
INSERT INTO LibroAutor(IDLibroAutor,ID_Libro,ID_Autor) VALUES('LA0001', '121.686R52','A0005');--Aplica lo mismo con lo del trigger
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('144H86','A0006');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('155.33G65','A0007');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('296.162Z63','A0008');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('299.79281P651','A0009');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('305.42P463','A0010');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('305.56M57','A0011');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('513.B336','A0013');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('577.O6229','A0012');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('621.3A734','A0014');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('629.130972R84','A0015');
INSERT INTO LibroAutor(ID_Libro,ID_Autor)VALUES('972003D52_1','A0002');
go
-----sp_RegistrarLibroAutor '027.009S42',A0003;--Podemos registrar de igual forma con procedimiento
-------------------------------------------------------------------------------- 
INSERT INTO TipoPersona(Descripcion)--Es id es identity
VALUES ('Administrador'),
       ('Empleado');
go 
INSERT INTO Usuario(Nombre, A_Paterno, A_Materno, Edad, EscuelaProcedencia, Calle, Telefono, Email, ID_TipoPersona,Contrasenia)--El ID usuario es identity
VALUES('DAVID', 'NAVA','GARCIA',20,'ITSSNP','ANGEL WENCESLAO CABRERA','7641291840','david.nava.garcia4@gmail.com',3,'dav01gar01'),--La Ciudad por DEFAULT es Zacatlán
      ('LUIS', 'CABRERA','LOBATO',35,'LUIS CABRERA LOBATO','JOSE MARIA MORELOS','7641291840','luiscabreralobato@gmail.com',2,'luiscl02'),--Las observaciones por default es NINGUNA 
      ('JUAN ADOLFO', 'LOPEZ','CASTILLA',21,'LUIS CABRERA LOBATO','RUIZ  CORTINEZ','7971171350','juangutierrezsoto@gmail.com',2,'adolfo03');--La fecha de creacion por default es: GETDATE();
go 
INSERT INTO Usuario(Nombre, A_Paterno, A_Materno, Edad, EscuelaProcedencia, Ciudad, Calle, Telefono, Email)
VALUES('DIANA', 'NAVA','JUAREZ',17,'CENTRO ESCOLAR','ZACATLÁN','MIGUEL HIDALGO','123456789','diananava@gmail.com'),--El tipo de Persona por default es 1 = Lector,
      ('ANGEL', 'SANDOVAL','GARCIA',39,null,'CHIGNAHUAPAN','2 DE ABRIL','123456789',null),--La escuela y el Email pueden ser Null
      ('JAIR', 'MARQUEZ','LUNA',19,null,'ZACATLÁN','20 DE NOVIEMBRE','123456789',null),--Si usamos el procedmiento colocará a ESCUELA  = NINGUNA;
      ('VANESSA', 'MARQUEZ','LUNA',26,'BUAP','ZACATLÁN','JOSE CLEMENTE OROZCO','123456789','vanesaluna@gmail.com'),
      ('MANUEL', 'CASTILLO','GUTIERREZ',14,null,'ZACATLÁN','2 DE ABRIL','123456789',null),
      ('JONATHAN', 'SANTIAGO','JUAREZ',9,'PRIMARIA VENUZTIANO CARRANZA','CHIGNAHUAPAN','LUIS CABRERA LOBATO','123456789',null),
      ('DAVID', 'RAMIREZ','ALTAMIRANO',15,null,'ZACATLÁN','5 DE MAYO','123456789','davidramirez@hotmail.com'),
      ('KAREN', 'GUTIERREZ','VILLA',13,null,'ZACATLÁN','MANUEL DOBLADO','123456789','karengutierrez@gmail.com');
go
    --No se puede repetir un nombre completo (hay un indice unico)                       
------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(20,'027.009S42');--Tiene trigger para autogenerar codigo
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(25,'028.9P47');
INSERT INTO Ejemplar(IDEjemplar,NumEjemplar,ID_Libro) VALUES('EJ0001',30,'036C654_3');--Podemos ingresar de forma normal, pero aplica igual  lo del trigger
INSERT INTO Ejemplar(IDEjemplar,NumEjemplar,ID_Libro) VALUES('EJ0001',1,'121.686R52');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(1,'144H86');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(1,'155.33G65');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(2,'296.162Z63');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(3,'299.79281P651');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(3,'305.42P463');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(1,'305.56M57');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(1,'513.B336');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(1,'577.O6229');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(2,'621.3A734');
INSERT INTO Ejemplar(NumEjemplar,ID_Libro) VALUES(1,'972003D52_1');
go
----------------------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO Prestamo(ID_Usuario,ID_Ejemplar,FechaMaxDev, Observaciones                                             )--Es identity
VALUES(1,'EJ0001','2022/08/07','EL LIBRO SE ENTREGÓ EN PERFECTO ESTADO'),-- El 1 y 2 al inicio pertenecen al usuario
      (4,'EJ0003','07/08/2022','LA PORTADA DEL LIBRO SE ENTREGÓ UN POCO MALTRATADA'),
      (6,'EJ0002','02/08/2022','EL LIBRO SE ENCUENTRA EN BUEN ESTADO');  --Tiene un TRIGGER PARA reducir 1 a NumEjemplar de la tabla ejemplar al hacer una insercion 
--Tiene un DEFAULT  en FechaPrestamo = GetDate();
--Tiene un DEFAULT  en Devuelto = 0; --Es decir devuelte = false
--Tiene un DEFAULT  en FechaDevolucion = NULL; En teoria al insertar pues no puede haber un fecha en que ya lo devolvió
--Las inserciones de usuarios pueden ser muchos
--pero las inserciones de prestamos debe ser uno a uno, para que aplique el trigger de restar un Numejemplar a la tabla ejemplar
--porque si lo hacemos por mucho, solo restara uno, aun cuando hayamos hecho muchos prestamos
--    Ahora, la forma de poder hacer prestamo de una insercion de muchos es mientras prestamo de distinto ejemplar (como en el ejemplo de insercion: Ej0001 y EJ0003) POR ESO:
--si queremos hacer un prestamo de un ejemplar al mismo tiempo, esto solo descontara uno es alli donde necesariamente se debe hacer de unno en uno
--    ahora si estamos prestando distintos ejemplares, pues si podemos ingresarlos por monton
go
SELECT * FROM Usuario;
select * from libro;
select *from Ejemplar;
select * from Prestamo;
SELECT * from Usuario;
