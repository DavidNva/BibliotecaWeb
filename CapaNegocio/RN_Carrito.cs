using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class RN_Carrito
    {
        private BD_Carrito objCapaDato = new BD_Carrito(); /*Instancia una clase de la capa datos */

        public bool ExisteCarrito(int idLector, int idLibroEjemplar)
        {
            return objCapaDato.ExisteCarrito(idLector, idLibroEjemplar);
        }

        public bool OperacionCarrito(int idLector, int idEjemplarLibro, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCarrito(idLector, idEjemplarLibro, sumar, out Mensaje);
        }

        public int CantidadEnCarrito(int idLector)
        {
            return objCapaDato.CantidadEnCarrito(idLector);
        }
        //public List<EN_Carrito> ListarProducto(int idLector)
        //{
        //    return objCapaDato.ListarProducto(idLector);
        //}
        public bool EliminarCarrito(int idLector, int idEjemplarLibro)
        {
            return objCapaDato.EliminarCarrito(idLector, idEjemplarLibro);
        }
    }
}
