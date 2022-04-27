using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto.Funciones
{
    public class Conexion
    {
        private SQLiteDataAdapter DB;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;

        public List<string> sqlTablas = new List<string>();
        public List<string> sqlCampos = new List<string>();
        public List<string> sqlCondicionesY = new List<string>();
        public List<string> sqlCondicionesO = new List<string>();
        public List<string> sqlOrderBy = new List<string>();
        private List<string> sqlConsultas = new List<string>();

        public Conexion()
        {
        }

        public SqlConnectionStringBuilder ConexionSQLServer()
        {
            SqlConnectionStringBuilder con = null;
            this.reiniciarSql();
            con = new SqlConnectionStringBuilder();

            con.DataSource = "192.168.1.18";
            con.InitialCatalog = "db";
            con.UserID = "syscom";
            con.Password = "u.owner";


            //con.DataSource = "(local)";
            //con.InitialCatalog = "dbproyecto";
            //con.UserID = "admin";
            //con.Password = "adminova";

            return con;
        }

        public void select()
        {
            string sqlQuery = "select " + (this.sqlCampos.Count > 0 ? String.Join(",", this.sqlCampos.ToArray()) : " * ");
            sqlQuery += " from " + String.Join(" ", sqlTablas.ToArray());
            if ((this.sqlCondicionesY.Count() > 0) || (this.sqlCondicionesO.Count() > 0))
            {
                sqlQuery += " where ";
                sqlQuery += (this.sqlCondicionesY.Count() > 0 ? String.Join(" AND ", this.sqlCondicionesY.ToArray()) : "");
                sqlQuery += (this.sqlCondicionesO.Count() > 0 ? (this.sqlCondicionesY.Count() > 0 ? " AND " : "") + "(" + String.Join(" OR ", this.sqlCondicionesO.ToArray()) + ")" : "");
            }
            sqlQuery += (this.sqlOrderBy.Count() > 0 ? " order by " + String.Join(",", this.sqlOrderBy.ToArray()) : "");
            reiniciar();
            this.sqlConsultas.Add(sqlQuery);
        }

        public void reiniciar()
        {
            this.sqlCondicionesY.Clear();
            this.sqlCondicionesO.Clear();
            this.sqlTablas.Clear();
            this.sqlCampos.Clear();
            this.sqlOrderBy.Clear();
            this.DS = new DataSet();
            this.DT = new DataTable();
        }

        public void ejecutarSQL(string _pmTabla = null, bool result = true)
        {
            try
            {
                foreach (string sql in this.sqlConsultas)
                {
                    this.DB.SelectCommand = new SQLiteCommand(sql, this.sql_con);
                    this.DB.SelectCommand.CommandType = CommandType.Text;
                    this.DB.SelectCommand.Parameters.Clear();
                    switch (sql.Split(' ')[0].ToLower())
                    {
                        case "update":
                            this.DB.SelectCommand.ExecuteNonQuery();
                            break;
                        case "delete":
                            this.DB.SelectCommand.ExecuteNonQuery();
                            break;
                        case "insert":
                            this.DB.SelectCommand.ExecuteNonQuery();
                            break;
                        default:
                            this.DB.SelectCommand.ExecuteNonQuery();
                            if (result == true)
                            {
                                if (String.IsNullOrEmpty(_pmTabla))
                                    this.DB.Fill(this.DS);
                                else
                                    this.DB.Fill(this.DS, _pmTabla);
                            }
                            break;
                    }
                }
                this.sqlConsultas.Clear();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        public void reiniciarSql()
        {
            this.reiniciar();
            this.sqlConsultas.Clear();
        }

        public DataTable obtenerTabla(string _pmNombreTablaProc)
        {
            return DS.Tables[_pmNombreTablaProc];
        }
    }
}