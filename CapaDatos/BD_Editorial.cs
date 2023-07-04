using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data; /*Acceso a sql conections*/
namespace CapaDatos
{
    public class BD_Editorial
    {
        public List<EN_Editorial> Listar()
        {
            List<EN_Editorial> lista = new List<EN_Editorial>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT IdEditorial, Descripcion, Activo FROM Editorial";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Editorials a la lista*/
                                new EN_Editorial()
                                {
                                    IdEditorial = dr["IdEditorial"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Editorial>();
            }

            return lista;
        }

        public string Registrar(EN_Editorial obj, out string Mensaje)//out indica parametro de salida
        {
            string IdAutogenerado = "0"; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarEditorial", oConexion);
                    /*Los primeros valores de estos parametros, es la del procedimiento del sql y el segundo de donde toma el valor (las propieaddes de la clase EN_Editorial*/
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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
        public bool Editar(EN_Editorial obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarEditorial", oConexion);
                    cmd.Parameters.AddWithValue("IdEditorial", obj.IdEditorial);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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

        public bool Eliminar(string id, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarEditorial", oConexion);
                    cmd.Parameters.AddWithValue("IdEditorial", id);
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
        public List<EN_Editorial> ListarEditorialPorCategoria(string idCategoria) /*Para listar las Editorials a filtrar en la presentacion de tienda*/
        {
            List<EN_Editorial> lista = new List<EN_Editorial>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    //string query = "SELECT IDEditorial, Descripcion, Activo FROM Editorial";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select distinct m.IDEditorial, m.Descripcion from Libro l");
                    sb.AppendLine("inner join categoria c on c.IDCategoria = l.ID_Categoria");
                    sb.AppendLine("inner join Editorial m on m.IDEditorial = l.ID_Editorial and m.Activo = 1");//--muestra solo las Editorials que estan activadas
                    sb.AppendLine("where c.IDCategoria = iif(@idCategoria = 'T', c.IDCategoria, @idCategoria)");//--si el idcategoria = 0 muestra todas, pero si no, muestra solo la indicada

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@idCategoria", idCategoria);//Pasamos el parametro de categoria
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Editorial la lista*/
                                new EN_Editorial()
                                {
                                    IdEditorial = dr["IdEditorial"].ToString(),//En este caso solo necesitamos el id y descripcion de la Editorial
                                    Descripcion = dr["Descripcion"].ToString(),
                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Editorial>();
            }

            return lista;
        }
    }
}
