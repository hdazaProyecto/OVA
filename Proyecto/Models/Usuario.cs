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
        public int idNivelEstudios { get; set; }
        public string nivelEstudiodesc { get; set; }
        public bool docente { get; set; }
        public HttpPostedFileBase filefotografia { get; set; }

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        /// <summary>
        /// Método que permite registrar un numevo usuario.
        /// </summary>
        /// <param name="usuario">Argumento usuario, modelo de datos usuario.</param>
        /// <returns>Retorna modelo de datos con el usiario creado</returns>
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
                parametros.Add(new SqlParameter("@idNivelEstudios", usuario.idNivelEstudios));
                parametros.Add(new SqlParameter("@fecha", System.DateTime.Now));
                dsusuario = new DataSet();
                server.ejecutarQuery(@"INSERT INTO Usuarios (userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,fecha,idNivelEstudios)
                                        VALUES
                                    (@userName, @userPassword, @correoElectronico, @nombre, @apellidos, @estado, @idRol, @fecha,@idNivelEstudios)                                      
                                    SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional,CAST(ISNULL(U.idNivelEstudios,0) AS INT) idNivelEstudios,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName = P.userName
                                        LEFT JOIN NivelEstudio N ON U.idNivelEstudios=N.idNivelEstudios WHERE UPPER(U.userName) = UPPER(@userName) AND userPassword = @userPassword", parametros, out dsusuario);
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
                        idNivelEstudios = r.Field<int>("idNivelEstudios"),
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

        /// <summary>
        /// Método que peromite actualizar usuario.
        /// </summary>
        /// <param name="usuario">Argumento usuario, modelo de datos usuario.</param>
        /// <returns>Retorna modelo de datos con el usiario actualizado</returns>
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
                    parametros.Add(new SqlParameter("@idNivelEstudios", usuario.idNivelEstudios));
                    parametros.Add(new SqlParameter("@fecha", System.DateTime.Now));
                    dsusuario = new DataSet();
                    server.ejecutarQuery(@"UPDATE Usuarios
                                           SET userPassword = @userPassword,
                                               correoElectronico = @correoElectronico,
                                               nombre = @nombre,
                                               apellidos = @apellidos,
                                               estado = @estado,
                                               idRol = @idRol,
                                               fechaModifica = getdate(),
                                               idNivelEstudios = @idNivelEstudios
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
                                        SELECT U.userName,userPassword,correoElectronico,nombre,apellidos,estado,idRol,profesion,
                                        perfilProfesional,CAST(ISNULL(U.idNivelEstudios,0) AS INT) idNivelEstudios,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName = P.userName
                                        LEFT JOIN NivelEstudio N ON U.idNivelEstudios=N.idNivelEstudios WHERE UPPER(U.userName) = UPPER(@userName) AND U.userPassword = @userPassword AND U.estado=1", parametros, out dsusuario);
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
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return us;
        }

        /// <summary>
        /// Método que busva usuario y verifica su existencia.
        /// </summary>
        /// <returns>Retorna modelo de datos con el usiario</returns>
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
                                        perfilProfesional, ISNULL(E.idNivelEstudios, 0) idNivelEstudios, descripcion, fotografia FROM Usuarios U
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
                        idNivelEstudios = r.Field<int>("idNivelEstudios"),
                        nivelEstudiodesc = r.Field<string>("descripcion"),
                        fotografia = r.Field<String>("fotografia")
                    }).FirstOrDefault();
                }
            }
            return us;
        }

        /// <summary>
        /// Método que permite buscar todos los profesores creados nivel 2.
        /// </summary>
        /// <returns>Retorna modelo de datos con el usiario</returns>
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
                                        perfilProfesional,CAST(ISNULL(U.idNivelEstudios,0) AS INT) idNivelEstudios,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName = P.userName
                                        LEFT JOIN NivelEstudio N ON U.idNivelEstudios=N.idNivelEstudios WHERE estado=1 AND idRol=2", parametros, out dProfesores);
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
                        idNivelEstudios = r.Field<int>("idNivelEstudios"),
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

        /// <summary>
        /// Método que permite buscar todos los usuarios ceados en la plataforma
        /// </summary>
        /// <param name="id">Argumento id, rol del usuario que hace la consulta.</param>
        /// <returns>Retorna lista de modelo usuarios</returns>
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
                                        perfilProfesional,CAST(ISNULL(U.idNivelEstudios,0) AS INT) idNivelEstudios,descripcion,fotografia FROM Usuarios U
                                        LEFT JOIN Profesores P ON U.userName = P.userName
                                        LEFT JOIN NivelEstudio N ON U.idNivelEstudios=N.idNivelEstudios
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
                        idNivelEstudios = r.Field<int>("idNivelEstudios"),
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

        /// <summary>
        /// Método que permite cambiar el estado al usuario de Activo a inactivo o viceversa.
        /// </summary>
        /// <param name="id">Argumento id, rol del usuario que hace la consulta.</param>
        /// <param name="userName">Argumento userName, musuario al cual se desea actualizar estado.</param>
        /// <returns></returns>
        public List<Usuario> cambiarestadoUsu(string userName,int id, string user)
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
                                        perfilProfesional,CAST(ISNULL(U.idNivelEstudios,0) AS INT) idNivelEstudios,descripcion,fotografia FROM Usuarios U
                                           LEFT JOIN Profesores P ON U.userName = P.userName
                                        LEFT JOIN NivelEstudio N ON U.idNivelEstudios=N.idNivelEstudios
                                        WHERE U.IdRol >= @idRol
                                        INSERT INTO Auditoria (tabla,registro,fecha,userName) VALUES 
                                        ('Usuarios',(select case when estado=1 then 'Activación ' else 'Desactivación ' end from  Usuarios  WHERE userName=@userName) +' de usuario '+ @userName,getdate(),'" + user + @"')", parametros, out dusuarios);
                server.close();

                if (dusuarios != null && dusuarios.Tables[0].Rows.Count > 0)
                {
                    EnvioCorreo em = null;
                    string ErrorEm = "";
                    dtusuarios = new DataTable();
                    dtusuarios = dusuarios.Tables[0];

                   int rol = dtusuarios.Rows.Cast<DataRow>().Where(rf => rf.Field<string>("userName") == userName).FirstOrDefault().Field<int>("idRol");
                    string nombre = dtusuarios.Rows.Cast<DataRow>().Where(rf => rf.Field<string>("userName") == userName).FirstOrDefault().Field<string>("nombre");
                    string apellidos = dtusuarios.Rows.Cast<DataRow>().Where(rf => rf.Field<string>("userName") == userName).FirstOrDefault().Field<string>("apellidos");
                    bool estado = dtusuarios.Rows.Cast<DataRow>().Where(rf => rf.Field<string>("userName") == userName).FirstOrDefault().Field<bool>("estado");
                    string correo = dtusuarios.Rows.Cast<DataRow>().Where(rf => rf.Field<string>("userName") == userName).FirstOrDefault().Field<string>("correoElectronico");
                    string desEstado = estado ? "Activado" : "Desactivado";

                    //envio de correo
                    em = new EnvioCorreo("Actualizacion de estado", "<body><p align = 'justify'>Respetad@ " + nombre + " " + apellidos + " <br><br/>El estado de su usuario ha sido actualizado. a: "+ desEstado + " <br></p></body>");
                    em.envio(out ErrorEm, correo, true);


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
                        idNivelEstudios = r.Field<int>("idNivelEstudios"),
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

        /// <summary>
        /// Método que lista los niveles de usuario creados en la base de datos.
        /// </summary>
        /// <returns>Retorna lista de niveles educativos</returns>
        public List<SelectListItem> comNivelEdu()
        {
            DataTable NivelEstudio = new DataTable();
            conexion = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conexion.ConexionSQLServer();
            ConSqlServer server = new ConSqlServer(con);
            server.ejecutarQuery(@"select idNivelEstudios,descripcion from NivelEstudio", new List<SqlParameter>(), out NivelEstudio);
            return Combo(NivelEstudio, "descripcion", "idNivelEstudios", null);
        }

        /// <summary>
        /// Método que permite generar lista para llenar combo.
        /// </summary>
        /// <returns></returns>
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