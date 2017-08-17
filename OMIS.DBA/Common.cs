using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Globalization;

namespace OMIS.DBA
{
    public class Common : DataConvert
    {

        #region  JavaScriptSerializer

        public static JavaScriptSerializer Json = new JavaScriptSerializer();

        public static string Serialize(object o)
        {
            return Json.Serialize(o);
        }

        public static Dictionary<string, object> Deserialize(string json)
        {
            if (json.Trim().Equals(string.Empty) || !CheckIsJsonData(json))
            {
                return new Dictionary<string, object>();
            }
            return Json.Deserialize<Dictionary<string, object>>(json);
        }
        #endregion
        
        #region  Request
        
        #region  GetRequest
        private static bool _FindKey(HttpRequest hr, string key, ref string val)
        {
            if (hr[key] != null)
            {
                val = hr[key].ToString();
                return true;
            }
            return false;
        }

        private static string GetRequest(HttpRequest hr, string key)
        {
            string val = string.Empty;
            if (!_FindKey(hr, key.Trim(), ref val))
            {
                if (new Regex("[,;\\|]").IsMatch(key))
                {
                    //分割多个关键字
                    string[] arrKey = key.Split(new string[] { "|", ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string k in arrKey)
                    {
                        if (_FindKey(hr, k.Trim(), ref val))
                        {
                            return val;
                        }
                    }
                }
            }
            return val;
        }

        private static string GetRequest(string key)
        {
            return GetRequest(HttpContext.Current.Request, key);
        }
        #endregion

        #region  Escape
        private static string _Escape(string val, bool isEscape)
        {
            return isEscape ? Escape(ref val) : val;
        }

        private static string _Escape(string val)
        {
            return _Escape(val, false);
        }

        private static string _Decode(string val, bool isDecode)
        {
            return isDecode ? Decode(val) : val;
        }
        #endregion

        #region  Request
        public static string Request(string key)
        {
            return GetRequest(key).Trim();
        }

        public static string Request(string key, string val)
        {
            string _val = GetRequest(key).Trim();
            return _val.Length > 0 ? _val : val;
        }

        public static string Request(string key, bool isDecode)
        {
            string _val = GetRequest(key).Trim();
            return _val.Trim().Length > 0 ? _Decode(_val, isDecode) : string.Empty;
        }

        public static string Request(string key, bool isDecode, string val)
        {
            string _val = GetRequest(key).Trim();
            return _val.Trim().Length > 0 ? _Decode(_val, isDecode) : val;
        }

        public static string Request(string key, bool isDecode, bool isEscape)
        {
            string _val = GetRequest(key).Trim();
            return _val.Length > 0 ? _Escape(_Decode(_val, isDecode), isEscape) : string.Empty;
        }

        public static string Request(string key, bool isDecode, bool isEscape, string val)
        {
            string _val = GetRequest(key).Trim();
            return _val.Length > 0 ? _Escape(_Decode(_val, isDecode), isEscape) : val;
        }

        public static string Request(string key, bool isDecode, bool isEscape, bool isTrim)
        {
            string _val = isTrim ? GetRequest(key).Trim() : GetRequest(key);
            return _val.Trim().Length > 0 ? _Escape(_Decode(_val, isDecode), isEscape) : string.Empty;
        }

        public static string Request(string key, bool isDecode, bool isEscape, bool isTrim, string val)
        {
            string _val = isTrim ? GetRequest(key).Trim() : GetRequest(key);
            return _val.Trim().Length > 0 ? _Escape(_Decode(_val, isDecode), isEscape) : val;
        }
        #endregion

        #region  Request (int)
        public static int Request(string key, int val)
        {
            return ConvertValue(GetRequest(key).Split('.')[0], val);
        }
        #endregion

        #region  Request (float)
        public static float Request(string key, float val)
        {
            return ConvertValue(GetRequest(key), val);
        }
        #endregion

        #region  Request (double)
        public static double Request(string key, double val)
        {
            return ConvertValue(GetRequest(key), val);
        }
        #endregion

        #region  Request (Dictionary<string,object>)
        public static string Request(string key, out Dictionary<string, object> dic)
        {
            try
            {
                string _val = _Decode(GetRequest(key).Trim(), true);

                dic = _val.StartsWith("{") && _val.EndsWith("}") ? Json.Deserialize<Dictionary<string, object>>(_val) : new Dictionary<string, object>();

                return _val;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #endregion

        #region JsonResult

        public static string ToJsonValue(string s)
        {
            return Json.Serialize(s);
        }

        #region  JsonResult

        public static string ToJsonResult(Dictionary<string, object> dic)
        {
            return Json.Serialize(dic);
        }

        public static string ToJsonResult()
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 } });
        }

