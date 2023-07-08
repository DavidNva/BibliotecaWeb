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
        //public List<EN_Ejemplar> Listar()
        //{
        //    List<EN_Ejemplar> lista = new List<EN_Ejemplar>();
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            StringBuilder sb = new StringBuilder(); //Permite hacer saltos de linea

        //            sb.AppendLine("select distinct ej.IDEjemplarLibro, ej.Id_Libro, l.IdLibro, l.Codigo, l.Titulo, l.Paginas,c.IDCategoria, c.Descripcion[DesCategoria],");
        //            sb.AppendLine("e.IDEditorial, e.Descripcion[DesEditorial], s.IDSala, s.Descripcion[DesSala],");
        //            sb.AppendLine("l.Ejemplares,l.AñoEdicion, l.Volumen, l.RutaImagen, l.NombreImagen,l.Observaciones, l.Activo");
        //            sb.AppendLine("from Libro l");
        //            sb.AppendLine("inner join Categoria c on c.IDCategoria = l.Id_Categoria");
        //            sb.AppendLine("inner join Editorial e on e.IDEditorial = l.ID_Editorial");
        //            sb.AppendLine("inner join Sala s on s.IDSala = l.ID_Sala");
        //            sb.AppendLine("inner join Ejemplar ej on l.IDLibro = Ej.ID_Libro order by l.IDLibro desc");

        //            //string query = "select IDEjemplar,Nombres,Apellidos,Ciudad, Calle, Telefono, Correo,Clave,Tipo,Reestablecer,Activo from Ejemplar";                    //string query = "select IDEjemplar,Nombres,Apellidos,Ciudad, Calle, Telefono, Correo,Clave,tp.IdTipoPersona,tp.Descripcion[Tipo],Reestablecer,Activo from Ejemplar inner join TipoPersona TP on tp.IdTipoPersona = Ejemplar.Tipo";

        //            SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
        //            cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
        //            {
        //                while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
        //                {
        //                    lista.Add(/*Agrega un nuevo Ejemplar a la lista*/
        //                        new EN_Ejemplar()
        //                        {
        //                            IdEjemplarLibro = Convert.ToInt32(dr["IDEjemplarLibro"]),
        //                            //Id_Libro = Convert.ToInt32(dr["Id_Libro"]),
        //                            IdLibro = Convert.ToInt32(dr["IdLibro"]),
        //                            Codigo = dr["Codigo"].ToString(),
        //                            Titulo = dr["Titulo"].ToString(),
        //                            //Ubicacion = dr["Ubicacion"].ToString(),
        //                            Paginas = Convert.ToInt32(dr["Paginas"]),
        //                            oId_Categoria = new EN_Categoria() { IdCategoria = dr["IDCategoria"].ToString(), Descripcion = dr["DesCategoria"].ToString() },
        //                            oId_Editorial = new EN_Editorial() { IdEditorial = dr["IdEditorial"].ToString(), Descripcion = dr["DesEditorial"].ToString() },
        //                            oId_Sala = new EN_Sala() { IdSala = dr["IdSala"].ToString(), Descripcion = dr["DesSala"].ToString() },
        //                            Ejemplares = Convert.ToInt32(dr["Ejemplares"]),
        //                            AñoEdicion = dr["AñoEdicion"].ToString(),
        //                            Volumen = Convert.ToInt32(dr["Volumen"]),
        //                            RutaImagen = dr["RutaImagen"].ToString(),
        //                            NombreImagen = dr["NombreImagen"].ToString(),
        //                            Observaciones = dr["Observaciones"].ToString(),
        //                            Activo = Convert.ToBoolean(dr["Activo"])
        //                        }
        //                        );
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        lista = new List<EN_Ejemplar>();
        //    }

        //    return lista;
        //}

        //public List<EN_Ejemplar> ListarEjemplarLibro()
        //{
        //    List<EN_Ejemplar> lista = new List<EN_Ejemplar>();
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
        //        {
        //            //string query = "SELECT IdEjemplar, Descripcion, Activo FROM Ejemplar";
        //            StringBuilder sb = new StringBuilder();
        //            sb.AppendLine("select e.IDEjemplarLibro, l.IDLibro, e.Activo from Libro l");
        //            sb.AppendLine("inner join Ejemplar e on e.ID_Libro = l.IDLibro and e.Activo = 1");
                    

        //            SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
        //            cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
        //            {
        //                while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
        //                {
        //                    lista.Add(/*Agrega una nueva Ejemplars a la lista*/
        //                        new EN_Ejemplar()
        //                        {
        //                            IdEjemplarLibro = Convert.ToInt32(dr["IdEjemplarLibro"]),
        //                            Id_Libro = Convert.ToInt32(dr["IdLibro"]),//Estamos usando idLibro directamente porque se hizo un inner join con libro
        //                            Activo = Convert.ToBoolean(dr["Activo"])
        //                        });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        lista = new List<EN_Ejemplar>();
        //    }

        //    return lista;
        //}
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
