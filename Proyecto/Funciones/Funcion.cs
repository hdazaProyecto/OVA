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

        private static string dir = HttpContext.Current.Server.MapPath("~/");
        public static List<string> tareas = new List<string>();

        /// <summary>
        /// Método que recibe cadena de string ruta de archivo para cifrarlo a Base64
        /// </summary>
        /// <param name="agValor">Argumento agValor, ruta de cadena que se va a cifrar.</param>
        /// <returns>Retorna una cadena en base 64</returns>
        public static string stringBase64(string agValor)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(agValor));
        }

        /// <summary>
        /// Método que recibe cadena de string en Base64 y lo quita el cifrado
        /// </summary>
        /// <param name="agValor">Argumento agValor, cadena de string en base64.</param>
        /// <returns></returns>
        public static string base64String(string agValor)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(agValor));
        }

        /// <summary>
        /// Método que peromite escribir escribir archivo de texto .
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método que peromite validar estructura de cadena con estandares para correo electronico.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ValidateEmail(string Email)
        {
            Dictionary<string, string> item = new Dictionary<string, string>();
            try
            {
                Email.Split(';').ToList().ForEach(f =>
                {
                    if (f.Split('<').Count() == 2)
                        item.Add(f.Split('<')[1].Replace(">", "").Trim(), f.Split('<')[0].Trim());
                    else if (Email.Contains('@'))
                        item.Add(f.Trim(), f.Trim());
                });
            }
            catch
            { }

            return item;
        }
    }
}