using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Prestamo
    {
		public int IdPrestamo { get; set; }
		//public int Id_Lector { get; set; }
        public EN_Libro oId_Libro { get; set; }
        public EN_Lector oId_Lector { get; set; }
        public int TotalLibro { get; set; }
		//public decimal MontoTotal { get; set; }
		public bool Activo { get; set; }//o activo
		
		public string FechaPrestamo { get; set; }
		public string FechaDevolucion { get; set; }
		public int DiasDePrestamo { get; set; }
		public string Observaciones { get; set; }
		//public string IdTransaccion { get; set; } /*Para el servicio de paypal y poder identificar la transaccion*/
		public List<EN_DetallePrestamo> oDetallePrestamo { get; set; } /*Checar si colocar el prefijo Id_ o no*/

	}
}
