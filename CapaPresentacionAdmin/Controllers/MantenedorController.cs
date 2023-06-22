using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
//using Newtonsoft.Json;

namespace CapaPresentacionAdmin.Controllers
{
    //[Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View(); /*Retorna la vista con el nombre de Categoria (Dentro de la carpeta vista,, dentro de Mantenedor hay un "Categoria"*/
        }
        public ActionResult Marca()
        {
            return View();
        }
        //public ActionResult Libros()
        //{
        //    return View();
        //}
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarTipoPersona() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_TipoPersona> oLista = new List<EN_TipoPersona>();
            oLista = new RN_TipoPersona().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        /*--------------CATEGORIA---------------------*/
        //#region CATEGORIA
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarCategoria() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Categoria> oLista = new List<EN_Categoria>();
            oLista = new RN_Categoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpPost]
        public JsonResult GuardarCategoria(EN_Categoria objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.IdCategoria == "0")/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             una categoria nueva, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Categoria().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a una categoria, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_Categoria().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult EliminarCategoria(string id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        //#endregion
        ///*-----------------------MARCA----------------*/
        //#region MARCA
        //[HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        //public JsonResult ListarMarcas() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        //{
        //    List<EN_Marca> oLista = new List<EN_Marca>();
        //    oLista = new RN_Marca().Listar();
        //    return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        //    /*El json da los datos, jala los datos de esa lista, en data*/
        //}

        //[HttpPost]
        //public JsonResult GuardarMarca(EN_Marca objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        //{
        //    object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
        //    string mensaje = string.Empty;

        //    if (objeto.IdMarca == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
        //     una categoria nueva, por lo que se ha dado dando clic con el boton de crear*/
        //    {
        //        resultado = new RN_Marca().Registrar(objeto, out mensaje);/*El metodo registrar
        //         de tipo int, devuelve el id registrado*/
        //    }
        //    else
        //    {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
        //         a una categoria, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
        //         con los alert comentados*/
        //        resultado = new RN_Marca().Editar(objeto, out mensaje);
        //    }
        //    return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        //}

        //[HttpPost]
        //public JsonResult EliminarMarca(int id)
        //{
        //    bool respuesta = false;
        //    string mensaje = string.Empty;

        //    respuesta = new RN_Marca().Eliminar(id, out mensaje);

        //    return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //}
        //#endregion 
        ///*-----------------------PRODUCTO----------------*/
        //#region PRODUCTO
        //[HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        //public JsonResult ListarProducto() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        //{
        //    List<EN_Producto> oLista = new List<EN_Producto>();
        //    oLista = new RN_Producto().Listar();
        //    return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        //    /*El json da los datos, jala los datos de esa lista, en data*/
        //}

        //[HttpPost]
        //public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        //{

        //    /*Resultado = Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
        //    string mensaje = string.Empty;

        //    bool operacionExitosa = true;
        //    bool guardarImagenExito = true;

        //    EN_Producto oProducto = new EN_Producto();
        //    //Se referencia a "using Newtonsoft.Json;" para convertir el string objeto (de parametros)
        //    //a tipo objeto con JsonConvert
        //    oProducto = JsonConvert.DeserializeObject<EN_Producto>(objeto);

        //    decimal precio;
        //    /*Tratar de convertir el precio con un decimal con formato regional que necesitamos (Se referencia el using globalizatio para Culture Info
        //     Al convertir el formato de texto en decimal va a decirle que los decimales son puntos, y que la cultura regional es de MX, el resultado de la convercion lo guarda en la variable precio (que es decimal*/
        //    if (decimal.TryParse(oProducto.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-MX"), out precio))
        //    {
        //        /*Si esta todo correcto
        //         * Ahora si pasa el valor convertido a Precio de la entidad EN_Productos
        //         */
        //        oProducto.Precio = precio;
        //    }
        //    else /*Si hay algun problemas*/
        //    {
        //        return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (oProducto.IdProducto == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
        //     una categoria nueva, por lo que se ha dado dando clic con el boton de crear*/
        //    {
        //        int idProductoGenerado = new RN_Producto().Registrar(oProducto, out mensaje);/*El metodo registrar
        //         de tipo int, devuelve el id registrado*/
        //        if (idProductoGenerado != 0)/*Si se pudo registrar correctamente el producto*/
        //        {
        //            oProducto.IdProducto = idProductoGenerado;/*Toma el id*/
        //        }
        //        else
        //        {
        //            operacionExitosa = false;
        //        }
        //    }
        //    else
        //    {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
        //         a una categoria, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
        //         con los alert comentados*/
        //        operacionExitosa = new RN_Producto().Editar(oProducto, out mensaje);
        //    }

        //    if (operacionExitosa)/*Si la operacion es true  pasamos  actualizar una ruta de imagen, guardar una imagen*/
        //    {
        //        if (archivoImagen != null)
        //        {
        //            /*Toda la logica para guardar en la carpeta y actualizar la tablas*/
        //            /*Ruta de imagen con ConfiguracionManager, haciendo referencia using a system.Configuration para acceder a webConfig*/
        //            string rutaGuardar = ConfigurationManager.AppSettings["ServidorFotos"]; /*Guarda las imagenes en esa ruta especificada*/
        //            /*Se referencia a .io para path*/
        //            string extensionImagen = Path.GetExtension(archivoImagen.FileName);
        //            /*Creamos un nombre de imagen personalizado, el codigo del producto mas extension*/
        //            string nombreImagen = string.Concat(oProducto.IdProducto.ToString(), extensionImagen);

        //            try
        //            {
        //                /*Guarda en la ruta con un respectivo nombres*/
        //                archivoImagen.SaveAs(Path.Combine(rutaGuardar, nombreImagen));
        //            }
        //            catch (Exception ex)
        //            {
        //                /*Si existe un error*/
        //                string msg = ex.Message;
        //                guardarImagenExito = false;
        //            }
        //            if (guardarImagenExito)/*Si es true*/
        //            {
        //                /*Para guardar en la base de datos*/
        //                oProducto.RutaImagen = rutaGuardar;
        //                oProducto.NombreImagen = nombreImagen;
        //                bool respuesta = new RN_Producto().GuardarDatosImagen(oProducto, out mensaje);
        //            }
        //            else /*En el caso de que la imagen no haya sido guardada con exitos*/
        //            {
        //                /*Como el registro del producto y el registro de la imagen son operaciones distintas
        //                 puede que se ejecute una de manera correcta y otra no*/
        //                mensaje = "Se guardo el producto pero hubo problemas con la imagen";
        //            }
        //        }
        //    }
        //    return Json(new { operacionExitosa = operacionExitosa, idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //    //return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //}


        ///*Metodo para ver imagen de producto en venta, x64*/
        //[HttpPost]
        //public JsonResult ImagenProducto(int id)
        //{
        //    bool conversion;
        //    /*Seleccionamos un producto en especifico con where*/
        //    EN_Producto oProducto = new RN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

        //    /*Obtenemos la ruta de imagen convertida a base64*/
        //    string textoBase64 = RN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);

        //    return Json(new
        //    {
        //        conversion = conversion,
        //        textobase64 = textoBase64,
        //        extension = Path.GetExtension(oProducto.NombreImagen) /*El nombre de la imagen ya tiene concatenada la extension*/
        //    },
        //        JsonRequestBehavior.AllowGet
        //    );
        //}
        //[HttpPost]
        //public JsonResult EliminarProducto(int id)
        //{
        //    bool respuesta = false;
        //    string mensaje = string.Empty;

        //    respuesta = new RN_Producto().Eliminar(id, out mensaje);

        //    return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

    }
}