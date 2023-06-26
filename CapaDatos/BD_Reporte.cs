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
    public class BD_Reporte
    {
        public EN_DashBoard VerDashBoard()
        {
            EN_DashBoard objeto = new EN_DashBoard();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            objeto = new EN_DashBoard
                            {
                                TotalLector = Convert.ToInt32(dr["TotalLector"]),
                                TotalPrestamo = Convert.ToInt32(dr["TotalPrestamo"]),
                                TotalLibro = Convert.ToInt32(dr["TotalLibro"]),
                                TotalEjemplares = Convert.ToInt32(dr["TotalEjemplares"]),
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                objeto = new EN_DashBoard();
            }

            return objeto;
        }

        //public List<EN_Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        //{
        //    List<EN_Reporte> lista = new List<EN_Reporte>();
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {

        //            SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oConexion);
        //            cmd.CommandType = CommandType.StoredProcedure;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/
        //            cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
        //            cmd.Parameters.AddWithValue("fechaFin", fechaFin);
        //            cmd.Parameters.AddWithValue("idTransaccion", idTransaccion);
        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
        //            {
        //                while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
        //                {
        //                    lista.Add(/*Agrega un nuevo usuario a la lista*/
        //                        new EN_Reporte()
        //                        {
        //                            /*Lo que esta dentro de los corchetes es el nombre de la columna de la tabla generada con el procedimiento almacenado*/
        //                            FechaVenta = dr["FechaVenta"].ToString(),
        //                            Cliente = dr["Cliente"].ToString(),
        //                            Producto = dr["Producto"].ToString(),
        //                            Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
        //                            Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),//Checar este .tostring();
        //                            Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-MX")),
        //                            IdTransaccion = dr["IdTransaccion"].ToString()

        //                        }
        //                        );
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        lista = new List<EN_Reporte>();
        //    }

        //    return lista;
        //}
    }
}
