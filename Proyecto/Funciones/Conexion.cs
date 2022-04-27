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
            //this.DB = new SQLiteDataAdapter();
            //this.DS = new DataSet();

            //sql_con = new SQLiteConnection
            //("Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "\\BD.reg;Version=3;");

            //if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\BD.reg"))
            //    createDataBase();

            //if (sql_con.State != ConnectionState.Open)
            //    sql_con.Open();
        }

        //private void createDataBase()
        //{
        //    SQLiteConnection.CreateFile(System.AppDomain.CurrentDomain.BaseDirectory + "\\BD.reg");
        //    sql_con.Open();
        //    sql_cmd = new SQLiteCommand(@"CREATE TABLE ConfigBD (
	       //     cnfidbd varchar(80) NOT NULL,
	       //     cnfnombre varchar(80) NOT NULL,
	       //     cnfservidor varchar(40) NOT NULL,
	       //     cnfusuario varchar(20) NOT NULL,
	       //     cnfclave varchar(20) NOT NULL,
	       //     cnfbasedatos varchar(20) NOT NULL,
	       //     PRIMARY KEY(cnfidbd)
        //    );
        //    CREATE INDEX ConfigBD_cnfnombre_idx ON ConfigBD (cnfnombre);
        //    CREATE INDEX ConfigBD_cnfbasedatos_idx ON ConfigBD (cnfbasedatos);

        //    CREATE TABLE ConfigEmail (
	       //     emlnombre varchar(80) NOT NULL,
	       //     emlservidor varchar(40) NOT NULL,
	       //     emlusuario varchar(20) NOT NULL,
	       //     emlclave varchar(20) NOT NULL,
	       //     emlpuerto INT NOT NULL,
	       //     emlssl BIT NOT NULL,
	       //     PRIMARY KEY(emlNombre)
        //    );", sql_con);
        //    sql_cmd.ExecuteNonQuery();
        //}

        //public SqlConnectionStringBuilder obtenerConexionSQLServer(string dbNombreDBServer)
        //{
        //    SqlConnectionStringBuilder con = null;

        //    this.sqlTablas.Add("ConfigBD");
        //    this.sqlCampos.Add("cnfservidor,cnfusuario,cnfclave,cnfbasedatos");
        //    this.sqlCondicionesY.Add("lower(cnfnombre) = '" + dbNombreDBServer.ToLower() + "'");
        //    this.select();
        //    this.ejecutarSQL("ConfigBD");
        //    DataTable dt = this.obtenerTabla("ConfigBD");
        //    this.reiniciarSql();

        //    if (dt.Rows.Count > 0)
        //    {
        //        if (!String.IsNullOrEmpty(dt.Rows[0].Field<string>("cnfservidor")))
        //        {
        //            con = new SqlConnectionStringBuilder();
        //            con.DataSource = dt.Rows[0].Field<string>("cnfservidor");
        //            con.InitialCatalog = dt.Rows[0].Field<string>("cnfbasedatos");
        //            con.UserID = dt.Rows[0].Field<string>("cnfusuario");
        //            con.Password = dt.Rows[0].Field<string>("cnfclave");
        //        }
        //    }
        //    else
        //    {
        //    }

        //    return con;
        //}

        public SqlConnectionStringBuilder ConexionSQLServer()
        {
            SqlConnectionStringBuilder con = null;
            this.reiniciarSql();
            con = new SqlConnectionStringBuilder();

            //con.DataSource = "192.168.1.18";
            //con.InitialCatalog = "db";
            //con.UserID = "syscom";
            //con.Password = "u.owner";


            con.DataSource = "(local)";
            con.InitialCatalog = "dbproyecto";
            con.UserID = "admin";
            con.Password = "adminova";

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