using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    [FilterConfig.SessionTimeout]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Cuenta usuario)
        {
            Usuario us = null;
            if (ModelState.IsValid)
            { 
                us = usuario.Existe();
                if (us != null)
                {
                    Session["Usuario"] = us;
                    Session["Logueado"] = true;
                    return RedirectToAction("Index");
                } else
                {
                    @ViewBag.Usuario = "NO se pude acceder al sitio, usuario o password incorrectos vualva a intertar";
                    us = null;
                }
                
            }
            else
            {                
                @ViewBag.Usuario = "El usuario y password son requeridos!";
            }
            return View(us);
        }

        public ActionResult Plataforma()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Foro()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Contenido()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Cuenta()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Logout()
        {
            try
            {
                Session.RemoveAll();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Dispose(true);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult RegistrarUsu(Usuario usu)
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(Usuario usuario)
        {
            Usuario usu = usuario.registrarUsuario(usuario);
            return RedirectToAction("Index");
        }
    }
}