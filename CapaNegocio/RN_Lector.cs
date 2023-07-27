using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class RN_Lector
    {
        private BD_Lector objCapaDato = new BD_Lector(); /*Instancia una clase de la capa datos */
        
        public List<EN_Lector> Listar() /*Usa una clase de la capa entidad*/
        {
            return objCapaDato.Listar();/*Retorna el metodo listar de la instancia de la capa Datos*/
        }

        public List<EN_Lector> ListarLectorParaPrestamo()
        {
            return objCapaDato.ListarLectorParaPrestamo();
        }


        public int Registrar(EN_Lector obj, out string Mensaje)
        {
            Mensaje = string.Empty;
          
            
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del lector no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Los apellidos del lector no puede ser vacio";
            }
            else if (obj.Edad == 0)
            {
                Mensaje = "Debe ingresar una edad válida";
            }
            else if (obj.Edad > 125)
            {
                Mensaje = "El rango de edad válido es entre 1 - 125";
            }
            else if (string.IsNullOrEmpty(obj.Escuela) || string.IsNullOrWhiteSpace(obj.Escuela))
            {
                Mensaje = "La escuela del lector no puede ser vacio";//Por default tendra NINGUNA por si el lector no asiste a alguna escuela
            }
            else if (string.IsNullOrEmpty(obj.GradoGrupo) || string.IsNullOrWhiteSpace(obj.GradoGrupo))
            {
                Mensaje = "El grado y el grupo del lector no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Ciudad) || string.IsNullOrWhiteSpace(obj.Ciudad))
            {
                Mensaje = "La ciudad del lector no puede ser vacio";//Por default tendra NINGUNA por si el lector no asiste a alguna escuela
            }
            else if (string.IsNullOrEmpty(obj.Calle) || string.IsNullOrWhiteSpace(obj.Calle))
            {
                Mensaje = "La calle del lector no puede ser vacio";//Por default tendra NINGUNA por si el lector no asiste a alguna escuela
            }
            else if (string.IsNullOrEmpty(obj.Telefono) || string.IsNullOrWhiteSpace(obj.Telefono))
            {
                Mensaje = "El teléfono del lector no puede ser vacio";//Por default tendra NINGUNA por si el lector no asiste a alguna escuela
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del lector no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Clave) || string.IsNullOrWhiteSpace(obj.Clave))
            {
                Mensaje = "La contraseña del lector no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.ConfirmarClave) || string.IsNullOrWhiteSpace(obj.ConfirmarClave))
            {
                Mensaje = "Debes confirmar la contraseña ingresada";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                obj.Clave = RN_Recursos.ConvertirSha256(obj.Clave);/*Encripta la clave generada*/
                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;/*No se ha creado un usuario*/
            }

        }


        public bool CambiarClave(int idLector, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idLector, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(int idLector, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = RN_Recursos.GenerarClave();//Va a encripar este valor
            bool resultado = objCapaDato.ReestablecerClave(idLector, RN_Recursos.ConvertirSha256(nuevaClave), out Mensaje);


            if (resultado)//si resultado es verdadero
            {
                string asunto = "Contraseña reestablecida"; /*En los signos de excalamcion de la linea ed abajo, se trae la variable clave*/
                string mensajeCorreo = "<h3>Su cuenta fue reestablecida correctamente</h3> <br> <p>Su contraseña para acceder ahora es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaClave);/*Aqui solo trae la clave creada*/
                bool respuesta = RN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }


        }
    }
}
