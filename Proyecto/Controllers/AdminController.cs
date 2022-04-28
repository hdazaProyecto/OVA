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
            tema = tema.existe();
            return View(tema);
        }

        [HttpPost]
        public ActionResult GuardarTema(Tema pTema)
        {
            //Tema t = t.GuardarTema(pTema);
            return RedirectToAction("Index");
        }
    }
}