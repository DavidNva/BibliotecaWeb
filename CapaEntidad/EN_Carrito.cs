using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Carrito
    {
        public int IdCarrito { get; set; }
        public EN_Lector oId_Lector { get; set; }
        public EN_Libro oId_Libro { get; set; }
        public EN_Ejemplar oId_Ejemplar { get; set; }
        public int CantidadEjemplares { get; set; }
    }
    /*
     * IdCarrito int primary key identity,
	    IdCliente int references Cliente(IdCliente),
	    IdProducto int references Producto(IdProducto),
	    Cantidad int--Cuantas unidades para este producto esta seleccionando el cliente
     */
}
