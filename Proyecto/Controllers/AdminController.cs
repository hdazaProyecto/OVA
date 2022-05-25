using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Funciones;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// Método que retorna la vista Index para el documento configuracion de tema.
        /// </summary>
        /// <returns>Retorna modelo tema si hay una sesion activa si no retorna el index del Home</returns>
        public ActionResult Index()
        {
            if (@TempData["Mensaje"] != null)
               @ViewBag.Usuario = @TempData["Mensaje"].ToString();
            Tema tema = new Tema();
            if(Session["Usuario"] != null)
            {
                tema = tema.existe();
                return View(tema);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// Método que retorna lista de unidades.
        /// </summary>
        /// <returns>Retorna lista de modelo de unidades si hay una sesion activa si no retorna el index del Home</returns>
        public ActionResult ListarUnidades()
        {
            if (@TempData["Mensaje"] != null)
                @ViewBag.Usuario = @TempData["Mensaje"].ToString();
            Unidades uni = new Unidades();
            List<Unidades> unidad = new List<Unidades>();
            if (Session["Usuario"] != null)
            {
                unidad = uni.listarUnidades();
                return View(unidad);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        /// <summary>
        /// Método que retorna lista de unidades.
        /// </summary>
        /// <returns>Retorna lista de modelo de unidades si hay una sesion activa si no retorna el index del Home</returns>
        public ActionResult listarRecursos()
        {
            List<Recursos> recursos = new List<Recursos>();
            Recursos recurso = new Recursos();
            if (Session["Usuario"] != null)
            {
                recursos = recurso.listarRecursos();
                return View(recursos);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Método que permite agregar o actualizar tema principal del OVA.
        /// </summary>
        /// <returns>Retorna modelo tema para llenar formulario index de administrador si hay una sesion activa si no retorna el index del Home</returns>
        [HttpPost]
        public ActionResult GuardarTema(Tema pTema)
        {
            if (Session["Usuario"] != null)
            {
                string ruta = Server.MapPath("~/Archivos/");
                if (pTema.file != null)
                {
                    pTema.imagen = Path.GetFileName(pTema.file.FileName);
                    pTema.file.SaveAs(ruta + pTema.imagen);
                }               
                Tema Tema = pTema.gestionarTema(pTema);
                if (Tema != null)
                {
                    @TempData["Mensaje"] = "Tema Creado con éxito";
                }
                else
                {
                    @TempData["Mensaje"] = "Error al Crear el Tema.";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// Método que permite agregar unidades recibiendo el modelo de unidad.
        /// </summary>
        /// <returns>Retorna modelo de unidad para llenar vista AgregarUnudades si hay una sesion activa si no retorna el index del Home</returns>
        [HttpPost]
        public ActionResult GuardarUnidad(Unidades pUnidad)
        {
            if (Session["Usuario"] != null)
            {
                Unidades Unidades = pUnidad.gestionarUnidad(pUnidad);
                if (Unidades != null)
                {
                    @TempData["Mensaje"] = "Unidad creada con éxito.";
                }
                else
                {
                    @TempData["Mensaje"] = "Error al crear la Unidad.";
                }
                return RedirectToAction("ListarUnidades");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        /// <summary>
        /// Método que permite buscar una unidad y llenar la vista parcial de EditarUnidades.
        /// </summary>
        /// <returns>Retorna un modelo unidad recibiendo el id de la unidad</returns>
        public ActionResult EditarUnidades(int idUnidad)
        {
            Unidades unidad = new Unidades();
            if (Session["Usuario"] != null)
            {
                unidad = unidad.editarUnidades(idUnidad);
                return View(unidad);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Método que permite buscar un recurso y llenar la vista parcial de EditarRecurso.
        /// </summary>
        /// <returns>Retorna un modelo recurso recibiendo el id del recurso</returns>
        public ActionResult EditarRecurso(int idRecurso)
        {   
            if (Session["Usuario"] != null)
            {
                Recursos recurso = new Recursos();
                ViewBag.unidades = recurso.comUnidades();
                recurso = recurso.BuscarRecursos(idRecurso);
                return View(recurso);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

   
        /// <summary>
        /// Método que permite guardar o editar configuracion del e_mail de la plataforma.
        /// </summary>
        /// <returns>Retorna un modelo con los datos de la configuracion del email</returns>
        public ActionResult GuardarConfEmail(ConfigEmail conf)
        {
            if (Session["Usuario"] != null)
            {
                ConfigEmail configuracion = conf.gestioanarConfiguracion(conf);
                if (configuracion.servidor != null)
                {
                    @ViewBag.Usuario = "los datos se guardaron exitosamente";
                }
                else
                {
                    @ViewBag.Usuario = "Error al guardar los datos";
                }
                return View("email", configuracion);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Método que permite enviar correo de prueba para validar que el email se configuró de forma correcta.
        /// </summary>
        /// <returns>redirecciona a la vista de email</returns>
        public ActionResult EmailPrueba()
        {
            if (Session["Usuario"] != null)
            {
                ConfigEmail configuracion = new ConfigEmail();
                bool envio = configuracion.envioPrueba();
                if (envio)
                {
                    @TempData["Mensaje"] = "Envio correcto, revisar correo electronico";
                }
                else
                {
                    @TempData["Mensaje"] = "Error en el envio de correo";
                }
                return RedirectToAction("email");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Método que permite cambiar el estado a la unidad de Activo a inactivo o viceversa.
        /// </summary>
        /// <returns>Retorna una lista de unidades para llenar vista ListarUnidades</returns>
        public ActionResult cambiarestadounida(int idUnidad)
        {
            Unidades unidad = new Unidades();
            List<Unidades> listunidad = new List<Unidades>();
            if (Session["Usuario"] != null)
            {
                listunidad = unidad.cambiarestadouni(idUnidad);
                if (listunidad != null)
                {
                    @TempData["Mensaje"] = "Estado de unidad actualizado";
                }
                else
                {
                    @TempData["Mensaje"] = "Error al actualizar unidad";
                }
                return RedirectToAction("ListarUnidades");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Método que permite cambiar el estado al recurso de Activo a inactivo o viceversa.
        /// </summary>
        /// <returns>Retorna una lista de unidades para llenar vista listarRecursos</returns>
        public ActionResult cambiarestadorecurso(int idRecurso)
        {
            Recursos recurso = new Recursos();
            List<Recursos> listrecurso = new List<Recursos>();
            if (Session["Usuario"] != null)
            {
                listrecurso = recurso.cambiarestadorec(idRecurso);
                return RedirectToAction("listarRecursos", listrecurso);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Método que permite cambiar el estado a un usuario de Activo a inactivo o viceversa.
        /// </summary>
        /// <returns>Retorna una lista de usuarios para llenar vista listarRecursos</returns>
        public ActionResult cambiarestadousuario(string userName)
        {
            if (Session["Usuario"] != null)
            {
                Usuario usu = new Usuario();
                List<Usuario> listUsuarios = new List<Usuario>();
                Usuario us = (Usuario)Session["Usuario"];
                listUsuarios = usu.cambiarestadoUsu(userName,us.idRol);
                return RedirectToAction("gestionarusuarios", listUsuarios);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Método que permite listar los usuarios.
        /// </summary>
        /// <returns>Retorna lista de usuarios para llenar vista </returns>
        public ActionResult gestionarusuarios()
        {
            if (Session["Usuario"] != null)
            {
                Usuario usu = new Usuario();
                List<Usuario> listUsuarios = new List<Usuario>();
                Usuario us = (Usuario)Session["Usuario"];
                listUsuarios = usu.listarUsuarios(us.idRol);
                return View(listUsuarios);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        /// <summary>
        /// Método que permite mostar vista parcial AgregarUnidades.
        /// </summary>
        /// <returns>Muestra vista para agregar nueva unidad</returns>
        public ActionResult AgregarUnidades(Unidades unidad = null)
        {
            if (Session["Usuario"] != null)
            {
                return View(unidad);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Método que permite guardar o editar un recurso y guardar archivos en la raiz del sitio.
        /// </summary>
        /// <returns>Retorna modelo recursos</returns>
        public ActionResult AgregarRecurso(Recursos recurso)
        {
            ViewBag.unidades = recurso.comUnidades();
            if (Session["Usuario"] != null)
            {
                string ruta = Server.MapPath("~/Archivos/");
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);
                if (recurso.fileArchivo != null)
                {
                    recurso.archivo = Path.GetFileName(recurso.fileArchivo.FileName); ;
                    recurso.fileArchivo.SaveAs(ruta + recurso.archivo);
                }
                if (recurso.fileImagen != null)
                {
                    recurso.imagen = Path.GetFileName(recurso.fileImagen.FileName); ;
                    recurso.fileImagen.SaveAs(ruta + recurso.imagen);
                }

                if (recurso.nombre != null)
                {
                    recurso = recurso.gestionarrecurso(recurso);
                    return RedirectToAction("listarRecursos");
                }
                return View(recurso);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        /// <summary>
        /// Método que muestar la vista parcial con los datos del email configuradopara salida de correo de la plataforma.
        /// </summary>
        /// <returns>Retorna un modelo con os datos configurados del e_mail</returns>
        public ActionResult email()
        {
            if (@TempData["Mensaje"] != null)
                ViewBag.Usuario = @TempData["Mensaje"].ToString();
            if (Session["Usuario"] != null)
            {
                ConfigEmail configEmail = new ConfigEmail();
                configEmail = configEmail.ConsultaConfiguracion();
                return View(configEmail);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        /// <summary>
        /// Método que.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult busqueda(string busqueda)
        {
            return RedirectToAction("gestionarusuarios", "Home");
        }
    }
}