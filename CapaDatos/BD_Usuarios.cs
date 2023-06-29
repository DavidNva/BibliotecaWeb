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
    }
}
