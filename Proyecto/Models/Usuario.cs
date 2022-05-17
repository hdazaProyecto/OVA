using Proyecto.Funciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public int nivelEstudio { get; set; }
        public string nivelEstudiodesc { get; set; }
        public bool docente { get; set; }
        public HttpPostedFileBase filefotografia { get; set; }

        private Conexion conexion;
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
                parametros.Add(new SqlParameter("@nivelEstudio", usuario.nivelEstudio));
                parametros.Add(new SqlParameter("@fecha", System.DateTime.Now));
                dsusuario = new DataSet();
                server.ejecutarQuery(@"INSERT INTO Usuarios (userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,fecha)
                                        VALUES
                                    (@userName, @userPassword, @correoElectronico, @nombre, @apellidos, @estado, @idRol, @fecha)
                                        INSERT INTO Estudiantes (nivelEstudio,userName)
                                                VALUES
                                                (@nivelEstudio,@userName); 
                                    SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional,CAST(ISNULL(E.nivelEstudio,0) AS INT) nivelEstudio,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName=P.userName
                                        LEFT JOIN Estudiantes E ON U.userName=E.userName
                                        LEFT JOIN NivelEstudio N ON E.nivelEstudio=N.idNivel WHERE UPPER(U.userName) = UPPER(@userName) AND userPassword = @userPassword", parametros, out dsusuario);
                server.close();

                if (dsusuario != null && dsusuario.Tables[0].Rows.Count > 0)
                {
                    EnvioCorreo em = null;
                    string ErrorEm = "";
                    dtusuario = new DataTable();
                    dtusuario = dsusuario.Tables[0];
                    
                    //envio de correo
                    em = new EnvioCorreo("Bienvenida !!!!!", "<body><p align = 'justify'>Bienvenid@ "+ dtusuario.Rows[0].Field<string>("nombre") +" "+ dtusuario.Rows[0].Field<string>("apellidos") + " <br><br/>Gracias por haberte dado de alta en el sitio.<br><br/>Accede a nuestro sitio con los siguientes datos <br>Usuario: "+ dtusuario.Rows[0].Field<string>("userName") + "<br>Contraseña: "+ Funcion.base64String(dtusuario.Rows[0].Field<string>("userPassword")) + "<br><br><br/> Atentamente,<br/><b/><br/><br/><br/><br/><br/></p></body>");
                    em.envio(out ErrorEm, dtusuario.Rows[0].Field<string>("correoElectronico"), true);
                    
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
                        nivelEstudio = r.Field<int>("nivelEstudio"),
                        nivelEstudiodesc = r.Field<string>("descripcion"),
                        fotografia = r.Field<String>("fotografia")
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
                    parametros.Add(new SqlParameter("@perfilProfesional", String.IsNullOrWhiteSpace(usuario.perfilProfesional) ? DBNull.Value : (object)usuario.perfilProfesional));
                    parametros.Add(new SqlParameter("@profesion", String.IsNullOrWhiteSpace(usuario.profesion) ? DBNull.Value : (object)usuario.profesion));
                    parametros.Add(new SqlParameter("@fotografia", String.IsNullOrWhiteSpace(usuario.fotografia) ? DBNull.Value : (object)usuario.fotografia));
                    parametros.Add(new SqlParameter("@nivelEstudio", usuario.nivelEstudio));
                    parametros.Add(new SqlParameter("@fecha", System.DateTime.Now));
                    dsusuario = new DataSet();
                    server.ejecutarQuery(@"UPDATE Usuarios
                                           SET userPassword = @userPassword,
                                               correoElectronico = @correoElectronico,
                                               nombre = @nombre,
                                               apellidos = @apellidos,
                                               estado = @estado,
                                               idRol = @idRol,
                                               fechaModifica = getdate()
                                         WHERE userName = @userName;
                                        IF (@idRol=2)
                                            IF EXISTS (SELECT * FROM Profesores WHERE userName=@userName)
                                                UPDATE Profesores
                                                SET profesion = @profesion,
                                                perfilProfesional = @perfilProfesional,
                                                fotografia = @fotografia
                                                WHERE userName = @userName;
                                            ELSE
                                                INSERT INTO Profesores (profesion,perfilProfesional,fotografia,userName)
                                                VALUES
                                                (@profesion,@perfilProfesional,@fotografia,@userName);  
                                        IF (@idRol=3)
                                            IF EXISTS (SELECT * FROM Estudiantes WHERE userName=@userName)
                                                UPDATE Estudiantes
                                                SET nivelEstudio = @nivelEstudio
                                                WHERE userName = @userName;
                                            ELSE
                                                INSERT INTO Estudiantes (nivelEstudio,userName)
                                                VALUES
                                                (@nivelEstudio,@userName);  
                                        SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional,CAST(ISNULL(E.nivelEstudio,0) AS INT) nivelEstudio,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName=P.userName
                                        LEFT JOIN Estudiantes E ON U.userName=E.userName
                                        LEFT JOIN NivelEstudio N ON E.nivelEstudio=N.idNivel WHERE UPPER(U.userName) = UPPER(@userName) AND U.userPassword = @userPassword AND U.estado=1", parametros, out dsusuario);
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
                            nivelEstudio = r.Field<int>("nivelEstudio"),
                            nivelEstudiodesc = r.Field<string>("descripcion"),
                            fotografia = r.Field<String>("fotografia")
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
                server.ejecutarQuery(@"SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional, ISNULL(E.nivelEstudio, 0) nivelEstudio, descripcion, fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName = P.userName
                                        LEFT JOIN Estudiantes E ON U.userName = E.userName
                                        LEFT JOIN NivelEstudio N ON E.nivelEstudio = N.idNivel WHERE UPPER(userName) = UPPER(@userName) AND userPassword = @userPassword", parametros, out dsusuario);
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
                        nivelEstudio = r.Field<int>("nivelEstudio"),
                        nivelEstudiodesc = r.Field<string>("descripcion"),
                        fotografia = r.Field<String>("fotografia")
                    }).FirstOrDefault();
                }
            }
            return us;
        }

        public List<Usuario> listarProfesores()
        {
            List<Usuario> Profesores = new List<Usuario>();
            try
            {
                DataSet dProfesores;
                DataTable dtProfesores;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                server.ejecutarQuery(@"SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional,CAST(ISNULL(E.nivelEstudio,0) AS INT) nivelEstudio,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName=P.userName
                                        LEFT JOIN Estudiantes E ON U.userName=E.userName
                                        LEFT JOIN NivelEstudio N ON E.nivelEstudio=N.idNivel WHERE estado=1 AND idRol=2", parametros, out dProfesores);
                server.close();

                if (dProfesores != null && dProfesores.Tables[0].Rows.Count > 0)
                {
                    dtProfesores = new DataTable();
                    dtProfesores = dProfesores.Tables[0];

                    Profesores = dtProfesores.AsEnumerable().Select(r => new Usuario()
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
                        nivelEstudio = r.Field<int>("nivelEstudio"),
                        nivelEstudiodesc = r.Field<string>("descripcion"),
                        fotografia = r.Field<String>("fotografia")
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return Profesores;
        }

        public List<Usuario> listarUsuarios(int id)
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                DataSet dusuarios;
                DataTable dtusuarios;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idRol", id == 1 ? 2 : 3));
                server.ejecutarQuery(@"SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional,CAST(ISNULL(E.nivelEstudio,0) AS INT) nivelEstudio,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName=P.userName
                                        LEFT JOIN Estudiantes E ON U.userName=E.userName
                                        LEFT JOIN NivelEstudio N ON E.nivelEstudio=N.idNivel
                                        WHERE U.IdRol >= @idRol", parametros, out dusuarios);
                server.close();

                if (dusuarios != null && dusuarios.Tables[0].Rows.Count > 0)
                {
                    dtusuarios = new DataTable();
                    dtusuarios = dusuarios.Tables[0];

                    usuarios = dtusuarios.AsEnumerable().Select(r => new Usuario()
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
                        nivelEstudio = r.Field<int>("nivelEstudio"),
                        nivelEstudiodesc = r.Field<string>("descripcion"),
                        fotografia = r.Field<String>("fotografia")
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return usuarios;
        }

        public List<Usuario> cambiarestadoUsu(string userName,int id)
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                DataSet dusuarios;
                DataTable dtusuarios;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idRol", id == 1 ? 2 : 3));
                parametros.Add(new SqlParameter("@userName", userName));
                server.ejecutarQuery(@"UPDATE Usuarios SET estado=CASE WHEN estado=1 THEN 0 ELSE 1 END WHERE userName=@userName
                                        SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional,CAST(ISNULL(E.nivelEstudio,0) AS INT) nivelEstudio,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName=P.userName
                                        LEFT JOIN Estudiantes E ON U.userName=E.userName
                                        LEFT JOIN NivelEstudio N ON E.nivelEstudio=N.idNivel
                                        WHERE U.IdRol >= @idRol", parametros, out dusuarios);
                server.close();

                if (dusuarios != null && dusuarios.Tables[0].Rows.Count > 0)
                {
                    dtusuarios = new DataTable();
                    dtusuarios = dusuarios.Tables[0];
                    string rol = dtusuarios.Rows[0]["idRol"].ToString();
                    if (rol == "2")
                    {

                    }

                    usuarios = dtusuarios.AsEnumerable().Select(r => new Usuario()
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
                        nivelEstudio = r.Field<int>("nivelEstudio"),
                        nivelEstudiodesc = r.Field<string>("descripcion"),
                        fotografia = r.Field<String>("fotografia")
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return usuarios;
        }

        public List<SelectListItem> comNivelEdu()
        {
            DataTable _unidades = new DataTable();
            conexion = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conexion.ConexionSQLServer();
            ConSqlServer server = new ConSqlServer(con);
            server.ejecutarQuery(@"select idNivel,descripcion from NivelEstudio", new List<SqlParameter>(), out _unidades);
            return Combo(_unidades, "descripcion", "idNivel", null);
        }

        public static List<SelectListItem> Combo(DataTable agOrigenDatos, string agDisplay, string agValue, string agSelectedValue)
        {
            return agOrigenDatos.Rows.Cast<DataRow>().Select(r => new SelectListItem
            {
                Text = r[agDisplay].ToString(),
                Value = r[agValue].ToString(),
                Selected = r[agValue].ToString().Equals(agSelectedValue)
            }).ToList();
        }
    }
}