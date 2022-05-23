using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult recurso(int recurso)
        {
            if (Session["Usuario"] != null)
            {
                plataforma p = new plataforma();
                Usuario usu = (Usuario)Session["Usuario"];
                p = p.visor(recurso, usu.userName);
                return PartialView("Index", p);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult gestionarEvidencia(Evidencia evidencia)
        {
            string ruta = Server.MapPath("~/Archivos/");
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
            if (evidencia.fileArchivo != null)
            {
                evidencia.archivo = Path.GetFileName(evidencia.fileArchivo.FileName); ;
                evidencia.fileArchivo.SaveAs(ruta + evidencia.archivo);
            }
            evidencia.entregado = true;
            evidencia = evidencia.gestionarEvidencia(evidencia);
            return RedirectToAction("Index", "Plataforma");
        }
    }
}