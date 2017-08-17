using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.OracleClient;
using MySql.Data.MySqlClient;

namespace OMIS.DBA
{

    #region  数据库类型
    public enum DatabaseType
    {
        MySql,
        MsSql,
        Oracle,
    }
    #endregion

    #region  DataAccess
    public abstract class DataAccess : DataConvert
    {

        #region  DBConnString

        public static string DBConnString { get; set; }

        #endregion

        #region  DatabaseType

        public static DatabaseType DBTYPE = DatabaseType.MySql;

        #endregion
        

        #region  BuildSqlParam
        public static object[] BuildSqlParam(int count, List<SqlParam> list)
        {
            object[] param = null;
            switch (DBTYPE)
            {
                case DatabaseType.MsSql:
                    param = MssqlDataAccess.BuildSqlParam(count, list);
                    break;
                case DatabaseType.Oracle:
                    param = OracleDataAccess.BuildSqlParam(count, list);
                    break;
                case DatabaseType.MySql:
                default:
                    param = MysqlDataAccess.BuildSqlParam(count, list);
                    break;
            }
            return param;
        }

        public static object[] BuildSqlParam(int count, List<string> name, List<object> value)
        {
            object[] param = null;
            switch (DBTYPE)
            {
                case DatabaseType.MsSql:
                    param = MssqlDataAccess.BuildSqlParam(count, name, value);
                    break;
                case DatabaseType.Oracle:
                    param = OracleDataAccess.BuildSqlParam(count, name, value);
                    break;
                case DatabaseType.MySql:
                default:
                    param = MysqlDataAccess.BuildSqlParam(count, name, value);
                    break;
            }
            return param;
        }

        public static object[] BuildSqlParam(string name, object value)
        {
            object[] param = null;
            switch (DBTYPE)
            {
                case DatabaseType.MsSql:
                    param = MssqlDataAccess.BuildSqlParam(name, value);
                    break;
                case DatabaseType.Oracle:
                    param = OracleDataAccess.BuildSqlParam(name, value);
                    break;
                case DatabaseType.MySql:
                default:
                    param = MysqlDataAccess.BuildSqlParam(name, value);
                    break;
            }
            return param;
        }
        #endregion


        #region  ExecuteDataSet
        public static DataSet ExecuteDataSet(string connString, string sql)
        {
            try
            {
                DataSet result = null;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.ExecuteDataSet(connString, sql);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.ExecuteDataSet(connString, sql);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.ExecuteDataSet(connString, sql);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet ExecuteDataSet(string connString, string sql, object[] param)
        {
            try
            {
                DataSet result = null;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.ExecuteDataSet(connString, sql, param != null ? (SqlParameter[])param : null);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.ExecuteDataSet(connString, sql, param != null ? (OracleParameter[])param : null);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.ExecuteDataSet(connString, sql, param != null ? (MySqlParameter[])param : null);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ExecuteScalar
        public static object ExecuteScalar(string connString, string sql)
        {
            try
            {
                object result = null;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.ExecuteScalar(connString, sql);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.ExecuteScalar(connString, sql);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.ExecuteScalar(connString, sql);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object ExecuteScalar(string connString, string sql, object[] param)
        {
            try
            {
                object result = null;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.ExecuteScalar(connString, sql, param != null ? (SqlParameter[])param : null);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.ExecuteScalar(connString, sql, param != null ? (OracleParameter[])param : null);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.ExecuteScalar(connString, sql, param != null ? (MySqlParameter[])param : null);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ExecuteNonQuery
        public static int ExecuteNonQuery(string connString, string sql)
        {
            try
            {
                int result = 0;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.ExecuteNonQuery(connString, sql);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.ExecuteNonQuery(connString, sql);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.ExecuteNonQuery(connString, sql);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int ExecuteNonQuery(string connString, string sql, object[] param)
        {
            try
            {
                int result = 0;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.ExecuteNonQuery(connString, sql, param != null ? (SqlParameter[])param : null);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.ExecuteNonQuery(connString, sql, param != null ? (OracleParameter[])param : null);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.ExecuteNonQuery(connString, sql, param != null ? (MySqlParameter[])param : null);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  Select
        public static DataSet Select(string connString, string sql)
        {
            try
            {
                return ExecuteDataSet(connString, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet Select(string connString, StringBuilder sql)
        {
            try
            {
                return ExecuteDataSet(connString, sql.ToString());
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet Select(string connString, StringBuilder sql, object[] param)
        {
            try
            {
                return ExecuteDataSet(connString, sql.ToString(), param);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static DataSet Select(string connString, string sql, object[] param)
        {
            try
            {
                return ExecuteDataSet(connString, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Scalar
        public static object Scalar(string connString, string sql)
        {
            try
            {
                return ExecuteScalar(connString, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object Scalar(string connString, StringBuilder sql)
        {
            try
            {
                return ExecuteScalar(connString, sql.ToString());
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object Scalar(string connString, StringBuilder sql, object[] param)
        {
            try
            {
                return ExecuteScalar(connString, sql.ToString(), param);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object Scalar(string connString, string sql, object[] param)
        {
            try
            {
                return ExecuteScalar(connString, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Insert
        public static int Insert(string connString, string sql)
        {
            try
            {
                return ExecuteNonQuery(connString, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Insert(string connString, StringBuilder sql)
        {
            try
            {
                return ExecuteNonQuery(connString, sql.ToString());
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Insert(string connString, string sql, object[] param)
        {
            try
            {
                return ExecuteNonQuery(connString, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Insert(string connString, StringBuilder sql, object[] param)
        {
            try
            {
                return ExecuteNonQuery(connString, sql.ToString(), param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Update
        public static int Update(string connString, string sql)
        {
            try
            {
                return ExecuteNonQuery(connString, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Update(string connString, StringBuilder sql)
        {
            try
            {
                return ExecuteNonQuery(connString, sql.ToString());
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Update(string connString, string sql, object[] param)
        {
            try
            {
                return ExecuteNonQuery(connString, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Update(string connString, StringBuilder sql, object[] param)
        {
            try
            {
                return ExecuteNonQuery(connString, sql.ToString(), param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Delete
        public static int Delete(string connString, string sql)
        {
            try
            {
                return ExecuteNonQuery(connString, sql);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Delete(string connString, StringBuilder sql)
        {
            try
            {
                return ExecuteNonQuery(connString, sql.ToString());
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Delete(string connString, string sql, object[] param)
        {
            try
            {
                return ExecuteNonQuery(connString, sql, param);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int Delete(string connString, StringBuilder sql, object[] param)
        {
            try
            {
                return ExecuteNonQuery(connString, sql.ToString(), param);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  CheckDatabaseExists
        public static bool CheckDatabaseExists(string connString, string dbName)
        {
            try
            {
                bool result = false;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.CheckDatabaseExists(connString, dbName);
                        break;
                    //case DatabaseType.Oracle:
                    //    result = OracleDataAccess.CheckDatabaseExists(connString, dbName);
                    //    break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.CheckDatabaseExists(connString, dbName);
                        break;
                }
                return result;
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

        public static bool CheckDataTableExists(string connString, string tableName, string dbName)
        {
            try
            {
                bool result = false;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.CheckDataTableExists(connString, tableName, dbName);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.CheckDataTableExists(connString, tableName, dbName);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.CheckDataTableExists(connString, tableName, dbName);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
    #endregion

}