using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Common
{
    public abstract class DataConvert : DataCheck
    {
        
        #region  ConvertValue

        #region  ConvertToInt32
        public static int ConvertValue(object o, int val)
        {
            return o != null ?ConvertValue(o.ToString(), val) : val;
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
        #endregion

        #region  ConvertDictionaryValue
        public static int ConvertValue(Dictionary<string, object> dic, string key, int val)
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

        public static float ConvertValue(Dictionary<string, object> dic, string key, float val)
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

        public static double ConvertValue(Dictionary<string, object> dic, string key, double val)
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

        public static string ConvertValue(Dictionary<string, object> dic, string key)
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

        public static string ConvertValue(Dictionary<string, object> dic, string key, string val)
        {
            foreach (string k in _ParseKey(key))
            {
                if (dic.ContainsKey(k))
                {
                    return ConvertValue(dic[k]);
                }
            }
            return dic.ContainsKey(key) ? ConvertValue(dic[key], val) : val;
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
        #endregion


        #region  ConvertDateTime
        public static string ConvertDateTime(string dt)
        {
            return CheckIsDateTime(dt) ? DateTime.Parse(dt).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
        }

        public static string ConvertDateTime(string dt, string replace)
        {
            return CheckIsDateTime(dt) ? DateTime.Parse(dt).ToString("yyyy-MM-dd HH:mm:ss") : replace;
        }

        public static string ConvertDateTime(string dt, string replace, string format)
        {
            return CheckIsDateTime(dt) ? DateTime.Parse(dt).ToString(format) : replace;
        }

        public static string ConvertDate(string dt)
        {
            return CheckIsDateTime(dt) ? DateTime.Parse(dt).ToString("yyyy-MM-dd") : string.Empty;
        }

        public static string ConvertDate(string dt, string format)
        {
            return CheckIsDateTime(dt) ? DateTime.Parse(dt).ToString(format) : string.Empty;
        }

        public static string ConvertDate(string dt, string replace, string format)
        {
            return CheckIsDateTime(dt) ? DateTime.Parse(dt).ToString(format) : replace;
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
                        string type = ParsePropertyType(p.PropertyType);
                        string _name = dicField[p.Name].ToString();
                        object val = CheckNumberType(type) ? (object)ConvertValue(dr[_name], 0) : ConvertValue(dr[p.Name]);

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
                        string type = ParsePropertyType(p.PropertyType);
                        string _name = dicField[p.Name].ToString();
                        object val = CheckNumberType(type) ? (object)ConvertValue(dr[_name], 0) : ConvertValue(dr[p.Name]);

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
                        string type = ParsePropertyType(p.PropertyType);
                        string _name = dicField[p.Name].ToString();
                        object val = CheckNumberType(type) ? (object)ConvertValue(dr[_name], 0) : ConvertValue(dr[_name]);

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
                        string type = ParsePropertyType(p.PropertyType);
                        string _name = dicField[p.Name].ToString();
                        object val = CheckNumberType(type) ? (object)ConvertValue(dr[_name], 0) : ConvertValue(dr[_name]);

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
            try { return FillDataValue(dr, true); }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, bool isConvertName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    string name = col.ColumnName;
                    object val = CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(dr[name], 0) : ConvertValue(dr[name]);
                    dic.Add(isConvertName ? FieldConvertKey(name) : name, val);
                }
                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region FillDataValue (FilterField)
        public static Dictionary<string, object> FillDataValue(DataRow dr, string[] fields)
        {
            try { return FillDataValue(dr, SetField(fields)); }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, string[,] fields)
        {
            try { return FillDataValue(dr, SetField(fields)); }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, string[] fields, bool isConvertName)
        {
            try { return FillDataValue(dr, SetField(fields), isConvertName); }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, string[,] fields, bool isConvertName)
        {
            try { return FillDataValue(dr, SetField(fields), isConvertName); }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, Dictionary<string, string> dicField)
        {
            try { return FillDataValue(dr, dicField, true); }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, Hashtable dicField)
        {
            try { return FillDataValue(dr, dicField, true); }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, Dictionary<string, string> dicField, bool isConvertName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    string name = col.ColumnName;
                    if (dicField.ContainsKey(name))
                    {
                        object val = CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(dr[name], 0) : ConvertValue(dr[name]);
                        dic.Add(isConvertName ? FieldConvertKey(name) : name, val);
                    }
                }
                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static Dictionary<string, object> FillDataValue(DataRow dr, Hashtable dicField, bool isConvertName)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    string name = col.ColumnName;
                    if (dicField.ContainsKey(name))
                    {
                        object val = CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(dr[name], 0) : ConvertValue(dr[name]);
                        dic.Add(isConvertName ? FieldConvertKey(name) : name, val);
                    }
                }
                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }


        public static Dictionary<string, object> FillDataValue(DataRow dr, string[,] fields, bool isConvertName, bool isReplaceName)
        {
            try { return FillDataValue(dr, SetField(fields), isConvertName, isReplaceName); }
            catch (Exception ex) { throw (ex); }
        }

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
                    string name = col.ColumnName;
                    if (dicField.ContainsKey(name))
                    {
                        object val = CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(dr[name], 0) : ConvertValue(dr[name]);
                        dic.Add(isReplaceName ? dicField[name].ToString() : isConvertName ? FieldConvertKey(name) : name, val);
                    }
                }
                return dic;
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
                    string name = col.ColumnName;
                    if (dicField.ContainsKey(name))
                    {
                        object val = CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(dr[name], 0) : ConvertValue(dr[name]);
                        dic.Add(isReplaceName ? dicField[name].ToString() : isConvertName ? FieldConvertKey(name) : name, val);
                    }
                }
                return dic;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  FillDataToList
        public static List<object> FillDataToList(DataRow dr)
        {
            try
            {
                List<object> list = new List<object>();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    string name = col.ColumnName;
                    list.Add(CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(dr[name], 0) : ConvertValue(dr[name]));
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static List<object> FillDataToList(DataRow dr, string[] fields)
        {
            try { return FillDataToList(dr, SetField(fields)); }
            catch (Exception ex) { throw (ex); }
        }

        public static List<object> FillDataToList(DataRow dr, Dictionary<string, string> dicField)
        {
            try
            {
                List<object> list = new List<object>();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    string name = col.ColumnName;
                    if (dicField.ContainsKey(name))
                    {
                        list.Add(CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(dr[name], 0) : ConvertValue(dr[name]));
                    }
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
                    string name = col.ColumnName;
                    if (dicField.ContainsKey(name))
                    {
                        list.Add(CheckNumberType(ParsePropertyType(col.DataType)) ? (object)ConvertValue(dr[name], 0) : ConvertValue(dr[name]));
                    }
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

    }
}