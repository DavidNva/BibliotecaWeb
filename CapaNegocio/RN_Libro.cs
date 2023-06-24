using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class RN_Libro
    {
        private BD_Libro objCapaDato = new BD_Libro(); /*Instancia una clase de la capa datos */

        public List<EN_Libro> Listar() /*Usa una clase de la capa entidad*/
        {
            return objCapaDato.Listar();/*Retorna el metodo listar de la instancia de la capa Datos*/
        }

        public int Registrar(EN_Libro obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.IdLibro) || string.IsNullOrWhiteSpace(obj.IdLibro))
            {
                Mensaje = "La código del Libro no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Titulo) || string.IsNullOrWhiteSpace(obj.Titulo))
            {
                Mensaje = "El titulo del Libro no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Ubicacion) || string.IsNullOrWhiteSpace(obj.Ubicacion))
            {
                Mensaje = "La ubicacion del Libro no puede ser vacio";
            }
            else if (obj.oId_Categoria.IdCategoria == "0")/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar una categoria";
            }
            else if (obj.oId_Editorial.IdEditorial == "0")/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar una editorial";
            }
            else if (obj.oId_Sala.IdSala == "0")/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar una sala";
            }
            else if (obj.Ejemplares == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debe ingresar el número de ejemplares del Libro";
            }
            else if (string.IsNullOrEmpty(obj.AñoEdicion) || string.IsNullOrWhiteSpace(obj.AñoEdicion))
            {
                Mensaje = "El año de edicion del Libro no puede ser vacio";
            }
            else if (obj.Volumen == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debe ingresar el volúmen del Libro";
            }
            else if (string.IsNullOrEmpty(obj.Observaciones) || string.IsNullOrWhiteSpace(obj.Observaciones))
            {
                Mensaje = "Las observaciones del Libro no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;/*No se ha creado la categoria*/
            }

        }

        public bool Editar(EN_Libro obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.IdLibro) || string.IsNullOrWhiteSpace(obj.IdLibro))
            {
                Mensaje = "La código del Libro no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Titulo) || string.IsNullOrWhiteSpace(obj.Titulo))
            {
                Mensaje = "El titulo del Libro no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Ubicacion) || string.IsNullOrWhiteSpace(obj.Ubicacion))
            {
                Mensaje = "La ubicacion del Libro no puede ser vacio";
            }
            else if (obj.oId_Categoria.IdCategoria == "0")/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar una categoria";
            }
            else if (obj.oId_Editorial.IdEditorial == "0")/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar una editorial";
            }
            else if (obj.oId_Sala.IdSala == "0")/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar una sala";
            }
            else if (obj.Ejemplares == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debe ingresar el número de ejemplares del Libro";
            }
            else if (string.IsNullOrEmpty(obj.AñoEdicion) || string.IsNullOrWhiteSpace(obj.AñoEdicion))
            {
                Mensaje = "El año de edicion del Libro no puede ser vacio";
            }
            else if (obj.Volumen == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debe ingresar el volúmen del Libro";
            }
            else if (string.IsNullOrEmpty(obj.Observaciones) || string.IsNullOrWhiteSpace(obj.Observaciones))
            {
                Mensaje = "Las observaciones del Libro no puede ser vacio";
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
        public bool GuardarDatosImagen(EN_Libro obj, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        }
        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}
