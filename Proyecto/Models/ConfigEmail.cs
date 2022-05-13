using Proyecto.Funciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class ConfigEmail
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string servidor { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public int puerto { get; set; }
        public bool ssl { get; set; }

        private Conexion conexion;
        DataSet dsusuario;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;

        public ConfigEmail ConsultaConfiguracion()
        {
            ConfigEmail conf = new ConfigEmail();
            try
            {
                DataSet dconfiguracion = new DataSet();
                DataTable dtconfiguracion = new DataTable();
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                server.ejecutarQuery(@"SELECT TOP 1 * FROM configEmail", parametros, out dconfiguracion);
                server.close();
                if (dconfiguracion != null && dconfiguracion.Tables[0].Rows.Count > 0)
                {
                    dtconfiguracion = dconfiguracion.Tables[0];
                    conf = dtconfiguracion.Rows.Cast<DataRow>().Select(r => new ConfigEmail
                    {
                        nombre = r.Field<string>("nombre"),
                        servidor = r.Field<string>("servidor"),
                        usuario = r.Field<string>("usuario"),
                        clave = r.Field<string>("clave"),
                        puerto = r.Field<int>("puerto"),
                        ssl = r.Field<bool>("ssl"),
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return conf;
        }

        public ConfigEmail gestioanarConfiguracion(ConfigEmail configuracion)
        {
            ConfigEmail conf = new ConfigEmail();
            try
            {
                DataSet dconfiguracion = new DataSet();
                DataTable dtconfiguracion = new DataTable();
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@id", configuracion.id));
                parametros.Add(new SqlParameter("@nombre", configuracion.nombre));
                parametros.Add(new SqlParameter("@servidor", configuracion.servidor));
                parametros.Add(new SqlParameter("@usuario", configuracion.usuario));
                parametros.Add(new SqlParameter("@clave", Funcion.stringBase64(Funcion.Encriptar(configuracion.clave))));
                parametros.Add(new SqlParameter("@puerto", configuracion.puerto));
                parametros.Add(new SqlParameter("@ssl", configuracion.ssl));
                server.ejecutarQuery(@"IF EXISTS (SELECT * FROM configEmail WHERE id=@id) 
                                        BEGIN
	                                        UPDATE configEmail
		                                    SET nombre = @nombre,
		                                    servidor = @servidor,
		                                    usuario = @usuario,
		                                    clave = @clave,
		                                    puerto = @puerto,
		                                    ssl = @ssl
		                                    WHERE id=@id
                                        END
                                        ELSE
                                        BEGIN
                                            INSERT INTO configEmail (nombre,servidor,usuario,clave,puerto,ssl) VALUES (@nombre,@servidor,@usuario,@clave,@puerto,@ssl)
                                        END 
                                            SELECT TOP 1 * FROM configEmail", parametros, out dconfiguracion);
                server.close();
                if (dconfiguracion != null && dconfiguracion.Tables[0].Rows.Count > 0)
                {
                    dtconfiguracion = dconfiguracion.Tables[0];
                    conf = dtconfiguracion.Rows.Cast<DataRow>().Select(r => new ConfigEmail
                    {
                        nombre = r.Field<string>("nombre"),
                        servidor = r.Field<string>("servidor"),
                        usuario = r.Field<string>("usuario"),
                        clave = Funcion.base64String(Funcion.DesEncriptar(r.Field<string>("clave"))),
                        puerto = r.Field<int>("puerto"),
                        ssl = r.Field<bool>("ssl"),
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return conf;
        }
    }
}