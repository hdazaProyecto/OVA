using Proyecto.Funciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Cuenta
    {

        [Required(ErrorMessage = "Digite el Id del Usuario.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Digite una contraseña.")]
        public string userPassword { get; set; }
            
        private Conexion conSqlite;
        DataSet dsusuario;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        public Usuario Existe()
        {
            Usuario us = null;
            DataSet dsusuario;
            DataTable dtusuario;
            conSqlite = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conSqlite.ConexionSQLServer();
            if (con != null)
            {
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@userName", this.userName));
                parametros.Add(new SqlParameter("@userPassword", Funcion.stringBase64(this.userPassword)));
                dsusuario = new DataSet();
                server.ejecutarQuery("SELECT * FROM Usuarios WHERE UPPER(userName) = UPPER(@userName) AND userPassword = @userPassword", parametros, out dsusuario);
                server.close();

                if (dsusuario != null && dsusuario.Tables[0].Rows.Count > 0)
                {
                    dtusuario = new DataTable();
                    dtusuario = dsusuario.Tables[0];
                    us = dtusuario.Rows.Cast<DataRow>().Select(r => new Usuario
                    {
                        userName = r.Field<string>("userName"),
                        userPassword = r.Field<string>("userPassword"),
                        correoElectronico = r.Field<string>("correoElectronico"),
                        nombre = r.Field<string>("nombre"),
                        apellidos = r.Field<string>("apellidos"),
                        estado = r.Field<bool>("estado"),
                        idRol = r.Field<int>("idRol")
                    }).FirstOrDefault();
                }
            }
            return us;
        }
    }
}