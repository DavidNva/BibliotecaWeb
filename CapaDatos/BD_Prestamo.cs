using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data; /*Acceso a sql conections*/
using System.Globalization;
using QuestPDF.Fluent;//Para exportar a pdf
using QuestPDF.Helpers;
using System.IO;

namespace CapaDatos
{
    public class BD_Prestamo
    {
        public bool Registrar(EN_Prestamo obj, DataTable DetallePrestamo,/* DataTable EjemplarActivo,*/ out string Mensaje)//out indica parametro de salida
        {
            bool respuesta = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("Id_Lector", obj.oId_Lector.IdLector);
                    cmd.Parameters.AddWithValue("IdLibro", obj.oId_Libro.IdLibro);
                    cmd.Parameters.AddWithValue("TotalLibro", obj.TotalLibro);
                    cmd.Parameters.AddWithValue("DiasDePrestamo", obj.DiasDePrestamo);
                    cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);
                    //SE AGREGO ESTA COLUMNA PARA APLICAR LA ELIMINACION EN CASCADA EN CASO DE QUE SE ELIMINE EL LIBRO
                    cmd.Parameters.AddWithValue("Id_Ejemplar", obj.oId_Ejemplar.IdEjemplarLibro);

                    cmd.Parameters.AddWithValue("DetallePrestamo", DetallePrestamo);//El data table debe tener las mismas columnas de la estructura creada
                                                                                    //en sql (las 3 creadas: IdLibro, Cantidad, Total)

                    // cmd.Parameters.AddWithValue("EjemplarActivo", EjemplarActivo);

                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

