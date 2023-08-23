using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data; /*Acceso a sql conections*/
/*La capa datos referencia a la capa entidad*/
/*La capa Negocio hace referencia a la capa entidad y a la capa datos*/
/*La capa presentacion (CapaPresentacionAdmin y CapaPresentacionTienda) hace referencia a la capa entiad y negocio*/
using QuestPDF.Fluent;//Para exportar a pdf
using QuestPDF.Helpers;
using System.IO;

namespace CapaDatos
{
    public class BD_Usuarios
    {
        public List<EN_Usuario> Listar()
        {
            List<EN_Usuario> lista = new List<EN_Usuario>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select u.IDUsuario,u.Nombres,u.Apellidos,u.Ciudad, u.Calle, u.Telefono, u.Correo,u.Clave,");
                    sb.AppendLine("tp.IdTipoPersona,tp.Descripcion [Tipo], u.Reestablecer,u.Activo");
                    sb.AppendLine("from USUARIO u ");
                    sb.AppendLine("inner join TipoPersona tp on tp.IdTipoPersona = u.Tipo");
                    //string query = "select IDUsuario,Nombres,Apellidos,Ciudad, Calle, Telefono, Correo,Clave,Tipo,Reestablecer,Activo from USUARIO";                    //string query = "select IDUsuario,Nombres,Apellidos,Ciudad, Calle, Telefono, Correo,Clave,tp.IdTipoPersona,tp.Descripcion[Tipo],Reestablecer,Activo from USUARIO inner join TipoPersona TP on tp.IdTipoPersona = Usuario.Tipo";
                    
                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo usuario a la lista*/
                                new EN_Usuario()
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Ciudad = dr["Ciudad"].ToString(),
                                    Calle = dr["Calle"].ToString(),
                                    Telefono = dr["Telefono"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    //Tipo = Convert.ToInt32(dr["Tipo"]),
                                    oId_TipoPersona = new EN_TipoPersona() {IdTipoPersona = Convert.ToInt32(dr["IdTipoPersona"]), Descripcion = dr["Tipo"].ToString() },
                                    Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),/*Admite 1 y 0*/
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Usuario>();
            }

            return lista;
        }

        public int Registrar(EN_Usuario obj, out string Mensaje)//out indica parametro de salida
        {
            int IdAutogenerado = 0; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Ciudad", obj.Ciudad);
                    cmd.Parameters.AddWithValue("Calle", obj.Calle);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Tipo", obj.oId_TipoPersona.IdTipoPersona);
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

        public bool Editar(EN_Usuario obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("IDUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Ciudad", obj.Ciudad);
                    cmd.Parameters.AddWithValue("Calle", obj.Calle);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Tipo", obj.oId_TipoPersona.IdTipoPersona);
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

        public bool Eliminar(int id, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", id);
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

        public bool CambiarClave(int idUsuario, string nuevaClave, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update usuario set clave = @nuevaClave, reestablecer = 0 where IdUsuario = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@Id", idUsuario);
                    cmd.Parameters.AddWithValue("nuevaClave", nuevaClave);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    //El ExecuteNonQuery ejecuta una accion y devuelve el numero de filas afectadas
                    //Cuando eliminamos un registro de la tabla, entonces si el total de filas afectadas
                    //es mayor a 0 entonces será verdadero, pero si no es mayor a 0, entonces significa
                    //que hubo un problema al eliminar por lo que enviara un false, eso lo almacenamos en resultado
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public bool ReestablecerClave(int idUsuario, string clave, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update usuario set clave = @clave, reestablecer = 1 where IdUsuario = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@Id", idUsuario);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    //El ExecuteNonQuery ejecuta una accion y devuelve el numero de filas afectadas
                    //Cuando eliminamos un registro de la tabla, entonces si el total de filas afectadas
                    //es mayor a 0 entonces será verdadero, pero si no es mayor a 0, entonces significa
                    //que hubo un problema al eliminar por lo que enviara un false, eso lo almacenamos en resultado
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public byte[] GenerarPDF() //public ActionResult DescargarPdfUsuarios<T>(List<T> oLista)
        {
            List<EN_Usuario> oLista = new List<EN_Usuario>();

            //oLista = new RN_Usuarios().Listar();
            oLista = new BD_Usuarios().Listar();

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
                            .Text("Usuarios").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text(DateTime.Now.ToString("dd-MM-yyyy"));

                        });

                    });

                    // page.Content().Background(Colors.Yellow.Medium);
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        int totalUsuarios = 0;
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                //columns.RelativeColumn(3);
                                
                                //columns.ConstantColumn(100);
                                //columns.RelativeColumn();
                                columns.RelativeColumn();//Nombre de  Usuario

                                columns.RelativeColumn();//Domicilio
                                columns.RelativeColumn();//Telefono
                                columns.RelativeColumn();//Correo
                                //columns.RelativeColumn();//Tipo
                                
                                columns.ConstantColumn(60);//Tipo
                                columns.ConstantColumn(50);//Activo
                            });

                            tabla.Header(header =>
                            {
                                //header.Cell().Background("#257272")
                                // .Padding(2).Text("Código").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Usuario").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(2).Text("Domicilio").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Telefono").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Correo").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Tipo").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Activo").FontColor("#fff");
                            });

                            foreach (EN_Usuario Usuarios in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {

                                //tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                //.Padding(2).Text(Usuarios.IdUsuario.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(Usuarios.Nombres + " " + Usuarios.Apellidos).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(Usuarios.Ciudad + ", Calle: " + Usuarios.Calle).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(Usuarios.Telefono.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(Usuarios.Correo.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(Usuarios.oId_TipoPersona.Descripcion.ToString()).FontSize(10);

                                if (Usuarios.Activo)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("Sí").FontSize(10);
                                }
                                else
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("No").FontSize(10);
                                }
                                totalUsuarios++;
                            }


                        });

                        //col1.Item().AlignRight().Text("Total: 1500").FontSize(12);
                        col1.Item().AlignRight().Text($"Total de Usuarios: {totalUsuarios}").FontSize(12);


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
            //return File(stream, "applicacion/pdf", "detallePrestamo.pdf");
            //return View();
        }
    }
}
