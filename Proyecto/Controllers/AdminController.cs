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
        string prueba;
        //Tema tema = new Tema();
        //Unidades unidad = new Unidades();

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

        public ActionResult Unidades()
        {
            Unidades unidad = new Unidades();
            unidad = unidad.listarUnidades();
            return View(unidad);
        }


        [HttpPost]
        public ActionResult GuardarTema(Tema pTema)
        {
            prueba = "Prueba de envio.";
            Tema Tema = pTema.gestionarTema(pTema);
            return RedirectToAction("Index");
            //return View(Tema);
        }
        
        [HttpPost]
        public ActionResult GuardarUnidad(Unidades pUnidad)
        {
            prueba = "Prueba de envio.";
            Unidades Unidades = pUnidad.gestionarUnidad(pUnidad);
            return RedirectToAction("Unidades");
        }

    }
}