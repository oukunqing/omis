using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.DBA
{
    public abstract class DataConvert : DataCheck
    {

        #region  ConvertValue

        #region  ConvertToInt32
        public static int ConvertValue(object o, int val)
        {
            return o != null ? ConvertValue(o.ToString(), val) : val;
        }

        public static int ConvertValue(string num, int val)
        {
            return IsIntNumber(num) ? Convert.ToInt32(num) : val;
        }
        #endregion

        #region  ConvertToSingle
        public static float ConvertValue(object o, float val)
        {
            return o != null ? ConvertValue(o.ToString(), val) : val;
        }

        public static float ConvertValue(string num, float val)
        {
            return IsNumber(num) ? Convert.ToSingle(num) : val;
        }
        #endregion

        #region  ConvertToDouble
        public static double ConvertValue(object o, double val)
        {
            return o != null ? ConvertValue(o.ToString(), val) : val;
        }

        public static double ConvertValue(string num, double val)
        {
            return IsNumber(num) ? Convert.ToDouble(num) : val;
        }
        #endregion

        #region  ConvertToDecimal
        public static decimal ConvertValue(object o, decimal val)
        {
            return o != null ? ConvertValue(o.ToString(), val) : val;
        }

        public static decimal ConvertValue(string num, decimal val)
        {
            return IsNumber(num) ? Convert.ToDecimal(num) : val;
        }
        #endregion

        #region  ConvertToString
        public static string ConvertValue(object o)
        {
            return o != null ? o.ToString() : string.Empty;
        }

        public static string ConvertValue(object o, string val)
        {
            return o != null ? ConvertValue(o.ToString(), val) : val;
        }

        public static string ConvertValue(string s, string val)
        {
            return s.Trim().Equals(string.Empty) ? val : s;
        }
        #endregion

        #endregion

        #region _ParseKey
        private static string[] _ParseKey(string key)
        {
            return key.Split(new string[] { "|", ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool _ContainsKey(Dictionary<string, object> dic, string key)
        {
            return dic.Keys.Contains(key, StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region  ConvertDictionaryValue
        public static int ConvertValue(Dictionary<string, object> dic, string key, int val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (_ContainsKey(dic, k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return _ContainsKey(dic, key) ? ConvertValue(dic[key], val) : val;
        }

        public static float ConvertValue(Dictionary<string, object> dic, string key, float val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (_ContainsKey(dic, k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return _ContainsKey(dic, key) ? ConvertValue(dic[key], val) : val;
        }

        public static double ConvertValue(Dictionary<string, object> dic, string key, double val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (_ContainsKey(dic, k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return _ContainsKey(dic, key) ? ConvertValue(dic[key], val) : val;
        }

        public static decimal ConvertValue(Dictionary<string, object> dic, string key, decimal val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (_ContainsKey(dic, k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return _ContainsKey(dic, key) ? ConvertValue(dic[key], val) : val;
        }

        public static string ConvertValue(Dictionary<string, object> dic, string key)
        {
            foreach (string k in _ParseKey(key))
            {
                if (_ContainsKey(dic, k))
                {
                    return ConvertValue(dic[k]);
                }
            }
            return _ContainsKey(dic, key) ? ConvertValue(dic[key]) : string.Empty;
        }

        public static string ConvertValue(Dictionary<string, object> dic, string key, string val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (_ContainsKey(dic, k))
                {
                    return ConvertValue(dic[k]);
                }
            }
            return _ContainsKey(dic, key) ? ConvertValue(dic[key], val) : val;
        }
        #endregion

        #region  ConvertDictionaryValueByKeyTree
        public static object ConvertValue(Dictionary<string, object> dic, List<string> keyTree)
        {
            try
            {
                int c = keyTree.Count;
                int n = 0;
                if (c == 0)
                {
                    return dic;
                }

                foreach (string key in keyTree)
                {
                    n++;
                    if (dic.ContainsKey(key))
                    {
                        if (dic[key].GetType().ToString().IndexOf("Dictionary") > 0)
                        {
                            dic = (Dictionary<string, object>)dic[key];
                            if (n >= c)
                            {
                                return dic;
                            }
                        }
                        else
                        {
                            return dic[key];
                        }
                    }
                }
                return null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ConvertDictionary
        public static Dictionary<string, object> ConvertDictionary(Dictionary<string, object> dic, List<string> keyTree)
        {
            try
            {
                int c = keyTree.Count;
                int n = 0;
                if (c == 0)
                {
                    return dic;
                }

                foreach (string key in keyTree)
                {
                    n++;
                    if (dic.ContainsKey(key))
                    {
                        if (dic[key].GetType().ToString().IndexOf("Dictionary") > 0)
                        {
                            dic = (Dictionary<string, object>)dic[key];
                            if (n >= c)
                            {
                                return dic;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  ConvertHashtableValue
        public static int ConvertValue(Hashtable dic, string key, int val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (dic.ContainsKey(k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return dic.ContainsKey(key) ? ConvertValue(dic[key], val) : val;
        }

        public static float ConvertValue(Hashtable dic, string key, float val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (dic.ContainsKey(k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return dic.ContainsKey(key) ? ConvertValue(dic[key], val) : val;
        }

        public static double ConvertValue(Hashtable dic, string key, double val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (dic.ContainsKey(k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return dic.ContainsKey(key) ? ConvertValue(dic[key], val) : val;
        }

        public static decimal ConvertValue(Hashtable dic, string key, decimal val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (dic.ContainsKey(k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return dic.ContainsKey(key) ? ConvertValue(dic[key], val) : val;
        }

        public static string ConvertValue(Hashtable dic, string key)
        {
            foreach (string k in _ParseKey(key))
            {
                if (dic.ContainsKey(k))
                {
                    return ConvertValue(dic[k]);
                }
            }
            return dic.ContainsKey(key) ? ConvertValue(dic[key]) : string.Empty;
        }

        public static string ConvertValue(Hashtable dic, string key, string val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (dic.ContainsKey(k))
                {
                    return ConvertValue(dic[k], val);
                }
            }
            return dic.ContainsKey(key) ? ConvertValue(dic[key], val) : val;
        }
        #endregion

        #region  ConvertFieldValue
        public static int ConvertFieldValue(DataTable dt, int val)
        {
            return CheckTable(dt) ? ConvertValue(dt.Rows[0][0], val) : val;
        }

        public static int ConvertFieldValue(DataSet ds, int idx, int val)
        {
            return CheckTable(ds, idx) ? ConvertValue(ds.Tables[idx].Rows[0][0], val) : val;
        }

        public static int ConvertFieldValue(DataSet ds, int idx, string field, int val)
        {
            return CheckTable(ds, idx) ? ConvertValue(ds.Tables[idx].Rows[0][field], val) : val;
        }

        public static float ConvertFieldValue(DataTable dt, float val)
        {
            return CheckTable(dt) ? ConvertValue(dt.Rows[0][0], val) : val;
        }

        public static float ConvertFieldValue(DataSet ds, int idx, float val)
        {
            return CheckTable(ds, idx) ? ConvertValue(ds.Tables[idx].Rows[0][0], val) : val;
        }

        public static float ConvertFieldValue(DataSet ds, int idx, string field, float val)
        {
            return CheckTable(ds, idx) ? ConvertValue(ds.Tables[idx].Rows[0][field], val) : val;
        }

        public static decimal ConvertFieldValue(DataTable dt, decimal val)
        {
            return CheckTable(dt) ? ConvertValue(dt.Rows[0][0], val) : val;
        }

        public static decimal ConvertFieldValue(DataSet ds, int idx, decimal val)
        {
            return CheckTable(ds, idx) ? ConvertValue(ds.Tables[idx].Rows[0][0], val) : val;
        }

        public static decimal ConvertFieldValue(DataSet ds, int idx, string field, decimal val)
        {
            return CheckTable(ds, idx) ? ConvertValue(ds.Tables[idx].Rows[0][field], val) : val;
        }
        #endregion


        #region  ConvertDateTime
        public static string ConvertDateTime(string dt)
        {
            return ConvertDateTime(dt, "", "");
        }

        public static string ConvertDateTime(string dt, string replace)
        {
            return ConvertDateTime(dt, replace, "");
        }

        public static string ConvertDateTime(string dt, string replace, string format)
        {
            if (format.Equals(string.Empty))
            {
                format = "yyyy-MM-dd HH:mm:ss";
            }
            return CheckIsDateTime(dt) ? DateTime.Parse(dt).ToString(format) : replace;
        }

        public static string ConvertDate(string dt)
        {
            return ConvertDate(dt, "", "");
        }

        public static string ConvertDate(string dt, string format)
        {
            return ConvertDate(dt, "", format);
        }

        public static string ConvertDate(string dt, string replace, string format)
        {
            if (format.Equals(string.Empty))
            {
                format = "yyyy-MM-dd";
            }
            return CheckIsDateTime(dt) ? DateTime.Parse(dt).ToString(format) : replace;
        }
        #endregion

        #region  ToDateTime
        public static string ToDateTime(DateTime dt)
        {
            return dt != null ? dt.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
        }

        public static string ToDate(DateTime dt)
        {
            return dt != null ? dt.ToString("yyyy-MM-dd") : string.Empty;
        }
        #endregion

        #region  ConvertEmptyDateTime
        public static object ConvertEmptyDateTime(string dt)
        {
            if (CheckIsDateTime(dt))
            {
                return dt;
            }
            return System.DBNull.Value;
        }

        public static object ToDBNull(string dt)
        {
            return ConvertEmptyDateTime(dt);
        }
        #endregion

        #region  Swap
        public static bool Swap(ref string str, ref string str1)
        {
            string tmp = str;
            str = str1;
            str1 = tmp;

            return true;
        }

        public static bool Swap(ref int num, ref int num1)
        {
            int tmp = num;
            num = num1;
            num1 = tmp;

            return true;
        }
        #endregion

        #region  CompareTime
        /// <summary>
        /// CompareTime
        /// If minTime is greater than maxTime, then swap.
        /// </summary>
        /// <param name="minTime"></param>
        /// <param name="maxTime"></param>
        public static bool CompareTime(ref string minTime, ref string maxTime)
        {
            try
            {
                return 1 == DateTime.Parse(minTime).CompareTo(DateTime.Parse(maxTime)) ? Swap(ref minTime, ref maxTime) : false;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static bool CompareDateTime(ref string minTime, ref string maxTime)
        {
            try
            {
                return CompareTime(ref minTime, ref maxTime);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  ParsePropertyType
        private static bool IOF(string text, string s)
        {
            return text.IndexOf(s) >= 0;
        }

        private static string ParsePropertyType(Type t)
        {
            string _t = t.ToString().Replace("System.", "").ToLower();
            //timespan:mysql-time
            string _s = "string,datetime,timespan";
            string _dt = "datetime";
            //sbyte:mysql-tinyint;uint64:mysql-bit
            string _n = "int,int32,int16,int64,single,float,double,decimal,sbyte,uint64";
            string _i = "int";
            return (IOF(_s, _t) || IOF(_t, _dt)) ? "string" : (IOF(_n, _t) || IOF(_t, _i)) ? "num" : "object";
        }

        private static object ParseDataColumnValue(DataColumn col, object drKeyVal)
        {
            return CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(drKeyVal, 0m) : ConvertValue(drKeyVal);
        }

        private static object ParsePropertyValue(PropertyInfo p, object drKeyVal)
        {
            //return CheckNumberType(ParsePropertyType(p.PropertyType)) ? (object)ConvertValue(drKeyVal, 0m) : ConvertValue(drKeyVal);
            //decimal转换为int出错，之前是可以转换的

            if (CheckNumberType(ParsePropertyType(p.PropertyType)))
            {
                string t = p.PropertyType.ToString().ToLower();

                if (t.IndexOf("int") >= 0)
                {
                    return (object)ConvertValue(drKeyVal, 0);
                }
                else if (t.IndexOf("float") >= 0 || t.IndexOf("single") >= 0)
                {
                    return (object)ConvertValue(drKeyVal, 0.0f);
                }
                else if (t.IndexOf("double") >= 0)
                {
                    return (object)ConvertValue(drKeyVal, 0.0d);
                }
                return (object)ConvertValue(drKeyVal, 0m);
            }
            else
            {
                return ConvertValue(drKeyVal);
            }
        }
        #endregion

        #region  CheckNumberType
        private static bool CheckNumberType(string t)
        {
            return "num,int,float,double".IndexOf(t) >= 0;
        }
        #endregion

        #region  SetField
        public static Dictionary<string, string> SetField(string[] fields)
        {
            Dictionary<string, string> dicField = new Dictionary<string, string>();
            for (int i = 0, c = fields.Length; i < c; i++)
            {
                if (!dicField.ContainsKey(fields[i]))
                {
                    dicField.Add(fields[i], fields[i]);
                }
            }
            return dicField;
        }

        public static Dictionary<string, string> SetField(string[,] fields)
        {
            Dictionary<string, string> dicField = new Dictionary<string, string>();
            for (int i = 0, c = fields.Length / 2; i < c; i++)
            {
                if (!dicField.ContainsKey(fields[i, 0]))
                {
                    dicField.Add(fields[i, 0], fields[i, 1]);
                }
            }
            return dicField;
        }

        public static Dictionary<string, string> SetField(List<string> fields)
        {
            Dictionary<string, string> dicField = new Dictionary<string, string>();
            for (int i = 0, c = fields.Count; i < c; i++)
            {
                if (!dicField.ContainsKey(fields[i]))
                {
                    dicField.Add(fields[i], fields[i]);
                }
            }
            return dicField;
        }

        public static Dictionary<string, string> SetField(List<string[]> fields)
        {
            Dictionary<string, string> dicField = new Dictionary<string, string>();
            for (int i = 0, c = fields.Count; i < c; i++)
            {
                if (!dicField.ContainsKey(fields[i][0]))
                {
                    dicField.Add(fields[i][0], fields[i][1]);
                }
            }
            return dicField;
        }
        #endregion


        #region  FillClassValue
        public static object FillClassValue(object o, string[,] fields, DataRow dr, out Dictionary<string, object> dic)
        {
            try { return FillClassValue(o, SetField(fields), dr, out dic); }
            catch (Exception ex) { throw (ex); }
        }

        public static object FillClassValue(object o, Dictionary<string, string> dicField, DataRow dr, out Dictionary<string, object> dic)
        {
            try
            {
                dic = new Dictionary<string, object>();
                foreach (PropertyInfo p in o.GetType().GetProperties())
                {
                    if (p.CanWrite && dicField.ContainsKey(p.Name))
                    {
                        string name = dicField[p.Name].ToString();
                        object val = ParsePropertyValue(p, dr[name]);

                        p.SetValue(o, val, null);

                        dic.Add(p.Name, val);
                    }
                }
                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object FillClassValue(object o, Hashtable dicField, DataRow dr, out Dictionary<string, object> dic)
        {
            try
            {
                dic = new Dictionary<string, object>();
                foreach (PropertyInfo p in o.GetType().GetProperties())
                {
                    if (p.CanWrite && dicField.ContainsKey(p.Name))
                    {
                        string name = dicField[p.Name].ToString();
                        object val = ParsePropertyValue(p, dr[name]);

                        p.SetValue(o, val, null);

                        dic.Add(p.Name, val);
                    }
                }
                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object FillClassValue(object o, string[,] fields, DataRow dr)
        {
            try { return FillClassValue(o, SetField(fields), dr); }
            catch (Exception ex) { throw (ex); }
        }

        public static object FillClassValue(object o, Dictionary<string, string> dicField, DataRow dr)
        {
            try
            {
                foreach (PropertyInfo p in o.GetType().GetProperties())
                {
                    if (p.CanWrite && dicField.ContainsKey(p.Name))
                    {
                        string name = dicField[p.Name].ToString();
                        object val = ParsePropertyValue(p, dr[name]);

                        p.SetValue(o, val, null);
                    }
                }
                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static object FillClassValue(object o, Hashtable dicField, DataRow dr)
        {
            try
            {
                foreach (PropertyInfo p in o.GetType().GetProperties())
                {
                    if (p.CanWrite && dicField.ContainsKey(p.Name))
                    {
                        string name = dicField[p.Name].ToString();
                        object val = ParsePropertyValue(p, dr[name]);

                        p.SetValue(o, val, null);
                    }
                }
                return o;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ConvertClassValue
        public static Dictionary<string, object> ConvertClassValue(object o, string[] keys)
        {
            return ConvertClassValue(o, SetField(keys));
        }

        public static Dictionary<string, object> ConvertClassValue(object o, string[,] keys)
        {
            return ConvertClassValue(o, SetField(keys));
        }

        public static Dictionary<string, object> ConvertClassValue(object o, Dictionary<string, string> dicField)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (dicField.ContainsKey(p.Name))
                {
                    dic.Add(p.Name, p.GetValue(o, null));
                }
            }
            return dic;
        }

        public static Dictionary<string, object> ConvertClassValue(object o, Hashtable dicField)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (dicField.ContainsKey(p.Name))
                {
                    dic.Add(p.Name, p.GetValue(o, null));
                }
            }
            return dic;
        }

        public static Dictionary<string, object> ConvertClassValue(object o, string[,] keys, bool isReplaceName)
        {
            return ConvertClassValue(o, SetField(keys), isReplaceName);
        }

        /// <summary>
        /// ConvertClassValue
        /// </summary>
        /// <param name="o"></param>
        /// <param name="dicField">Dictionary<string:类属性名称, string:字典属性名称（JSON字段名称）></param>
        /// <param name="isReplaceName">是否要替换类属性名称为自定义的JSON字段名称</param>
        /// <returns></returns>
        public static Dictionary<string, object> ConvertClassValue(object o, Dictionary<string, string> dicField, bool isReplaceName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (dicField.ContainsKey(p.Name))
                {
                    dic.Add(isReplaceName ? dicField[p.Name].ToString() : p.Name, p.GetValue(o, null));
                }
            }
            return dic;
        }

        public static Dictionary<string, object> ConvertClassValue(object o, Hashtable dicField, bool isReplaceName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (dicField.ContainsKey(p.Name))
                {
                    dic.Add(isReplaceName ? dicField[p.Name].ToString() : p.Name, p.GetValue(o, null));
                }
            }
            return dic;
        }
        #endregion

        #region  ConvertClassToList
        public List<object> ConvertClassToList(object o, string[] keys)
        {
            return ConvertClassToList(o, SetField(keys));
        }

        public static List<object> ConvertClassToList(object o, string[,] keys)
        {
            return ConvertClassToList(o, SetField(keys));
        }

        public static List<object> ConvertClassToList(object o, Dictionary<string, string> dicField)
        {
            List<object> list = new List<object>();
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (dicField.ContainsKey(p.Name))
                {
                    list.Add(p.GetValue(o, null));
                }
            }
            return list;
        }

        public static List<object> ConvertClassToList(object o, Hashtable dicField)
        {
            List<object> list = new List<object>();
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                if (dicField.ContainsKey(p.Name))
                {
                    list.Add(p.GetValue(o, null));
                }
            }
            return list;
        }
        #endregion


        #region FillDataValue
        public static Dictionary<string, object> FillDataValue(DataRow dr)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    dic.Add(col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, bool isConvertName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    dic.Add(isConvertName ? FieldConvertKey(col.ColumnName) : col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValue(dr));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, bool isConvertName)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValue(dr, isConvertName));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region FillDataValue (FilterField)

        #region  Fill

        public static Dictionary<string, object> FillDataValue(DataRow dr, Dictionary<string, string> dicField)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (dicField.ContainsKey(col.ColumnName))
                    {
                        dic.Add(col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, string[] fields)
        {
            try
            {
                return FillDataValue(dr, SetField(fields));
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, List<string> fields)
        {
            try
            {
                return FillDataValue(dr, SetField(fields));
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, string[,] fields)
        {
            try
            {
                return FillDataValue(dr, SetField(fields));
            }
            catch (Exception ex) { throw (ex); }
        }


        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, Dictionary<string, string> dicField)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValue(dr, dicField));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, string[] fields)
        {
            try
            {
                return FillDataValue(dt, SetField(fields));
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, List<string> fields)
        {
            try
            {
                return FillDataValue(dt, SetField(fields));
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, string[,] fields)
        {
            try
            {
                return FillDataValue(dt, SetField(fields));
            }
            catch (Exception ex) { throw (ex); }
        }

        #endregion

        #region  Fill ConvertName

        public static Dictionary<string, object> FillDataValue(DataRow dr, Dictionary<string, string> dicField, bool isConvertName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (dicField.ContainsKey(col.ColumnName))
                    {
                        dic.Add(isConvertName ? FieldConvertKey(col.ColumnName) : col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, string[] fields, bool isConvertName)
        {
            try
            {
                return FillDataValue(dr, SetField(fields), isConvertName);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, List<string> fields, bool isConvertName)
        {
            try
            {
                return FillDataValue(dr, SetField(fields), isConvertName);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, string[,] fields, bool isConvertName)
        {
            try
            {
                return FillDataValue(dr, SetField(fields), isConvertName);
            }
            catch (Exception ex) { throw (ex); }
        }


        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, Dictionary<string, string> dicField, bool isConvertName)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValue(dr, dicField, isConvertName));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, string[] fields, bool isConvertName)
        {
            try
            {
                return FillDataValue(dt, SetField(fields), isConvertName);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, List<string> fields, bool isConvertName)
        {
            try
            {
                return FillDataValue(dt, SetField(fields), isConvertName);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, string[,] fields, bool isConvertName)
        {
            try
            {
                return FillDataValue(dt, SetField(fields), isConvertName);
            }
            catch (Exception ex) { throw (ex); }
        }

        #endregion

        #region  Fill ConvertName ReplaceName
        /// <summary>
        /// FillDataValue
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dicField">Dictionary<string:数据字段名称, string:字典属性名称></param>
        /// <param name="isConvertName">是否转换字段名称</param>
        /// <param name="isReplaceName">是否替换字段名称</param>
        /// <returns></returns>
        public static Dictionary<string, object> FillDataValue(DataRow dr, Dictionary<string, string> dicField, bool isConvertName, bool isReplaceName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (dicField.ContainsKey(col.ColumnName))
                    {
                        dic.Add(isReplaceName ? dicField[col.ColumnName].ToString() : isConvertName ? FieldConvertKey(col.ColumnName) : col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, string[,] fields, bool isConvertName, bool isReplaceName)
        {
            try
            {
                return FillDataValue(dr, SetField(fields), isConvertName, isReplaceName);
            }
            catch (Exception ex) { throw (ex); }
        }


        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, Dictionary<string, string> dicField, bool isConvertName, bool isReplaceName)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValue(dr, dicField, isConvertName, isReplaceName));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, string[,] fields, bool isConvertName, bool isReplaceName)
        {
            try
            {
                return FillDataValue(dt, SetField(fields), isConvertName, isReplaceName);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  Fill By Hashtable

        public static Dictionary<string, object> FillDataValue(DataRow dr, Hashtable dicField, bool isConvertName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (dicField.ContainsKey(col.ColumnName))
                    {
                        dic.Add(isConvertName ? FieldConvertKey(col.ColumnName) : col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, Hashtable dicField)
        {
            try
            {
                return FillDataValue(dr, dicField, true);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, Hashtable dicField, bool isConvertName, bool isReplaceName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (dicField.ContainsKey(col.ColumnName))
                    {
                        dic.Add(isReplaceName ? dicField[col.ColumnName].ToString() : isConvertName ? FieldConvertKey(col.ColumnName) : col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, Hashtable dicField, bool isConvertName)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValue(dr, dicField, isConvertName));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, Hashtable dicField)
        {
            try
            {
                return FillDataValue(dt, dicField, true);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValue(DataTable dt, Hashtable dicField, bool isConvertName, bool isReplaceName)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValue(dr, dicField, isConvertName, isReplaceName));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region Fill ExcludeField

        public static Dictionary<string, object> FillDataValueEx(DataRow dr, Dictionary<string, string> dicField)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                bool hasField = dicField.Count > 0;

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (!hasField || !dicField.ContainsKey(col.ColumnName))
                    {
                        dic.Add(col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValueEx(DataRow dr, List<string> excludeFields)
        {
            try
            {
                return FillDataValueEx(dr, SetField(excludeFields));
            }
            catch (Exception ex) { throw (ex); }
        }


        public static List<Dictionary<string, object>> FillDataValueEx(DataTable dt, Dictionary<string, string> dicField)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValueEx(dr, dicField));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValueEx(DataTable dt, List<string> excludeFields)
        {
            try
            {
                return FillDataValueEx(dt, SetField(excludeFields));
            }
            catch (Exception ex) { throw (ex); }
        }


        public static Dictionary<string, object> FillDataValueEx(DataRow dr, Dictionary<string, string> dicField, bool isConvertName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                bool hasField = dicField.Count > 0;

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (!hasField || !dicField.ContainsKey(col.ColumnName))
                    {
                        dic.Add(isConvertName ? FieldConvertKey(col.ColumnName) : col.ColumnName, ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValueEx(DataRow dr, List<string> excludeFields, bool isConvertName)
        {
            try
            {
                return FillDataValueEx(dr, SetField(excludeFields), isConvertName);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValueEx(DataTable dt, Dictionary<string, string> dicField, bool isConvertName)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataValueEx(dr, dicField, isConvertName));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<Dictionary<string, object>> FillDataValueEx(DataTable dt, List<string> excludeFields, bool isConvertName)
        {
            try
            {
                return FillDataValueEx(dt, SetField(excludeFields), isConvertName);
            }
            catch (Exception ex) { throw (ex); }
        }

        #endregion

        #endregion

        #region  FillDataToList
        public static List<object> FillDataToList(DataRow dr)
        {
            try
            {
                List<object> list = new List<object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    list.Add(ParseDataColumnValue(col, dr[col.ColumnName]));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<List<object>> FillDataToList(DataTable dt)
        {
            try
            {
                List<List<object>> list = new List<List<object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataToList(dr));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<object> FillDataToList(DataRow dr, string[] fields)
        {
            try
            {
                return FillDataToList(dr, SetField(fields));
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<List<object>> FillDataToList(DataTable dt, string[] fields)
        {
            try
            {
                return FillDataToList(dt, SetField(fields));
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<object> FillDataToList(DataRow dr, Dictionary<string, string> dicField)
        {
            try
            {
                List<object> list = new List<object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (dicField.ContainsKey(col.ColumnName))
                    {
                        list.Add(ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<List<object>> FillDataToList(DataTable dt, Dictionary<string, string> dicField)
        {
            try
            {
                List<List<object>> list = new List<List<object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataToList(dr, dicField));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<object> FillDataToList(DataRow dr, Hashtable dicField)
        {
            try
            {
                List<object> list = new List<object>();

                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (dicField.ContainsKey(col.ColumnName))
                    {
                        list.Add(ParseDataColumnValue(col, dr[col.ColumnName]));
                    }
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<List<object>> FillDataToList(DataTable dt, Hashtable dicField)
        {
            try
            {
                List<List<object>> list = new List<List<object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(FillDataToList(dr, dicField));
                }

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  FieldConvertKey (string_string ConvertTo StringString)
        private static string FieldConvertKey(string fieldName)
        {
            StringBuilder name = new StringBuilder();
            if (!fieldName.Equals(string.Empty))
            {
                string[] arrName = fieldName.Split('_');
                foreach (string str in arrName)
                {
                    name.Append(str.Substring(0, 1).ToUpper());
                    name.Append(str.Substring(1));
                }
            }
            return name.ToString();
        }
        #endregion

        #region  ConvertNumberToList
        public static List<int> ConvertNumberToList(string str)
        {
            return ConvertNumberToList(str, new string[] { "," });
        }

        /// <summary>
        /// 转换数字字符串为数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static List<int> ConvertNumberToList(string str, string[] delimiter)
        {
            List<int> list = new List<int>();

            string[] arr = str.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in arr)
            {
                if (IsIntNumber(s))
                {
                    list.Add(Convert.ToInt32(s));
                }
            }
            return list;
        }
        #endregion


        #region  ParseDataValueList
        public static string ParseDataValueList(DataTable dt)
        {
            try
            {
                return ParseDataValueList(dt, 0);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string ParseDataValueList(DataTable dt, int colIndex)
        {
            try
            {
                StringBuilder ids = new StringBuilder();
                if (CheckTable(dt))
                {
                    int n = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        ids.Append(n++ > 0 ? "," : "");
                        ids.Append(dr[colIndex].ToString());
                    }
                }
                return ids.ToString();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ParseDataValue
        public static string ParseDataValue(DataTable dt, string fieldName)
        {
            try
            {
                return CheckTable(dt) ? ConvertValue(dt.Rows[0][fieldName]) : "";
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string ParseDataValue(DataTable dt, int colIndex)
        {
            try
            {
                return CheckTable(dt) ? ConvertValue(dt.Rows[0][colIndex]) : "";
            }
            catch (Exception ex) { throw (ex); }
        }

        public static int ParseDataCount(DataTable dt)
        {
            try
            {
                return ConvertValue(ParseDataValue(dt, 0), 0);
            }
            catch (Exception ex) { throw (ex); }

        }
        #endregion


        #region  PushToDictionary
        public static bool PushToDictionary(Dictionary<string, object> dic, string key, object value)
        {
            if (dic != null)
            {
                if (dic.ContainsKey(key)) { dic[key] = value; }
                else { dic.Add(key, value); }
                return true;
            }
            return false;
        }

        public static bool PushToDictionary(Dictionary<string, string> dic, string key, string value)
        {
            if (dic != null)
            {
                if (dic.ContainsKey(key)) { dic[key] = value; }
                else { dic.Add(key, value); }
                return true;
            }
            return false;
        }

        public static bool PushToDictionary(Dictionary<string, int> dic, string key, int value)
        {
            if (dic != null)
            {
                if (dic.ContainsKey(key)) { dic[key] = value; }
                else { dic.Add(key, value); }
                return true;
            }
            return false;
        }

        public static bool PushToDictionary(Dictionary<int, int> dic, int key, int value)
        {
            if (dic != null)
            {
                if (dic.ContainsKey(key)) { dic[key] = value; }
                else { dic.Add(key, value); }
                return true;
            }
            return false;
        }

        public static bool PushToHashtable(Hashtable ht, string key, object value)
        {
            if (ht != null)
            {
                if (ht.ContainsKey(key)) { ht[key] = value; }
                else { ht.Add(key, value); }
                return true;
            }
            return false;
        }
        #endregion


    }
}