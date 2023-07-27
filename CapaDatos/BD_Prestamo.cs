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
    public class BD_Prestamo
    {
        public bool Registrar(EN_Prestamo obj, DataTable DetallePrestamo,/* DataTable EjemplarActivo,*/ out string Mensaje)//out indica parametro de salida
        {
            bool respuesta = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("Id_Lector", obj.oId_Lector);
                    cmd.Parameters.AddWithValue("TotalLibro", obj.TotalLibro);
                    cmd.Parameters.AddWithValue("DiasDePrestamo", obj.DiasDePrestamo);
                    cmd.Parameters.AddWithValue("Observaciones", obj.Observaciones);
                    cmd.Parameters.AddWithValue("DetallePrestamo", DetallePrestamo);//El data table debe tener las mismas columnas de la estructura creada
                                                                                    //en sql (las 3 creadas: IdLibro, Cantidad, Total)

                    // cmd.Parameters.AddWithValue("EjemplarActivo", EjemplarActivo);

                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }


        public List<EN_DetallePrestamo> ListarPrestamos(int idLector)
        {
            List<EN_DetallePrestamo> lista = new List<EN_DetallePrestamo>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_ListarPrestamos(@idLector)";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@idLector", idLector);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Libro la lista*/
                                new EN_DetallePrestamo()
                                {
                                    oId_Libro = new EN_Libro()//Esto era EN_Libro (Recordar que se cambio a Ejemplar)
                                    {

                                        Codigo = dr["Codigo"].ToString(),
                                        Titulo = dr["Titulo"].ToString(),
                                        //Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),//Indica que los decimales los trabaje con puntos
                                        //oId_Categoria = new EN_Categoria() { Descripcion = dr["Cantidad"].ToString() },
                                        RutaImagen = dr["RutaImagen"].ToString(),
                                        NombreImagen = dr["NombreImagen"].ToString()
                                    },
                                    CantidadEjemplares = Convert.ToInt32(dr["CantidadEjemplares"]),
                                    Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-MX")),
                                    IdDetallePrestamo = Convert.ToInt32(dr["IdDetallePrestamo"]),


                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_DetallePrestamo>();
            }

            return lista;
        }

        public List<EN_Prestamo> ListarPrestamosCompleto()
        {
            List<EN_Prestamo> lista = new List<EN_Prestamo>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    StringBuilder sb = new StringBuilder(); //Permite hacer saltos de linea
                    sb.AppendLine("SELECT p.IdPrestamo, lec.IDLector, CONCAT(lec.Nombres,' ',lec.Apellidos) [NombreLector],");
                    sb.AppendLine("p.TotalLibro, p.FechaPrestamo, p.FechaDevolucion,");
                    sb.AppendLine("p.DiasDePrestamo, p.Observaciones, p.Activo ");
                    //sb.AppendLine(",l.RutaImagen, l.NombreImagen,l.Codigo,l.Titulo,");
                    //sb.AppendLine("DP.CantidadEjemplares, DP.Total, DP.IdDetallePrestamo ");
                    sb.AppendLine("FROM DETALLEPrestamo DP");
                    sb.AppendLine("INNER JOIN Ejemplar ej ON ej.IDEjemplarLibro = DP.IDEjemplar");
                    sb.AppendLine("INNER JOIN Libro l ON l.IdLibro = ej.ID_Libro");
                    sb.AppendLine("INNER JOIN Prestamo p ON p.IdPrestamo = DP.IdPrestamo");
                    sb.AppendLine("inner join Lector lec on lec.IdLector = p.ID_Lector order by DP.IdDetallePrestamo DESC");


                    //string query = "select * from fn_ListarPrestamos(@idLector)";

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    //cmd.Parameters.AddWithValue("@idLector", idLector);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Libro la lista*/
                                new EN_Prestamo()
                                {
                                    IdPrestamo = Convert.ToInt32(dr["IdPrestamo"]),
                                    oId_Lector = new EN_Lector() { IdLector = Convert.ToInt32(dr["IDLector"]), NombreCompletoLector = dr["NombreLector"].ToString() },
                                    TotalLibro = Convert.ToInt32(dr["TotalLibro"]),
                                    FechaPrestamo = dr["FechaPrestamo"].ToString(),
                                    FechaDevolucion = dr["FechaDevolucion"].ToString(),
                                    DiasDePrestamo = Convert.ToInt32(dr["DiasDePrestamo"]), 
                                    Observaciones = dr["Observaciones"].ToString(),
                                    //Tipo = Convert.ToInt32(dr["Tipo"]),   
                                    Activo = Convert.ToBoolean(dr["Activo"]),
                                    //oDetallePrestamo = new EN_DetallePrestamo() { IdDetallePrestamo = Convert.ToInt32(dr["IdDetallePrestamo"]), Total = dr["NombreLector"].ToString() },


                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Prestamo>();
            }

            return lista;
        }
    }
}