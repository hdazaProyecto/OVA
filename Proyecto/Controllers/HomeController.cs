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

        public ActionResult Cuenta(Usuario usu)
        {
            if (@TempData["Mensaje"] != null)
                ViewBag.Usuario = @TempData["Mensaje"].ToString();
            ViewBag.nivelEdu = usuarios.comNivelEdu();
            if (Session["Usuario"] != null)
            {
                usu = (Usuario)Session["Usuario"];
                return View(usu);
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
            return View(usu);
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(Usuario usuario)
        {
            if (usuario.docente)
            {
                usuario.estado = false;
                usuario.idRol = 2;
            }
            else
            {
                usuario.estado = true;
                usuario.idRol = 3;
            }
            ViewBag.nivelEdu = usuarios.comNivelEdu();
            if (usuario.userPassword == usuario.userPassword2)
            {
                Usuario us = usuario.registrarUsuario(usuario);
                if(us != null)
                {
                    if (us.idRol == 2)
                    {
                        @TempData["Mensaje"] = "Usuario registrado con éxito. Recuerde que recibirá un correo con la confirmación de la activación de su usuario";
                    }
                    else
                    {
                        @TempData["Mensaje"] = "Usuario registrado con éxito.";
                    }
                }
                else
                {
                    @TempData["Mensaje"] = "Error al crear el usuario. El usuario ya existe prueba con otro usuario o recupera contraseña.";
                }
                                
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Usuario = "Las contraseñas no son iguales verifica y reintente nuevamente ";
                return RedirectToAction("RegistrarUsu", usuario);
            }

        }

        public ActionResult RecuperarContrasena(Cuenta usuario)
        {
            Cuenta re = new Cuenta();
            bool envio = re.recPassword(usuario.userName);
            if (envio)
            {
                @TempData["Mensaje"] = "El password se a enviado s u correo por favor revisar y volver a ingresar, en caso de no ver el correo por favor revisar su carpeta de Spam ";
            }
            else
            {
                @TempData["Mensaje"] = "Error al recuperar el password, comuníquese con el administrador";
            }
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
            if (us != null)
            {
                @TempData["Mensaje"] = "Usuario actualizado correctamente";
                Session["Usuario"] = us;                
            }
            else
            {
                @TempData["Mensaje"] = "Error al actualizar el usiario";
            }
            return RedirectToAction("Cuenta", us);
        }
    }
}