using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class RN_Usuarios
    {
        private BD_Usuarios objCapaDato = new BD_Usuarios(); /*Instancia una clase de la capa datos */

        public List<EN_Usuario> Listar() /*Usa una clase de la capa entidad*/
        {
            return objCapaDato.Listar();/*Retorna el metodo listar de la instancia de la capa Datos*/
        }

        public int Registrar(EN_Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Ciudad) || string.IsNullOrWhiteSpace(obj.Ciudad))
            {
                Mensaje = "La ciudad del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Calle) || string.IsNullOrWhiteSpace(obj.Calle))
            {
                Mensaje = "La calle del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Telefono) || string.IsNullOrWhiteSpace(obj.Telefono))
            {
                Mensaje = "El teléfono del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                /*Antes de registrar, se debe enviar un correo al usuario donde
                 * estará la contraseña para que acceda al sistema, entonces:*/
                string clave = RN_Recursos.GenerarClave();//Va a encripar este valor
                //Aqui ira la logica de enviar un correo al usuario
                string asunto = "Creacion de Cuenta"; /*En los signos de excalamcion de la linea ed abajo, se trae la variable clave*/
                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3> <br> <p>Su contraseña para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);/*Aqui solo trae la clave creada*/
                 
                bool respuesta = RN_Recursos.EnviarCorreo(obj.Correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    obj.Clave = RN_Recursos.ConvertirSha256(clave);/*Encripta la clave generada*/
                    return objCapaDato.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }

            }
            else
            {
                return 0;/*No se ha creado un usuario*/
            }

        }
        public bool Editar(EN_Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Ciudad) || string.IsNullOrWhiteSpace(obj.Ciudad))
            {
                Mensaje = "La ciudad del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Calle) || string.IsNullOrWhiteSpace(obj.Calle))
            {
                Mensaje = "La calle del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Telefono) || string.IsNullOrWhiteSpace(obj.Telefono))
            {
                Mensaje = "El teléfono del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        public bool CambiarClave(int idUsuario, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idUsuario, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(int idUsuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = RN_Recursos.GenerarClave();//Va a encripar este valor

            bool resultado = objCapaDato.ReestablecerClave(idUsuario, RN_Recursos.ConvertirSha256(nuevaClave), out Mensaje);

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
