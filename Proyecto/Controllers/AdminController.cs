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

        //Formularios

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

        //Acciones de formularios

        [HttpPost]
        public ActionResult GuardarTema(Tema pTema)
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
                @TempData["Mensaje"] = "Los datos se guardaron exitosamente";
            }
            return RedirectToAction("Index");
            //return View(Tema);
        }
        
        [HttpPost]
        public ActionResult GuardarUnidad(Unidades pUnidad)
        {
            Unidades Unidades = pUnidad.gestionarUnidad(pUnidad);
            return RedirectToAction("ListarUnidades");
        }

        
        public ActionResult AgregarRecurso(Recursos recurso)
        {
            ViewBag.unidades = recurso.comUnidades();
            if (Session["Usuario"] != null)
            {
                string ruta = Server.MapPath("~/Archivos/");
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);
                if (recurso.file != null)
                {
                    string nombreArchivo = Path.GetFileName(recurso.file.FileName);
                    recurso.file.SaveAs(ruta+nombreArchivo);
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

    }
}