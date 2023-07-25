using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            EN_Usuario oUsuario = new EN_Usuario();
            //lista el usuario con el corre y clave dada
            oUsuario = new RN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == RN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)/*Si no encontró el usuario*/
            {
                ViewBag.Error = "Correo o contraseña no correcta";
                /*El view bag guarda informacion a compartir en la misma vista la que estamos utilizando*/
                return View();//retorna la misma vistas
            }
            else /*Pero si lo encontró*/
            {
                if (oUsuario.Reestablecer) /*Si reestablecer es verdadera, significa que esta entrando por primera vez, entonces*/
                { /*Por lo que debe cambiar su contraseña a una personalizada*/
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    /*TempData lo usamos para guardar informacion y compartirlo con otras vistas, multiples vistas dentro de un mismo controlador*/
                    return RedirectToAction("CambiarClave"); /*Como estamos en la misma carpeta, no necesitamos especificar una nueva carpeta*/
                }

                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);/*Se crea una autentificacion del usuario por su correo*/

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home"); /*Referencia al dashboard principal*/
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            EN_Usuario oUsuario = new EN_Usuario();

            oUsuario = new RN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            if (oUsuario.Clave != RN_Recursos.ConvertirSha256(claveActual)) /*Si la clave que tiene el usuario no es igual a la que esta poniendo*/
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vclave"] = "";/*View data pemrite almacenar valores mas simples, como cadenas de texto*/
                /*Como la clave actual no es correcta, la colocamos en vacio*/
                ViewBag.Error = "La contraseña actual no es correcta (Verifique la clave que se le envío al correo registrado)";
                return View();
            }
            else if (nuevaClave != confirmarClave)/*En el caso que si sea correcta, pero al confirmar nueva clave no coincida*/
            {
                TempData["IdUsuario"] = idUsuario;/*Para mantener esta informacion temporal*/
                ViewData["vclave"] = claveActual; /*De esta forma si nos equivovamos en la validacion de la nueva contraseña, ya no se va
                                                   a eliminar o borrar al validar de nuevo*/
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaClave = RN_Recursos.ConvertirSha256(nuevaClave); /*Encripta la nueva clave si todo va correcto*/
            string mensaje = string.Empty;

            bool respuesta = new RN_Usuarios().CambiarClave(int.Parse(idUsuario), nuevaClave, out mensaje); /*Cambia la clave*/

            if (respuesta) /*Si el cambio ha sido correcta*/
            {
                return RedirectToAction("Index"); /*Redirecciona al login*/
            }
            else /*En caso de que haya un error*/
            {
                TempData["IdUsuario"] = idUsuario; /*No debemos perder este idUsuario*/
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            EN_Usuario oUsuario = new EN_Usuario();
            oUsuario = new RN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (oUsuario == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
                return View();
            }
            string mensaje = string.Empty;
            bool respuesta = new RN_Usuarios().ReestablecerClave(oUsuario.IdUsuario, correo, out mensaje);
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
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();//Eliminamos la autentificacion del usuario, por lo que deberá volver a iniciar sesion
            return RedirectToAction("Index", "Acceso");
        }
    }
}