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

        /// <summary>
        /// Método que carga datos en el home de la aplicacion.
        /// </summary>
        /// <returns>Retorna modelo plataforma para llenar datos</returns>
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

        /// <summary>
        /// Método que permite realizar el logue en la aplicación.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método que permite redireccionar a foro que se pretende implementar en una mejora o nueva version.
        /// </summary>
        /// <returns></returns>
        public ActionResult Foro()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Método que permite mostrar lista de docentes avilitados para la plataforma.
        /// </summary>
        /// <returns>Retorna una lista de modelo de usuarios con la informacion de los docentes habilitados</returns>
        public ActionResult Contact()
        {
            Usuario us = new Usuario();
            List<Usuario> profesores = new List<Usuario>();
            profesores = us.listarProfesores();
            return View(profesores);
        }

        /// <summary>
        /// Método que muestra informacion de la cuanta del usuario (estudiante, docente, administrador).
        /// </summary>
        /// <returns>Retorna modelo con los datos del usuario para cargar vista parcial</returns>
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

        /// <summary>
        /// Método que muestra organigrama con la informacion del contenido del programa.
        /// </summary>
        /// <returns>Retorna un modelo con la informacion de la plataforma</returns>
        public ActionResult Contenido()
        {
            if (Session["Usuario"] != null)
            {
                plataforma p = new plataforma();
                p = p.ModelPlataforma();
                return View(p);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        /// <summary>
        /// Método que permite destruir una sesion.
        /// </summary>
        /// <returns>Redirecciona al index del home y destruye la sesión.</returns>
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

        /// <summary>
        /// Método que permite mostrar vista para registar nuevo usuario.
        /// </summary>
        /// <returns>Retorna vista parcial para registrar usuario</returns>
        public ActionResult RegistrarUsu(Usuario usu)
        {
            if (@TempData["Mensaje"] != null)
                ViewBag.Usuario = @TempData["Mensaje"].ToString();
            ViewBag.nivelEdu = usuarios.comNivelEdu();
            return View(usu);
        }

        /// <summary>
        /// Método que permite reguistrar un usuario nuevo y envio de correo coan la informacion del usuario.
        /// </summary>
        /// <returns>Retorna modelo de usuario con datos del usuario que se acaba de crear</returns>
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

        /// <summary>
        /// Método que permite recuperar la contraseña enviando datos de ingreso al correo registrado del usuario.
        /// </summary>
        /// <returns>Retorna a la vista inical de la aplicacion despues de enviar correo con datos de autenticacion</returns>
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

        /// <summary>
        /// Método que permite actualizar informacion del usuario o completar informacion de docente registrado.
        /// </summary>
        /// <returns>Retyormna datos de usuario para mostar vista parcial cuanta</returns>
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