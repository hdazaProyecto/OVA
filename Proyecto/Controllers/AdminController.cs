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
        // GET: Admin
        public ActionResult Index()
        {
            if (@TempData["Mensaje"] != null)
                ViewBag.Usuario = @TempData["Mensaje"].ToString();
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

        public ActionResult ListarUnidades()
        {
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
                    @ViewBag.Usuario = "Los datos se guardaron exitosamente";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult GuardarUnidad(Unidades pUnidad)
        {
            if (Session["Usuario"] != null)
            {
                Unidades Unidades = pUnidad.gestionarUnidad(pUnidad);
                return RedirectToAction("ListarUnidades");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult AgregarUnidades(Unidades unidad = null)
        {
            //Unidades unidad = new Unidades();
            if (Session["Usuario"] != null)
            {
                return View(unidad);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
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
                    recurso = recurso.gestionarUnidad(recurso);
                    return RedirectToAction("listarRecursos");
                }
                return View(recurso);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

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

        public ActionResult cambiarestadounida(int idUnidad)
        {
            Unidades unidad = new Unidades();
            List<Unidades> listunidad = new List<Unidades>();
            if (Session["Usuario"] != null)
            {
                listunidad = unidad.cambiarestadouni(idUnidad);
                return RedirectToAction("ListarUnidades", listunidad);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
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

    }
}