using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using OMIS.DBUtility;

namespace OMIS.DBA
{
    public abstract class MssqlDataAccess : DataConvert
    {

        #region  DBConnString

        public static string DBConnString { get; set; }

        #endregion

        #region  BuildSqlParam
        public static object[] BuildSqlParam(int count, List<SqlParam> list)
        {
            SqlParameter[] param = new SqlParameter[count];
            int n = 0;
            foreach (SqlParam p in list)
            {
                if (p != null)
                {
                    param[n++] = new SqlParameter(p.Name, p.Value);
                }
            }
            return param;
        }

        public static SqlParameter[] BuildMsSqlParam(int count, List<SqlParam> list)
        {
            SqlParameter[] param = new SqlParameter[count];
            int n = 0;
            foreach (SqlParam p in list)
            {
                if (p != null)
                {
                    param[n++] = new SqlParameter(p.Name, p.Value);
                }
            }
            return param;
        }

        public static object[] BuildSqlParam(int count, List<string> name, List<object> value)
        {
            SqlParameter[] param = new SqlParameter[count];
            int n = 0;
            for (int i = 0; i < count; i++)
            {
                param[n++] = new SqlParameter(name[i], value[i]);
            }
            return param;
        }

        public static SqlParameter[] BuildMsSqlParam(int count, List<string> name, List<object> value)
        {
            SqlParameter[] param = new SqlParameter[count];
            int n = 0;
            for (int i = 0; i < count; i++)
            {
                param[n++] = new SqlParameter(name[i], value[i]);
            }
            return param;
        }

        public static object[] BuildSqlParam(string name, object value)
        {
            return new SqlParameter[] { new SqlParameter(name, value) };
        }

        public static SqlParameter[] BuildMsSqlParam(string name, object value)
        {
            return new SqlParameter[] { new SqlParameter(name, value) };
        }
        #endregion


        #region  ExecuteDataSet
        public static DataSet ExecuteDataSet(string conn, string sql)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? MssqlHelper.ExecuteDataSet(conn, sql) : null;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet ExecuteDataSet(string conn, string sql, SqlParameter[] param)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? MssqlHelper.ExecuteDataSet(conn, CommandType.Text, sql, param) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ExecuteScalar
        public static object ExecuteScalar(string conn, string sql)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? MssqlHelper.ExecuteScalar(conn, sql) : null;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object ExecuteScalar(string conn, string sql, SqlParameter[] param)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? MssqlHelper.ExecuteScalar(conn, CommandType.Text, sql, param) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ExecuteNonQuery
        public static int ExecuteNonQuery(string conn, string sql)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? MssqlHelper.ExecuteNonQuery(conn, sql) : -1;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int ExecuteNonQuery(string conn, string sql, SqlParameter[] param)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? MssqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param) : -1;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  Select
        public static DataSet Select(string conn, string sql)
        {
            try
            {
                return ExecuteDataSet(conn, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet Select(string conn, string sql, SqlParameter[] param)
        {
            try
            {
                return ExecuteDataSet(conn, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Scalar
        public static object Scalar(string conn, string sql)
        {
            try
            {
                return ExecuteScalar(conn, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object Scalar(string conn, string sql, SqlParameter[] param)
        {
            try
            {
                return ExecuteScalar(conn, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Insert
        public static int Insert(string conn, string sql)
        {
            try
            {
                return ExecuteNonQuery(conn, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Insert(string conn, string sql, SqlParameter[] param)
        {
            try
            {
                return ExecuteNonQuery(conn, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Update
        public static int Update(string conn, string sql)
        {
            try
            {
                return ExecuteNonQuery(conn, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Update(string conn, string sql, SqlParameter[] param)
        {
            try
            {
                return ExecuteNonQuery(conn, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Delete
        public static int Delete(string conn, string sql)
        {
            try
            {
                return ExecuteNonQuery(conn, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Delete(string conn, string sql, SqlParameter[] param)
        {
            try
            {
                return ExecuteNonQuery(conn, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  CheckDatabaseExists
        public static bool CheckDatabaseExists(string conn, string dbName)
        {
            try
            {
                if (Filter(ref dbName).Length > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append(String.Format(" select count(name) as dc from master.dbo.sysdatabases where name = '{0}'; ", dbName));
                    sql.Append(";");

                    return Convert.ToInt32(ExecuteScalar(conn, sql.ToString()).ToString()) > 0;
                }
                return false;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  CheckDataTableExists
        public static bool CheckDataTableExists(string conn, string tableName)
        {
            try
            {
                return CheckDataTableExists(conn, tableName, string.Empty);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static bool CheckDataTableExists(string conn, string tableName, string dbName)
        {
            try
            {
                if (Filter(ref tableName).Length > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append(Filter(ref dbName).Length > 0 ? String.Format("use {0};", dbName) : "");
                    sql.Append(String.Format(" select count(name) as dc from sysobjects where name = '{0}'; ", tableName));
                    sql.Append(";");

                    return Convert.ToInt32(ExecuteScalar(conn, sql.ToString()).ToString()) > 0;
                }
                return false;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}