using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>数据库提供者</summary>
    public static class ETLDbProvider
    {
        /// <summary>Odbc</summary>
        public const string Odbc = "System.Data.Odbc";
        /// <summary>OleDb</summary>
        public const string OleDb = "System.Data.OleDb";
        /// <summary>SQLServer</summary>
        public const string SqlClient = "System.Data.SqlClient";
        /// <summary>MS提供的Oracle客户端</summary>
        public const string MSOracleClient = "System.Data.OracleClient";
        /// <summary>Oracle客户端</summary>
        public const string OracleClient = "Oracle.ManagedDataAccess.Client";
        /// <summary>Oracle原生客户端</summary>
        public const string OracleUnmanagedClient = "Oracle.DataAccess.Client";
    }
}
