using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

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

        public List<EN_Reporte> Prestamos(string fechaInicio, string fechaFin, string codigo)
        {
            List<EN_Reporte> lista = new List<EN_Reporte>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {



                    //string query = "select Id  from Prestamo";
                    //StringBuilder sb = new StringBuilder();
                    //sb.AppendLine("select CONVERT(char(10), p.FechaPrestamo,103) [FechaPrestamo] , CONCAT(lc.Nombres, ' ', lc.Apellidos)[Lector],");
                    //sb.AppendLine("l.Titulo[Libro], dp.CantidadEjemplares, p.Estado, dp.Total,l.Codigo as IdLibro");
                    //sb.AppendLine("from DetallePrestamo dp");
                    //sb.AppendLine("inner join Libro l on l.Codigo = dp.IDLibro");
                    //sb.AppendLine("inner join Prestamo p on p.IdPrestamo = dp.IdPrestamo");
                    //sb.AppendLine("inner join Lector lc on lc.IdLector = p.Id_Lector");
                    //SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    //cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    //oConexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_ReportePrestamos", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/
                    cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("fechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("codigo", codigo);
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo usuario a la lista*/
                                new EN_Reporte()
                                {
                                    /*Lo que esta dentro de los corchetes es el nombre de la columna de la tabla generada con el procedimiento almacenado*/
                                    FechaPrestamo = dr["FechaPrestamo"].ToString(),
                                    Lector = dr["Lector"].ToString(),
                                    Libro = dr["Libro"].ToString(),
                                    //Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
                                    CantidadEjemplares = Convert.ToInt32(dr["CantidadEjemplares"]),//Checar este .tostring();
                                    Estado = Convert.ToBoolean(dr["Estado"]),//Devuelto = 1 o no devuelto = 0
                                    Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-MX")),
                                    Codigo = dr["Codigo"].ToString()
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Reporte>();
            }

            return lista;
        }
        //public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)//out indica parametro de salida
        //{
        //    bool resultado = false;

        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            SqlCommand cmd = new SqlCommand("update cliente set clave = @nuevaClave, reestablecer = 0 where IdCliente = @Id", oConexion);
        //            cmd.Parameters.AddWithValue("@Id", idCliente);
        //            cmd.Parameters.AddWithValue("nuevaClave", nuevaClave);
        //            cmd.CommandType = CommandType.Text;

        //            oConexion.Open();
        //            //El ExecuteNonQuery ejecuta una accion y devuelve el numero de filas afectadas
        //            //Cuando eliminamos un registro de la tabla, entonces si el total de filas afectadas
        //            //es mayor a 0 entonces será verdadero, pero si no es mayor a 0, entonces significa
        //            //que hubo un problema al eliminar por lo que enviara un false, eso lo almacenamos en resultado
        //            resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = false;
        //        Mensaje = ex.Message;

        //    }
        //    return resultado;
        //}
        //public List<EN_Reporte> Prestamos(string fechaInicio, string fechaFin, string idLibro)
        //{
        //    List<EN_Reporte> lista = new List<EN_Reporte>();
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {

        //            SqlCommand cmd = new SqlCommand("sp_ReportePrestamos", oConexion);
        //            cmd.CommandType = CommandType.StoredProcedure;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/
        //            cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
        //            cmd.Parameters.AddWithValue("fechaFin", fechaFin);
        //            cmd.Parameters.AddWithValue("idLibro", idLibro);
        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
        //            {
        //                while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
        //                {
        //                    lista.Add(/*Agrega un nuevo usuario a la lista*/
        //                        new EN_Reporte()
        //                        {
        //                            /*Lo que esta dentro de los corchetes es el nombre de la columna de la tabla generada con el procedimiento almacenado*/
        //                            FechaPrestamo = dr["FechaPrestamo"].ToString(),
        //                            Lector = dr["Lector"].ToString(),
        //                            Libro = dr["Libro"].ToString(),
        //                            //Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
        //                            CantidadEjemplares = Convert.ToInt32(dr["CantidadEjemplares"]),//Checar este .tostring();
        //                            Estado = Convert.ToBoolean(dr["Estado"]),//Devuelto = 1 o no devuelto = 0
        //                            Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-MX")),
        //                            IdLibro = dr["IdLibro"].ToString() 

        //                        }
        //                        );
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lista = new List<EN_Reporte>();
        //        string mensaje = ex.Message;
        //    }

        //    return lista;
        //}
    }
}
