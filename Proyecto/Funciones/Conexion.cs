using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
//using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto.Funciones
{
    public class Conexion
    {
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        //private SQLiteCommand sql_cmd;

        public List<string> sqlTablas = new List<string>();
        public List<string> sqlCampos = new List<string>();
        public List<string> sqlCondicionesY = new List<string>();
        public List<string> sqlCondicionesO = new List<string>();
        public List<string> sqlOrderBy = new List<string>();
        private List<string> sqlConsultas = new List<string>();

        public Conexion()
        {
        }

        /// <summary>
        /// Método que permite la conexion a la base de datos configurada.
        /// </summary>
        /// <returns>Retorna datos de la conexion a la base de datos</returns>
        public SqlConnectionStringBuilder ConexionSQLServer()
        {
            SqlConnectionStringBuilder con = null;
            this.reiniciarSql();
            con = new SqlConnectionStringBuilder();

            con.DataSource = "demos.syscom.com.co";
            //base de datos produccion de plataforma publicada
            con.InitialCatalog = "Proyecto";
            //Base de datos de pruebas de plataforma
            //con.InitialCatalog = "db";
            con.UserID = "syscom";
            con.Password = "u.owner";
            return con;
        }

        ///// <summary>
        ///// Método que.
        ///// </summary>
        ///// <returns></returns>
        //public void select()
        //{
        //    string sqlQuery = "select " + (this.sqlCampos.Count > 0 ? String.Join(",", this.sqlCampos.ToArray()) : " * ");
        //    sqlQuery += " from " + String.Join(" ", sqlTablas.ToArray());
        //    if ((this.sqlCondicionesY.Count() > 0) || (this.sqlCondicionesO.Count() > 0))
        //    {
        //        sqlQuery += " where ";
        //        sqlQuery += (this.sqlCondicionesY.Count() > 0 ? String.Join(" AND ", this.sqlCondicionesY.ToArray()) : "");
        //        sqlQuery += (this.sqlCondicionesO.Count() > 0 ? (this.sqlCondicionesY.Count() > 0 ? " AND " : "") + "(" + String.Join(" OR ", this.sqlCondicionesO.ToArray()) + ")" : "");
        //    }
        //    sqlQuery += (this.sqlOrderBy.Count() > 0 ? " order by " + String.Join(",", this.sqlOrderBy.ToArray()) : "");
        //    reiniciar();
        //    this.sqlConsultas.Add(sqlQuery);
        //}

        /// <summary>
        /// Método que permite reiniciar clases pata consultas.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método que permite reiniciar clases.
        /// </summary>
        /// <returns></returns>
        public void reiniciarSql()
        {
            this.reiniciar();
            this.sqlConsultas.Clear();
        }

        ///// <summary>
        ///// Método que.
        ///// </summary>
        ///// <returns></returns>
        //public DataTable obtenerTabla(string _pmNombreTablaProc)
        //{
        //    return DS.Tables[_pmNombreTablaProc];
        //}
    }
}