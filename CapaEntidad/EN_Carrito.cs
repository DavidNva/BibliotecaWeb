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
        public EN_Lector oId_Cliente { get; set; }
        public EN_Libro oId_Producto { get; set; }
        public int Cantidad { get; set; }
    }
    /*
     * IdCarrito int primary key identity,
	    IdCliente int references Cliente(IdCliente),
	    IdProducto int references Producto(IdProducto),
	    Cantidad int--Cuantas unidades para este producto esta seleccionando el cliente
     */
}
