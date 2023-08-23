using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using QuestPDF.Fluent;//Para exportar a pdf
using QuestPDF.Helpers;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize] //Para no poder acceder a menos que este logeado
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        [Authorize]
        public ActionResult Categoria()
        {
            return View(); /*Retorna la vista con el nombre de Categoria (Dentro de la carpeta vista,, dentro de Mantenedor hay un "Categoria"*/
        }
        public ActionResult CategoriaPruebaSelect() 
        {
            return View(); /*Retorna la vista con el nombre de Categoria (Dentro de la carpeta vista,, dentro de Mantenedor hay un "Categoria"*/
        }
        public ActionResult Sala()
        {
            return View();
        }
        public ActionResult Editorial()
        {
            return View();
        }
        public ActionResult Autor()
        {
            return View();
        }
        public ActionResult Libros()
        {
            return View();
        }
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarTipoPersona() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_TipoPersona> oLista = new List<EN_TipoPersona>();/*Para la parte de usuarios en el comboBox*/
            oLista = new RN_TipoPersona().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        ///*--------------Generar PDF--------------------*/ 
        #region GenerarPDF
        //public ActionResult DescargarPdf()
        //{
        //    List<EN_Categoria> oLista = new List<EN_Categoria>();
        //    oLista = new RN_Categoria().Listar();

        //    var data = Document.Create(document =>
        //    {
        //        document.Page(page =>
        //        {
        //            // page content
        //            page.Margin(30);
        //            // page.Header().Height(100).Background(Colors.Blue.Medium);
        //            page.Header().ShowOnce().Row(row =>
        //            {//el ShowOnce sirve para que el header solo aparezca en la primera hoja
        //                //D:\ConsolePdf\ExportarPdf_Web\Content\images\cuborubikcode.png
        //                var rutaImagen = Path.Combine("D:\\ConsolePdf\\ExportarPdf_Web\\Content\\images\\cuborubikcode.png");

        //                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);

        //                row.ConstantItem(150).Image(imageData);


        //                //row.ConstantItem(140).Height(60).Placeholder();//Elegimos el ancho del item

        //                row.RelativeItem().Column(col =>//El ancho se coloca relativamente automatica
        //                {
        //                    col.Item().AlignCenter().Text("Biblioteca: Luis Cabrera").Bold().FontSize(14);
        //                    col.Item().AlignCenter().Text("Pueabl, Puebla").Bold().FontSize(9);
        //                    col.Item().AlignCenter().Text("123 456 7890").Bold().FontSize(9);
        //                    col.Item().AlignCenter().Text("example@gmail.com").Bold().FontSize(9);
        //                    //col.Item().Background(Colors.Orange.Medium).Height(10);
        //                    //col.Item().Background(Colors.Green.Medium).Height(10);
        //                });
        //                row.RelativeItem().Column(col =>
        //                {
        //                    col.Item().Border(1).BorderColor("#257272").
        //                    AlignCenter().Text("RUC 1234567890");

        //                    col.Item().Background("#257272").Border(1)
        //                    .BorderColor("#257272").AlignCenter()
        //                    .Text("Boleto de Venta").FontColor("#fff");

        //                    col.Item().Border(1).BorderColor("#257272").
        //                    AlignCenter().Text("B0001 - 234");

        //                });

        //            });

        //            // page.Content().Background(Colors.Yellow.Medium);
        //            page.Content().PaddingVertical(10).Column(col1 =>
        //            {
        //                col1.Item().Column(col2 =>//Columna de datos de usuario
        //                {
        //                    col2.Item().Text("Datos del Usuario").Underline().Bold();

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("Nombre: ").SemiBold().FontSize(10);
        //                        txt.Span("David Nava").FontSize(10);
        //                    });

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("DNI: ").SemiBold().FontSize(10);
        //                        txt.Span("0877625727").FontSize(10);
        //                    });

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("Dirección: ").SemiBold().FontSize(10);
        //                        txt.Span("Calle Luis Cabrera S/N").FontSize(10);
        //                    });
        //                });

        //                col1.Item().LineHorizontal(0.5f);
        //                col1.Item().Table(tabla =>
        //                {//Seccion de la tabla
        //                    tabla.ColumnsDefinition(columns =>
        //                    {
        //                        columns.RelativeColumn(3);
        //                        columns.RelativeColumn();
        //                        columns.RelativeColumn();
        //                        columns.RelativeColumn();
        //                    });

        //                    tabla.Header(header =>
        //                    {
        //                        header.Cell().Background("#257272")
        //                        .Padding(2).Text("Libro").FontColor("#fff");

        //                        header.Cell().Background("#257272")
        //                         .Padding(2).Text("Precio Unitario").FontColor("#fff");

        //                        header.Cell().Background("#257272")
        //                        .Padding(2).Text("Cantidad").FontColor("#fff");

        //                        header.Cell().Background("#257272")
        //                        .Padding(2).Text("Total").FontColor("#fff");
        //                    });

        //                    foreach (EN_Categoria cat in oLista)
        //                    //foreach (var item in Enumerable.Range(1, 45))
        //                    {
        //                        var cantidad = Placeholders.Random.Next(1, 10);
        //                        var precio = Placeholders.Random.Next(5, 15);
        //                        var total = cantidad * precio;

        //                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                        .Padding(2).Text(Placeholders.Label()).FontSize(10);

        //                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                        .Padding(2).Text(cat.IdCategoria.ToString()).FontSize(10);

        //                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                        .Padding(2).Text(cat.Descripcion).FontSize(10);

        //                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                        .Padding(2).AlignRight().Text(cat.Activo.ToString()).FontSize(10);
        //                    }
        //                    //foreach (EN_Categoria cat in oLista)
        //                    ////foreach (var item in Enumerable.Range(1, 45))
        //                    //{
        //                    //    var cantidad = Placeholders.Random.Next(1, 10);
        //                    //    var precio = Placeholders.Random.Next(5, 15);
        //                    //    var total = cantidad * precio;

        //                    //    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                    //    .Padding(2).Text(Placeholders.Label()).FontSize(10);

        //                    //    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                    //    .Padding(2).Text(cantidad.ToString()).FontSize(10);

        //                    //    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                    //    .Padding(2).Text($"S/.{precio}").FontSize(10);

        //                    //    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                    //    .Padding(2).AlignRight().Text($"S/.{total}").FontSize(10);
        //                    //}
        //                });

        //                col1.Item().AlignRight().Text("Total: 1500").FontSize(12);


        //                col1.Item().Background(Colors.Grey.Lighten3).Padding(10)//Seccion de comentarios
        //                .Column(column =>
        //                {
        //                    column.Item().Text("Comentarios").FontSize(14);
        //                    column.Item().Text(Placeholders.LoremIpsum());
        //                    column.Spacing(5);
        //                });

        //                col1.Spacing(10);
        //            });

        //            page.Footer()
        //            .AlignRight()
        //            .Text(txt =>
        //            {
        //                txt.Span("Pagina ").FontSize(10);
        //                txt.CurrentPageNumber().FontSize(10);

        //                txt.Span(" de ").FontSize(10);
        //                txt.TotalPages().FontSize(10);
        //            });
        //            //page.Footer().Height(50).Background(Colors.Red.Medium);
        //        });
        //    }).GeneratePdf();

        //    Stream stream = new MemoryStream(data);
        //    return File(stream, "applicacion/pdf", "detallePrestamo.pdf");
        //    //return View();
        //}


        /*--------------Generar PDF--------------------*/
        // public ActionResult DescargarPdf(List<Object> oLista, object RN, object EN)
        //public ActionResult DescargarPdf<T>(List<T> oLista ) where T : class
        //public ActionResult DescargarPdf<T>(List<T> oLista, Func<T, TableCell> rowFunc) where T : class

        //public ActionResult DescargarPdf()
        //public ActionResult DescargarPdfCategoria<T>(List<T> oLista)
        //{
        //    //List<EN_Categoria> oLista = new List<EN_Categoria>();
        //    //oLista = new RN_Categoria().Listar();

        //    var data = Document.Create(document =>
        //    {
        //        document.Page(page =>
        //        {
        //            // page content
        //            page.Margin(30);
        //            // page.Header().Height(100).Background(Colors.Blue.Medium);
        //            page.Header().ShowOnce().Row(row =>
        //            {//el ShowOnce sirve para que el header solo aparezca en la primera hoja
        //                //D:\ConsolePdf\ExportarPdf_Web\Content\images\cuborubikcode.png
        //               var rutaImagen = Path.Combine("D:\\ConsolePdf\\ExportarPdf_Web\\Content\\images\\cuborubikcode.png");

        //                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);

        //                row.ConstantItem(150).Image(imageData);

        //                //row.ConstantItem(140).Height(60).Placeholder();//Elegimos el ancho del item

        //                row.RelativeItem().Column(col =>//El ancho se coloca relativamente automatica
        //                {
        //                    col.Item().AlignCenter().Text("Biblioteca: Luis Cabrera").Bold().FontSize(14);
        //                    col.Item().AlignCenter().Text("Puebla, Puebla").Bold().FontSize(9);
        //                    col.Item().AlignCenter().Text("123 456 7890").Bold().FontSize(9);
        //                    col.Item().AlignCenter().Text("example@gmail.com").Bold().FontSize(9);
        //                    //col.Item().Background(Colors.Orange.Medium).Height(10);
        //                    //col.Item().Background(Colors.Green.Medium).Height(10);
        //                });
        //                row.RelativeItem().Column(col =>
        //                {
        //                    col.Item().Border(1).BorderColor("#257272").
        //                    AlignCenter().Text("RUC 1234567890");

        //                    col.Item().Background("#257272").Border(1)
        //                    .BorderColor("#257272").AlignCenter()
        //                    .Text("Boleto de Venta").FontColor("#fff");

        //                    col.Item().Border(1).BorderColor("#257272").
        //                    AlignCenter().Text("B0001 - 234");

        //                });

        //            });

        //            // page.Content().Background(Colors.Yellow.Medium);
        //            page.Content().PaddingVertical(10).Column(col1 =>
        //            {
        //                col1.Item().Column(col2 =>//Columna de datos de usuario
        //                {
        //                    col2.Item().Text("Datos del Usuario").Underline().Bold();

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("Nombre: ").SemiBold().FontSize(10);
        //                        txt.Span("David Nava").FontSize(10);
        //                    });

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("DNI: ").SemiBold().FontSize(10);
        //                        txt.Span("0877625727").FontSize(10);
        //                    });

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("Dirección: ").SemiBold().FontSize(10);
        //                        txt.Span("Calle Luis Cabrera S/N").FontSize(10);
        //                    });
        //                });

        //                col1.Item().LineHorizontal(0.5f);
        //                col1.Item().Table(tabla =>
        //                {//Seccion de la tabla
        //                    tabla.ColumnsDefinition(columns =>
        //                    {
        //                        //columns.RelativeColumn(3);
        //                        columns.ConstantColumn(100);
        //                        //columns.RelativeColumn();
        //                        columns.RelativeColumn();
        //                        //columns.RelativeColumn();
        //                        columns.ConstantColumn(100);
        //                    });

        //                    tabla.Header(header =>
        //                    {
        //                        header.Cell().Background("#257272")
        //                         .Padding(2).Text("Código").FontColor("#fff");

        //                        header.Cell().Background("#257272")
        //                        .Padding(2).Text("Descripción").FontColor("#fff");

        //                        header.Cell().Background("#257272")
        //                        .Padding(2).Text("Activo").FontColor("#fff");
        //                    });

        //                    //foreach (EN_Categoria cat in oLista)
        //                    ////foreach (var item in Enumerable.Range(1, 45))
        //                    //{

        //                    //    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                    //    .Padding(2).Text(cat.IdCategoria.ToString()).FontSize(10);

        //                    //    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                    //    .Padding(2).Text(cat.Descripcion).FontSize(10);

        //                    //    if (cat.Activo)
        //                    //    {
        //                    //        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                    //        .Padding(2).Text("Sí").FontSize(10);
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                    //        .Padding(2).Text("No").FontSize(10);
        //                    //    }
        //                    //}
        //                    foreach (T item in oLista)
        //                    {
        //                        // Use reflection to get the property values of the item
        //                        PropertyInfo[] properties = typeof(T).GetProperties();
        //                        foreach (PropertyInfo property in properties)
        //                        {
        //                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                                .Padding(2).Text(property.GetValue(item).ToString()).FontSize(10);
        //                        }
        //                    }

        //                });

        //                col1.Item().AlignRight().Text("Total: 1500").FontSize(12);


        //                col1.Item().Background(Colors.Grey.Lighten3).Padding(10)//Seccion de comentarios
        //                .Column(column =>
        //                {
        //                    column.Item().Text("Comentarios").FontSize(14);
        //                    column.Item().Text(Placeholders.LoremIpsum());
        //                    column.Spacing(5);
        //                });

        //                col1.Spacing(10);
        //            });

        //            page.Footer()
        //            .AlignRight()
        //            .Text(txt =>
        //            {
        //                txt.Span("Pagina ").FontSize(10);
        //                txt.CurrentPageNumber().FontSize(10);

        //                txt.Span(" de ").FontSize(10);
        //                txt.TotalPages().FontSize(10);
        //            });
        //            //page.Footer().Height(50).Background(Colors.Red.Medium);
        //        });
        //    }).GeneratePdf();

        //    Stream stream = new MemoryStream(data);
        //    return File(stream, "applicacion/pdf", "detallePrestamo.pdf");
        //    //return View();
        //}

        public ActionResult DescargarPDF_Categoria()
        {
            // Llama al método de la capa de negocios para generar el PDF
            byte[] pdf = new RN_Categoria().GenerarPDF();
            // Devolver el PDF como una descarga al usuario
            return File(pdf, "application/pdf", "DatosCategorias_" + DateTime.Now.ToString() + ".pdf");
        }

        public ActionResult DescargarPDF_Editorial()
        {
            // Llama al método de la capa de negocios para generar el PDF
            byte[] pdf = new RN_Editorial().GenerarPDF();
            // Devolver el PDF como una descarga al usuario
            return File(pdf, "application/pdf", "DatosEditoriales_" + DateTime.Now.ToString() + ".pdf");
        }

        public ActionResult DescargarPDF_Sala()
        {
            // Llama al método de la capa de negocios para generar el PDF
            byte[] pdf = new RN_Sala().GenerarPDF();
            // Devolver el PDF como una descarga al usuario
            return File(pdf, "application/pdf", "Salas_" + DateTime.Now.ToString() + ".pdf");
        }

        public ActionResult DescargarPDF_Autor()
        {
            // Llama al método de la capa de negocios para generar el PDF
            byte[] pdf = new RN_Autor().GenerarPDF();
            // Devolver el PDF como una descarga al usuario
            return File(pdf, "application/pdf", "Autores_" + DateTime.Now.ToString() + ".pdf");
        }

        #endregion

        /*--------------CATEGORIA---------------------*/
        #region CATEGORIA



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
        #endregion

        /*--------------SALA---------------------*/
        #region SALA
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarSala() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Sala> oLista = new List<EN_Sala>();
            oLista = new RN_Sala().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpPost]
        public JsonResult GuardarSala(EN_Sala objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.IdSala == "0")/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             una categoria nueva, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Sala().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a una categoria, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_Sala().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult EliminarSala(string id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Sala().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /*--------------EDITORIAL---------------------*/
        #region EDITORIAL
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarEditorial() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Editorial> oLista = new List<EN_Editorial>();
            oLista = new RN_Editorial().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpPost]
        public JsonResult GuardarEditorial(EN_Editorial objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.IdEditorial == "0")/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             una Editorial nueva, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Editorial().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a una Editorial, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_Editorial().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarEditorial(string id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Editorial().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /*--------------AUTOR---------------------*/
        #region AUTOR
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarAutor() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Autor> oLista = new List<EN_Autor>();
            oLista = new RN_Autor().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpPost]
        public JsonResult GuardarAutor(EN_Autor objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.IdAutor == "0")/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             una Autor nueva, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Autor().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a una Autor, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_Autor().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult EliminarAutor(string id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Autor().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /*-----------------------LIBRO----------------*/
        #region LIBRO

        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarLibro() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Libro> oLista = new List<EN_Libro>();
            oLista = new RN_Libro().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarCategoriaEnLibro() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Categoria> oLista = new List<EN_Categoria>();
            oLista = new RN_Categoria().ListarCategoriaEnLibro();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarEditorialEnLibro() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Editorial> oLista = new List<EN_Editorial>();
            oLista = new RN_Editorial().ListarEditorialEnLibro();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }
        [HttpPost]
        public JsonResult GuardarLibro(string objeto, HttpPostedFileBase archivoImagen) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            //object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            bool operacionExitosa = true;
            bool guardarImagenExito = true;

            EN_Libro oLibro = new EN_Libro();
            //Se referencia a "using Newtonsoft.Json;" para convertir el string objeto(de parametros) //a tipo objeto con JsonConvert
            oLibro = JsonConvert.DeserializeObject<EN_Libro>(objeto);

            //decimal paginas;
            //decimal ejemplares;
            ////decimal volumen;
            ///*Tratar de convertir el precio con un decimal con formato regional que necesitamos (Se referencia el using globalizatio para Culture Info
            // Al convertir el formato de texto en decimal va a decirle que los decimales son puntos, y que la cultura regional es de MX, el resultado de la convercion lo guarda en la variable precio (que es decimal*/
            //if (decimal.TryParse(oLibro.EjemplaresTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-MX"), out ejemplares))
            //{
            //    /*Si esta todo correcto
            //     * Ahora si pasa el valor convertido a Precio de la entidad EN_Productos
            //     */
            //    oLibro.Ejemplares = ejemplares;
            //}
            ////else if (decimal.TryParse(oLibro.PaginasTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-MX"), out paginas))
            ////{
            ////    /*Si esta todo correcto
            ////     * Ahora si pasa el valor convertido a Precio de la entidad EN_Productos
            ////     */
            ////    oLibro.Paginas = paginas;
            ////}
            ////else if (decimal.TryParse(oLibro.VolumenTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-MX"), out volumen))
            ////{
            ////    /*Si esta todo correcto
            ////     * Ahora si pasa el valor convertido a Precio de la entidad EN_Productos
            ////     */
            ////    oLibro.Volumen = volumen;
            ////}
            //else /*Si hay algun problemas*/
            //{
            //    return Json(new { operacionExitosa = false, mensaje = "El formato ejemplares debe ser en número" }, JsonRequestBehavior.AllowGet);
            //}


            if (oLibro.IdLibro == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             una Libro nuevo, por lo que se ha dado dando clic con el boton de crear*/
            {
                int idLibroGenerado = new RN_Libro().Registrar(oLibro, out mensaje);/*El metodo registrar
        //         de tipo string, devuelve el id registrado*/
                if (idLibroGenerado != 0)/*Si se pudo registrar correctamente el Libro*/
                {
                    oLibro.IdLibro = idLibroGenerado;/*Toma el id*/
                }
                else
                {
                    operacionExitosa = false;
                }

            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a una Libro, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                operacionExitosa = new RN_Libro().Editar(oLibro, out mensaje);
            }

            if (operacionExitosa)/*Si la operacion es true  pasamos  actualizar una ruta de imagen, guardar una imagen*/
            {
                if (archivoImagen != null)
                {
                    /*Toda la logica para guardar en la carpeta y actualizar la tablas*/
                    /*Ruta de imagen con ConfiguracionManager, haciendo referencia using a system.Configuration para acceder a webConfig*/
                    string rutaGuardar = ConfigurationManager.AppSettings["ServidorFotos"]; /*Guarda las imagenes en esa ruta especificada*/
                    /*Se referencia a .io para path*/
                    string extensionImagen = Path.GetExtension(archivoImagen.FileName);
                    /*Creamos un nombre de imagen personalizado, el codigo del Libro mas extension*/
                    string nombreImagen = string.Concat(oLibro.IdLibro.ToString(), extensionImagen);

                    try
                    {
                        /*Guarda en la ruta con un respectivo nombres*/
                        archivoImagen.SaveAs(Path.Combine(rutaGuardar, nombreImagen));
                    }
                    catch (Exception ex)
                    {
                        /*Si existe un error*/
                        string msg = ex.Message;
                        guardarImagenExito = false;
                    }
                    if (guardarImagenExito)/*Si es true*/
                    {
                        /*Para guardar en la base de datos*/
                        oLibro.RutaImagen = rutaGuardar;
                        oLibro.NombreImagen = nombreImagen;
                        bool respuesta = new RN_Libro().GuardarDatosImagen(oLibro, out mensaje);
                    }
                    else /*En el caso de que la imagen no haya sido guardada con exitos*/
                    {
                        /*Como el registro del Libro y el registro de la imagen son operaciones distintas
                         puede que se ejecute una de manera correcta y otra no*/
                        mensaje = "Se guardo el Libro pero hubo problemas con la imagen";
                    }
                }
            }
            return Json(new { operacionExitosa = operacionExitosa, idGenerado = oLibro.IdLibro, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            //return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        /*Metodo para ver imagen de Libro en venta, x64*/
        [HttpPost]
        public JsonResult ImagenLibro(int id)
        {
            bool conversion;
            /*Seleccionamos un Libro en especifico con where*/
            EN_Libro oLibro = new RN_Libro().Listar().Where(p => p.IdLibro == id).FirstOrDefault();

            /*Obtenemos la ruta de imagen convertida a base64*/
            string textoBase64 = RN_Recursos.ConvertirBase64(Path.Combine(oLibro.RutaImagen, oLibro.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textobase64 = textoBase64,
                extension = Path.GetExtension(oLibro.NombreImagen) /*El nombre de la imagen ya tiene concatenada la extension*/
            },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult EliminarLibro(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Libro().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult OperacionEjemplarLibro(int idLibro, bool sumar)//Va a permitir operar las cantidades del producto dentro del carrito
        {
           // int idLector = ((EN_Lector)Session["Lector"]).IdLector;//Obtenemos el idLector de acuerdo a la sesion iniciada donde estan los datos
            //de dicho clinte, convertimos la ssion a tipo Lector y traemos su id

            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new RN_Libro().OperacionEjemplarLibro(idLibro, sumar, out mensaje);//true es igual sumar = 1 = true, restar = 0 = false 

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}