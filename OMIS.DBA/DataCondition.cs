using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.DBA
{

    public enum SearchTactics
    {
        And,
        AndInOrder,
        Or,
        Not,
        None,
    }

    public abstract class DataCondition : DataAccess
    {


        #region  DealSingleQuotes
        private static string CheckSingleQuotes(string sql)
        {
            return sql.IndexOf("'{0}'") > 0 ? "{0}" : "'{0}'";
        }

        private static string ClearSingleQuotes(ref string con)
        {
            con = con.Replace("'", "");
            return con;
        }

        private static string ClearSingleQuotes(string con)
        {
            return con.Replace("'", "");
        }
        #endregion

        #region  BuildCondition
        public static string BuildCondition(string sql, string val)
        {
            if (sql.Equals(string.Empty))
            {
                return string.Empty;
            }
            return !Filter(ref val).Equals(string.Empty) ? String.Format(sql, val) : string.Empty;
        }

        public static string BuildCondition(string sql, ref string val)
        {
            if (sql.Equals(string.Empty))
            {
                return string.Empty;
            }
            return !Filter(ref val).Equals(string.Empty) ? String.Format(sql, val) : string.Empty;
        }

        public static string BuildCondition(string sql, int val, int threshold)
        {
            if (sql.Equals(string.Empty))
            {
                return string.Empty;
            }
            return val > threshold ? String.Format(sql, val) : string.Empty;
        }
        #endregion


        #region  BuildInCondition
        /// <summary>
        /// BuildInCondition（where in condition [string]）
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="sqlMulti"></param>
        /// <param name="sqlSingle"></param>
        /// <returns></returns>
        public static string BuildInCondition(string condition, string sqlMulti, string sqlSingle)
        {
            try
            {
                if (!CheckParam(new string[] { condition, sqlMulti, sqlSingle }))
                {
                    return string.Empty;
                }
                //if (ClearSingleQuotes(ref condition).IndexOf(',') > 0)
                if (Filter(ref condition).IndexOf(',') > 0)
                {
                    return String.Format(sqlMulti, String.Format(CheckSingleQuotes(sqlMulti), condition.Replace(",", "','")));
                }
                else
                {
                    return String.Format(sqlSingle, String.Format(CheckSingleQuotes(sqlSingle), condition));
                }
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  BuildInIdCondition
        /// <summary>
        /// BuildInIdCondition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="sqlMulti"></param>
        /// <param name="sqlSingle"></param>
        /// <returns></returns>
        public static string BuildInIdCondition(string condition, string sqlMulti, string sqlSingle)
        {
            try
            {
                if (!CheckParam(new string[] { condition, sqlMulti, sqlSingle }))
                {
                    return string.Empty;
                }
                else if (IsIntNumber(condition))
                {
                    if (Convert.ToInt32(condition) < 0)
                    {
                        return string.Empty;
                    }
                }
                return ClearSingleQuotes(String.Format(condition.IndexOf(",") > 0 ? sqlMulti : sqlSingle, condition)); 
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  BuildInNumberCondition
        /// <summary>
        /// BuildInNumberCondition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="sqlMulti"></param>
        /// <param name="sqlSingle"></param>
        /// <returns></returns>
        public static string BuildInNumberCondition(string condition, string sqlMulti, string sqlSingle)
        {
            try
            {
                if (!CheckParam(new string[] { condition, sqlMulti, sqlSingle }))
                {
                    return string.Empty;
                }
                else if (!IsNumber(condition))
                {
                    return string.Empty;
                }
                return ClearSingleQuotes(String.Format(condition.IndexOf(",") > 0 ? sqlMulti : sqlSingle, condition));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  BuildSearchCondition
        public static string BuildSearchCondition(string keywords, string format)
        {
            try
            {
                return BuildSearchCondition(keywords, format, SearchTactics.Or, new string[] { " ", ",", "|", ";" }, 5);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string BuildSearchCondition(string keywords, string format, SearchTactics tactics)
        {
            try
            {
                return BuildSearchCondition(keywords, format, tactics, new string[] { " ", ",", "|", ";" }, 5);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string BuildSearchCondition(string keywords, string format, SearchTactics tactics, string[] delimiter)
        {
            try
            {
                return BuildSearchCondition(keywords, format, tactics, delimiter, 5);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string BuildSearchCondition(string keywords, string format, SearchTactics tactics, string[] delimiter, int keyMaxCount)
        {
            try
            {
                format = format.Trim();

                if (!CheckParam(new string[] { keywords, format }))
                {
                    return string.Empty;
                }

                StringBuilder con = new StringBuilder();
                Hashtable htKeys = new Hashtable();
                int n = 0;

                string[] arrKey = keywords.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                switch (tactics)
                {
                    case SearchTactics.And:
                        foreach (string key in arrKey)
                        {
                            if (!htKeys.ContainsKey(key))
                            {
                                if (n >= keyMaxCount)
                                {
                                    break;
                                }
                                con.Append(String.Format(format, key));
                                htKeys.Add(key, key);
                                n++;
                            }
                        }
                        break;
                    case SearchTactics.AndInOrder:
                        StringBuilder newKey = new StringBuilder();
                        foreach (string key in arrKey)
                        {
                            if (!htKeys.ContainsKey(key))
                            {
                                if (n >= keyMaxCount)
                                {
                                    break;
                                }
                                newKey.Append("%");
                                newKey.Append(key);
                                htKeys.Add(key, key);
                                n++;
                            }
                        }
                        con.Append(String.Format(format, newKey.ToString().Substring(1)));
                        break;
                    case SearchTactics.Or:
                        string newFormat = format;
                        string prefix = " and ";
                        int pos = format.IndexOf(' ');
                        Regex reg = new Regex("^(where|and|or)", RegexOptions.IgnoreCase);
                        if (reg.IsMatch(format) && pos > 0)
                        {
                            prefix = format.Substring(0, pos);
                            newFormat = format.Substring(pos + 1);
                        }
                        con.Append(prefix);
                        con.Append("(");
                        foreach (string key in arrKey)
                        {
                            if (!htKeys.ContainsKey(key))
                            {
                                if (n >= keyMaxCount)
                                {
                                    break;
                                }
                                con.Append(n > 0 ? " or " : "");
                                con.Append(String.Format(newFormat, key));
                                htKeys.Add(key, key);
                                n++;
                            }
                        }
                        con.Append(")");
                        break;
                    case SearchTactics.Not:
                        break;
                    case SearchTactics.None:
                    default:
                        con.Append(String.Format(format, keywords));
                        break;
                }

                return con.ToString();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  BuildLimitCondition
        /// <summary>
        /// BuildLimitCondition
        /// </summary>
        /// <param name="pi">PageIndex</param>
        /// <param name="ps">PageSize</param>
        /// <returns>
        /// return pi >= 0 && ps > 0 ? limit : ""
        /// </returns>
        public static string BuildLimitCondition(int pi, int ps)
        {
            return pi >= 0 && ps > 0 ? String.Format(" limit {0},{1} ", pi * ps, ps) : string.Empty;
        }
        #endregion


        #region  BuildTimeCondition
        public static string BuildTimeCondition(string field, string start, string end)
        {
            string con = string.Empty;
            if (Filter(ref field).Equals(string.Empty))
            {
                return con;
            }

            if (Filter(ref start).Length > 0 && Filter(ref end).Length > 0)
            {
                con = String.Format(" and {0} between '{1}' and '{2}' ", field, start, end);
            }
            else if (start.Length > 0)
            {
                con = String.Format(" and {0} >= '{1}' ", field, start);
            }
            else if (end.Length > 0)
            {
                con = String.Format(" and {0} <= '{1}' ", end);
            }

            return con;
        }
        #endregion

        #region  BuildTimeStampCondition
        public static string BuildTimeStampCondition(string field, int start, int end)
        {
            string con = string.Empty;
            if (Filter(ref field).Equals(string.Empty))
            {
                return con;
            }

            if (start > 0 && end > 0)
            {
                con = String.Format(" and {0} between {1} and {2} ", field, start, end);
            }
            else if (start > 0)
            {
                con = String.Format(" and {0} >= {1} ", field, start);
            }
            else if (end > 0)
            {
                con = String.Format(" and {0} <= {1} ", end);
            }

            return con;
        }
        #endregion

    }

}