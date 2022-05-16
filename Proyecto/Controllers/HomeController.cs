using Proyecto.Funciones;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    [FilterConfig.SessionTimeout]
    public class HomeController : Controller
    {
        Recursos recurso = new Recursos();
        Usuario usuarios = new Usuario();
        plataforma p = new plataforma();
        public ActionResult Index()
        {
            if (@TempData["Mensaje"] != null)
                ViewBag.Usuario = @TempData["Mensaje"].ToString();
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
                @ViewBag.Usuario = "Bienvenido "+us.nombre +", gracias por iniciar sesion.";
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
            plataforma p = new plataforma();
            p = p.ModelPlataforma();
            return View(p);
        }

        public ActionResult Foro()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Usuario us = new Usuario();
            List<Usuario> profesores = new List<Usuario>();
            profesores = us.listarProfesores();
            return View(profesores);
        }

        public ActionResult Cuenta()
        {
            ViewBag.nivelEdu = usuarios.comNivelEdu();
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
            if (@TempData["Mensaje"] != null)
                ViewBag.Usuario = @TempData["Mensaje"].ToString();
            ViewBag.nivelEdu = usuarios.comNivelEdu();
            ViewBag.Message = "Your application description page.";
            return View(usu);
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(Usuario usuario)
        {
            ViewBag.nivelEdu = usuarios.comNivelEdu();
            if (usuario.userPassword == usuario.userPassword2)
            {
                Usuario us = usuario.registrarUsuario(usuario);
                @TempData["Mensaje"] = "Usuario registrado";
                return RedirectToAction("Index");
            }
            else
            {
                @TempData["Mensaje"] = "Por favor verificar Password";
                return RedirectToAction("RegistrarUsu", usuario);
            }

        }

        public ActionResult RecuperarContrasena(Cuenta usuario)
        {
            Cuenta re = new Cuenta();
            re.recPassword(usuario.userName);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ActualizarUsuario(Usuario usuario)
        {
            string ruta = Server.MapPath("~/Archivos/");
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
            if (usuario.filefotografia != null)
            {
                usuario.fotografia = usuario.userName+"-"+ Path.GetFileName(usuario.filefotografia.FileName);
                usuario.filefotografia.SaveAs(ruta + usuario.fotografia);
            }
            Usuario us = usuario.actualizarUsuario(usuario);
            Session["Usuario"] = us;
            return RedirectToAction("Cuenta");
        }
    }
}