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

        public bool OperacionEjemplarLibro(int idLibro, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionEjemplarLibro(idLibro, sumar, out Mensaje);
        }

        public int Registrar(EN_Libro obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.Codigo) || string.IsNullOrWhiteSpace(obj.Codigo))
            {
                Mensaje = "El código del Libro no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.Titulo) || string.IsNullOrWhiteSpace(obj.Titulo))
            {
                Mensaje = "El título del Libro no puede ser vacio";
            }
            //else if (string.IsNullOrEmpty(obj.Ubicacion) || string.IsNullOrWhiteSpace(obj.Ubicacion))
            //{
            //    Mensaje = "La ubicación del Libro no puede ser vacio";

            //}
            else if (obj.Paginas == 0)
            {
                Mensaje = "Debe ingresar las paginas del libro -> El valor debe ser mayor a 0";
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
            else if (obj.Ejemplares == 0)
            {
                Mensaje = "Debe ingresar los ejemplares del libro -> El valor debe ser mayor a 0";
            }
            else if (string.IsNullOrEmpty(obj.AñoEdicion) || string.IsNullOrWhiteSpace(obj.AñoEdicion))
            {
                Mensaje = "El año de edición del Libro no puede ser vacio";

            }
            else if (obj.Volumen == 0)
            {
                Mensaje = "Debe ingresar el volúmen del libro en número -> El valor debe ser mayor a 0";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;/*No se ha creado la Libro*/
            }

        }


        public bool Editar(EN_Libro obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.Codigo) || string.IsNullOrWhiteSpace(obj.Codigo))
            {
                Mensaje = "El código del Libro no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.Titulo) || string.IsNullOrWhiteSpace(obj.Titulo))
            {
                Mensaje = "El título del Libro no puede ser vacio";
            }
            //else if (string.IsNullOrEmpty(obj.Ubicacion) || string.IsNullOrWhiteSpace(obj.Ubicacion))
            //{
            //    Mensaje = "La ubicación del Libro no puede ser vacio";

            //}
            else if (obj.Paginas == 0)
            {
                Mensaje = "Debe ingresar las paginas del libro";
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
            else if (obj.Ejemplares == 0)
            {
                Mensaje = "Debe ingresar los ejemplares del libro";
            }
            else if (string.IsNullOrEmpty(obj.AñoEdicion) || string.IsNullOrWhiteSpace(obj.AñoEdicion))
            {
                Mensaje = "El año de edición del Libro no puede ser vacio";

            }
            else if (obj.Volumen == 0)
            {
                Mensaje = "Debe ingresar el volúmen del libro en número";
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
        public bool GuardarDatosImagen(EN_Libro obj, out string mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out mensaje);
        }
        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
        //private BD_Libro objCapaDato = new BD_Libro(); /*Instancia una clase de la capa datos */

        //public List<EN_Libro> Listar() /*Usa una clase de la capa entidad*/
        //{
        //    return objCapaDato.Listar();/*Retorna el metodo listar de la instancia de la capa Datos*/
        //}

        //public string Registrar(EN_Libro obj, out string Mensaje)
        //{
        //    Mensaje = string.Empty;
        //    //Validaciones para que la caja de texto no este vacio o con espacios
        //    if (string.IsNullOrEmpty(obj.IdLibro) || string.IsNullOrWhiteSpace(obj.IdLibro))
        //    {
        //        Mensaje = "La código del Libro no puede ser vacio";
        //    }
        //    //else if (string.IsNullOrEmpty(obj.Titulo) || string.IsNullOrWhiteSpace(obj.Titulo))
        //    //{
        //    //    Mensaje = "El titulo del Libro no puede ser vacio";
        //    //}
        //    else if (string.IsNullOrEmpty(obj.Ubicacion) || string.IsNullOrWhiteSpace(obj.Ubicacion))
        //    {
        //        Mensaje = "La ubicacion del Libro no puede ser vacio";
        //    }
        //    else if (obj.Paginas == 0)/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debe ingresar el número de páginas del Libro";
        //    }
        //    else if (obj.oId_Categoria.IdCategoria == "0")/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debes seleccionar una categoria";
        //    }
        //    else if (obj.oId_Editorial.IdEditorial == "0")/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debes seleccionar una editorial";
        //    }
        //    else if (obj.oId_Sala.IdSala == "0")/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debes seleccionar una sala";
        //    }
        //    else if (obj.Ejemplares == 0)/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debe ingresar el número de ejemplares del Libro";
        //    }
        //    else if (string.IsNullOrEmpty(obj.AñoEdicion) || string.IsNullOrWhiteSpace(obj.AñoEdicion))
        //    {
        //        Mensaje = "El año de edicion del Libro no puede ser vacio";
        //    }
        //    else if (obj.Volumen == 0)/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debe ingresar el volúmen del Libro";
        //    }
        //    else if (string.IsNullOrEmpty(obj.Observaciones) || string.IsNullOrWhiteSpace(obj.Observaciones))
        //    {
        //        Mensaje = "Las observaciones del Libro no puede ser vacio";
        //    }

        //    if (string.IsNullOrEmpty(Mensaje))
        //    {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

        //        return objCapaDato.Registrar(obj, out Mensaje);
        //    }
        //    else
        //    {
        //        return "0";/*No se ha creado la categoria*/
        //    }

        //}

        //public bool Editar(EN_Libro obj, out string Mensaje)
        //{
        //    Mensaje = string.Empty;
        //    //Validaciones para que la caja de texto no este vacio o con espacios
        //    if (string.IsNullOrEmpty(obj.IdLibro) || string.IsNullOrWhiteSpace(obj.IdLibro))
        //    {
        //        Mensaje = "La código del Libro no puede ser vacio";
        //    }
        //    else if (string.IsNullOrEmpty(obj.Titulo) || string.IsNullOrWhiteSpace(obj.Titulo))
        //    {
        //        Mensaje = "El titulo del Libro no puede ser vacio";
        //    }
        //    else if (string.IsNullOrEmpty(obj.Ubicacion) || string.IsNullOrWhiteSpace(obj.Ubicacion))
        //    {
        //        Mensaje = "La ubicacion del Libro no puede ser vacio";
        //    }
        //    else if (obj.Paginas == 0)/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debe ingresar el número de páginas del Libro";
        //    }
        //    else if (obj.oId_Categoria.IdCategoria == "0")/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debes seleccionar una categoria";
        //    }
        //    else if (obj.oId_Editorial.IdEditorial == "0")/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debes seleccionar una editorial";
        //    }
        //    else if (obj.oId_Sala.IdSala == "0")/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debes seleccionar una sala";
        //    }
        //    else if (obj.Ejemplares == 0)/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debe ingresar el número de ejemplares del Libro";
        //    }
        //    else if (string.IsNullOrEmpty(obj.AñoEdicion) || string.IsNullOrWhiteSpace(obj.AñoEdicion))
        //    {
        //        Mensaje = "El año de edicion del Libro no puede ser vacio";
        //    }
        //    else if (obj.Volumen == 0)/*Si no ha seleccionado ninguna marca*/
        //    {
        //        Mensaje = "Debe ingresar el volúmen del Libro";
        //    }
        //    else if (string.IsNullOrEmpty(obj.Observaciones) || string.IsNullOrWhiteSpace(obj.Observaciones))
        //    {
        //        Mensaje = "Las observaciones del Libro no puede ser vacio";
        //    }
        //    if (string.IsNullOrEmpty(Mensaje))
        //    {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
        //        return objCapaDato.Editar(obj, out Mensaje);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public bool GuardarDatosImagen(EN_Libro obj, out string Mensaje)
        //{
        //    return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        //}
        //public bool Eliminar(string id, out string Mensaje)
        //{
        //    return objCapaDato.Eliminar(id, out Mensaje);
        //}
    }
}
