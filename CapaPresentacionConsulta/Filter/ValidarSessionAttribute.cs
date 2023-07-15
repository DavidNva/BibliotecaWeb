using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
namespace CapaPresentacionConsulta.Filter
{
    public class ValidarSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Lector"] == null)//A la sesion lector creada anteriormente
            {
                filterContext.Result = new RedirectResult("~/Acceso/Index");//Si el cliente es igual a null, redirige al login de lector
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}