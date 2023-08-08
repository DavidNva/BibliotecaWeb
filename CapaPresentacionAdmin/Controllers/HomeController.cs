using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
//using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize] //No va a poder ingresar a ninguna de estas vistas si no se encuentra autorizado

    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();/*Retorna la vista con el nombre de Index (Dentro de la carpeta vista, hay un Index*/
        }
        [Authorize]
        public ActionResult Usuarios()
        {
            return View();/*Retorna la vista con el nombre de Usuarios(Dentro de la carpeta vista, hay un home, dentro usuarios*/
        }
        [Authorize]
        public ActionResult Lectores()
        {
            return View();/*Retorna la vista con el nombre de Usuarios(Dentro de la carpeta vista, hay un home, dentro usuarios*/
        }
        [Authorize]
        public ActionResult Prestamos()
        {
            return View();/*Retorna la vista con el nombre de Usuarios(Dentro de la carpeta vista, hay un home, dentro usuarios*/
        }
        /*--------------PRESTAMOS--------------------*/
        #region PRESTAMOS
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarPrestamosCompleto() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Prestamo> oLista = new List<EN_Prestamo>();
            oLista = new RN_Prestamo().ListarPrestamosCompleto();/*Esta declarado en RN_Prestamos, capa negocio*/
            
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/

        }

        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarLectorParaPrestamo() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Lector> oLista = new List<EN_Lector>();
            oLista = new RN_Lector().ListarLectorParaPrestamo();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarLibrosParaPrestamo() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Libro> oLista = new List<EN_Libro>();
            oLista = new RN_Libro().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }
        [HttpPost]
        public JsonResult ObtenerEjemplarPorLibro(int idLibro)//Con parametros
        {
            List<EN_Ejemplar> oLista = new List<EN_Ejemplar>();

            oLista = new RN_Ejemplar().ListarEjemplarLibro(idLibro);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GuardarPrestamo(List<EN_Prestamo> oListaPrestamo, EN_Prestamo objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        //{

        //    DataTable detallePrestamo = new DataTable();
        //    detallePrestamo.Locale = new CultureInfo("es-MX"); //Comenzamos a crear las columnas que necesita esta table
        //    detallePrestamo.Columns.Add("IdEjemplar", typeof(string));//antes era IdLibro
        //    detallePrestamo.Columns.Add("CantidadEjemplares", typeof(int));
        //    detallePrestamo.Columns.Add("Total", typeof(decimal));

        //    foreach (EN_Prestamo oEjemplar in oListaPrestamo)//por cada carrito en la lista carrito
        //    {
        //        decimal subTotal = Convert.ToDecimal(objeto.TotalLibro) /** oCarrito.oId_Libro.Precio*/;
        //        //        //Antes se multiplicaaba por el precio, ahoa simplemente pasamos la cantidad directamente

        //        //total += subTotal;//Va aumentando el valor de total con cada iteracion
        //        detallePrestamo.Rows.Add(new object[]
        //        {
        //                //oCarrito.oId_Ejemplar.IdEjemplarLibro,//Estamos trabajando con ejemplar, pero en este caso solo es una lista entonces no hay problema
        //                //oCarrito.oId_Libro.oId_Ejemplar.IdEjemplarLibro,//Estamos trabajando con ejemplar, pero en este caso solo es una lista entonces no hay problema
        //                //oCarrito.Cantidad,
        //                oEjemplar.oId_Libro.oId_Ejemplar.IdEjemplarLibro,
        //                oEjemplar.TotalLibro,
        //                subTotal

        //        });

        //    }


        //    object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
        //    string mensaje = string.Empty;
        //    //decimal total = 0;


        //    if (objeto.IdPrestamo == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
        //     un Prestamo nuevo, por lo que se ha dado dando clic con el boton de crear*/
        //    {
        //        //resultado = new RN_Prestamo().Registrar(objeto, detallePrestamo, out mensaje);/*El metodo registrar
        //        resultado = new RN_Prestamo().RegistrarPrestamo2(objeto, out mensaje);/*El metodo registrar
        //         de tipo int, devuelve el id registrado*/
        //    }
        //    else
        //    {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
        //         a un Prestamo, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
        //         con los alert comentados*/
        //        resultado = new RN_Prestamo().Editar(objeto, out mensaje);
        //    }
        //    return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        //}

        //[HttpPost]
        //public JsonResult GuardarPrestamo(EN_Prestamo objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        //{
        //    object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
        //    string mensaje = string.Empty;

        //    if (objeto.IdPrestamo == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
        //     un Prestamo nuevo, por lo que se ha dado dando clic con el boton de crear*/
        //    {
        //        resultado = new RN_Prestamo().RegistrarPrestamo2(objeto, out mensaje);/*El metodo registrar
        //         de tipo int, devuelve el id registrado*/
        //    }
        //    else
        //    {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
        //         a un Prestamo, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
        //         con los alert comentados*/
        //        resultado = new RN_Prestamo().Editar(objeto, out mensaje);
        //    }
        //    return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public JsonResult GuardarPrestamo(List<EN_Prestamo> oListaPrestamo, EN_Prestamo objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            DataTable detallePrestamo = new DataTable();
            detallePrestamo.Locale = new CultureInfo("es-MX"); //Comenzamos a crear las columnas que necesita esta table
            detallePrestamo.Columns.Add("IdEjemplar", typeof(string));//antes era IdLibro
            detallePrestamo.Columns.Add("CantidadEjemplares", typeof(int));
            detallePrestamo.Columns.Add("Total", typeof(decimal));
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            foreach (EN_Prestamo oEjemplar in oListaPrestamo)//por cada carrito en la lista carrito
            {
                decimal subTotal = Convert.ToDecimal(objeto.TotalLibro) /** oCarrito.oId_Libro.Precio*/;
                //        //Antes se multiplicaaba por el precio, ahoa simplemente pasamos la cantidad directamente

                //total += subTotal;//Va aumentando el valor de total con cada iteracion
                //Dar una condicionar que si el oEjemplar es diferente de null
                if(oListaPrestamo == null || oEjemplar == null || oEjemplar.oId_Libro.oId_Ejemplar==null)
                {
                    mensaje = "No hay un ejemplar disponible para el libro seleccionado";
                }
                else
                {
                    detallePrestamo.Rows.Add(new object[]
                    {
                        //oCarrito.oId_Ejemplar.IdEjemplarLibro,//Estamos trabajando con ejemplar, pero en este caso solo es una lista entonces no hay problema
                        //oCarrito.oId_Libro.oId_Ejemplar.IdEjemplarLibro,//Estamos trabajando con ejemplar, pero en este caso solo es una lista entonces no hay problema
                        //oCarrito.Cantidad,
                        oEjemplar.oId_Libro.oId_Ejemplar.IdEjemplarLibro,
                        oEjemplar.TotalLibro,
                        subTotal

                    });
                }
                
            }

            if (objeto.IdPrestamo == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             un Prestamo nuevo, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Prestamo().Registrar(objeto, detallePrestamo, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a un Prestamo, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_Prestamo().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public JsonResult EliminarPrestamo(int id)
        //{
        //    bool respuesta = false;
        //    string mensaje = string.Empty;

        //    respuesta = new RN_Prestamo().Eliminar(id, out mensaje);
        //    return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        //}
        [HttpPost]
        public JsonResult FinalizarPrestamo(/*int id, int idEjemplarLibro, int idLibro */EN_Prestamo objeto)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Prestamo().FinalizarPrestamo(objeto, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult EliminarPrestamo(int id, int idEjemplarLibro, int idLibro)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Prestamo().Eliminar(id, idEjemplarLibro, idLibro, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        /*--------------USUARIOS---------------------*/
        #region USUARIOS
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarUsuarios() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Usuario> oLista = new List<EN_Usuario>();
            oLista = new RN_Usuarios().Listar();/*Esta declarado en RN_Usuarios, capa negocio*/

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/

        }

        [HttpPost]
        public JsonResult GuardarUsuario(EN_Usuario objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.IdUsuario == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             un usuario nuevo, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Usuarios().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a un usuario, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_Usuarios().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Usuarios().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        /*--------------REPORTE Y DASHBOARD---------------------*/
        #region REPORTE Y DASHBOARD
        /*La consulta de busqueda por fecha o id transaccion*/
        [HttpGet]
        public JsonResult ListaReporte(string fechaInicio, string fechaFin, string codigo)
        {
            List<EN_Reporte> oLista = new List<EN_Reporte>();
            oLista = new RN_Reporte().Prestamos(fechaInicio, fechaFin, codigo);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet); /*Obtenemos el objeto del reporte*/
        }

        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            EN_DashBoard objeto = new RN_Reporte().VerDashBoard();

            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet); /*Obtenemos el objeto del reporte*/
        }


        [HttpPost]
        public FileResult ExportarPrestamo(string fechaInicio, string fechaFin, string codigo)
        {
            List<EN_Reporte> oLista = new List<EN_Reporte>();
            oLista = new RN_Reporte().Prestamos(fechaInicio, fechaFin, codigo);

            DataTable dt = new DataTable();
            dt.Locale = new System.Globalization.CultureInfo("es-MX"); /*Configuracion con mexico*/
            dt.Columns.Add("Fecha Prestamo", typeof(string));
            dt.Columns.Add("Lector", typeof(string));
            dt.Columns.Add("Libro", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Estado", typeof(bool));
            dt.Columns.Add("Codigo      -", typeof(string));

            foreach (EN_Reporte rp in oLista)
            {
                dt.Rows.Add(new object[]{
                    rp.FechaPrestamo,
                    rp.Lector,
                    rp.Libro,
                    rp.CantidadEjemplares,
                    rp.Estado,
                    rp.Codigo
                });
            }
            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePrestamos" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
        #endregion

        /*--------------LECTOR---------------------*/
        #region LECTOR
        //-------------------------------------------------- LECTOR ---------------------------------------------------------
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarLectores() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Lector> oLista = new List<EN_Lector>();
            oLista = new RN_Lector().Listar();/*Esta declarado en RN_Lector, capa negocio*/

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/

        }

        [HttpPost]
        public JsonResult GuardarLector(EN_Lector objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.IdLector == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             un usuario nuevo, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Lector().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a un usuario, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_Lector().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        //[HttpPost]
        //public ActionResult Registrar(EN_Lector objeto) //Para mostrar vista de login
        //{
        //    int resultado;
        //    string mensaje = string.Empty;
        //    ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
        //    ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
        //    ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

        //    if (objeto.Clave != objeto.ConfirmarClave)
        //    {
        //        ViewBag.Error = "Las contraseñas no coinciden";
        //        return View();
        //    }
        //    resultado = new RN_Lector().Registrar(objeto, out mensaje);
        //    if (resultado > 0)
        //    {
        //        ViewBag.Error = null;/*No hay error*/
        //        return RedirectToAction("Index", "Acceso");
        //    }
        //    else
        //    {
        //        ViewBag.Error = mensaje;
        //        return View();
        //    }
        //}
        [HttpPost]
        public JsonResult EliminarLector(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Lector().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}