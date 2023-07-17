using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data; /*Acceso a sql conections*/
using System.Globalization;

namespace CapaDatos
{
    public class BD_Carrito
    {
        //si realmente existen un Libro dentro de un carrito de un Lector
        public bool ExisteCarrito(int idLector, int idLibro)//out indica parametro de salida
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdLector", idLector);
                    cmd.Parameters.AddWithValue("IdLibro", idLibro);

                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    //Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;

        }
        public bool OperacionCarrito(int idLector, int idLibro, int idEjemplar, bool sumar, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = true;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdLector", idLector);
                    cmd.Parameters.AddWithValue("IdLibro", idLibro);//ya tiene el id del libro, que ya estuve relacionado con el idejemplarlibro
                    cmd.Parameters.AddWithValue("IdEjemplar", idEjemplar);//ya tiene el id del libro, que ya estuve relacionado con el idejemplarlibro
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
        //public bool OperacionCarrito(int idLector, int idLibros, int idEjemplarLibro, bool sumar, out string Mensaje)//out indica parametro de salida
        //{
        //    bool resultado = true;

        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", oConexion);
        //            cmd.Parameters.AddWithValue("IdLector", idLector);
        //            cmd.Parameters.AddWithValue("IdLibros", idLibros);//ya tiene el id del libro, que ya estuve relacionado con el idejemplarlibro
        //            cmd.Parameters.AddWithValue("IdEjemplarLibro", idEjemplarLibro);
        //            cmd.Parameters.AddWithValue("Sumar", sumar);
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
        // public bool ExisteCarrito(int idLector, int idLibro)
        public int CantidadEnCarrito(int idLector)//out indica parametro de salida
        {
            int resultado = 0;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from Carrito where IdLector = @idLector", oConexion);
                    cmd.Parameters.AddWithValue("@idLector", idLector);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;
        }

        //Para carrito + o -
        public List<EN_Carrito> ListarLibro(int idLector)
        {
            List<EN_Carrito> lista = new List<EN_Carrito>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_obtenerCarritoLector(@idLector)";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@idLector", idLector);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Libro la lista*/
                                new EN_Carrito()
                                {
                                    oId_Libro = new EN_Libro()     
                                    {
                                        IdLibro = Convert.ToInt32(dr["IdLibro"]),
                                        Codigo = dr["Codigo"].ToString(),
                                        oId_Ejemplar = new EN_Ejemplar() { IdEjemplarLibro = Convert.ToInt32(dr["DesEjemplar"]) },
                                        //oId_Categoria = new EN_Categoria() { Descripcion = dr["DesEjemplar"].ToString() },
                                        //oId_Ejemplar = new EN_Ejemplar() { IdEjemplarLibro = Convert.ToInt32 (dr["IDEjemplarLibro"]) },
                                        Titulo = dr["Titulo"].ToString(),
                                        Ejemplares = Convert.ToInt32(dr["Ejemplares"]),//Indica que los decimales los trabaje con puntos
                                        //oId_Categoria = new EN_Categoria() { Descripcion = dr["Cantidad"].ToString() },
                                        RutaImagen = dr["RutaImagen"].ToString(),
                                        NombreImagen = dr["NombreImagen"].ToString()
                                    },
                                    Cantidad = Convert.ToInt32(dr["Cantidad"])

                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Carrito>();
            }

            return lista;
        }

        public bool EliminarCarrito(int idLector, int idLibro)//out indica parametro de salida
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdLector", idLector);
                    cmd.Parameters.AddWithValue("IdLibro", idLibro);

                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    //Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;

        }
    }
}

