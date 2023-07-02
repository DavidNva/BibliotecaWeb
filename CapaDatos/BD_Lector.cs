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
    public class BD_Lector
    {
        public int Registrar(EN_Lector obj, out string Mensaje)//out indica parametro de salida
        {
            int IdAutogenerado = 0; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLector", oConexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Edad", obj.Edad);
                    cmd.Parameters.AddWithValue("Genero", obj.Genero);
                    cmd.Parameters.AddWithValue("Escuela", obj.Escuela);
                    cmd.Parameters.AddWithValue("GradoGrupo", obj.GradoGrupo);
                    cmd.Parameters.AddWithValue("Ciudad", obj.Ciudad);
                    cmd.Parameters.AddWithValue("Calle", obj.Calle);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    //cmd.Parameters.AddWithValue("Activo", obj.Activo);
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
        public List<EN_Lector> Listar()
        {
            List<EN_Lector> lista = new List<EN_Lector>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IDLector,Nombres,Apellidos,Edad, Genero, Escuela, GradoGrupo, Ciudad, Calle, Telefono, Correo,Clave,Reestablecer, Activo from Lector";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo usuario a la lista*/
                                new EN_Lector()
                                {
                                    IdLector = Convert.ToInt32(dr["IdLector"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Edad = Convert.ToInt32(dr["Edad"]),
                                    Genero = Convert.ToBoolean(dr["Genero"]),
                                    Escuela = dr["Escuela"].ToString(),
                                    GradoGrupo = dr["GradoGrupo"].ToString(),
                                    Ciudad = dr["Ciudad"].ToString(),
                                    Calle = dr["Calle"].ToString(),
                                    Telefono = dr["Telefono"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
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
                lista = new List<EN_Lector>();
            }

            return lista;
        }
        public bool CambiarClave(int idLector, string nuevaClave, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update Lector set clave = @nuevaClave, reestablecer = 0 where IdLector = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@Id", idLector);
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
        public bool ReestablecerClave(int idLector, string clave, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update Lector set clave = @clave, reestablecer = 1 where IdLector = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@Id", idLector);
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
    }
}
