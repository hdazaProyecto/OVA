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
        Tema tema = new Tema();

        // GET: Admin
        public ActionResult Index()
        {
            if (!String.IsNullOrWhiteSpace(ViewBag.Message))
            {
                ViewBag.Message = prueba;
            }
            else
            {
                ViewBag.Message = "Prueba de envio Index.";
            }

            tema = tema.existe();
            return View(tema);
        }

        public ActionResult Check(ActionResult destino)
        {
            ActionResult act = RedirectToAction("Index");

            if (Session["Usuario"] != null)
                act = destino;
            return act;
        }


        [HttpPost]
        public ActionResult GuardarTema(Tema pTema)
        {
            prueba = "Prueba de envio.";
            Tema Tema = pTema.gestionarTema(pTema);
            return RedirectToAction("Index");
            //return View(Tema);
        }
    }
}