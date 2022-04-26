using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Proyecto.Funciones
{
    public class ConSqlServer
    {
        private SqlConnection con = null;
        private SqlTransaction tran = null;

        public ConSqlServer()
        {
            if (con == null)
                throw new System.ArgumentException("No se han asignado los datos de conexión.");
        }

        public ConSqlServer(SqlConnectionStringBuilder stringConn)
        {
            stringConn.ConnectTimeout = 0;
            con = new SqlConnection(stringConn.ConnectionString);
        }

        public ConnectionState abrirConexion(bool isTransact = false)
        {
            try
            {
                if (con != null)
                {
                    con.Open();
                    if (isTransact)
                        tran = con.BeginTransaction();
                }
                else
                    con = new SqlConnection();
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }
            return con.State;
        }

        public void close()
        {
            if (this.tran != null)
                if (this.tran.Connection.State == ConnectionState.Open)
                    this.tran.Rollback();

            this.con.Close();
            this.con.Dispose();
        }

        public bool ejecutarQuery(string SqlQuery, List<SqlParameter> _parametros, out DataTable _Datos, CommandType tipo = CommandType.Text, bool istransactional = false)
        {
            bool resultado = false;
            _Datos = new DataTable();
            try
            {
                if (con != null)
                {
                    try
                    {
                        if (this.con.State != ConnectionState.Open)
                            abrirConexion(istransactional);
                        SqlCommand command = new SqlCommand(SqlQuery, con);
                        if (this.tran != null)
                            command.Transaction = this.tran;
                        command.CommandTimeout = 0;
                        command.CommandType = tipo;
                        command.Parameters.AddRange(_parametros.ToArray());
                        _Datos.Load(command.ExecuteReader());
                        _parametros = new List<SqlParameter>();
                        resultado = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parametros != null && _parametros.Count > 0)
                        {
                            _parametros.Cast<SqlParameter>().ToList().ForEach(p => {
                                //Log.tareas.Add("Parámetro nombre:" + p.ParameterName + " Valor:" + p.Value);
                            });

                        }
                        Funcion.tareas.Add("Error al ejecutar la consulta " + SqlQuery + " [mensaje: " + ex.Message + "]");
                        Funcion.write();
                    }
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error al ejecutar la consulta" + SqlQuery + " [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return resultado;
        }

        public bool ejecutarQuery(string SqlQuery, List<SqlParameter> _parametros, out DataSet _Datos, CommandType tipo = CommandType.Text)
        {

            bool resultado = false;
            _Datos = new DataSet();
            try
            {
                if (con != null)
                {
                    try
                    {
                        if (this.con.State != ConnectionState.Open)
                            abrirConexion();

                        SqlDataAdapter adapter = new SqlDataAdapter(SqlQuery, this.con);
                        adapter.SelectCommand.CommandType = tipo;
                        adapter.SelectCommand.Parameters.Clear();
                        adapter.SelectCommand.CommandTimeout = 9999999;
                        _Datos.Clear();
                        adapter.SelectCommand.Parameters.AddRange(_parametros.ToArray());
                        adapter.Fill(_Datos);

                        resultado = true;
                    }
                    catch (Exception ex)
                    {
                        Funcion.tareas.Add("Error al ejecutar la consulta" + SqlQuery + " [mensaje: " + ex.Message + "]");
                        Funcion.write();
                    }
                }
            }
            catch (Exception ex)
            {
                Funcion.tareas.Add("Error al ejecutar la consulta" + SqlQuery + " [mensaje: " + ex.Message + "]");
                Funcion.write();
            }
            return resultado;
        }

    }
}