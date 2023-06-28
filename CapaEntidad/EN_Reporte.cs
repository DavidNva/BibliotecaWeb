using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Reporte
    {
        public string FechaPrestamo { get; set; }

        public string Lector { get; set; }

        public string Libro { get; set; }

        //public decimal Precio { get; set; }

        public int CantidadEjemplares { get; set; }
        public bool Estado { get; set; }

        public decimal Total { get; set; }

        public string Codigo { get; set; } //El IdLibro
    }
}
