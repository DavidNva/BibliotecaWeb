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
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.FechaPrestamo) || string.IsNullOrWhiteSpace(obj.FechaPrestamo))
            {
                Mensaje = "La fecha del préstamo no puede ser vacio";
            }
            //if (string.IsNullOrEmpty(obj.FechaDevolucion) || string.IsNullOrWhiteSpace(obj.FechaDevolucion))
            //{
            //    Mensaje = "La fecha de devolucion del préstamo no puede ser vacio";
            //}
            else if (obj.DiasDePrestamo == 0)
            {
                Mensaje = "Debe ingresar los dias de préstamo del libro -> El valor debe ser mayor a 0";
            }
            
            else if (obj.oId_Libro.IdLibro == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar un Libro";
            }
            else if (obj.oId_Ejemplar.IdEjemplarLibro == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar un ejemplar disponible para el libro seleccionado. Verifica que el libro cuente con al menos un ejemplare disponible";
            }
            else if (obj.oId_Lector.IdLector == 0)/*Si no ha seleccionado ningun lector*/
            {
                Mensaje = "Debes seleccionar un Lector";
            }
            else if (obj.TotalLibro == 0)
            {
                Mensaje = "Debes ingresar la cantidad de libros a prestar -> El valor debe ser mayor a 0";
            }
            else if (string.IsNullOrEmpty(obj.Observaciones) || string.IsNullOrWhiteSpace(obj.Observaciones))
            {
                Mensaje = "El campo observaciones no puede ser vacio";

            }


            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                //return objCapaDato.Registrar(obj, out Mensaje);
                return objCapaDato.Registrar(obj, DetallePrestamo, /*EjemplarActivo, */ out Mensaje);
            }
            else
            {
                return false;/*No se ha creado la Libro*/
            }
            //return objCapaDato.Registrar(obj, DetallePrestamo, /*EjemplarActivo, */ out Mensaje);
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
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.FechaPrestamo) || string.IsNullOrWhiteSpace(obj.FechaPrestamo))
            {
                Mensaje = "La fecha del préstamo no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.FechaDevolucion) || string.IsNullOrWhiteSpace(obj.FechaDevolucion))
            {
                Mensaje = "La fecha de devolucion del préstamo no puede ser vacio";
            }
            else if (obj.DiasDePrestamo == 0)
            {
                Mensaje = "Debe ingresar los dias de préstamo del libro -> El valor debe ser mayor a 0";
            }
            
            else if (obj.oId_Libro.IdLibro == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar un Libro";
            }
            else if (obj.oId_Ejemplar.IdEjemplarLibro == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar un ejemplar disponible paraa el libro seleccionado";
            }
            else if (obj.oId_Lector.IdLector == 0)/*Si no ha seleccionado ningun lector*/
            {
                Mensaje = "Debes seleccionar un Lector";
            }
            else if (obj.TotalLibro == 0)
            {
                Mensaje = "Debes ingresar la cantidad de libros a prestar -> El valor debe ser mayor a 0";
            }
            else if (string.IsNullOrEmpty(obj.Observaciones) || string.IsNullOrWhiteSpace(obj.Observaciones))
            {
                Mensaje = "El campo observaciones no puede ser vacio";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
           
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