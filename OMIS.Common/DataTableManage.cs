using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace OMIS.Common
{
    public class DataTableManage
    {

        #region  创建DataTable并填充数据
        /// <summary>
        /// 创建DataTable并填充数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="arrColName"></param>
        /// <param name="lstColValue"></param>
        /// <returns></returns>
        public static DataTable BuildDataTable(string tableName, string[] arrColName, List<string[]> lstColValue)
        {
            try
            {
                DataTable dt = CreateDataTable(tableName, arrColName);

                foreach (string[] arrColValue in lstColValue)
                {
                    FillDataRow(dt, arrColName, arrColValue);
                }

                return dt;
            }
            catch (Exception ex) { throw (ex); }
        }
        /// <summary>
        /// 创建DataTable并填充数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="arrColName"></param>
        /// <param name="arrColValue"></param>
        /// <returns></returns>
        public static DataTable BuildDataTable(string tableName, string[] arrColName, string[] arrColValue)
        {
            try
            {
                DataTable dt = CreateDataTable(tableName, arrColName);

                FillDataRow(dt, arrColName, arrColValue);

                return dt;
            }
            catch (Exception ex) { throw (ex); }
        }
        /// <summary>
        /// 创建DataTable并填充数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <param name="colValue"></param>
        /// <returns></returns>
        public static DataTable BuildDataTable(string tableName, string colName, string colValue)
        {
            try
            {
                DataTable dt = CreateDataTable(tableName, colName);

                FillDataRow(dt, colName, colValue);

                return dt;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  创建DataTable
        public static DataTable CreateDataTable(string tableName, string[] arrColName)
        {
            DataTable dt = new DataTable(tableName);

            foreach (string str in arrColName)
            {
                dt.Columns.Add(str);
            }
            return dt;
        }

        public static DataTable CreateDataTable(string tableName, string colName)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(colName);

            return dt;
        }
        #endregion

        #region  填充数据行
        public static void FillDataRow(DataTable dt, string[] arrColName, string[] arrColValue)
        {
            DataRow dr = dt.NewRow();
            if (arrColName.Length <= arrColValue.Length)
            {
                for (int i = 0, c = arrColName.Length; i < c; i++)
                {
                    dr[arrColName[i]] = arrColValue[i];
                }
            }

            dt.Rows.Add(dr);
        }

        public static void FillDataRow(DataTable dt, string colName, string colValue)
        {
            DataRow dr = dt.NewRow();
            dr[colName] = colValue;

            dt.Rows.Add(dr);
        }
        #endregion

    }
}