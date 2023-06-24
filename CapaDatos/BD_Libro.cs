using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
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

                    sb.AppendLine("select l.IdLibro, l.Titulo, l.Ubicacion, l.Paginas,c.IDCategoria, c.Descripcion[DesCategoria],");
                    sb.AppendLine("e.IDEditorial, e.Descripcion[DesEditorial], s.IDSala, s.Descripcion[DesSala],");
                    sb.AppendLine("l.Ejemplares,l.AñoEdicion, l.Volumen, l.RutaImagen, l.NombreImagen,l.Observaciones, l.Activo");
                    sb.AppendLine("from Libro l");
                    sb.AppendLine("inner join Categoria c on c.IDCategoria = l.Id_Categoria");
                    sb.AppendLine("inner join Editorial e on e.IDEditorial = l.ID_Editorial");
                    sb.AppendLine("inner join Sala s on s.IDSala = l.ID_Sala");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Libro la lista*/
                                new EN_Libro()
                                {
                                    IdLibro = dr["IdLibro"].ToString(),
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
                                });
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

        public string Registrar(EN_Libro obj, out string Mensaje)//out indica parametro de salida
        {
            string IdAutogenerado = "0"; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLibro", oConexion);
                    /*Los primeros valores de estos parametros, es la del procedimiento del sql y el segundo de donde toma el valor (las propieaddes de la clase EN_Libro*/
                    cmd.Parameters.AddWithValue("IdLibro", obj.IdLibro);
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
                    IdAutogenerado = cmd.Parameters["Resultado"].Value.ToString();
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdAutogenerado = "0";/*Regresa a 0*/
                Mensaje = ex.Message;

            }
            return IdAutogenerado; /*Si cambia a un nuevo valor al agregar un nuevo id*/

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

        public bool Eliminar(string id, out string Mensaje)//out indica parametro de salida
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
    }
}
