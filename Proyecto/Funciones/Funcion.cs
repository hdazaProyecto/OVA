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

        public static string stringBase64(string agValor)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(agValor));
        }

        public static string base64String(string agValor)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(agValor));
        }

        public static string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public static string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
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