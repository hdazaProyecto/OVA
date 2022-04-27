using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Proyecto.Funciones
{
    public class Funcion
    {           

        private static string dir = plataforma.RutaApp ?? HttpContext.Current.Server.MapPath("~/");
        public static List<string> tareas = new List<string>();

        public static string stringBase64(string agValor)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(agValor));
        }

        public static string base64String(string agValor)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(agValor));
        }

        public static void write()
        {
            using (StreamWriter sw = new StreamWriter(dir + "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + ".txt", true))
            {
                foreach (var i in tareas)
                {
                    sw.WriteLine(DateTime.Now + ": " + i);
                }
                sw.Close();
            }
            tareas.Clear();
        }

    }
}