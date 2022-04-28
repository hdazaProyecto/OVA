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
        public DateTime fechaModifica { get; set; }
        public string userName { get; set; }

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        public Tema gestionarTema(Tema Ptema)
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
                parametros.Add(new SqlParameter("@descripcion", Ptema.descripcion));
                parametros.Add(new SqlParameter("@imagen", Ptema.imagen));
                parametros.Add(new SqlParameter("@estado", Ptema.estado));
                parametros.Add(new SqlParameter("@fecha", Ptema.fecha));
                parametros.Add(new SqlParameter("@fechaModifica", Ptema.fechaModifica));
                parametros.Add(new SqlParameter("@userName", Ptema.userName));
                dtema = new DataSet();
                server.ejecutarQuery(@" IF EXISTS (SELECT * FROM Tema WHERE idTema=@idTema) 
                                        BEGIN
                                           UPDATE Tema
                                               SET nombre = @nombre
                                                  ,descripcion = @descripcion,
                                                  ,imagen = @imagen,
                                                  ,estado = @estado,
                                                  ,fecha = @fecha,
                                                  ,fechaModifica = @fechaModifica,
                                                  ,userName = @userName
                                             WHERE idTema=@idTema
                                        END
                                        ELSE
                                        BEGIN
                                            INSERT INTO Tema
                                                   (nombre,descripcion,imagen,estado,fecha,fechaModifica,userName)
                                             VALUES
                                                   (@nombre,@descripcion,@imagen,@estado,@fecha,@fechaModifica,@userName)
                                        END SELECT * FROM Tema WHERE idTema=@idTema", parametros, out dtema);
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
                        fechaModifica = r.Field<DateTime>("fechaModifica"),
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

        public Tema existe()
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
                        fechaModifica = r.Field<DateTime>("fechaModifica"),
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