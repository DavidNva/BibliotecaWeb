using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class RN_Prestamo
    {
        private BD_Prestamo objCapaDato = new BD_Prestamo(); /*Instancia una clase de la capa datos */

        public bool Registrar(EN_Prestamo obj, DataTable DetallePrestamo,/* DataTable EjemplarActivo,*/ out string Mensaje)
        {
            return objCapaDato.Registrar(obj, DetallePrestamo, /*EjemplarActivo, */ out Mensaje);
        }
        public int RegistrarPrestamo2(EN_Prestamo obj, out string Mensaje)
        {
            return objCapaDato.RegistrarPrestamo2(obj, out Mensaje);
        }

        public List<EN_DetallePrestamo> ListarPrestamos(int idLector)
        {
            return objCapaDato.ListarPrestamos(idLector);
        }

        public List<EN_Prestamo> ListarPrestamosCompleto()
        {
            return objCapaDato.ListarPrestamosCompleto();
        }

        public bool Editar(EN_Prestamo obj, out string Mensaje)
        {
            return objCapaDato.Editar(obj, out Mensaje);
        }

        //public bool Eliminar(int id, out string Mensaje)
        //{
        //    return objCapaDato.Eliminar(id, out Mensaje);
        //}
        public bool Eliminar(int id, int idEjemplarLibro, int idLibro, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, idEjemplarLibro, idLibro, out Mensaje);
        }
        //public List<EN_DetallePrestamo> ListarPrestamos(int idLector)
        //{
        //    return objCapaDato.ListarPrestamos(idLector);
        //}
    }
}