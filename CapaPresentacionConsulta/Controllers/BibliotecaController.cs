using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

using System.IO;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
//using CapaPresentacionTienda.Filter;
//using CapaEntidad.Paypal;

namespace CapaPresentacionConsulta.Controllers
{
    public class BibliotecaController : Controller
    {
        // GET: Biblioteca
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult DetalleLibro(int idLibro = 0)//por default recibe 0
        //{
        //    EN_Libro oLibro = new EN_Libro();
        //    bool conversion;

        //    oLibro = new RN_Libro().Listar().Where(p => p.IdLibro == idLibro).FirstOrDefault();//El id a encontrar debe ser el especificado como parametro

        //    if (oLibro != null)
        //    {//Actualiza la extension
        //        oLibro.Base64 = RN_Recursos.ConvertirBase64(Path.Combine(oLibro.RutaImagen, oLibro.NombreImagen), out conversion);
        //        oLibro.Extension = Path.GetExtension(oLibro.NombreImagen);
        //    }

        //    return View(oLibro);
        //}

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<EN_Categoria> lista = new List<EN_Categoria>();//una lista para categorias posibles a filtrar
            lista = new RN_Categoria().Listar();//Devuelve la lista de categorias actuales

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarEditorialPorCategoria(string idCategoria)
        {
            List<EN_Editorial> lista = new List<EN_Editorial>();//una lista para categorias posibles a filtrar
            lista = new RN_Editorial().ListarEditorialPorCategoria(idCategoria);//Devuelve la lista de categorias actuales

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarLibros(string idCategoria, string idEditorial)//cambiamos de tipo int a string
        {
            List<EN_Libro> lista = new List<EN_Libro>();
            bool conversion;

            lista = new RN_Libro().Listar().Select(l => new EN_Libro()
            {
                IdLibro = l.IdLibro,
                Codigo = l.Codigo,
                Titulo = l.Titulo,
                Paginas = l.Paginas,
                oId_Categoria = l.oId_Categoria,
                oId_Editorial = l.oId_Editorial,
                oId_Sala = l.oId_Sala,
                Ejemplares = l.Ejemplares,
                AñoEdicion = l.AñoEdicion,
                Volumen = l.Volumen,
                RutaImagen = l.RutaImagen,
                Base64 = RN_Recursos.ConvertirBase64(Path.Combine(l.RutaImagen, l.NombreImagen), out conversion),//como este metodo pide un parametro de salida, enviamos conversion
                Extension = Path.GetExtension(l.NombreImagen),
                Observaciones = l.Observaciones,
                Activo = l.Activo
            }).Where(l =>
            //Si el id categoria es  = 0, entonces colocas el idcategoria y si no colocas el id dado en parametro (la del usuario), dependiendo de eso
            //hara la busqueda por filtro, teniendo en cuenta el stock y si el Libros esta activo
                l.oId_Categoria.IdCategoria == (idCategoria == "0" ? l.oId_Categoria.IdCategoria : idCategoria) &&
                l.oId_Editorial.IdEditorial == (idEditorial == "0" ? l.oId_Editorial.IdEditorial : idEditorial) &&
                l.Ejemplares > 0 && l.Activo == true //Solo muestra Libros activos y con un stock mayor a 0
                ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue; //Indica que este json result no va a tener ningun limite en su contenido

            return jsonresult;

        }


        //-----------------------------------------Metodos para carrito ---------------------------------------------
        //[HttpPost]
        //public JsonResult AgregarCarrito(int idLibro)
        //{
        //    int idLector = ((EN_Lector)Session["Lector"]).IdLector;//Obtenemos el idLector de acuerdo a la sesion iniciada donde estan los datos
        //    //de dicho clinte, convertimos la ssion a tipo Lector y traemos su id
        //    bool existe = new RN_Carrito().ExisteCarrito(idLector, idLibro); //Valida si existe el Libro dentro del carrito del Lector

        //    bool respuesta = false;
        //    string mensaje = string.Empty;
        //    if (existe)
        //    {
        //        mensaje = "El Libro ya existe en el carrito";
        //    }
        //    else
        //    {
        //        respuesta = new RN_Carrito().OperacionCarrito(idLector, idLibro, true, out mensaje);//true es igual sumar = 1
        //    }
        //    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult CantidadEnCarrito()//devuelve la cantidad que tiene los Libros segun el carrito del Lector
        //{
        //    int idLector = ((EN_Lector)Session["Lector"]).IdLector;
        //    int cantidad = new RN_Carrito().CantidadEnCarrito(idLector);

        //    return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult ListarLibrosCarrito()
        //{
        //    int idLector = ((EN_Lector)Session["Lector"]).IdLector;//Se debe iniciar sesion para tener el id del Lector o dará error
        //    List<EN_Carrito> oLista = new List<EN_Carrito>();
        //    bool conversion;
        //    //por cada en_carrito que encuentre en la lista crea un nuevo carrito
        //    oLista = new RN_Carrito().ListarLibro(idLector).Select(oc => new EN_Carrito()
        //    {
        //        oId_Libro = new EN_Libro()
        //        {
        //            IdLibro = oc.oId_Libro.IdLibro,
        //            Codigo = oc.oId_Libro.Codigo,
        //            Titulo = oc.oId_Libro.Titulo,
        //            oId_Editorial = oc.oId_Libro.oId_Editorial,
        //            Ejemplares = oc.oId_Libro.Ejemplares,//Esto se va a mostrar en el carrito de compras
        //            RutaImagen = oc.oId_Libro.RutaImagen,
        //            Base64 = RN_Recursos.ConvertirBase64(Path.Combine(oc.oId_Libro.RutaImagen, oc.oId_Libro.NombreImagen), out conversion),
        //            Extension = Path.GetExtension(oc.oId_Libro.NombreImagen)

        //        },
        //        Cantidad = oc.Cantidad
        //    }).ToList();
        //    return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);//devuelve toda la lista de Libros que pertenecen al carrito de un Lector
        //}

        //[HttpPost]
        //public JsonResult OperacionCarrito(int idLibro, bool sumar)
        //{
        //    int idLector = ((EN_Lector)Session["Lector"]).IdLector;//Obtenemos el idLector de acuerdo a la sesion iniciada donde estan los datos
        //    //de dicho clinte, convertimos la ssion a tipo Lector y traemos su id

        //    bool respuesta = false;
        //    string mensaje = string.Empty;
        //    respuesta = new RN_Carrito().OperacionCarrito(idLector, idLibro, sumar, out mensaje);//true es igual sumar = 1 = true, restar = 0 = false 

        //    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult EliminarCarrito(int idLibro)
        //{

        //    int idLector = ((EN_Lector)Session["Lector"]).IdLector;

        //    bool respuesta = false;

        //    string mensaje = string.Empty;

        //    respuesta = new RN_Carrito().EliminarCarrito(idLector, idLibro);

        //    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult ObtenerDepartamento()
        //{
        //    List<EN_Departamento> oLista = new List<EN_Departamento>();

        //    oLista = new RN_Ubicacion().ObtenerDepartamento();
        //    return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult ObtenerProvincia(string idDepartamento)//Con parametros
        //{
        //    List<EN_Provincia> oLista = new List<EN_Provincia>();

        //    oLista = new RN_Ubicacion().ObtenerProvincia(idDepartamento);
        //    return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult ObtenerDistrito(string idProvincia, string idDepartamento)//Con parametros
        //{
        //    List<EN_Distrito> oLista = new List<EN_Distrito>();

        //    oLista = new RN_Ubicacion().ObtenerDistrito(idProvincia, idDepartamento);
        //    return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        //}
        //[ValidarSession]
        //[Authorize]
        //public ActionResult Carrito()//Solo los que han iniciado sesión
        //{
        //    return View();
        //}

        ////Boton de procesar pago
        //[HttpPost]
        //public async Task<JsonResult> ProcesarPago(List<EN_Carrito> oListaCarrito, EN_Venta oVenta)//Con parametros
        //{//los servicios de paypal obligan a trabajar de manera asincrona
        //    decimal total = 0;
        //    DataTable detalleVenta = new DataTable();
        //    detalleVenta.Locale = new CultureInfo("es-MX");
        //    //Comenzamos a crear las columnas que necesita esta table
        //    detalleVenta.Columns.Add("IdLibro", typeof(string));
        //    detalleVenta.Columns.Add("Cantidad", typeof(int));
        //    detalleVenta.Columns.Add("Total", typeof(decimal));//Esta tabla viene a ser la representacion de la estructura creada en sql (EDetalle_Venta)

        //    List<Item> oListaItem = new List<Item>();//Almacenará todos los Libros del carrito

        //    foreach (EN_Carrito oCarrito in oListaCarrito)//por cada carrito en la lista carrito
        //    {
        //        decimal subTotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oId_Libro.Precio;

        //        total += subTotal;//Va aumentando el valor de total con cada iteracion

        //        oListaItem.Add(new Item()//Toda la lista de Libros hacia paypal, con la informacion requerida
        //        {
        //            name = oCarrito.oId_Libro.Nombre,
        //            quantity = oCarrito.Cantidad.ToString(),
        //            unit_amount = new UnitAmount()
        //            {
        //                currency_code = "USD",//tipo de moneda
        //                value = oCarrito.oId_Libro.Precio.ToString("G", new CultureInfo("es-MX"))
        //            }
        //        }); ;
        //        detalleVenta.Rows.Add(new object[]
        //        {
        //            oCarrito.oId_Libro.IdLibro,
        //            oCarrito.Cantidad,
        //            subTotal
        //        });
        //    }

        //    PurchaseUnit purchaseUnit = new PurchaseUnit()
        //    {
        //        amount = new Amount()
        //        {
        //            currency_code = "USD",
        //            value = total.ToString("G", new CultureInfo("es-MX")),
        //            breakdown = new Breakdown()
        //            {
        //                item_total = new ItemTotal()
        //                {
        //                    currency_code = "USD",
        //                    value = total.ToString("G", new CultureInfo("es-MX"))
        //                }
        //            }
        //        },
        //        description = "compra de articulo de mi tienda",
        //        items = oListaItem
        //    };

        //    Checkout_Order oCheckOutOrder = new Checkout_Order()
        //    {
        //        intent = "CAPTURE",
        //        purchase_units = new List<PurchaseUnit>() { purchaseUnit },
        //        application_context = new ApplicationContext()
        //        {
        //            brand_name = "MiTiendaCode.com",
        //            landing_page = "NO_PREFERENCE",
        //            user_action = "PAY_NOW",//Accion para que paypal muestre el monto de pago
        //            return_url = "https://localhost:44330/Tienda/PagoEfectuado",
        //            cancel_url = "https://localhost:44330/Tienda/Carrito"
        //        }
        //    };

        //    oVenta.MontoTotal = total;
        //    oVenta.Id_Lector = ((EN_Lector)Session["Lector"]).IdLector;
        //    TempData["Venta"] = oVenta;  //Almacena informacion que vamos a poder compartir a traves de metodos (Todo el obj de venta)
        //    TempData["DetalleVenta"] = detalleVenta; //Almacena todo el dataTable

        //    RN_Paypal oPaypal = new RN_Paypal();
        //    Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();

        //    response_paypal = await oPaypal.CrearSolicitud(oCheckOutOrder);//Pasamos toda la configuracion que hemos creado

        //    //return Json(new { Status = true, Link = "/Tienda/PagoEfectuado?IdTransaccion=code0001&status=true" }, JsonRequestBehavior.AllowGet);
        //    return Json(response_paypal, JsonRequestBehavior.AllowGet);

        //    //Enviamos dos parametros el id de transaccon y un status como true
        //    //Por el momento la estructura es estatica (por lo pronto hasta aqui es una simulacion de paypal)
        //}

        //[ValidarSession]
        //[Authorize]
        //public async Task<ActionResult> PagoEfectuado()//Para la vista
        //{
        //    string token = Request.QueryString["token"];
        //    //bool status = Convert.ToBoolean( Request.QueryString["status"]);
        //    RN_Paypal oPaypal = new RN_Paypal();
        //    Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();
        //    response_paypal = await oPaypal.AprobarPago(token);//Respuesta de la ejecucion del metodo aprobarPago()

        //    ViewData["Status"] = response_paypal.Status;//ViewData sirve para poder almacenar informacion que se compartira con la misma vista en la que nos encontramos

        //    if (response_paypal.Status) //si estatus es verdadero
        //    {
        //        EN_Prestamo oPrestamo = (EN_Prestamo)TempData["Prestamo"];//TempData sirve para compartir informacion entre metodos que pertenecen o estan dentro de
        //        //un mismo controlador, de esta forma podemos acceder a este temp data del metodo anterior
        //        //Lo convertimos en un objeto de Prestamo
        //        DataTable detallePrestamo = (DataTable)TempData["DetallePrestamo"];//La informaciion lo convertimos en datatable

        //        oPrestamo.IdTransaccion = response_paypal.Response.purchase_units[0].payments.captures[0].id;//El id de transacction

        //        string mensaje = string.Empty;//Por defecto el mensaje es vacio

        //        bool respuesta = new RN_Prestamo().Registrar(oPrestamo, detallePrestamo, out mensaje);

        //        ViewData["IdTransaccion"] = oPrestamo.IdTransaccion;
        //    }

        //    return View();
        //}

        //[ValidarSession]
        //[Authorize]
        //public ActionResult MisCompras()
        //{
        //    int idLector = ((EN_Lector)Session["Lector"]).IdLector;//Se debe iniciar sesion para tener el id del Lector o dará error
        //    List<EN_DetallePrestamo> oLista = new List<EN_DetallePrestamo>();
        //    bool conversion;
        //    //por cada en_carrito que encuentre en la lista crea un nuevo carrito
        //    oLista = new RN_Prestamo().ListarCompras(idLector).Select(oc => new EN_DetallePrestamo()
        //    {
        //        oId_Libro = new EN_Libro()
        //        {
        //            Nombre = oc.oId_Libro.Nombre,
        //            Precio = oc.oId_Libro.Precio,
        //            //RutaImagen = oc.oId_Libro.RutaImagen,
        //            Base64 = RN_Recursos.ConvertirBase64(Path.Combine(oc.oId_Libro.RutaImagen, oc.oId_Libro.NombreImagen), out conversion),
        //            Extension = Path.GetExtension(oc.oId_Libro.NombreImagen)

        //        },
        //        Cantidad = oc.Cantidad,
        //        Total = oc.Total,
        //        IdTransaccion = oc.IdTransaccion
        //    }).ToList();

        //    return View(oLista);//devuelve toda la lista de Libros que pertenecen al carrito de un Lector
        //}
    }
}