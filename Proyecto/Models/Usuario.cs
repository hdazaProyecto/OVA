using Proyecto.Funciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Usuario
    {
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string userPassword2 { get; set; }
        public string correoElectronico { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public bool estado { get; set; }
        public int idRol { get; set; }
        public string profesion { get; set; }
        public string perfilProfesional { get; set; }
        public string fotografia { get; set; }

        private Conexion conexion;
        DataSet dsusuario;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        public Usuario registrarUsuario(Usuario usuario)
        {
            Usuario us = null;
            DataSet dsusuario;
            DataTable dtusuario;
            conexion = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conexion.ConexionSQLServer();
            try
            {
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@userName", usuario.userName));
                parametros.Add(new SqlParameter("@userPassword", Funcion.stringBase64(usuario.userPassword)));
                parametros.Add(new SqlParameter("@correoElectronico", usuario.correoElectronico));
                parametros.Add(new SqlParameter("@nombre", usuario.nombre));
                parametros.Add(new SqlParameter("@apellidos", usuario.apellidos));
                parametros.Add(new SqlParameter("@estado", usuario.estado));
                parametros.Add(new SqlParameter("@idRol", usuario.idRol));
                parametros.Add(new SqlParameter("@fecha", System.DateTime.Now));
                dsusuario = new DataSet();
                server.ejecutarQuery(@"INSERT INTO Usuarios (userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,fecha)
                                        VALUES
                                    (@userName, @userPassword, @correoElectronico, @nombre, @apellidos, @estado, @idRol, @fecha)
                                    SELECT * FROM Usuarios WHERE UPPER(userName) = UPPER(@userName) AND userPassword = @userPassword", parametros, out dsusuario);
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
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return us;
        }

        public Usuario actualizarUsuario(Usuario usuario)
        {
            Usuario us = null;
            DataSet dsusuario;
            DataTable dtusuario;
            conexion = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conexion.ConexionSQLServer();
            try
            {
                if (con != null)
                {
                    ConSqlServer server = new ConSqlServer(con);
                    parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("@userName", usuario.userName));
                    parametros.Add(new SqlParameter("@userPassword", Funcion.stringBase64(usuario.userPassword)));
                    parametros.Add(new SqlParameter("@correoElectronico", usuario.correoElectronico));
                    parametros.Add(new SqlParameter("@nombre", usuario.nombre));
                    parametros.Add(new SqlParameter("@apellidos", usuario.apellidos));
                    parametros.Add(new SqlParameter("@estado", usuario.estado));
                    parametros.Add(new SqlParameter("@idRol", usuario.idRol));
                    parametros.Add(new SqlParameter("@fecha", System.DateTime.Now));
                    dsusuario = new DataSet();
                    server.ejecutarQuery(@"UPDATE Usuarios
                                           SET userPassword = @userPassword
                                              ,correoElectronico = @correoElectronico
                                              ,nombre = @nombre
                                              ,apellidos = @apellidos
                                              ,estado = @estado
                                              ,idRol = @idRol
                                              ,fechaModifica = getdate()
                                         WHERE userName = @userName 
                                    SELECT * FROM Usuarios WHERE UPPER(userName) = UPPER(@userName) AND userPassword = @userPassword", parametros, out dsusuario);
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
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return us;
        }

        public Usuario Existe()
        {
            Usuario us = null;
            DataSet dsusuario;
            DataTable dtusuario;
            conexion = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conexion.ConexionSQLServer();
            if (con != null)
            {
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@userName", userName));
                parametros.Add(new SqlParameter("@userPassword", Funcion.stringBase64(userPassword)));
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