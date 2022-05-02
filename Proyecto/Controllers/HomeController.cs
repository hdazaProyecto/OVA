using Proyecto.Funciones;
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
        Recursos recurso = new Recursos();
        plataforma p = new plataforma();
        public ActionResult Index()
        {            
            p = p.ModelPlataforma();
            if (p != null)
            {
                Session["plataforma"] = p;
            }
            return View(p);
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
            p = p.ModelPlataforma();
            if (p != null)
            {
                Session["plataforma"] = p;
            }
            if (us != null)
            {
                Session["Usuario"] = us;
                Session["Logueado"] = true;                
            }
            else
            {
                @ViewBag.Usuario = "NO se pude acceder al sitio";

                us = null;
            }
            return View(p);
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
            plataforma p = new plataforma();
            p = p.ModelPlataforma();
            return View(p);
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
            catch (Exception)
            {
                Funcion.tareas.Add("Sesion terminada");
                Funcion.write();
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

        public ActionResult RecuperarContrasena(string usuario)
        {
            EnvioCorreo envio = new EnvioCorreo();
            envio.envio();
            return RedirectToAction("Index", "Home");
        }
    }
}