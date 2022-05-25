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
        /// <summary>
        /// Método que permite mostar vista parcial de plataforma principal para navegacion en contenido del OVA.
        /// </summary>
        /// <returns>Retorna un modelo de plataforma (informacion de tema, lista de unidades y lista de recursos)</returns>
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

        /// <summary>
        /// Método que permite hacer la busteda de contenido para mostar en el visor de aplicativo.
        /// </summary>
        /// <returns>Retorna un modelo de plataforma (informacion de tema, lista de unidades y lista de recursos)</returns>
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

        /// <summary>
        /// Método que peromite el registro de evidencia de parte del estudiante.
        /// </summary>
        /// <returns>Retorna al visor de la plataforma despues de registar evidencia</returns>
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