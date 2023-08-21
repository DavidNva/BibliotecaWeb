using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class RN_Categoria
    {
        private BD_Categoria objCapaDato = new BD_Categoria(); /*Instancia una clase de la capa datos */

        public List<EN_Categoria> Listar() /*Usa una clase de la capa entidad*/
        {
            return objCapaDato.Listar();/*Retorna el metodo listar de la instancia de la capa Datos*/
        }

        public List<EN_Categoria> ListarCategoriaEnLibro() /*Usa una clase de la capa entidad*/
        {
            return objCapaDato.ListarCategoriaEnLibro();/*Retorna el metodo listar de la instancia de la capa Datos*/
        }

        public string Registrar(EN_Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoria no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return "0";/*No se ha creado la categoria*/
            }

        }

        public bool Editar(EN_Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoria no puede ser vacio";
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

        public bool Eliminar(string id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        public byte[] GenerarPDF()
        {
            return objCapaDato.DescargarPdf();
        }




        public byte[] DescargarPdf()
        //public ActionResult DescargarPdfCategoria<T>(List<T> oLista)
        {
            List<EN_Categoria> oLista = new List<EN_Categoria>();

            //oLista = new RN_Categoria().Listar();
            oLista = new BD_Categoria().Listar();

            var data = Document.Create(document =>
            {
                document.Page(page =>
                {
                    // page content
                    page.Margin(30);
                    // page.Header().Height(100).Background(Colors.Blue.Medium);
                    page.Header().ShowOnce().Row(row =>
                    {//el ShowOnce sirve para que el header solo aparezca en la primera hoja
                     //D:\ConsolePdf\ExportarPdf_Web\Content\images\cuborubikcode.png
                        var rutaImagen = Path.Combine("D:\\ConsolePdf\\ExportarPdf_Web\\Content\\images\\cuborubikcode.png");

                        byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);

                        row.ConstantItem(150).Image(imageData);

                        //row.ConstantItem(140).Height(60).Placeholder();//Elegimos el ancho del item

                        row.RelativeItem().Column(col =>//El ancho se coloca relativamente automatica
                        {
                            col.Item().AlignCenter().Text("Biblioteca: Luis Cabrera").Bold().FontSize(14);
                            col.Item().AlignCenter().Text("Puebla, Puebla").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("123 456 7890").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("example@gmail.com").Bold().FontSize(9);
                            //col.Item().Background(Colors.Orange.Medium).Height(10);
                            //col.Item().Background(Colors.Green.Medium).Height(10);
                        });
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text("RUC 1234567890");

                            col.Item().Background("#257272").Border(1)
                            .BorderColor("#257272").AlignCenter()
                            .Text("Boleto de Venta").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text("B0001 - 234");

                        });

                    });

                    // page.Content().Background(Colors.Yellow.Medium);
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Item().Column(col2 =>//Columna de datos de usuario
                        {
                            col2.Item().Text("Datos del Usuario").Underline().Bold();

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Nombre: ").SemiBold().FontSize(10);
                                txt.Span("David Nava").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("DNI: ").SemiBold().FontSize(10);
                                txt.Span("0877625727").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Dirección: ").SemiBold().FontSize(10);
                                txt.Span("Calle Luis Cabrera S/N").FontSize(10);
                            });
                        });

                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                //columns.RelativeColumn(3);
                                columns.ConstantColumn(100);
                                //columns.RelativeColumn();
                                columns.RelativeColumn();
                                //columns.RelativeColumn();
                                columns.ConstantColumn(100);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272")
                                 .Padding(2).Text("Código").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Descripción").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Activo").FontColor("#fff");
                            });

                            foreach (EN_Categoria cat in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.IdCategoria.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.Descripcion).FontSize(10);

                                if (cat.Activo)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("Sí").FontSize(10);
                                }
                                else
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("No").FontSize(10);
                                }
                            }


                        });

                        col1.Item().AlignRight().Text("Total: 1500").FontSize(12);


                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)//Seccion de comentarios
                        .Column(column =>
                        {
                            column.Item().Text("Comentarios").FontSize(14);
                            column.Item().Text(Placeholders.LoremIpsum());
                            column.Spacing(5);
                        });

                        col1.Spacing(10);
                    });

                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);

                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                    //page.Footer().Height(50).Background(Colors.Red.Medium);
                });
            }).GeneratePdf();

            MemoryStream stream = new MemoryStream(data);
            //return stream.
            return stream.ToArray();
            //return File(stream, "applicacion/pdf", "detallePrestamo.pdf");
            //return View();
        }

    }
}
