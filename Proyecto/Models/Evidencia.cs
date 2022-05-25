using Proyecto.Funciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Evidencia
    {
        public int idEvidencia { get; set; }
        public string archivo { get; set; }
        public HttpPostedFileBase fileArchivo { get; set; }
        public string observacion { get; set; }
        public int idTema { get; set; }
        public int idUnidad { get; set; }
        public int idRecurso { get; set; }
        public string retroalimentacion { get; set; }
        public int puntosAlcanzados { get; set; }
        public bool entregado { get; set; }
        public string userName { get; set; }

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
                devidencia = new DataSet();
                server.ejecutarQuery(@" IF EXISTS (SELECT * FROM Evidencias WHERE idEvidencia=@idEvidencia) 
                                        BEGIN
                                           UPDATE Evidencias
                                               SET archivo = @archivo,
                                                  observacion = @observacion,
                                                  retroalimentacion = @retroalimentacion,
                                                  entregado = @entregado                                               
                                             WHERE idEvidencia=@idEvidencia
                                        END
                                        ELSE
                                        BEGIN
                                            INSERT INTO Evidencias
                                                   (archivo,observacion,idTema,idUnidad,idRecurso,retroalimentacion,entregado,userName)
                                             VALUES
                                                    (@archivo,@observacion,@idTema,@idUnidad,@idRecurso,@retroalimentacion,@entregado,@userName)
                                        END SELECT * FROM Evidencias WHERE userName=@userName and idrecurso=@idrecurso", parametros, out devidencia);
                server.close();

                if (devidencia != null && devidencia.Tables[0].Rows.Count > 0)
                {
                    dtevidencia = new DataTable();
                    dtevidencia = devidencia.Tables[0];
                    evidencia = dtevidencia.Rows.Cast<DataRow>().Select(r => new Evidencia
                    {
                        idEvidencia = r.Field<int>("idEvidencia"),
                        archivo = r.Field<string>("archivo"),
                        observacion = r.Field<string>("observacion"),
                        idTema = r.Field<int>("idTema"),
                        idUnidad = r.Field<int>("idUnidad"),
                        idRecurso = r.Field<int>("idRecurso"),
                        retroalimentacion = r.Field<string>("retroalimentacion"),
                        entregado = r.Field<bool>("entregado"),
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