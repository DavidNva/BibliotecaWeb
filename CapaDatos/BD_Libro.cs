using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.IO;

namespace CapaDatos
{
    public class BD_Libro
    {
        public List<EN_Libro> Listar()
        {
            List<EN_Libro> lista = new List<EN_Libro>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                   StringBuilder sb = new StringBuilder(); //Permite hacer saltos de linea

                    //En este punto (21-07-23) se agrega distinct y el inner join con Ejemplar para validar que 
                    //cuando se liste los libros no se listen aquellos que aun no tienen relacion con ejemplar y idLibro
                    //ya que estos dos van de la mano, por lo que el numero de ejemplares de libro debe ser igual 
                    //al total de ejemplares referenciados de ese idLibro en la tabla ejemplar
                    sb.AppendLine("select distinct l.IdLibro, l.Codigo, l.Titulo, l.Paginas,c.IDCategoria, c.Descripcion[DesCategoria],");
                    sb.AppendLine("e.IDEditorial, e.Descripcion[DesEditorial], s.IDSala, s.Descripcion[DesSala],");
                    sb.AppendLine("l.Ejemplares,l.AñoEdicion, l.Volumen, l.RutaImagen, l.NombreImagen,l.Observaciones, l.Activo");
                    sb.AppendLine("from Libro l");
                    sb.AppendLine("inner join Categoria c on c.IDCategoria = l.Id_Categoria");
                    sb.AppendLine("inner join Editorial e on e.IDEditorial = l.ID_Editorial");
                    sb.AppendLine("inner join Ejemplar ej on ej.ID_Libro = l.IDLibro ");
                    sb.AppendLine("inner join Sala s on s.IDSala = l.ID_Sala order by l.IDLibro desc");
                    //string query = "select IDLibro,Nombres,Apellidos,Ciudad, Calle, Telefono, Correo,Clave,Tipo,Reestablecer,Activo from Libro";                    //string query = "select IDLibro,Nombres,Apellidos,Ciudad, Calle, Telefono, Correo,Clave,tp.IdTipoPersona,tp.Descripcion[Tipo],Reestablecer,Activo from Libro inner join TipoPersona TP on tp.IdTipoPersona = Libro.Tipo";

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo Libro a la lista*/
                                new EN_Libro()
                                {
                                    IdLibro = Convert.ToInt32(dr["IdLibro"]),
                                    Codigo = dr["Codigo"].ToString(),
                                    Titulo = dr["Titulo"].ToString(),
                                    //Ubicacion = dr["Ubicacion"].ToString(),
                                    Paginas = Convert.ToInt32(dr["Paginas"]),
                                    oId_Categoria = new EN_Categoria() { IdCategoria = dr["IDCategoria"].ToString(), Descripcion = dr["DesCategoria"].ToString() },
                                    oId_Editorial = new EN_Editorial() { IdEditorial = dr["IdEditorial"].ToString(), Descripcion = dr["DesEditorial"].ToString() },
                                    oId_Sala = new EN_Sala() { IdSala = dr["IdSala"].ToString(), Descripcion = dr["DesSala"].ToString() },
                                    Ejemplares = Convert.ToInt32(dr["Ejemplares"]),
                                    AñoEdicion = dr["AñoEdicion"].ToString(),
                                    Volumen = Convert.ToInt32(dr["Volumen"]),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString(),
                                    Observaciones = dr["Observaciones"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Libro>();
            }

            return lista;
        }

        public int Registrar(EN_Libro obj, out string Mensaje)//out indica parametro de salida
        {
            int IdAutogenerado = 0; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLibro", oConexion);
                   cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Titulo", obj.Titulo);
                    cmd.Parameters.AddWithValue("Paginas", obj.Paginas);
                    //cmd.Parameters.AddWithValue("Ubicacion", obj.Ubicacion);
                    cmd.Parameters.AddWithValue("IDCategoria", obj.oId_Categoria.IdCategoria);
                    cmd.Parameters.AddWithValue("IDEditorial", obj.oId_Editorial.IdEditorial);
                    cmd.Parameters.AddWithValue("IDSala", obj.oId_Sala.IdSala);
                    cmd.Parameters.AddWithValue("Ejemplares", obj.Ejemplares);
                    cmd.Parameters.AddWithValue("AñoEdicion", obj.AñoEdicion);
                    cmd.Parameters.AddWithValue("Volumen", obj.Volumen);
                    cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool Editar(EN_Libro obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarLibro", oConexion);
                    cmd.Parameters.AddWithValue("IdLibro", obj.IdLibro);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Titulo", obj.Titulo);
                    cmd.Parameters.AddWithValue("Paginas", obj.Paginas);
                    //cmd.Parameters.AddWithValue("Ubicacion", obj.Ubicacion);
                    cmd.Parameters.AddWithValue("IDCategoria", obj.oId_Categoria.IdCategoria);
                    cmd.Parameters.AddWithValue("IDEditorial", obj.oId_Editorial.IdEditorial);
                    cmd.Parameters.AddWithValue("IDSala", obj.oId_Sala.IdSala);
                    cmd.Parameters.AddWithValue("Ejemplares", obj.Ejemplares);
                    cmd.Parameters.AddWithValue("AñoEdicion", obj.AñoEdicion);
                    cmd.Parameters.AddWithValue("Volumen", obj.Volumen);
                    cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool GuardarDatosImagen(EN_Libro obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "update Libro set RutaImagen = @rutaImagen, NombreImagen = @nombreImagen where IdLibro = @idLibro";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@rutaImagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreImagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("@idLibro", obj.IdLibro);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true; /*Si la query realiza una accion, mayor de 0*/
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen";
                    }

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public bool Eliminar(int id, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarLibro", oConexion);
                    cmd.Parameters.AddWithValue("IdLibro", id);
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

        public bool OperacionEjemplarLibro(int idLibro, bool sumar, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = true;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionEjemplarLibro", oConexion);
                    //cmd.Parameters.AddWithValue("IdLector", idLector);
                    cmd.Parameters.AddWithValue("IdLibro", idLibro);//ya tiene el id del libro, que ya estuve relacionado con el idejemplarlibro
                    cmd.Parameters.AddWithValue("Sumar", sumar);
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

        public byte[] GenerarPDF() //public ActionResult DescargarPdfLibro<T>(List<T> oLista)
        {
            List<EN_Libro> oLista = new List<EN_Libro>();

            //oLista = new RN_Libro().Listar();
            oLista = new BD_Libro().Listar();

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
                            .Text("Libros").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text(DateTime.Now.ToString("dd-MM-yyyy"));

                        });

                    });

                    // page.Content().Background(Colors.Yellow.Medium);
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        int totalLibro = 0;
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();//Codigo

                                columns.RelativeColumn();//Libro
                                
                                columns.ConstantColumn(30);//Pág

                                columns.RelativeColumn();//Categoria
                                columns.RelativeColumn();//Editorial
                                columns.ConstantColumn(40);//Sala

                                columns.ConstantColumn(35);//Stock

                                columns.RelativeColumn();//Observaciones
                                columns.ConstantColumn(40);//Activo
                                //columns.RelativeColumn();//Observaciones


                            });

