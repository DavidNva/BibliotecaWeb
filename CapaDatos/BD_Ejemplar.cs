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
    public class BD_Ejemplar
    {
        public bool ActualizarEjemplarActivo(int idLector, int idEjemplar)//out indica parametro de salida
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ActualizarEjemplarActivo", oConexion);
                    cmd.Parameters.AddWithValue("IdLector", idLector);
                    cmd.Parameters.AddWithValue("IdEjemplar", idEjemplar);

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

        public List<EN_Ejemplar> ListarEjemplarLibro(int idlibro) /*Para listar las Ejemplars a filtrar en la presentacion de tienda*/
        {
            List<EN_Ejemplar> lista = new List<EN_Ejemplar>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    //string query = "SELECT IDEjemplar, Descripcion, Activo FROM Ejemplar";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select e.IDEjemplarLibro, l.IDLibro, e.Activo from Libro l");
                    sb.AppendLine("inner join Ejemplar e on e.ID_Libro = l.IDLibro and e.Activo = 1");//--muestra solo las Ejemplars que estan activadas
                    sb.AppendLine("where l.IdLibro =  @idLibro");//--si el idcategoria = 0 muestra todas, pero si no, muestra solo la indicada

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@idLibro", idlibro);//Pasamos el parametro de categoria
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Ejemplar la lista*/
                                new EN_Ejemplar()
                                {
                                    IdEjemplarLibro = Convert.ToInt32(dr["IdEjemplarLibro"]),
                                    IdLibro = Convert.ToInt32(dr["IdLibro"]),//Estamos usando idLibro directamente porque se hizo un inner join con libro
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Ejemplar>();
            }

            return lista;
        }
    }
}
