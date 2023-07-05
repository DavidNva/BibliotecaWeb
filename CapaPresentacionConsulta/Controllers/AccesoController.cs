using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaEntidad;
using CapaNegocio;
namespace CapaPresentacionConsulta.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index() //Para mostrar vista de login
        {
            return View();
        }
        public ActionResult Registrar() //Para mostrar formulario de registro Lector
        {
            return View();
        }
        public ActionResult Reestablecer() //Para mostrar el reestablecer la contraseña de Lector
        {
            return View();
        }
        public ActionResult CambiarClave() //Para mostrar cambio de clave del Lector
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(EN_Lector objeto) //Para mostrar vista de login
        {
            int resultado;
            string mensaje = string.Empty;
            string edad = objeto.Edad.ToString();
            //string genero = objeto.Genero.ToString();
            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Edad"] = string.IsNullOrEmpty(edad) ? "" : objeto.Edad.ToString();
            
            ViewData["Genero"] =  objeto.Genero.ToString();
            ViewData["Escuela"] = string.IsNullOrEmpty(objeto.Escuela) ? "" : objeto.Escuela;
            ViewData["GradoGrupo"] = string.IsNullOrEmpty(objeto.GradoGrupo) ? "" : objeto.GradoGrupo;
            ViewData["Ciudad"] = string.IsNullOrEmpty(objeto.Ciudad) ? "" : objeto.Ciudad;
            ViewData["Calle"] = string.IsNullOrEmpty(objeto.Calle) ? "" : objeto.Calle;
            ViewData["Telefono"] = string.IsNullOrEmpty(objeto.Telefono) ? "" : objeto.Telefono;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;
            /*View data pemrite almacenar valores mas simples, como cadenas de texto*/
            if (objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            resultado = new RN_Lector().Registrar(objeto, out mensaje);
            if (resultado > 0)
            {
                ViewBag.Error = null;/*No hay error*/
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(string correo, string clave) //Para mostrar vista de login
        {
            EN_Lector oLector = null;
            oLector = new RN_Lector().Listar().Where(item => item.Correo == correo && item.Clave == RN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oLector == null)
            {
                ViewBag.Error = "Correo o contraseña no son correctas";
                return View();
            }
            else
            {
                if (oLector.Reestablecer) /*Si el reestablecer esta activo*/
                {
                    TempData["IdLector"] = oLector.IdLector;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else /*Si no es necesario que el Lector reestablezca su contraseña*/
                {
                    FormsAuthentication.SetAuthCookie(oLector.Correo, false); /*Se crear una autentificacion por cookie del usuario*/

                    /*Creamos una sesion llamada Lector, será ocupada en el form de layout de inicio*/
                    Session["Lector"] = oLector;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Biblioteca");
                }

            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo) //Para mostrar el reestablecer la contraseña de Lector
        {
            EN_Lector oLector = new EN_Lector();
            oLector = new RN_Lector().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (oLector == null)
            {
                ViewBag.Error = "No se encontró un Lector relacionado a ese correo";
                return View();
            }
            string mensaje = string.Empty;
            bool respuesta = new RN_Lector().ReestablecerClave(oLector.IdLector, correo, out mensaje);
            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else /*Si es false*/
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }
        [HttpPost]
        public ActionResult CambiarClave(string idLector, string claveActual, string nuevaClave, string confirmarClave) //Para mostrar cambio de clave del Lector
        {
            EN_Lector oLector = new EN_Lector();

            oLector = new RN_Lector().Listar().Where(u => u.IdLector == int.Parse(idLector)).FirstOrDefault();

            if (oLector.Clave != RN_Recursos.ConvertirSha256(claveActual)) /*Si la clave que tiene el usuario no es igual a la que esta poniendo*/
            {
                TempData["IdLector"] = idLector;
                ViewData["vclave"] = "";/*View data pemrite almacenar valores mas simples, como cadenas de texto*/
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaClave != confirmarClave)/*En el caso que si sea correcta, pero al confirmar nueva clave no coincida*/
            {
                TempData["IdLector"] = idLector;/*Para mantener esta informacion temporal*/
                ViewData["vclave"] = claveActual; /*De esta forma si nos equivovamos en la validacion de la nueva contraseña, ya no se va
                                                   a eliminar o borrar al validar de nuevo*/
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";
            nuevaClave = RN_Recursos.ConvertirSha256(nuevaClave); /*Encripta la nueva clave si todo va correcto*/
            string mensaje = string.Empty;

            bool respuesta = new RN_Lector().CambiarClave(int.Parse(idLector), nuevaClave, out mensaje); /*Cambia la clave*/

            if (respuesta) /*Si el cambio ha sido correcta*/
            {
                return RedirectToAction("Index"); /*Redirecciona al login*/
            }
            else /*En caso de que haya un error*/
            {
                TempData["IdLector"] = idLector;
                ViewBag.Error = mensaje;
                return View();
            }

        }
        public ActionResult CerrarSesion()
        {
            Session["Lector"] = null;
            FormsAuthentication.SignOut();
            //return RedirectToAction("Index", "Acceso");
            return RedirectToAction("Index", "Biblioteca");
        }
    }
}