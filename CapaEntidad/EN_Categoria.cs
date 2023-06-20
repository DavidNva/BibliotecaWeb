using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Categoria
    {
        /* De la base
         * IdCategoria int primary key identity,
	        Descripcion varchar(100),
	        Activo bit default 1,
	        FechaRegistro datetime default getdate()
         */
        public string IdCategoria { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        //public string FechaRegistro { get; set; }
    }
}
