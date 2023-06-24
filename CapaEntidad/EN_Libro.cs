using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Libro
    {
		public string IdLibro { get; set; }
		public string Titulo { get; set; }
		public string Ubicacion { get; set; }
		public int Paginas { get; set; }
		public EN_Categoria oId_Categoria { get; set; }
		public EN_Editorial oId_Editorial { get; set; }
		public EN_Sala oId_Sala { get; set; }
		public int Ejemplares { get; set; }
		//public string NumEdicion { get; set; }
		public string AñoEdicion { get; set; }
		public int Volumen { get; set; }
		public string RutaImagen { get; set; }
		public string NombreImagen { get; set; }
		public string Observaciones { get; set; }
		public bool Activo { get; set; }
	}
}
