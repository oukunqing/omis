using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.OracleClient;
using MySql.Data.MySqlClient;
using OMIS.DBA;

namespace OMIS.DAL
{

    #region  DataAccess
    public abstract class DataAccess : OMIS.DBA.DataAccess
    {

        #region GetMaxId
        public static int GetMaxId(string conn, string tableName, string fieldName)
        {
            try
            {
                if (!CheckParam(conn, ref tableName, ref fieldName))
                {
                    return -1;
                }
                int result = 0;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.GetMaxId(conn, tableName, fieldName);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.GetMaxId(conn, tableName, fieldName);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.GetMaxId(conn, tableName, fieldName);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  GetMaxLevel
        public static int GetMaxLevel(string conn, string tableName, string fieldName)
        {
            try
            {
                if (!CheckParam(conn, ref tableName, ref fieldName))
                {
                    return -1;
                }
                int result = -1;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.GetMaxLevel(conn, tableName, fieldName);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.GetMaxLevel(conn, tableName, fieldName);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.GetMaxLevel(conn, tableName, fieldName);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  GetTypeLevel
        public static int GetTypeLevel(string conn, string levelFieldName, string tableName, string fieldName, int fieldValue)
        {
            try
            {
                if (!CheckParam(conn, ref tableName, ref fieldName))
                {
                    return -1;
                }
                int result = -1;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.GetTypeLevel(conn, levelFieldName, tableName, fieldName, fieldValue);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.GetTypeLevel(conn, levelFieldName, tableName, fieldName, fieldValue);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.GetTypeLevel(conn, levelFieldName, tableName, fieldName, fieldValue);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  GetChildCount
        public static int GetChildCount(string conn, string tableName, string fieldName, int fieldValue)
        {
            try
            {
                if (!CheckParam(conn, ref tableName, ref fieldName))
                {
                    return -1;
                } 
                int result = -1;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.GetChildCount(conn, tableName, fieldName, fieldValue);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.GetChildCount(conn, tableName, fieldName, fieldValue);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.GetChildCount(conn, tableName, fieldName, fieldValue);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  GetDataCount
        public static int GetDataCount(string conn, string tableName, string fieldName, int fieldValue, string dataTableName, string dataFieldName)
        {
            try
            {
                return GetDataCount(conn, tableName, fieldName, fieldValue, dataTableName, dataFieldName, fieldName);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int GetDataCount(string conn, string tableName, string fieldName, int fieldValue, string dataTableName, string dataFieldName, string relationFieldName)
        {
            try
            {
                if (!CheckParam(conn, ref tableName, ref fieldName))
                {
                    return -1;
                }
                int result = -1;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.GetDataCount(conn, tableName, fieldName, fieldValue, dataTableName, dataFieldName, relationFieldName);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.GetDataCount(conn, tableName, fieldName, fieldValue, dataTableName, dataFieldName, relationFieldName);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.GetDataCount(conn, tableName, fieldName, fieldValue, dataTableName, dataFieldName, relationFieldName);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  GetChildCountAndDataCount
        public static int[] GetChildCountAndDataCount(string conn, string tableName, string fieldName, int fieldValue, string dataTableName, string dataFieldName)
        {
            try
            {
                return GetChildCountAndDataCount(conn, tableName, fieldName, fieldValue, dataTableName, dataFieldName, fieldName);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int[] GetChildCountAndDataCount(string conn, string tableName, string fieldName, int fieldValue, string dataTableName, string dataFieldName, string relationFieldName)
        {
            try
            {
                if (!CheckParam(conn, ref tableName, ref fieldName))
                {
                    return new int[] { -1, -1 };
                }
                int[] result = new int[2];
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.GetChildCountAndDataCount(conn, tableName, fieldName, fieldValue, dataTableName, dataFieldName, relationFieldName);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.GetChildCountAndDataCount(conn, tableName, fieldName, fieldValue, dataTableName, dataFieldName, relationFieldName);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.GetChildCountAndDataCount(conn, tableName, fieldName, fieldValue, dataTableName, dataFieldName, relationFieldName);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  UpdateParentTree
        public static int UpdateParentTree(string conn, string tableName, string fieldName, int fieldValue)
        {
            try
            {
                return UpdateParentTree(conn, tableName, fieldName, fieldValue, 0, "level");
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int UpdateParentTree(string conn, string tableName, string fieldName, int fieldValue, int minLevel)
        {
            try
            {
                return UpdateParentTree(conn, tableName, fieldName, fieldValue, minLevel, "level");
            }
            catch (Exception ex) { throw (ex); }
        }
        
        public static int UpdateParentTree(string conn, string tableName, string fieldName, int fieldValue, int minLevel, string levelFieldName)
        {
            try
            {
                if (!CheckParam(conn, ref tableName, ref fieldName))
                {
                    return -1;
                }
                int result = -1;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.UpdateParentTree(conn, tableName, fieldName, fieldValue, minLevel, levelFieldName);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.UpdateParentTree(conn, tableName, fieldName, fieldValue, minLevel, levelFieldName);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.UpdateParentTree(conn, tableName, fieldName, fieldValue, minLevel, levelFieldName);
                        break;
                }
                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region  CheckDataIsExist
        public static bool CheckDataIsExist(string conn, string tableName, string priKeyFieldName, string fieldName, string fieldValue, int priKeyFieldValue)
        {
            try
            {
                return CheckDataIsExist(conn, tableName, priKeyFieldName, fieldName, fieldValue, priKeyFieldValue, null);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static bool CheckDataIsExist(string conn, string tableName, string priKeyFieldName, string fieldName, string fieldValue, int priKeyFieldValue, Dictionary<string, int> dicField)
        {
            try
            {
                if (!CheckParam(conn, ref tableName, ref fieldName))
                {
                    return false;
                }
                bool result = false;
                switch (DBTYPE)
                {
                    case DatabaseType.MsSql:
                        result = MssqlDataAccess.CheckDataIsExist(conn, tableName, priKeyFieldName, fieldName, fieldValue, priKeyFieldValue, dicField);
                        break;
                    case DatabaseType.Oracle:
                        result = OracleDataAccess.CheckDataIsExist(conn, tableName, priKeyFieldName, fieldName, fieldValue, priKeyFieldValue, dicField);
                        break;
                    case DatabaseType.MySql:
                    default:
                        result = MysqlDataAccess.CheckDataIsExist(conn, tableName, priKeyFieldName, fieldName, fieldValue, priKeyFieldValue, dicField);
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