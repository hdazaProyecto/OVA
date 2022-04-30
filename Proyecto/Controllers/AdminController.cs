using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
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
            Tema Tema = pTema.gestionarTema(pTema);
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
            if (Session["Usuario"] != null)
            {
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
            Recursos recurso = new Recursos();           
            if (Session["Usuario"] != null)
            {
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