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

        //public bool ExisteCarrito(int idLector, int idLibroEjemplar)
        //{
        //    return objCapaDato.ExisteCarrito(idLector, idLibroEjemplar);
        //}
        public bool ExisteCarrito(int idLector, int idLibro)
        {
            return objCapaDato.ExisteCarrito(idLector, idLibro); 
        }
        public bool ExisteEjemplarInactivo(int idLector, int idLibro, int idEjemplar)
        {
            return objCapaDato.ExisteEjemplarInactivo(idLector, idLibro, idEjemplar);
        }
        //public bool OperacionCarrito(int idLector,int idLibros ,int idEjemplarLibro, bool sumar, out string Mensaje)
        //{
        //    return objCapaDato.OperacionCarrito(idLector, idLibros, idEjemplarLibro, sumar, out Mensaje);
        //}
        public bool OperacionCarrito(int idLector, int idLibro, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCarrito(idLector, idLibro, sumar, out Mensaje);
        }
        public int CantidadEnCarrito(int idLector)
        {
            return objCapaDato.CantidadEnCarrito(idLector);
        }
        public List<EN_Carrito> ListarLibro(int idLector)
        {
            return objCapaDato.ListarLibro(idLector);
        }
        public bool EliminarCarrito(int idLector, int idLibro)
        {
            return objCapaDato.EliminarCarrito(idLector, idLibro);
        }
    }
}
