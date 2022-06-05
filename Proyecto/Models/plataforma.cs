using Proyecto.Funciones;
using System;
using System.Collections.Generic;
using Proyecto.Models;
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

        public int idUnidad { get; set; }
        public string nombreUnidad { get; set; }
        public string descripcionUnidad { get; set; }

        public int idRecurso { get; set; }
        public string nombreRecurso { get; set; }
        public string descripcionRecurso { get; set; }
        public string tipoRecurso { get; set; }
        public string url { get; set; }
        public string archivo { get; set; }
        public string imagen { get; set; }
        public bool evidencia { get; set; }
        public string descEvidencia { get; set; }
        public int puntosRecurso { get; set; }
        public string archivoEvidencia { get; set; }
        public string observacion { get; set; }
        public Evidencia evidenciaM { get; set; }

        public List<Unidades> unidades { get; set; }
        public List<Recursos> recursos { get; set; }

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        /// <summary>
        /// Método que permite hacer la busteda de contenido para mostar en el visor de aplicativo.
        /// </summary>
        /// <returns>Retorna un modelo de plataforma (informacion de tema, lista de unidades y lista de recursos)</returns>
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
                server.ejecutarQuery(@"SELECT R.*,U.nombre nomUnidad FROM Recursos R LEFT JOIN Unidades U ON R.idUnidad=U.idUnidad WHERE R.estado=1", parametros, out drecurso);
                server.close();

                if (dtema != null && dtema.Tables[0].Rows.Count > 0)
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
                        estado = r.Field<bool>("estado"),
                        idUnidad = r.Field<int>("idUnidad"),
                        nomUnidad = r.Field<string>("nomUnidad"),
                        fecha = r.Field<DateTime>("fecha"),
                        userName = r.Field<string>("userName"),
                        evidencia = r.Field<bool>("evidencia"),
                        tipoRecurso = r.Field<string>("tipoRecurso"),
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

        /// <summary>
        /// Método que permite hacer la busteda de contenido para mostar en el visor de aplicativo y recurso especifico con datos de evidencia de un estudiante o usuario.
        /// </summary>
        /// <param name="recurso">Argumento recurso, id del recurso a consultar.</param>
        /// <param name="usuario">Argumento usuario, usuario autenticado para consultar evidencias si hay asiciadas al recurso.</param>
        /// <returns>Retorna un modelo de plataforma (informacion de tema, lista de unidades y lista de recursos) con el recurso consultado</returns>
        public plataforma visor(int recurso, string usuario)
        {
            plataforma Plataforma = new plataforma();
            try
            {
                DataSet dtema;
                DataSet dunidad;
                DataSet drecurso;
                DataSet devidencias;
                DataTable dttema;
                DataTable dtunidad;
                DataTable dtrecurso;
                DataTable dtevidencias;
                Plataforma.evidenciaM = new Evidencia();
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                server.ejecutarQuery(@"SELECT * FROM tema WHERE estado=1", parametros, out dtema);
                server.ejecutarQuery(@"SELECT * FROM Unidades WHERE estado=1", parametros, out dunidad);
                server.ejecutarQuery(@"SELECT R.*,U.nombre nomUnidad FROM Recursos R LEFT JOIN Unidades U ON R.idUnidad=U.idUnidad WHERE R.estado=1", parametros, out drecurso);
                server.ejecutarQuery(@"SELECT idEvidencia,archivo,observacion,CAST(idTema AS INT) idTema,CAST(idUnidad AS INT) idUnidad,CAST(idRecurso AS INT) idRecurso,retroalimentacion,CAST(ISNULL(puntosAlcanzados,0) AS INT) puntosAlcanzados,entregado,userName FROM Evidencias WHERE idRecurso =" + recurso + " and userName ='" + usuario + "'", parametros, out devidencias);
                server.close();

                if (dtema.Tables.Count != 0 && dtema.Tables[0].Rows.Count > 0)
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

                if (devidencias.Tables.Count != 0 && devidencias.Tables[0].Rows.Count > 0)
                {
                    dtevidencias = new DataTable();
                    dtevidencias = devidencias.Tables[0];
                    Plataforma.evidenciaM = dtevidencias.Rows.Cast<DataRow>().Select(r => new Evidencia
                    {
                        idEvidencia = r.Field<int>("idEvidencia"),
                        archivo = r.Field<string>("archivo"),
                        observacion = r.Field<string>("observacion"),
                        idTema = r.Field<int>("idTema"),
                        idUnidad = r.Field<int>("idUnidad"),
                        idRecurso = r.Field<int>("idRecurso"),
                        retroalimentacion = r.Field<string>("retroalimentacion"),
                        puntosAlcanzados = r.Field<int>("puntosAlcanzados"),
                        entregado = r.Field<bool>("entregado"),
                    }).FirstOrDefault();
                }

                if (drecurso != null && drecurso.Tables[0].Rows.Count > 0)
                {
                    dtrecurso = new DataTable();
                    dtrecurso = drecurso.Tables[0];

                    Plataforma.idRecurso = recurso;
                    Plataforma.nombreRecurso = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<string>("nombre");
                    Plataforma.evidencia = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<bool>("evidencia");
                    Plataforma.descEvidencia = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<string>("descripcionEvidencia");
                    Plataforma.puntosRecurso = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<int>("puntosRecurso");
                    Plataforma.descripcionRecurso = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<string>("descripcion");
                    Plataforma.url = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<string>("url");
                    Plataforma.archivo = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<string>("archivo");
                    Plataforma.imagen = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<string>("imagen");
                    Plataforma.idUnidad = dtrecurso.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idRecurso") == recurso).FirstOrDefault().Field<int>("idUnidad");

                    if (Plataforma.url != null)
                    {
                        Plataforma.tipoRecurso = "URL";
                    }
                    else if (Plataforma.archivo != null)
                    {
                        Plataforma.tipoRecurso = "ARCHIVO";
                    }
                    else
                    {
                        Plataforma.tipoRecurso = "IMAGEN";
                    }
                    Plataforma.recursos = dtrecurso.AsEnumerable().Select(r => new Recursos()
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
                        evidencia = r.Field<bool>("evidencia"),
                        tipoRecurso = r.Field<string>("tipoRecurso"),
                    }).ToList();

                }

                if (dunidad != null && dunidad.Tables[0].Rows.Count > 0)
                {
                    dtunidad = new DataTable();
                    dtunidad = dunidad.Tables[0];

                    Plataforma.nombreUnidad = dtunidad.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idUnidad") == Plataforma.idUnidad).FirstOrDefault().Field<string>("nombre");
                    Plataforma.descripcionUnidad = dtunidad.Rows.Cast<DataRow>().Where(rf => rf.Field<int>("idUnidad") == Plataforma.idUnidad).FirstOrDefault().Field<string>("descripcion");

                    Plataforma.unidades = dtunidad.AsEnumerable().Select(r => new Unidades()
                    {
                        idUnidad = r.Field<int>("idUnidad"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        idTema = r.Field<int>("idTema"),
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