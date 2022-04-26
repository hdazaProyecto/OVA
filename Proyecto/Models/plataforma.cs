﻿using Proyecto.Funciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class plataforma
    {
        public static string nombre { get; set; }
        public static string descripcion { get; set; }
        public static string imagen { get; set; }
        public static bool estado { get; set; }
        public static string RutaApp { get; set; }

        static plataforma()
        {
            try
            {
                ConSqlite conSqlite = new ConSqlite("");
                SqlConnectionStringBuilder con = conSqlite.obtenerConexionSQLServer("dbOVA");
                List<SqlParameter> parametros = new List<SqlParameter>();
                DataTable Tema = new DataTable();

                if (con != null)
                {
                    ConSqlServer server = new ConSqlServer(con);
                    server.ejecutarQuery("select * from Tema", parametros, out Tema);
                    server.close();
                }

                if (Tema.Rows.Count > 0)
                {
                    nombre = Tema.Rows[0].Field<string>("nombre");
                    descripcion = Tema.Rows[0].Field<string>("descripcion");
                    imagen = Tema.Rows[0].Field<string>("imagen");
                    estado = Tema.Rows[0].Field<bool>("estado");
                    RutaApp = HttpContext.Current.Server.MapPath("~/");
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}