        public static string ToJsonResult(int result)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", result } });
        }

        public static string ToJsonResult(string key, object data)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 }, { key, data } });
        }

        public static string ToJsonResult(int result, string key, object data)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", result }, { key, data } });
        }
        #endregion

        #region  JsonList/JsonData/JsonTree

        public static string ToJsonList(object list)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 }, { "list", list } });
        }

        public static string ToJsonList(object list, int count)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 }, { "dataCount", count }, { "list", list } });
        }

        public static string ToJsonList(List<object> list, int count)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 }, { "dataCount", count }, { "list", list } });
        }

        public static string ToJsonData(object data)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 }, { "data", data } });
        }
        
        public static string ToJsonTree(List<object> list)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 }, { "tree", list } });
        }

        public static string ToJsonTree(object tree)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 }, { "tree", tree } });
        }

        public static string ToJsonId(int id)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 1 }, { "id", id } });
        }

        public static string ToJsonId(int result, int id)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", result }, { "id", id } });
        }

        public static string ToJsonStatus(int result, int status, string msg)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", result }, { "status", status }, { "msg", msg } });
        }
        #endregion

        #region  JsonMessage
        public static string ToJsonMessage(string msg)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 0 }, { "msg", msg } });
        }

        public static string ToJsonMessage(int result, string msg)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", result }, { "msg", msg } });
        }
        #endregion

        #region  JsonError
        public static string ToJsonError(string msg, string error)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", -1 }, { "msg", msg }, { "error", error } });
        }

        public static string ToJsonError(int result, string msg, string error)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", result }, { "msg", msg }, { "error", error } });
        }

        public static string ToJsonError(Exception ex)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", -1 }, { "msg", ex.Message }, { "error", ToExceptionCode(ex, HttpContext.Current) } });
        }

        public static string ToJsonError(Exception ex, HttpContext hc)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", -1 }, { "msg", ex.Message }, { "error", ToExceptionCode(ex, hc) } });
        }

        public static string ToJsonError(Exception ex, HttpContext hc, string msg)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", -1 }, { "msg", msg }, { "error", ToExceptionCode(ex, hc) } });
        }
        #endregion


        #region  JsonHello (only of testing)
        public static string ToJsonHello()
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", 0 }, { "msg", "hello world!" }, { "time", GetDateTime() } });
        }
        #endregion

        #endregion

        #region  ExceptionResult
        public static string ToExceptionCode(Exception ex)
        {
            return ToExceptionCode(ex, HttpContext.Current);
        }

        public static string ToExceptionCode(Exception ex, HttpContext hc)
        {
            string code = "[Message]: {0}\r\n[Source]: {1}\r\n[StackTrace]: {2}\r\n[TargetSite]: {3}\r\n[RawUrl]: {4}";
            return String.Format(code, ex.Message, ex.Source, ex.StackTrace, ex.TargetSite, hc.Request.RawUrl);
        }

        public static string ToExceptionResult(Exception ex)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", -1 }, { "msg", ex.Message }, { "error", ToExceptionCode(ex, HttpContext.Current) } });
        }

        public static string ToExceptionResult(Exception ex, HttpContext hc)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", -1 }, { "msg", ex.Message }, { "error", ToExceptionCode(ex, hc) } });
        }

        public static string ToMessageResult(int result, string msg, string error)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", result }, { "msg", msg }, { "error", error } });
        }

        public static string ToMessageResult(int result, string msg)
        {
            return Json.Serialize(new Dictionary<string, object>() { { "result", result }, { "msg", msg }, { "error", "" } });
        }

        #endregion

    }
}