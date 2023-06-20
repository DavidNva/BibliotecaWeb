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
    public class BD_TipoPersona
    {
        public List<EN_TipoPersona> Listar()
        {
            List<EN_TipoPersona> lista = new List<EN_TipoPersona>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT IdTipoPersona, Descripcion FROM TipoPersona";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva categorias a la lista*/
                                new EN_TipoPersona()
                                {
                                    IdTipoPersona = Convert.ToInt32(dr["IdTipoPersona"]),
                                    Descripcion = dr["Descripcion"].ToString(),
                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_TipoPersona>();
            }

            return lista;
        }
    }
}
