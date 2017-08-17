using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using OMIS.DBUtility;

namespace OMIS.DAL
{
    public abstract class MssqlDataAccess : OMIS.DBA.MssqlDataAccess
    {

        #region GetMaxId
        public static int GetMaxId(string conn, string tableName, string fieldName)
        {
            try
            {
                return Convert.ToInt32(MssqlHelper.ExecuteScalar(conn, String.Format("select isnull(max([{0}]),0) from [{1}] ", fieldName, tableName)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  GetMaxLevel
        public static int GetMaxLevel(string conn, string tableName, string fieldName)
        {
            try
            {
                return Convert.ToInt32(MssqlHelper.ExecuteScalar(conn, String.Format("select isnull(max([{0}]),0) from [{1}] ", fieldName, tableName)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  GetMinMaxLevel
        public static int[] GetMinMaxLevel(string conn, string tableName, string fieldName)
        {
            try
            {
                int[] ids = new int[2] { 0, 0 };
                DataSet ds = MysqlHelper.ExecuteDataSet(conn, String.Format("select ifnull(min([{0}]),0),ifnull(max([{0}]),0) from [{1}];", fieldName, tableName));
                if (CheckTable(ds, 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ids[0] = Convert.ToInt32(dr[0].ToString());
                    ids[1] = Convert.ToInt32(dr[1].ToString());
                }
                return ids;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  GetTypeLevel
        public static int GetTypeLevel(string conn, string levelFieldName, string tableName, string fieldName, int fieldValue)
        {
            try
            {
                return Convert.ToInt32(MssqlHelper.ExecuteScalar(conn, String.Format("select isnull([{0}],-1) as id,count([{1}]) from [{2}] dt where [{3}] = {4} ", levelFieldName, fieldName, tableName, fieldName, fieldValue)).ToString());
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  GetChildCount
        public static int GetChildCount(string conn, string tableName, string fieldName, int fieldValue)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(String.Format(" select count(distinct ct.[{0}]) as cc,t.[{0}] from [{1}] t ", fieldName, tableName));
                sql.Append(String.Format(" left outer join [{0}] ct on ct.`parent_id` = t.[{1}] ", tableName, fieldName));
                sql.Append(String.Format(" where t.[{0}] = {1} ", fieldName, fieldValue));

                return ConvertValue(Scalar(conn, sql.ToString()), 0);
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
                StringBuilder sql = new StringBuilder();
                sql.Append(String.Format(" select count(distinct d.[{0}]) as dc,t.[{1}] from [{2}] t ", dataFieldName, fieldName, tableName));
                sql.Append(String.Format(" left outer join [{0}] d on d.[{1}] = t.[{2}] ", dataTableName, relationFieldName, fieldName));
                sql.Append(String.Format(" where t.[{0}] = {1} ", fieldName, fieldValue));

                return ConvertValue(Scalar(conn, sql.ToString()), 0);
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
                StringBuilder sql = new StringBuilder();
                sql.Append(String.Format(" select count(distinct ct.[{0}]) as cc, count(distinct d.[{1}]) as dc,t.[{0}] from [{2}] t ",
                    fieldName, dataFieldName, tableName));
                sql.Append(String.Format(" left outer join [{0}] ct on ct.`parent_id` = t.[{1}] ", tableName, fieldName));
                sql.Append(String.Format(" left outer join [{0}] d on d.[{1}] = t.[{2}] ", dataTableName, relationFieldName, fieldName));
                sql.Append(String.Format(" where t.[{0}] = {1} ", fieldName, fieldValue));

                DataSet ds = Select(conn, sql.ToString());
                if (CheckTable(ds, 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    return new int[] { ConvertValue(dr[0], 0), ConvertValue(dr[1], 0) };
                }
                return new int[] { -1, -1 };
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region  CheckTableExist
        public static bool CheckTableExist(string conn, string tableName)
        {
            string sql = String.Format("select count(*) as exist from dbo.sysobjects where name = '{0}' and type = 'u';", tableName);
            return Convert.ToInt32(MssqlHelper.ExecuteScalar(conn, sql).ToString()) > 0;
        }
        #endregion

        #region  UpdateLevel
        public static int[] UpdateLevel(string conn, string tableName, string fieldName)
        {
            return UpdateLevel(conn, tableName, fieldName, "level");
        }

        public static int[] UpdateLevel(string conn, string tableName, string fieldName, string levelFieldName)
        {
            try
            {
                //获取最小最大层级
                int[] levels = GetMinMaxLevel(conn, tableName, levelFieldName);

                DataSet ds = DataAccess.Select(conn, String.Format("select [{0}],[parent_id] from [{1}] order by [{2}],[{0}];", fieldName, tableName, levelFieldName));
                if (CheckTable(ds, 0))
                {
                    StringBuilder sql = new StringBuilder();

                    string pids = "0";
                    int level = levels[0];

                    while (pids.Length > 0)
                    {
                        string filter = String.Format("parent_id in({0})", pids);
                        DataView dv = new DataView(ds.Tables[0], filter, "", DataViewRowState.CurrentRows);

                        StringBuilder ids = new StringBuilder();
                        foreach (DataRowView dr in dv)
                        {
                            ids.Append(",");
                            ids.Append(dr[0].ToString());
                        }
                        pids = ids.Length > 0 ? ids.ToString().Substring(1) : "";
                        if (pids.Length > 0)
                        {
                            sql.Append(String.Format("update [{0}] set [{1}] = {2} where [{3}] in({4});", tableName, levelFieldName, level, fieldName, pids));

                            level++;
                        }
                    }

                    if (sql.Length > 0)
                    {
                        MysqlHelper.ExecuteNonQuery(conn, sql.ToString());
                    }

                    //更改最大层级
                    levels[1] = level;
                }

                return levels;
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

        public static int UpdateParentTree(string conn, string tableName, string fieldName, int fieldValue, string levelFieldName)
        {
            try
            {
                return UpdateParentTree(conn, tableName, fieldName, fieldValue, 0, levelFieldName);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int UpdateParentTree(string conn, string tableName, string fieldName, int fieldValue, int minLevel, string levelFieldName)
        {
            try
            {
                //更新层级，保证层级不会乱掉
                int[] levels = UpdateLevel(conn, tableName, fieldName, levelFieldName);

                StringBuilder sql = new StringBuilder();
                sql.Append(String.Format("update [{0}] set [parent_id] = 0 where [parent_id] < 0;", tableName));

                //更新第一级
                sql.Append(String.Format("update [{0}] d ", tableName));
                sql.Append(String.Format("set d.[parent_tree] = concat('(',d.[{0}], ')') ", fieldName));
                sql.Append(String.Format("where d.[parent_id] = 0 and d.[{0}] = {1};", levelFieldName, levels[0]));

                for (int i = levels[0] + 1; i <= levels[1]; i++)
                {
                    sql.Append(String.Format("update [{0}] d,[{1}] pd set ", tableName, tableName));
                    sql.Append(String.Format("d.[parent_tree] = concat(ifnull(pd.[parent_tree],''),',(',d.[{0}], ')') ", fieldName));
                    sql.Append(String.Format("where d.[parent_id] = pd.[{0}] ", fieldName));
                    sql.Append(String.Format("and d.[{0}] = {1}", levelFieldName, i));
                    sql.Append(";");
                }
                
                if (fieldValue > 0)
                {
                    sql.Append(String.Format("update [{0}] d,[{1}] pd set ", tableName, tableName));
                    sql.Append(String.Format("d.[parent_tree] = concat(ifnull(pd.[parent_tree],''),',(',d.[{0}], ')') ", fieldName));
                    sql.Append(String.Format("where d.[parent_id] = pd.[{0}] ", fieldName));
                    sql.Append(String.Format("and d.[{0}] = {1}", fieldName, fieldValue));
                    sql.Append(";");
                }
                
                if (minLevel >= 0 && minLevel != levels[0])
                {
                    //指定最小层级
                    sql.Append(String.Format("update [{0}] set [{1}] = [{1}] + ({2}-{3});", tableName, levelFieldName, minLevel, levels[0]));
                }

                return MssqlHelper.ExecuteNonQuery(conn, sql.ToString());
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region  CheckDataIsExist
        public static bool CheckDataIsExist(string conn, string tableName, string priKeyFieldName, string fieldName, string fieldValue, int priKeyFieldValue, Dictionary<string, int> dicField)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(String.Format(" select count(d.[{0}]) from [{1}] d where d.[{2}] = '{3}' ", priKeyFieldName, tableName, fieldName, fieldValue));
                if (priKeyFieldValue > 0)
                {
                    sql.Append(String.Format(" and d.[{0}] <> {1} ", priKeyFieldName, priKeyFieldValue));
                }
                if (dicField != null)
                {
                    foreach (KeyValuePair<string, int> kv in dicField)
                    {
                        if (kv.Value >= 0)
                        {
                            sql.Append(String.Format(" and d.[{0}] = {1} ", kv.Key, kv.Value));
                        }
                    }
                }
                sql.Append(";");

                return Convert.ToInt32(MysqlHelper.ExecuteScalar(conn, sql.ToString())) > 0;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }

    #region  MsDBA
    public abstract class MsDBA : MssqlDataAccess
    {
    }
    #endregion

}