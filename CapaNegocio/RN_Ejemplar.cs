using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class RN_Ejemplar
    {
        private BD_Ejemplar objCapaDato = new BD_Ejemplar();

        //public List<EN_Ejemplar> Listar() /*Usa una clase de la capa entidad*/
        //{
        //    return objCapaDato.Listar();/*Retorna el metodo listar de la instancia de la capa Datos*/
        //}
        public List<EN_Ejemplar> ListarEjemplarLibro(int idlibro)
        {
            return objCapaDato.ListarEjemplarLibro(idlibro);
        }
    }
}
