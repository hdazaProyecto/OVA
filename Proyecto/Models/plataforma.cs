using Proyecto.Funciones;
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
        public int idTema { get; set; }
        public string nombreTema { get; set; }
        public string descripcionTema { get; set; }
        public string imagenTema { get; set; }
        public List<Unidades> unidades { get; set; }
        public List<Recursos> recursos { get; set; }

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        public plataforma ModelPlataforma()
        {
            plataforma Plataforma = new plataforma();
            try
            {
                DataSet dtema;
                DataSet dunidad;
                DataSet drecurso;
                DataTable dttema;                
                DataTable dtunidad;                
                DataTable dtrecurso;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                server.ejecutarQuery(@"SELECT * FROM tema WHERE estado=1", parametros, out dtema);
                server.ejecutarQuery(@"SELECT * FROM Unidades WHERE estado=1", parametros, out dunidad);
                server.ejecutarQuery(@"SELECT * FROM recursos WHERE estado=1", parametros, out drecurso);
                server.close();

                if (dunidad != null && dunidad.Tables[0].Rows.Count > 0)
                {
                    dttema = new DataTable();
                    dttema = dtema.Tables[0];
                    Plataforma = dttema.Rows.Cast<DataRow>().Select(r => new plataforma
                    {
                        idTema = r.Field<int>("idTema"),
                        nombreTema = r.Field<string>("nombre"),
                        descripcionTema = r.Field<string>("descripcion"),
                        imagenTema = r.Field<string>("imagen"),
                    }).FirstOrDefault();
                }
                if (dunidad != null && dunidad.Tables[0].Rows.Count > 0)
                {
                    dtunidad = new DataTable();
                    dtunidad = dunidad.Tables[0];

                    Plataforma.unidades = dtunidad.AsEnumerable().Select(r => new Unidades()
                    {
                        idUnidad = r.Field<int>("idUnidad"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        imagen = r.Field<string>("imagen"),
                        idTema = r.Field<int>("idTema"),
                    }).ToList();

                }
                if (drecurso != null && drecurso.Tables[0].Rows.Count > 0)
                {
                    dtrecurso = new DataTable();
                    dtrecurso = drecurso.Tables[0];

                    Plataforma.recursos = dtrecurso.AsEnumerable().Select(r => new Recursos()
                    {
                        idRecurso = r.Field<int>("idRecurso"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        url = r.Field<string>("url"),
                        archivo = r.Field<string>("archivo"),
                        imagen = r.Field<string>("imagen"),
                        idUnidad = r.Field<int>("idUnidad"),
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return Plataforma;
        }
    }
}