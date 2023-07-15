using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_DetallePrestamo
    {
        public int IdDetallePrestamo { get; set; }
        public int IdPrestamo { get; set; }
        public EN_Ejemplar oId_Ejemplar { get; set; }//Antes este era EN_Libro: oId_Libro
        public EN_Libro oId_Libro { get; set; }//Antes este era EN_Libro: oId_Libro //SOLO SE USARÁ PARA EL ListadoPrestamosHechosPorEseLector
        public int CantidadEjemplares { get; set; }
        public decimal Total { get; set; }//Este debe ser int
        //public string IdTransaccion { get; set; } /*Para el servicio de paypal y poder identificar la transaccion*/
    }
}
