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
    public class Evidencia
    {
        public int idEvidencia { get; set; }
        public string archivo { get; set; }
        public HttpPostedFileBase fileArchivo { get; set; }
        [AllowHtml]
        public string observacion { get; set; }
        public int idTema { get; set; }
        public int idUnidad { get; set; }
        public string nombreUnidad { get; set; }
        public int idRecurso { get; set; }
        public string nombreRecurso { get; set; }
        [AllowHtml]
        public string retroalimentacion { get; set; }
        public int puntosAlcanzados { get; set; }
        public bool entregado { get; set; }
        public string userName { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;
        

        /// <summary>
        /// Método que peromite guardar o actualizar evidencias en base de datos.
        /// </summary>
        /// <param name="evidencia">Argumento evidencia, modelo de datos evidencia.</param>
        /// <returns>Retorna modelo de datos evidencia</returns>
        public Evidencia gestionarEvidencia(Evidencia evidencia)
        {
            Evidencia evi = new Evidencia();
            try
            {
                DataSet devidencia;
                DataTable dtevidencia;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idEvidencia", evidencia.idEvidencia));
                parametros.Add(new SqlParameter("@archivo", String.IsNullOrWhiteSpace(evidencia.archivo) ? DBNull.Value : (object)evidencia.archivo));
                parametros.Add(new SqlParameter("@observacion", String.IsNullOrWhiteSpace(evidencia.observacion) ? DBNull.Value : (object)evidencia.observacion));
                parametros.Add(new SqlParameter("@idTema", evidencia.idTema));
                parametros.Add(new SqlParameter("@idUnidad", evidencia.idUnidad));
                parametros.Add(new SqlParameter("@idRecurso", evidencia.idRecurso));
                parametros.Add(new SqlParameter("@retroalimentacion", String.IsNullOrWhiteSpace(evidencia.retroalimentacion) ? DBNull.Value : (object)evidencia.retroalimentacion));
                parametros.Add(new SqlParameter("@entregado", evidencia.entregado)); 
                parametros.Add(new SqlParameter("@userName", evidencia.userName));
                parametros.Add(new SqlParameter("@puntosAlcanzados", (object)evidencia.puntosAlcanzados == null ? DBNull.Value : (object)evidencia.puntosAlcanzados));
                devidencia = new DataSet();
                server.ejecutarQuery(@" IF EXISTS (SELECT * FROM Evidencias WHERE idEvidencia=@idEvidencia) 
                                        BEGIN
                                           UPDATE Evidencias
                                               SET archivo = @archivo,
                                                  observacion = @observacion,
                                                  retroalimentacion = @retroalimentacion,
                                                  puntosAlcanzados = @puntosAlcanzados,
                                                  entregado = @entregado                                               
                                             WHERE idEvidencia=@idEvidencia
                                        END
                                        ELSE
                                        BEGIN
                                            INSERT INTO Evidencias
                                                   (archivo,observacion,idTema,idUnidad,idRecurso,retroalimentacion,entregado,userName)
                                             VALUES
                                                    (@archivo,@observacion,@idTema,@idUnidad,@idRecurso,@retroalimentacion,@entregado,@userName)
                                        END SELECT E.idEvidencia,E.archivo,E.observacion,E.idTema,E.idUnidad,E.idRecurso,E.retroalimentacion,
                                        CAST(ISNULL(E.puntosAlcanzados,0) AS INT) puntosAlcanzados,E.entregado,E.userName,U.nombre,U.apellidos 
                                        FROM Evidencias E
                                        INNER JOIN Usuarios U ON E.userName=U.userName
                                        WHERE idEvidencia=@idEvidencia", parametros, out devidencia);
                server.close();

                if (devidencia != null && devidencia.Tables[0].Rows.Count > 0)
                {
                    dtevidencia = new DataTable();
                    dtevidencia = devidencia.Tables[0];
                    evi = dtevidencia.Rows.Cast<DataRow>().Select(r => new Evidencia
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
                        userName = r.Field<string>("userName"),
                        nombres = r.Field<string>("nombre"),
                        apellidos = r.Field<string>("apellidos"),
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return evi;
        }

        /// <summary>
        /// Método que peromite consultar evidencias en base de datos.
        /// </summary>
        /// <param name="evidencia">Argumento evidencia, modelo de datos evidencia.</param>
        /// <returns>Retorna modelo de datos evidencia</returns>
        public List<Evidencia> consultarEvidencias(Evidencia evidencia)
        {
            List<Evidencia> listEvidencia = new List<Evidencia>();
            try
            {
                DataSet devidencia;
                DataTable dtevidencia;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idEvidencia", (object)evidencia.idEvidencia == null ? DBNull.Value : (object)evidencia.idEvidencia));
                parametros.Add(new SqlParameter("@archivo", String.IsNullOrWhiteSpace(evidencia.archivo) ? DBNull.Value : (object)evidencia.archivo));
                parametros.Add(new SqlParameter("@observacion", String.IsNullOrWhiteSpace(evidencia.observacion) ? DBNull.Value : (object)evidencia.observacion));
                parametros.Add(new SqlParameter("@idTema", (object)evidencia.idTema == null ? DBNull.Value : (object)evidencia.idTema));
                parametros.Add(new SqlParameter("@idUnidad", evidencia.idUnidad == 0 ? DBNull.Value : (object)evidencia.idUnidad));
                parametros.Add(new SqlParameter("@idRecurso", evidencia.idRecurso == 0 ? DBNull.Value : (object)evidencia.idRecurso));
                parametros.Add(new SqlParameter("@retroalimentacion", String.IsNullOrWhiteSpace(evidencia.retroalimentacion) ? DBNull.Value : (object)evidencia.retroalimentacion));
                parametros.Add(new SqlParameter("@entregado", (object)evidencia.entregado == null ? DBNull.Value : (object)evidencia.entregado));
                parametros.Add(new SqlParameter("@userName", (object)evidencia.userName == null ? DBNull.Value : (object)evidencia.userName));
                devidencia = new DataSet();
                server.ejecutarQuery(@"select 
                                        e.idevidencia,e.userName,s.nombre,s.apellidos,e.observacion,e.archivo,e.idTema,u.idunidad,u.nombre nombreUnidad,e.idRecurso,r.nombre nombreRecurso,e.retroalimentacion,e.entregado,CAST(ISNULL(E.puntosAlcanzados,0) AS INT) puntosAlcanzados                                        
                                        from evidencias E 
                                        inner join usuarios S on e.userName=s.userName
                                        inner join unidades U on E.idUnidad=U.idUnidad 
                                        inner join Recursos R on e.idRecurso=R.idRecurso
                                        where E.idUnidad=ISNULL(@idUnidad,E.idUnidad) and E.idRecurso=ISNULL(@idRecurso,E.idRecurso) and E.userName=ISNULL(@userName,E.userName) AND S.estado=1 and r.estado=1", parametros, out devidencia);
                server.close();

                if (devidencia != null && devidencia.Tables[0].Rows.Count > 0)
                {
                    dtevidencia = new DataTable();
                    dtevidencia = devidencia.Tables[0];
                    listEvidencia = dtevidencia.Rows.Cast<DataRow>().Select(r => new Evidencia
                    {
                        idEvidencia = r.Field<int>("idEvidencia"),
                        archivo = r.Field<string>("archivo"),
                        observacion = r.Field<string>("observacion"),
                        idTema = r.Field<int>("idTema"),
                        idUnidad = r.Field<int>("idUnidad"),
                        nombreUnidad = r.Field<string>("nombreUnidad"),
                        idRecurso = r.Field<int>("idRecurso"),
                        nombreRecurso = r.Field<string>("nombreRecurso"),
                        retroalimentacion = r.Field<string>("retroalimentacion"),
                        entregado = r.Field<bool>("entregado"),
                        nombres = r.Field<string>("nombre"),
                        apellidos = r.Field<string>("apellidos"),
                        userName = r.Field<string>("userName"),
                        puntosAlcanzados = r.Field<int>("puntosAlcanzados"),
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }

            return listEvidencia;
        }

        /// <summary>
        /// Método que permite consultar una evidencia en base de datos.
        /// </summary>
        /// <param name="idEvidencia">Argumento evidencia, modelo de datos evidencia.</param>
        /// <returns>Retorna modelo de datos evidencia</returns>
        public Evidencia evidencia(int idEvidencia)
        {
            Evidencia evi = new Evidencia();
            try
            {
                DataSet devidencia;
                DataTable dtevidencia;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idEvidencia", idEvidencia));
                devidencia = new DataSet();
                server.ejecutarQuery(@"SELECT E.idEvidencia,E.archivo,E.observacion,E.idTema,E.idUnidad,E.idRecurso,E.retroalimentacion,
                                        CAST(ISNULL(E.puntosAlcanzados,0) AS INT) puntosAlcanzados,E.entregado,E.userName,U.nombre,U.apellidos 
                                        FROM Evidencias E
                                        INNER JOIN Usuarios U ON E.userName=U.userName
                                        WHERE idEvidencia=@idEvidencia", parametros, out devidencia);
                server.close();

                if (devidencia != null && devidencia.Tables[0].Rows.Count > 0)
                {
                    dtevidencia = new DataTable();
                    dtevidencia = devidencia.Tables[0];
                    evi = dtevidencia.Rows.Cast<DataRow>().Select(r => new Evidencia
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
                        userName = r.Field<string>("userName"),
                        nombres = r.Field<string>("nombre"),
                        apellidos = r.Field<string>("apellidos"),
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return evi;
        }
    }
}