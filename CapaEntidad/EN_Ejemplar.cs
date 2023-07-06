using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Ejemplar
    {
        //La tabla ejemplar solo tenemos estos dos valores
       
        public int IdEjemplarLibro { get; set; }
        public int Id_Libro { get; set; }

		//Pero como vamos a hacer una lista de libros con ejemplares, necesitamos
		public int IdLibro { get; set; }
		public string Codigo { get; set; }
		public string Titulo { get; set; }
		//public string Ubicacion { get; set; }
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

		/*Mas propiedades para guardar producto*/
		public string PaginasTexto { get; set; }
		public string EjemplaresTexto { get; set; }
		public string VolumenTexto { get; set; }

		public string Base64 { get; set; }/*Formato base64 en que se van a mostrar las imagenes*/
		public string Extension { get; set; }/*Del tipo de imagen, jpg o png*/
		/*Mas propiedades para guardar producto*/
	}
}
