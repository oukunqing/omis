using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace OMIS.Server.Common
{

    public abstract class DataCheck
    {

        #region  CheckTable
        public static bool CheckTable(DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0;
        }

        public static bool CheckTable(DataSet ds, int idx)
        {
            return idx >= 0 ? ds != null && ds.Tables.Count > idx && ds.Tables[idx].Rows.Count > 0 : false;
        }
        #endregion

        #region  CheckColumn
        public static bool CheckColumn(DataRow dr, string columnName)
        {
            return dr.Table.Columns.Contains(columnName);
        }

        public static bool CheckColumn(DataRowView drv, string columnName)
        {
            //return drv.Row.Table.Columns.Contains(columnName);
            return drv.DataView.Table.Columns.Contains(columnName);
        }
        #endregion
        

        #region  CheckEmpty
        public static bool CheckEmpty(object o)
        {
            return o.ToString().Equals(string.Empty);
        }
        #endregion

        #region  CheckIsNumber
        public static bool IsNumber(string number)
        {
            return new Regex(@"^[-+]?[0-9]+(\.[0-9]+)?$").IsMatch(number.Trim());
        }

        public static bool IsNumber(object number)
        {
            return new Regex(@"^[-+]?[0-9]+(\.[0-9]+)?$").IsMatch(number.ToString().Trim());
        }

        public static bool IsIntNumber(string number)
        {
            return new Regex(@"^[-+]?[0-9]+$").IsMatch(number.Trim());
        }
        #endregion

        #region  CheckIsJsonData
        public static bool IsJsonData(string s)
        {
            return s.Trim().Length > 0 ? (s.StartsWith("{") && s.EndsWith("}")) || (s.StartsWith("[") && s.EndsWith("]")) : false;
        }

        public static bool CheckIsJsonData(string s)
        {
            return IsJsonData(s);
        }
        #endregion


        #region  Trim
        public static string Trim(ref string s)
        {
            s = s.Trim();

            return s;
        }
        #endregion

        #region  Decode
        public static string Decode(ref string val)
        {
            val = HttpUtility.HtmlDecode(val);

            return val;
        }

        public static string Decode(string val)
        {
            return HttpUtility.HtmlDecode(val);
        }
        #endregion

        #region  Escape
        public static string Escape(ref string val)
        {
            val = Decode(val).Replace(@"\", @"\\").Replace("'", @"\'");

            return val;
        }

        public static string Escape(string val)
        {
            return Escape(ref val);
        }
        #endregion

        #region  Filter
        public static string Filter(ref string val)
        {
            val = Escape(ref val);

            return val;
        }

        public static string Filter(string val)
        {
            return Filter(ref val);
        }
        #endregion


        #region  CheckSql
        public static string CheckSql(ref string sql, string action)
        {
            if (!sql.Trim().ToLower().StartsWith(action + " "))
            {
                sql = string.Empty;
            }
            return sql;
        }

        public static string CheckSql(ref string sql)
        {
            if ("select,insert,update,delete".IndexOf(sql.ToLower().Trim().Split(' ')[0]) < 0)
            {
                sql = string.Empty;
            }
            return sql;
        }
        #endregion

        #region  CheckParamIsNotEmpty
        protected static bool CheckParam(string connString, ref string tableName, ref string fieldName)
        {

            return !connString.Equals(string.Empty) && !tableName.Equals(string.Empty) && !fieldName.Equals(string.Empty);
        }

        protected static bool CheckParam(string connString, string sql)
        {
            return !connString.Equals(string.Empty) && !sql.Equals(string.Empty);
        }

        public static bool CheckParam(string[] arr)
        {
            foreach (string str in arr)
            {
                if (str.Trim().Equals(string.Empty))
                {
                    return false;
                }
            }
            return arr.Length > 0;
        }

        public static bool CheckParam(List<string> list)
        {
            foreach (string str in list)
            {
                if (str.Trim().Equals(string.Empty))
                {
                    return false;
                }
            }
            return list.Count > 0;
        }
        #endregion


        #region  FilterStringList

        #region  CorrectString
        private static string CorrectString(string value)
        {
            return value.Replace("'", "\'");
        }

        private static string CorrectString(string value, string defaultValue)
        {
            return CheckEmpty(value) ? defaultValue : CorrectString(value);
        }
        #endregion

        public static string FilterStringList(ref string list)
        {
            return FilterStringList(ref list, ",", false);
        }

        public static string FilterStringList(ref string list, string delimiter)
        {
            return FilterStringList(ref list, delimiter, false);
        }

        public static string FilterStringList(ref string list, string delimiter, bool isValidate)
        {
            if (list.Equals(string.Empty))
            {
                return list;
            }
            else if (isValidate || list.EndsWith(delimiter) || list.StartsWith(delimiter) || list.IndexOf(String.Format("{0}{0}", delimiter)) >= 0)
            {
                string[] arr = list.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder tmp = new StringBuilder();
                foreach (string str in arr)
                {
                    string val = CorrectString(str, string.Empty);
                    if (!val.Equals(string.Empty))
                    {
                        tmp.Append(delimiter);
                        tmp.Append(val);
                    }
                }
                list = tmp.Length > 0 ? tmp.ToString().Substring(delimiter.Length) : string.Empty;
            }
            return list;
        }
        #endregion

        #region  FilterIdList
        private static string CorrectIdList(string idList)
        {
            return new Regex("[\\s|，、/;]").Replace(idList, ",");
        }

        public static string FilterIdList(ref string idList)
        {
            return FilterIdList(ref idList, ",", true);
        }

        public static string FilterIdList(ref string idList, string delimiter)
        {
            return FilterIdList(ref idList, delimiter, true);
        }

        public static string FilterIdList(ref string idList, string delimiter, bool isValidate)
        {
            idList = CorrectIdList(idList);
            if (idList.Equals(string.Empty) || idList.Equals("-1"))
            {
                return string.Empty;
            }
            else if (isValidate || idList.EndsWith(delimiter) || idList.StartsWith(delimiter) || idList.IndexOf(String.Format("{0}{0}", delimiter)) >= 0)
            {
                string[] arr = idList.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder tmp = new StringBuilder();
                foreach (string id in arr)
                {
                    if (IsIntNumber(id))
                    {
                        tmp.Append(delimiter);
                        tmp.Append(id.Trim());
                    }
                }
                idList = tmp.Length > 0 ? tmp.ToString().Substring(delimiter.Length) : string.Empty;
            }
            return idList;
        }
        #endregion

        #region  CheckIdInList
        public static bool CheckIdInList(string idList, int id)
        {
            if (id > 0 && idList.Length > 0)
            {
                string[] arrId = idList.Split(',');
                foreach (string str in arrId)
                {
                    if (IsIntNumber(str) && Convert.ToInt32(str) == id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region  CheckIdList
        /// <summary>
        /// CheckIdList
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static bool CheckIdList(ref string idList)
        {
            return CheckIdList(ref idList, true);
        }

        /// <summary>
        /// CheckIdListValidity
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="isNotZero"></param>
        /// <returns></returns>
        public static bool CheckIdList(ref string idList, bool isNotZero)
        {
            return FilterIdList(ref idList).Length > 0 && !idList.Equals("-1") && (isNotZero ? !idList.Equals("0") : true);
        }
        #endregion


        #region  DateTime

        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        #region  GetDateTime
        public static string GetDateTime()
        {
            return DateTime.Now.ToString(DateTimeFormat);
        }

        public static string GetDateTime(string format)
        {
            return DateTime.Now.ToString(format.Length > 0 ? format : DateTimeFormat);
        }

        public static string GetDateTime(DateTime dt)
        {
            return dt.ToString(DateTimeFormat);
        }

        public static string GetDateTime(DateTime dt, string format)
        {
            return dt.ToString(format.Length > 0 ? format : DateTimeFormat);
        }


        public static string GetDateTimeSecond()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string GetDateTimeTick()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan ts = DateTime.Now.Subtract(dtStart);
            return ts.Ticks.ToString().Substring(0, ts.Ticks.ToString().Length - 7);
        }

        public static string GetToday()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string GetTodayStartTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
        }

        public static string GetTodayEndTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
        }
        #endregion

        #endregion

        #region  CheckEndTime
        public static string CheckEndTime(string endTime)
        {
            if (!CheckIsDateTime(Trim(ref endTime)))
            {
                return string.Empty;
            }

            if (endTime.Length <= 10 || endTime.Split(' ').Length == 1)
            {
                return endTime + " 23:59:59";
            }

            return endTime;
        }

        public static string CheckEndTime(ref string endTime)
        {
            endTime = CheckEndTime(endTime);

            return endTime;
        }
        #endregion

        #region  CheckDateTime
        public static bool CheckIsDateTime(string dt)
        {
            try
            {
                return dt.Trim().Length > 0 ? DateTime.Parse(dt).Day > 0 : false;
            }
            catch (FormatException e) { return false; }
            catch (Exception ex) { return false; }
        }

        public static string CheckDateTime(string dt)
        {
            return CheckIsDateTime(dt) ? dt : GetDateTime();
        }
        #endregion

    }
}