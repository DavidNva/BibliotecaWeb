using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Lector
    {
		public int IdLector { get; set; }
		public string Nombres { get; set; }
		public string Apellidos { get; set; }
		public int  Edad { get; set; }
		public bool Genero { get; set; }
		public string Escuela { get; set; }
		public string GradoGrupo { get; set; }
		public string Ciudad { get; set; }
		public string Calle { get; set; }
		public string Telefono { get; set; }
		public string Correo { get; set; }
		public string Clave { get; set; }
		public string ConfirmarClave { get; set; }
		public bool Reestablecer { get; set; }
		public bool Activo { get; set; }
		/*
         * 	IdLector int not null CONSTRAINT PK_Lector PRIMARY KEY identity,
			Nombres nvarchar(100) not null,
		    Apellidos varchar(100) not null,
		    Edad tinyint not null,--Tiene un check (0-125),
		    Genero Char not null, --H o M
		    Escuela nvarchar(100) null, --Si usamos el SP: Su default es: NINGUNA
		    GradoGrupo nvarchar(100) null, --3ro de secundaria
		    Ciudad nvarchar(60) not null, --Default como --"Zacatlán"
		    Calle nvarchar(100) not null,
		    Telefono varchar(20) not null, 
			Correo nvarchar(100) not null,--No puede ser null porque es con el que se van a registrar
			Clave nvarchar(150) not null, --Contrase�as encriptadas
			Reestablecer bit default 1, -- Por default 1
			Activo bit default 1,
			FechaRegistro datetime default getdate()
         */
	}
}
