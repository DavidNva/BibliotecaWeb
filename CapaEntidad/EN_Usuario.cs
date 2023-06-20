using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class EN_Usuario
	{
		public int IdUsuario { get; set; }
		public string Nombres { get; set; }
		public string Apellidos { get; set; }
		public string Ciudad { get; set; }
		public string Calle { get; set; }
		public string Telefono { get; set; }
		public string Correo { get; set; }
		public int Tipo { get; set; }
		//public EN_TipoPersona oId_TipoPersona { get; set; }
		public string Clave { get; set; }
		public bool Reestablecer { get; set; }
		public bool Activo { get; set; }
	}
	/*
     * IdUsuario int primary key identity,
	Nombres varchar(100),
	Apellidos varchar(100),
	Correo varchar(100),
	Clave varchar(150), --Contrase�as encriptadas
	Reestablecer bit default 1, -- Por default 1
	Activo bit default 1,
	FechaRegistro datetime default getdate()
     */
}