        public int RegistrarPrestamo2(EN_Prestamo obj, out string Mensaje)//out indica parametro de salida
        {
            int IdAutogenerado = 0; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPrestamo2", oConexion);
                    cmd.Parameters.AddWithValue("Id_Lector", obj.oId_Lector.IdLector);
                    cmd.Parameters.AddWithValue("TotalLibro", obj.TotalLibro);
                    cmd.Parameters.AddWithValue("DiasDePrestamo", obj.DiasDePrestamo);
                    cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);
                    cmd.Parameters.AddWithValue("Id_Ejemplar", obj.oId_Ejemplar.IdEjemplarLibro);

                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    IdAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdAutogenerado = 0;
                Mensaje = ex.Message;

            }
            return IdAutogenerado;

        }

        public List<EN_DetallePrestamo> ListarPrestamos(int idLector)
        {
            List<EN_DetallePrestamo> lista = new List<EN_DetallePrestamo>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_ListarPrestamos(@idLector)";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@idLector", idLector);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Libro la lista*/
                                new EN_DetallePrestamo()
                                {
                                    oId_Libro = new EN_Libro()//Esto era EN_Libro (Recordar que se cambio a Ejemplar)
                                    {

                                        Codigo = dr["Codigo"].ToString(),
                                        Titulo = dr["Titulo"].ToString(),
                                        //Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),//Indica que los decimales los trabaje con puntos
                                        //oId_Categoria = new EN_Categoria() { Descripcion = dr["Cantidad"].ToString() },
                                        RutaImagen = dr["RutaImagen"].ToString(),
                                        NombreImagen = dr["NombreImagen"].ToString()
                                    },
                                    CantidadEjemplares = Convert.ToInt32(dr["CantidadEjemplares"]),
                                    Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-MX")),
                                    IdDetallePrestamo = Convert.ToInt32(dr["IdDetallePrestamo"]),


                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_DetallePrestamo>();
            }

            return lista;
        }

        /*
         *  // Convertir FechaPrestamo y FechaDevolucion a DateTime
                    DateTime fechaPrestamo = DateTime.Parse(obj.FechaPrestamo, CultureInfo.InvariantCulture);
                    DateTime fechaDevolucion = DateTime.Parse(obj.FechaDevolucion, CultureInfo.InvariantCulture);



                    //cmd.Parameters.AddWithValue("FechaPrestamo", obj.FechaPrestamo);
                    //cmd.Parameters.AddWithValue("FechaDevolucion", obj.FechaDevolucion);

                    cmd.Parameters.AddWithValue("FechaPrestamo", fechaPrestamo);
                    cmd.Parameters.AddWithValue("FechaDevolucion", fechaDevolucion);
         */


        public bool Editar(EN_Prestamo obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            DateTime fechaPrestamo = Convert.ToDateTime(obj.FechaPrestamo);
            
            DateTime fechaDevolucion = Convert.ToDateTime(obj.FechaDevolucion);


            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("IdPrestamo", obj.IdPrestamo);
                    cmd.Parameters.AddWithValue("Id_Lector", obj.oId_Lector.IdLector);
                    cmd.Parameters.AddWithValue("TotalLibro", obj.TotalLibro);
                    //cmd.Parameters.AddWithValue("Activo", obj.Activo); 
                    //cmd.Parameters.AddWithValue("FechaPrestamo", obj.FechaPrestamo);
                    //cmd.Parameters.AddWithValue("FechaDevolucion", obj.FechaDevolucion);

                    cmd.Parameters.AddWithValue("FechaPrestamo", fechaPrestamo.ToString("MM-dd-yyyy"));
                    //cmd.Parameters.AddWithValue("FechaDevolucion", fechaDevolucion.ToString("MM-dd-yyyy"));

                    cmd.Parameters.AddWithValue("DiasDePrestamo", obj.DiasDePrestamo);
                    cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);

                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        //sp_FinalizarPrestamo
        public bool FinalizarPrestamo(EN_Prestamo obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            DateTime fechaPrestamo = Convert.ToDateTime(obj.FechaPrestamo);

            DateTime fechaDevolucion = Convert.ToDateTime(obj.FechaDevolucion);


            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_FinalizarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("IdPrestamo", obj.IdPrestamo);
                    //cmd.Parameters.AddWithValue("Id_Lector", obj.oId_Lector.IdLector);
                    //cmd.Parameters.AddWithValue("TotalLibro", obj.TotalLibro);
                    //cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    //cmd.Parameters.AddWithValue("FechaPrestamo", obj.FechaPrestamo);
                    //cmd.Parameters.AddWithValue("FechaDevolucion", obj.FechaDevolucion);

                    //cmd.Parameters.AddWithValue("FechaPrestamo", fechaPrestamo.ToString("MM-dd-yyyy"));
                    cmd.Parameters.AddWithValue("FechaDevolucion", fechaDevolucion.ToString("MM-dd-yyyy"));

                    //cmd.Parameters.AddWithValue("DiasDePrestamo", obj.DiasDePrestamo);
                    cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);
                    cmd.Parameters.AddWithValue("IdEjemplarLibro", obj.oId_Ejemplar.IdEjemplarLibro);
                    cmd.Parameters.AddWithValue("IdLibro", obj.oId_Libro.IdLibro);

                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public bool Eliminar(int id, int idEjemplarLibro, int idLibro, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("IdPrestamo", id);
                    cmd.Parameters.AddWithValue("IdEjemplarLibro", idEjemplarLibro);
                    cmd.Parameters.AddWithValue("IdLibro", idLibro);
                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public List<EN_Prestamo> ListarPrestamosCompleto()
        {
            List<EN_Prestamo> lista = new List<EN_Prestamo>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    StringBuilder sb = new StringBuilder(); //Permite hacer saltos de linea
                    sb.AppendLine("SELECT p.IdPrestamo, lec.IDLector, CONCAT(lec.Nombres,' ',lec.Apellidos) [NombreLector],");
                    sb.AppendLine("p.TotalLibro, ");
                    sb.AppendLine("CONVERT(char(10), p.FechaPrestamo,103) [FechaPrestamo],");
                    sb.AppendLine("CONVERT(char(10), p.FechaDevolucion,103) [FechaDevolucion],");
                    sb.AppendLine("p.DiasDePrestamo, p.Observaciones, p.Activo,dp.IdEjemplar ");
                    sb.AppendLine(",l.IdLibro,l.Titulo");
                    //b.AppendLine(",l.RutaImagen, l.NombreImagen,l.Codigo,l.Titulo,");
                    //sb.AppendLine("DP.CantidadEjemplares, DP.Total, DP.IdDetallePrestamo ");
                    sb.AppendLine("FROM DETALLEPrestamo DP");
                    sb.AppendLine("INNER JOIN Ejemplar ej ON ej.IDEjemplarLibro = DP.IDEjemplar");
                    sb.AppendLine("INNER JOIN Libro l ON l.IdLibro = ej.ID_Libro");
                    sb.AppendLine("INNER JOIN Prestamo p ON p.IdPrestamo = DP.IdPrestamo");
                    sb.AppendLine("inner join Lector lec on lec.IdLector = p.ID_Lector order by DP.IdDetallePrestamo DESC");


                    //string query = "select * from fn_ListarPrestamos(@idLector)";

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    //cmd.Parameters.AddWithValue("@idLector", idLector);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Libro la lista*/
                                new EN_Prestamo()
                                {
                                    IdPrestamo = Convert.ToInt32(dr["IdPrestamo"]),
                                    oId_Lector = new EN_Lector() { IdLector = Convert.ToInt32(dr["IDLector"]), NombreCompletoLector = dr["NombreLector"].ToString() },
                                    TotalLibro = Convert.ToInt32(dr["TotalLibro"]),
                                    FechaPrestamo = dr["FechaPrestamo"].ToString(),
                                    FechaDevolucion = dr["FechaDevolucion"].ToString(),
                                    DiasDePrestamo = Convert.ToInt32(dr["DiasDePrestamo"]), 
                                    Observaciones = dr["Observaciones"].ToString(),
                                    //Tipo = Convert.ToInt32(dr["Tipo"]),   
                                    Activo = Convert.ToBoolean(dr["Activo"]),
                                    oId_Libro = new EN_Libro() { IdLibro = Convert.ToInt32(dr["IdLibro"]), Titulo = dr["Titulo"].ToString() },
                                    oId_Ejemplar = new EN_Ejemplar() { IdEjemplarLibro = Convert.ToInt32(dr["IdEjemplar"])}
                                    //oDetallePrestamo = new EN_DetallePrestamo() { IdDetallePrestamo = Convert.ToInt32(dr["IdDetallePrestamo"]), Total = dr["NombreLector"].ToString() },
                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Prestamo>();
            }

            return lista;
        }

        public byte[] GenerarPDF() //public ActionResult DescargarPdfPrestamo<T>(List<T> oLista)
        {
            List<EN_Prestamo> oLista = new List<EN_Prestamo>();

            //oLista = new RN_Prestamo().Listar();
            oLista = new BD_Prestamo().ListarPrestamosCompleto();

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
                            col.Item().AlignCenter().Text("Biblioteca: Luis Cabrera Lobato").Bold().FontSize(14);
                            col.Item().AlignCenter().Text("Puebla, Puebla").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("123 456 7890").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("example@gmail.com").Bold().FontSize(9);
                            //col.Item().Background(Colors.Orange.Medium).Height(10);
                            //col.Item().Background(Colors.Green.Medium).Height(10);
                        });
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text("Biblioteca");

                            col.Item().Background("#257272").Border(1)
                            .BorderColor("#257272").AlignCenter()
                            .Text("Préstamos").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text(DateTime.Now.ToString("dd-MM-yyyy"));

                        });

                    });

                    int totalPendientes = 0;
                    int totalDevueltos = 0;
                    // page.Content().Background(Colors.Yellow.Medium);
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        int totalPrestamo = 0;
                        
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(55);//Activo

                                columns.RelativeColumn();//Libro en prestamo
                                columns.RelativeColumn();//Lector
                                columns.ConstantColumn(60);//Fecha Prestamo

                                columns.ConstantColumn(65);//Fecha Devolución
                               
                                
                                columns.ConstantColumn(35);//Dias
                                columns.RelativeColumn();//Observaciones

                                
                            });

                            tabla.Header(header =>
                            {
                                //header.Cell().Background("#257272")
                                // .Padding(2).Text("Código").FontColor("#fff");
                                header.Cell().Background("#257272")
                                .Padding(2).Text("Préstamo").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Libro").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Lector").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Préstamo").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Devolución").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Dias").FontColor("#fff");

                                header.Cell().Background("#257272")
                              .Padding(2).Text("Observaciones").FontColor("#fff");

                                
                            });

                            foreach (EN_Prestamo prestamo in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {
                                if (prestamo.Activo)
                                {
                                    tabla.Cell().BorderBottom(0.5f).Background("#DC3545").BorderColor("#D9D9D9")
                                    .Padding(2).Text("Pendiente").FontSize(10).Bold().FontColor("#FFFFFF");//.BackgroundColor("#DC3545");
                                    totalPendientes++;
                                }
                                else
                                {
                                    tabla.Cell().BorderBottom(0.5f).Background("#198754").BorderColor("#D9D9D9")
                                    .Padding(2).Text("Devuelto").FontSize(10).FontColor("#FFFFFF");//.BackgroundColor("#198754"); Si solo queremos que el background aplique al texto
                                    totalDevueltos++;
                                }

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(prestamo.oId_Libro.Titulo).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(prestamo.oId_Lector.NombreCompletoLector.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(prestamo.FechaPrestamo.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(prestamo.FechaDevolucion.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(prestamo.DiasDePrestamo.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).Text(prestamo.Observaciones).FontSize(10);

                               

                               
                                totalPrestamo++;
                            }


                        });

                        //col1.Item().AlignRight().Text("Total: 1500").FontSize(12);
                        //col1.Item().AlignRight().Text($"Total de Préstamos: {totalPrestamo}").FontSize(12);
                        //col1.Item().AlignRight().Text($"Total de Préstamos: {totalPendientes}").FontSize(12);
                        //col1.Item().AlignRight().Text($"Total de Préstamos: {totalDevueltos}").FontSize(12);

                        

                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)//Seccion de comentarios
                        .Column(column =>
                        {
                            //column.Item().Text("Comentarios").FontSize(14);
                            //column.Item().Text(Placeholders.LoremIpsum());
                            //column.Spacing(5);
                            column.Item().AlignRight().Text($"Total de Préstamos: {totalPrestamo}").FontSize(12);
                            column.Item().AlignRight().Text($"Pendientes: {totalPendientes} | Devueltos: {totalDevueltos}").FontSize(12);
                            //column.Item().AlignRight().Text($"Total de Préstamos Devueltos: {totalDevueltos}").FontSize(12);
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