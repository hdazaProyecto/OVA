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
        Tema tema = new Tema();

        public ActionResult Index()
        {
            tema = tema.existe();
            Session["tema"] = tema;
            return View();
        }

        public ActionResult Check(ActionResult destino)
        {
            ActionResult act = RedirectToAction("Index");

            if (Session["Usuario"] != null)
                act = destino;
            return act;
        }

        [HttpPost]
        public ActionResult Index(Cuenta usuario)
        {
            Usuario us = usuario.Existe();

            if (us != null)
            {
                Session["Usuario"] = us;
                Session["Logueado"] = true;
                //return RedirectToAction("Index", "Resolucion", new { SinRes = true });
            }
            else
            {
                @ViewBag.Usuario = "NO se pude acceder al sitio";

                us = null;
            }
            return View();
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

        public ActionResult Cuenta()
        {
            if (Session["Usuario"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Contenido()
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
            Usuario us = usuario.registrarUsuario(usuario);
            //return View(us);
            return RedirectToAction("Index");
        }
    }
}