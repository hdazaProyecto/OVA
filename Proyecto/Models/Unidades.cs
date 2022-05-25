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
    public class Unidades
    {
        public int idUnidad { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string imagen { get; set; }
        public bool estado { get; set; }
        public int idTema { get; set; }        
        public DateTime fecha { get; set; }
        public DateTime? fechaModifica { get; set; }
        public string userName { get; set; }

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        /// <summary>
        /// Método que permite listar las unidades creadas.
        /// </summary>
        /// <returns>Retorna lista de modelo unidades</returns>
        public List<Unidades> listarUnidades()
        {
           List<Unidades>  unidad = new List<Unidades>();
            try
            {
                DataSet dunidad;
                DataTable dtunidad;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                server.ejecutarQuery(@"SELECT * FROM Unidades ORDER BY idUnidad", parametros, out dunidad);
                server.close();

                if (dunidad != null && dunidad.Tables[0].Rows.Count > 0)
                {
                    dtunidad = new DataTable();
                    dtunidad = dunidad.Tables[0];

                    unidad = dtunidad.AsEnumerable().Select(r => new Unidades()
                    {
                        idUnidad = r.Field<int>("idUnidad"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        estado = r.Field<bool>("estado"),
                        idTema = r.Field<int>("idTema"),
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
            return unidad;
        }

        /// <summary>
        /// Método que permite gestionar unidades crear o actualizar unidades
        /// </summary>
        /// <param name="Punidad">Argumento Punidad, modelo de datos Unidades.</param>
        /// <returns>Retotna modelo de Unidades</returns>
        public Unidades gestionarUnidad(Unidades Punidad)
        {
            Unidades unidad = new Unidades();
            try
            {
                DataSet dunidad;
                DataTable dtunidad;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idUnidad", Punidad.idUnidad));
                parametros.Add(new SqlParameter("@nombre", Punidad.nombre));
                parametros.Add(new SqlParameter("@descripcion", Punidad.descripcion));
                parametros.Add(new SqlParameter("@idTema", Punidad.idTema));
                parametros.Add(new SqlParameter("@estado", Punidad.estado));
                parametros.Add(new SqlParameter("@fecha", Punidad.fecha));
                parametros.Add(new SqlParameter("@fechaModifica", DateTime.Now));
                parametros.Add(new SqlParameter("@userName", Punidad.userName));
                dunidad = new DataSet();
                server.ejecutarQuery(@" IF EXISTS (SELECT * FROM Unidades WHERE idUnidad=@idUnidad) 
                                        BEGIN
                                           UPDATE Unidades
                                               SET nombre = @nombre,
                                                  descripcion = @descripcion,
                                                  estado = @estado,
                                                  idTema = @idTema,
                                                  fecha = @fecha,
                                                  fechaModifica = @fechaModifica,
                                                  userName = @userName
                                             WHERE idUnidad=@idUnidad
                                        END
                                        ELSE
                                        BEGIN
                                            INSERT INTO Unidades
                                                   (nombre,descripcion,estado,idTema,fecha,userName)
                                             VALUES
                                                   (@nombre,@descripcion,@estado,@idTema,@fecha,@userName)
                                        END SELECT * FROM Unidades", parametros, out dunidad);
                server.close();

                if (dunidad != null && dunidad.Tables[0].Rows.Count > 0)
                {
                    dtunidad = new DataTable();
                    dtunidad = dunidad.Tables[0];
                    unidad = dtunidad.Rows.Cast<DataRow>().Select(r => new Unidades
                    {
                        idUnidad = r.Field<int>("idUnidad"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        estado = r.Field<bool>("estado"),
                        idTema = r.Field<int>("idTema"),
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

            return unidad;
        }

        /// <summary>
        /// Método que busca una unidad especifica para editar.
        /// </summary>
        /// <param name="idUnidad">Argumento idUnidad, id de la unidad a consultar.</param>
        /// <returns>Retorna modelo de unidades</returns>
        public Unidades editarUnidades(int idUnidad)
        {
            Unidades unidad = new Unidades();
            try
            {
                DataSet dunidad;
                DataTable dtunidad;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idUnidad", idUnidad));
                server.ejecutarQuery(@"SELECT * FROM Unidades WHERE idUnidad=@idUnidad", parametros, out dunidad);
                server.close();

                if (dunidad != null && dunidad.Tables[0].Rows.Count > 0)
                {
                    dtunidad = new DataTable();
                    dtunidad = dunidad.Tables[0];
                    unidad = dtunidad.Rows.Cast<DataRow>().Select(r => new Unidades
                    {
                        idUnidad = r.Field<int>("idUnidad"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        estado = r.Field<bool>("estado"),
                        idTema = r.Field<int>("idTema"),
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
            return unidad;
        }

        /// <summary>
        /// Método que peromite actualizar estado de unidades de Activo a inactivo o viceversa.
        /// </summary>
        /// <param name="idUnidad">Argumento idUnidad, id de la unidad a consultar.</param>
        /// <returns>retorna listado de modelo de unidades</returns>
        public List<Unidades> cambiarestadouni(int idUnidad)
        {
            List<Unidades> unidad = new List<Unidades>();
            try
            {
                DataSet dunidad;
                DataTable dtunidad;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@idUnidad", idUnidad));
                server.ejecutarQuery(@"UPDATE Unidades SET estado=CASE WHEN estado=1 THEN 0 ELSE 1 END WHERE idUnidad=@idUnidad 
                                        SELECT * FROM Unidades ORDER BY idUnidad", parametros, out dunidad);
                server.close();

                if (dunidad != null && dunidad.Tables[0].Rows.Count > 0)
                {
                    dtunidad = new DataTable();
                    dtunidad = dunidad.Tables[0];

                    unidad = dtunidad.AsEnumerable().Select(r => new Unidades()
                    {
                        idUnidad = r.Field<int>("idUnidad"),
                        nombre = r.Field<string>("nombre"),
                        descripcion = r.Field<string>("descripcion"),
                        imagen = r.Field<string>("imagen"),
                        estado = r.Field<bool>("estado"),
                        idTema = r.Field<int>("idTema"),
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
            return unidad;
        }
    }
}