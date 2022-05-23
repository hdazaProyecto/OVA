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
            
        private Conexion conexion;
        DataSet dsusuario;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        public Usuario Existe()
        {
            Usuario us = null;
            DataSet dsusuario;
            DataTable dtusuario;
            conexion = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conexion.ConexionSQLServer();
            ConSqlServer server = new ConSqlServer(con);
            parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@userName", this.userName));
            parametros.Add(new SqlParameter("@userPassword", Funcion.stringBase64(this.userPassword)));
            server.ejecutarQuery(@"SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional, CAST(ISNULL(U.idNivelEstudios,0) AS INT) idNivelEstudios, descripcion, fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName = P.userName
                                        LEFT JOIN NivelEstudio N ON U.idNivelEstudios = N.idNivelEstudios WHERE UPPER(U.userName) = UPPER(@userName) AND userPassword = @userPassword and U.estado=1", parametros, out dsusuario);
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
                    idRol = r.Field<int>("idRol"),
                    profesion = r.Field<String>("profesion"),
                    perfilProfesional = r.Field<String>("perfilProfesional"),
                    idNivelEstudios = r.Field<int>("idNivelEstudios"),
                    nivelEstudiodesc = r.Field<string>("descripcion"),
                    fotografia = r.Field<String>("fotografia")
                }).FirstOrDefault();
            }
            return us;
        }

        public bool recPassword(string usuario)
        {
            DataTable dtusuario;
            bool envio = false;
            conexion = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conexion.ConexionSQLServer();
            ConSqlServer server = new ConSqlServer(con);
            parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@userName", usuario));
            server.ejecutarQuery("SELECT correoElectronico,userPassword FROM Usuarios WHERE UPPER(userName) = UPPER(@userName)", parametros, out dsusuario);
            server.close();
            if (dsusuario != null && dsusuario.Tables[0].Rows.Count > 0)
            {
                dtusuario = dsusuario.Tables[0];
                EnvioCorreo em = null;
                string ErrorEm = "";            
                em = new EnvioCorreo("Recuperacion de password", "<body><p align = 'justify'>recuperacion de password <br>contraseña " + Funcion.base64String(dtusuario.Rows[0].Field<string>("userPassword")) + "<br/> Atentamente,<br/><b/><br/><br/><br/><br/><br/></p></body>");
                envio = em.envio(out ErrorEm, dtusuario.Rows[0].Field<string>("correoElectronico"), true);
            }        
            return envio;
        }
    }
}