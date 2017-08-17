using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using OMIS.DBUtility;

namespace OMIS.DBA
{

    public abstract class OracleDataAccess : DataConvert
    {

        #region  DBConnString

        public static string DBConnString { get; set; }

        #endregion

        #region  BuildSqlParam
        public static object[] BuildSqlParam(int count, List<SqlParam> list)
        {
            OracleParameter[] param = new OracleParameter[count];
            int n = 0;
            foreach (SqlParam p in list)
            {
                if (p != null)
                {
                    param[n++] = new OracleParameter(p.Name, p.Value);
                }
            }
            return param;
        }

        public static OracleParameter[] BuildOracleSqlParam(int count, List<SqlParam> list)
        {
            OracleParameter[] param = new OracleParameter[count];
            int n = 0;
            foreach (SqlParam p in list)
            {
                if (p != null)
                {
                    param[n++] = new OracleParameter(p.Name, p.Value);
                }
            }
            return param;
        }

        public static object[] BuildSqlParam(int count, List<string> name, List<object> value)
        {
            OracleParameter[] param = new OracleParameter[count];
            int n = 0;
            for (int i = 0; i < count; i++)
            {
                param[n++] = new OracleParameter(name[i], value[i]);
            }
            return param;
        }

        public static OracleParameter[] BuildOracleSqlParam(int count, List<string> name, List<object> value)
        {
            OracleParameter[] param = new OracleParameter[count];
            int n = 0;
            for (int i = 0; i < count; i++)
            {
                param[n++] = new OracleParameter(name[i], value[i]);
            }
            return param;
        }

        public static object[] BuildSqlParam(string name, object value)
        {
            return new OracleParameter[] { new OracleParameter(name, value) };
        }

        public static OracleParameter[] BuildOracleSqlParam(string name, object value)
        {
            return new OracleParameter[] { new OracleParameter(name, value) };
        }
        #endregion


        #region  ExecuteDataSet
        public static DataSet ExecuteDataSet(string conn, string sql)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? OracleHelper.ExecuteDataSet(conn, sql) : null;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet ExecuteDataSet(string conn, string sql, OracleParameter[] param)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? OracleHelper.ExecuteDataSet(conn, CommandType.Text, sql, param) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ExecuteScalar
        public static object ExecuteScalar(string conn, string sql)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? OracleHelper.ExecuteScalar(conn, sql) : null;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object ExecuteScalar(string conn, string sql, OracleParameter[] param)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? OracleHelper.ExecuteScalar(conn, CommandType.Text, sql, param) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ExecuteNonQuery
        public static int ExecuteNonQuery(string conn, string sql)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? OracleHelper.ExecuteNonQuery(conn, sql) : -1;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int ExecuteNonQuery(string conn, string sql, OracleParameter[] param)
        {
            try
            {
                return CheckParam(conn, CheckSql(ref sql)) ? OracleHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param) : -1;
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

        public static DataSet Select(string conn, string sql, OracleParameter[] param)
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

        public static object Scalar(string conn, string sql, OracleParameter[] param)
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

        public static int Insert(string conn, string sql, OracleParameter[] param)
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

        public static int Update(string conn, string sql, OracleParameter[] param)
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

        public static int Delete(string conn, string sql, OracleParameter[] param)
        {
            try
            {
                return ExecuteNonQuery(conn, sql, param);
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
                    sql.Append(String.Format(" SELECT COUNT(*) FROM User_Tables WHERE table_name = '{0}'; ", tableName.ToUpper()));
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