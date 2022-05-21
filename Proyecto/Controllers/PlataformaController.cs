using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class PlataformaController : Controller
    {
        // GET: Plataforma
        public ActionResult Index()
        {
            plataforma p = new plataforma();
            p = p.ModelPlataforma();
            return View(p);
        }
        public ActionResult recurso(int recurso)
        {
            plataforma p = new plataforma();
            Usuario usu = (Usuario)Session["Usuario"];
            p = p.visor(recurso, usu.userName);
            return PartialView("Index",p);
        }
    }
}