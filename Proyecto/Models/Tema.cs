using Proyecto.Funciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Tema
    {
        public int idTema { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string imagen { get; set; }
        public bool estado { get; set; }
        public DateTime fecha { get; set; }
        public DateTime? fechaModifica { get; set; }
        public string userName { get; set; }
        public HttpPostedFileBase file { get; set; }

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        /// <summary>
        /// Método que permite guardar o actualizar tema.
        /// </summary>
        /// <param name="Ptema">Argumento Ptema, modelo de datos Tema.</param>
        /// <returns>Retotna modelo tema</returns>
        public Tema gestionarTema(Tema Ptema, string userName)
        {
            Tema tema = null;
            try
            {
                DataSet dtema;
                DataTable dttema;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idTema", Ptema.idTema));
                parametros.Add(new SqlParameter("@nombre", Ptema.nombre));
                parametros.Add(new SqlParameter("@descripcion", String.IsNullOrWhiteSpace(Ptema.descripcion) ? DBNull.Value : (object)Ptema.descripcion));
                parametros.Add(new SqlParameter("@imagen", String.IsNullOrWhiteSpace(Ptema.imagen) ? DBNull.Value : (object)Ptema.imagen));
                parametros.Add(new SqlParameter("@fecha", DateTime.Now));
                parametros.Add(new SqlParameter("@fechaModifica", DateTime.Now));
                parametros.Add(new SqlParameter("@userName", Ptema.userName));
                dtema = new DataSet();
                server.ejecutarQuery(@" IF EXISTS (SELECT * FROM Tema WHERE idTema=@idTema) 
                                        BEGIN
                                           UPDATE Tema
                                               SET nombre = @nombre,
                                                  descripcion = @descripcion,
                                                  imagen = @imagen,                                              
                                                  fechaModifica = @fechaModifica                                                 
                                             WHERE idTema=@idTema
                                            INSERT INTO Auditoria (tabla,registro,fecha,userName) VALUES ('Tema','Actualizar',getdate(),'" + userName + @"')
                                        END
                                        ELSE
                                        BEGIN
                                            INSERT INTO Tema
                                                   (nombre,descripcion,imagen,estado,fecha,userName)
                                             VALUES
                                                   (@nombre,@descripcion,@imagen,1,@fecha,@userName)
                                            INSERT INTO Auditoria (tabla,registro,fecha,userName) VALUES ('Tema','Crear',getdate(),'" + userName + @"')
                                        END SELECT * FROM Tema WHERE estado=1", parametros, out dtema);
                server.close();

                if (dtema != null && dtema.Tables[0].Rows.Count > 0)
                {
                    dttema = new DataTable();
                    dttema = dtema.Tables[0];
                    tema = dttema.Rows.Cast<DataRow>().Select(r => new Tema
                    {
                        idTema = r.Field<int>("idTema"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        imagen = r.Field<string>("imagen"),
                        estado = r.Field<bool>("estado"),
                        fecha = r.Field<DateTime>("fecha"),
                        fechaModifica = r.Field<DateTime>("fechaModifica") == null ? DateTime.Now : r.Field<DateTime>("fechaModifica"),
                        userName = r.Field<string>("userName"),
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }

            return tema;
        }

        /// <summary>
        /// Método que permite consultar si hay tema crado.
        /// </summary>
        /// <returns>Retorna modelo de datos para tema</returns>
        public Tema existe()
        {
            Tema tema = new Tema();
            try
            {
                DataSet dtema;
                DataTable dttema;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                dtema = new DataSet();
                server.ejecutarQuery(@"SELECT * FROM Tema WHERE estado=1", parametros, out dtema);
                server.close();

                if (dtema != null && dtema.Tables[0].Rows.Count > 0)
                {
                    dttema = new DataTable();
                    dttema = dtema.Tables[0];
                    tema = dttema.Rows.Cast<DataRow>().Select(r => new Tema
                    {
                        idTema = r.Field<int>("idTema"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        imagen = r.Field<string>("imagen"),
                        estado = r.Field<bool>("estado"),
                        fecha = r.Field<DateTime>("fecha"),                        
                        userName = r.Field<string>("userName"),
                    }).FirstOrDefault();
                }                
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }

            return tema;
        }
    }
}