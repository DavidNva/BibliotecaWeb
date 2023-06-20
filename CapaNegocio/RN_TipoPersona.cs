using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class RN_TipoPersona
    {
        private BD_TipoPersona objCapaDato = new BD_TipoPersona(); /*Instancia una clase de la capa datos */

        public List<EN_TipoPersona> Listar() /*Usa una clase de la capa entidad*/
        {
            return objCapaDato.Listar();/*Retorna el metodo listar de la instancia de la capa Datos*/
        }
    }
}
