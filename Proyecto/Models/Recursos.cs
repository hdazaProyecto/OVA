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
    public class Recursos
    {
        //Variables
        public int idRecurso { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string url { get; set; }
        public string archivo { get; set; }
        public string imagen { get; set; }
        public bool estado { get; set; }
        public int idUnidad { get; set; }
        public string nomUnidad { get; set; }
        public DateTime fecha { get; set; }
        public DateTime? fechaModifica { get; set; }
        public string userName { get; set; }

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        public List<SelectListItem>  comUnidades()
        {
            DataTable _unidades = new DataTable();
            conexion = new Conexion();
            con = new SqlConnectionStringBuilder();
            con = conexion.ConexionSQLServer();
            ConSqlServer server = new ConSqlServer(con);
            server.ejecutarQuery(@"select idUnidad,nombre unidades from unidades where estado=1", new List<SqlParameter>(), out _unidades);
            return Combo(_unidades, "unidades", "idUnidad",null);
        }
        public List<Recursos> listarRecursos()
        {
            List<Recursos> recurso = new List<Recursos>();
            try
            {
                DataSet drecurso;
                DataTable dtrecurso;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                server.ejecutarQuery(@"SELECT R.*,U.nombre nomUnidad FROM Recursos R LEFT JOIN Unidades U ON R.idUnidad=U.idUnidad WHERE R.estado=1 ORDER BY R.idUnidad,R.idRecurso", parametros, out drecurso);
                server.close();

                if (drecurso != null && drecurso.Tables[0].Rows.Count > 0)
                {
                    dtrecurso = new DataTable();
                    dtrecurso = drecurso.Tables[0];

                    recurso = dtrecurso.AsEnumerable().Select(r => new Recursos()
                    {
                        idRecurso = r.Field<int>("idRecurso"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        url = r.Field<string>("url"),
                        archivo = r.Field<string>("archivo"),
                        imagen = r.Field<string>("imagen"),
                        estado = r.Field<bool>("estado"),
                        idUnidad = r.Field<int>("idUnidad"),
                        nomUnidad = r.Field<string>("nomUnidad"),
                        fecha = r.Field<DateTime>("fecha"),
                        userName = r.Field<string>("userName"),
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return recurso;
        }

        public Recursos gestionarUnidad(Recursos Precurso)
        {
            Recursos recurso = new Recursos();
            try
            {
                DataSet drecurso;
                DataTable dtrecurso;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idrecurso", Precurso.idRecurso));
                parametros.Add(new SqlParameter("@nombre", Precurso.nombre));
                parametros.Add(new SqlParameter("@descripcion", Precurso.descripcion == null ? "" : Precurso.descripcion));
                parametros.Add(new SqlParameter("@url", Precurso.imagen == null ? "" : Precurso.imagen));
                parametros.Add(new SqlParameter("@imagen", Precurso.url == null ? "" : Precurso.url));
                parametros.Add(new SqlParameter("@archivo", Precurso.archivo == null ? "" : Precurso.archivo));
                parametros.Add(new SqlParameter("@idUnidad", Precurso.idUnidad));
                parametros.Add(new SqlParameter("@estado", Precurso.estado));
                parametros.Add(new SqlParameter("@fecha", Precurso.fecha));
                parametros.Add(new SqlParameter("@fechaModifica", DateTime.Now));
                parametros.Add(new SqlParameter("@userName", Precurso.userName));
                drecurso = new DataSet();
                server.ejecutarQuery(@" IF EXISTS (SELECT * FROM Recursos WHERE idrecurso=@idrecurso) 
                                        BEGIN
                                           UPDATE Recursos
                                               SET nombre = @nombre,
                                                  descripcion = @descripcion,
                                                  url = @url,
                                                  archivo = @archivo,
                                                  imagen = @imagen,
                                                  estado = @estado,
                                                  idUnidad = @idUnidad,
                                                  fecha = @fecha,
                                                  fechaModifica = @fechaModifica,
                                                  userName = @userName
                                             WHERE idrecurso=@idrecurso
                                        END
                                        ELSE
                                        BEGIN
                                            INSERT INTO Recursos
                                                   (nombre,descripcion,url,archivo,imagen,estado,idUnidad,fecha,userName)
                                             VALUES
                                                   (@nombre,@descripcion,@url,@archivo,@imagen,@estado,@idUnidad,@fecha,@userName)
                                        END SELECT * FROM Recursos", parametros, out drecurso);
                server.close();

                if (drecurso != null && drecurso.Tables[0].Rows.Count > 0)
                {
                    dtrecurso = new DataTable();
                    dtrecurso = drecurso.Tables[0];
                    recurso = dtrecurso.Rows.Cast<DataRow>().Select(r => new Recursos
                    {
                        idRecurso = r.Field<int>("idRecurso"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        url = r.Field<string>("url"),
                        archivo = r.Field<string>("archivo"),
                        imagen = r.Field<string>("imagen"),
                        estado = r.Field<bool>("estado"),
                        idUnidad = r.Field<int>("idUnidad"),
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
            return recurso;
        }

        public Recursos BuscarRecursos(int idRecurso)
        {
            Recursos recurso = new Recursos();
            try
            {
                DataSet drecurso;
                DataTable dtrecurso;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idrecurso", idRecurso));
                server.ejecutarQuery(@"SELECT * FROM Recursos WHERE idRecurso=@idRecurso", parametros, out drecurso);
                server.close();

                if (drecurso != null && drecurso.Tables[0].Rows.Count > 0)
                {
                    dtrecurso = new DataTable();
                    dtrecurso = drecurso.Tables[0];
                    recurso = dtrecurso.Rows.Cast<DataRow>().Select(r => new Recursos
                    {
                        idRecurso = r.Field<int>("idRecurso"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        url = r.Field<string>("url"),
                        archivo = r.Field<string>("archivo"),
                        imagen = r.Field<string>("imagen"),
                        estado = r.Field<bool>("estado"),
                        idUnidad = r.Field<int>("idUnidad"),
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
            return recurso;
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