                            tabla.Header(header =>
                            {
                                //header.Cell().Background("#257272")
                                // .Padding(2).Text("Código").FontColor("#fff");
                                header.Cell().Background("#257272")
                                .Padding(2).Text("Código").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Libro").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Pág").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Categoria").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Editorial").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Sala").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Stock").FontColor("#fff");

                                header.Cell().Background("#257272")
                              .Padding(2).Text("Observación").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Activo").FontColor("#fff");


                            });

                            foreach (EN_Libro Libro in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(Libro.Codigo).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(Libro.Titulo.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(Libro.Paginas.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(Libro.oId_Categoria.Descripcion.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(Libro.oId_Editorial.Descripcion.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(Libro.oId_Sala.Descripcion.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").AlignCenter()
                                .Padding(2).Text(Libro.Ejemplares.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).Text(Libro.Observaciones).FontSize(10);

                                if (Libro.Activo)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("Sí").FontSize(10);
                                }
                                else
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("No").FontSize(10);
                                }


                                totalLibro++;
                            }


                        });

                        //col1.Item().AlignRight().Text("Total: 1500").FontSize(12);
                        col1.Item().AlignRight().Text($"Total de Libros: {totalLibro}").FontSize(12);


                        //col1.Item().Background(Colors.Grey.Lighten3).Padding(10)//Seccion de comentarios
                        //.Column(column =>
                        //{
                        //    column.Item().Text("Comentarios").FontSize(14);
                        //    column.Item().Text(Placeholders.LoremIpsum());
                        //    column.Spacing(5);
                        //});

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
            //return File(stream, "applicacion/pdf", "detalleLibro.pdf");
            //return View();
        }
        //    List<EN_Libro> lista = new List<EN_Libro>();
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            StringBuilder sb = new StringBuilder(); //Permite hacer saltos de linea

        //            sb.AppendLine("select l.IdLibro, l.Titulo, l.Ubicacion, l.Paginas,c.IDCategoria, c.Descripcion[DesCategoria],");
        //            sb.AppendLine("e.IDEditorial, e.Descripcion[DesEditorial], s.IDSala, s.Descripcion[DesSala],");
        //            sb.AppendLine("l.Ejemplares,l.AñoEdicion, l.Volumen, l.RutaImagen, l.NombreImagen,l.Observaciones, l.Activo");
        //            sb.AppendLine("from Libro l");
        //            sb.AppendLine("inner join Categoria c on c.IDCategoria = l.Id_Categoria");
        //            sb.AppendLine("inner join Editorial e on e.IDEditorial = l.ID_Editorial");
        //            sb.AppendLine("inner join Sala s on s.IDSala = l.ID_Sala");

        //            SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
        //            cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
        //            {
        //                while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
        //                {
        //                    lista.Add(/*Agrega una nueva Libro la lista*/
        //                        new EN_Libro()
        //                        {
        //                            IdLibro = dr["IdLibro"].ToString(),
        //                            Titulo = dr["Titulo"].ToString(),
        //                            //Ubicacion = dr["Ubicacion"].ToString(),
        //                            Paginas = Convert.ToInt32(dr["Paginas"]),
        //                            oId_Categoria = new EN_Categoria() { IdCategoria = dr["IDCategoria"].ToString(), Descripcion = dr["DesCategoria"].ToString() },
        //                            oId_Editorial = new EN_Editorial() { IdEditorial = dr["IdEditorial"].ToString(), Descripcion = dr["DesEditorial"].ToString() },
        //                            oId_Sala = new EN_Sala() { IdSala = dr["IdSala"].ToString(), Descripcion = dr["DesSala"].ToString() },
        //                            Ejemplares = Convert.ToInt32(dr["Ejemplares"]), 
        //                            AñoEdicion = dr["AñoEdicion"].ToString(),
        //                            Volumen = Convert.ToInt32(dr["Volumen"]), 
        //                            RutaImagen = dr["RutaImagen"].ToString(),
        //                            NombreImagen = dr["NombreImagen"].ToString(),
        //                            Observaciones = dr["Observaciones"].ToString(),
        //                            Activo = Convert.ToBoolean(dr["Activo"])
        //                        });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        lista = new List<EN_Libro>();
        //    }

        //    return lista;
        //}

        //public string Registrar(EN_Libro obj, out string Mensaje)//out indica parametro de salida
        //{
        //    string IdAutogenerado = "0"; /*Recibe el id autogenerado*/

        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_RegistrarLibro", oConexion);
        //            /*Los primeros valores de estos parametros, es la del procedimiento del sql y el segundo de donde toma el valor (las propieaddes de la clase EN_Libro*/
        //            cmd.Parameters.AddWithValue("IdLibro", obj.IdLibro);
        //            cmd.Parameters.AddWithValue("Titulo", obj.Titulo);
        //            cmd.Parameters.AddWithValue("Paginas", obj.Paginas);
        //            //cmd.Parameters.AddWithValue("Ubicacion", obj.Ubicacion);
        //            cmd.Parameters.AddWithValue("IDCategoria", obj.oId_Categoria.IdCategoria);
        //            cmd.Parameters.AddWithValue("IDEditorial", obj.oId_Editorial.IdEditorial);
        //            cmd.Parameters.AddWithValue("IDSala", obj.oId_Sala.IdSala);
        //            cmd.Parameters.AddWithValue("Ejemplares", obj.Ejemplares);
        //            cmd.Parameters.AddWithValue("AñoEdicion", obj.AñoEdicion);
        //            cmd.Parameters.AddWithValue("Volumen", obj.Volumen);
        //            cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);
        //            cmd.Parameters.AddWithValue("Activo", obj.Activo);
        //            //Dos parametros de salida, un entero de resultaado y un string de mensaje
        //            cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            oConexion.Open();
        //            cmd.ExecuteNonQuery();
        //            IdAutogenerado = cmd.Parameters["Resultado"].Value.ToString();
        //            Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        IdAutogenerado = "0";/*Regresa a 0*/
        //        Mensaje = ex.Message;

        //    }
        //    return IdAutogenerado; /*Si cambia a un nuevo valor al agregar un nuevo id*/

        //}

        //public bool Editar(EN_Libro obj, out string Mensaje)//out indica parametro de salida
        //{
        //    bool resultado = false;

        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_EditarLibro", oConexion);
        //            cmd.Parameters.AddWithValue("IdLibro", obj.IdLibro);
        //            cmd.Parameters.AddWithValue("Titulo", obj.Titulo);
        //            cmd.Parameters.AddWithValue("Paginas", obj.Paginas);
        //            //cmd.Parameters.AddWithValue("Ubicacion", obj.Ubicacion);
        //            cmd.Parameters.AddWithValue("IDCategoria", obj.oId_Categoria.IdCategoria);
        //            cmd.Parameters.AddWithValue("IDEditorial", obj.oId_Editorial.IdEditorial);
        //            cmd.Parameters.AddWithValue("IDSala", obj.oId_Sala.IdSala);
        //            cmd.Parameters.AddWithValue("Ejemplares", obj.Ejemplares);
        //            cmd.Parameters.AddWithValue("AñoEdicion", obj.AñoEdicion);
        //            cmd.Parameters.AddWithValue("Volumen", obj.Volumen);
        //            cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);
        //            cmd.Parameters.AddWithValue("Activo", obj.Activo);
        //            //Dos parametros de salida, un entero de resultaado y un string de mensaje
        //            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            oConexion.Open();
        //            cmd.ExecuteNonQuery();
        //            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
        //            Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = false;
        //        Mensaje = ex.Message;

        //    }
        //    return resultado;
        //}


        //public bool GuardarDatosImagen(EN_Libro obj, out string Mensaje)
        //{
        //    bool resultado = false;
        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            string query = "update Libro set RutaImagen = @rutaImagen, NombreImagen = @nombreImagen where IdLibro = @idLibro";

        //            SqlCommand cmd = new SqlCommand(query, oConexion);
        //            cmd.Parameters.AddWithValue("@rutaImagen", obj.RutaImagen);
        //            cmd.Parameters.AddWithValue("@nombreImagen", obj.NombreImagen);
        //            cmd.Parameters.AddWithValue("@idLibro", obj.IdLibro);
        //            cmd.CommandType = CommandType.Text;

        //            oConexion.Open();
        //            if (cmd.ExecuteNonQuery() > 0)
        //            {
        //                resultado = true; /*Si la query realiza una accion, mayor de 0*/
        //            }
        //            else
        //            {
        //                Mensaje = "No se pudo actualizar imagen";
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = false;
        //        Mensaje = ex.Message;

        //    }
        //    return resultado;
        //}

        //public bool Eliminar(string id, out string Mensaje)//out indica parametro de salida
        //{
        //    bool resultado = false;

        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_EliminarLibro", oConexion);
        //            cmd.Parameters.AddWithValue("IdLibro", id);
        //            //Dos parametros de salida, un entero de resultaado y un string de mensaje
        //            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            oConexion.Open();

        //            cmd.ExecuteNonQuery();

        //            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
        //            Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = false;
        //        Mensaje = ex.Message;

        //    }
        //    return resultado;
        //}
    }
}
