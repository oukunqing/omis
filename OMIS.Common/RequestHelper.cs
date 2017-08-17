using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace OMIS.Common
{
    public class RequestHelper
    {
        
        #region  CheckIsNumber
        public static bool IsNumber(string number)
        {
            return new Regex(@"^[-+]?[0-9]+(\.[0-9]+)?$").IsMatch(number.Trim());
        }

        public static bool IsIntNumber(string number)
        {
            return new Regex(@"^[-+]?[0-9]+$").IsMatch(number.Trim());
        }
        #endregion

        #region  EscapeBackslash
        private static string EscapeBackslash(string value, bool isEscapeBackslash)
        {
            if (isEscapeBackslash)
            {
                value = value.Replace(@"\", @"\\");
            }
            return value;
        }

        private static string EscapeBackslash(string value)
        {
            return value.Replace(@"\", @"\\");
        }

        private static string Clear(string value, bool isTrim, bool isEscapeBackslash)
        {
            if (isTrim)
            {
                value = value.Trim();
            }
            if (isEscapeBackslash)
            {
                value = value.Replace(@"\", @"\\");
            }
            return value;
        }
        #endregion

        #region  GetRequest
        /// <summary>
        /// GetRequest
        /// </summary>
        /// <param name="hr"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetRequest(HttpRequest hr, string key)
        {
            string value = string.Empty;
            
            key = key.Trim();
            if (hr[key] != null)
            {
                value = hr[key].ToString();
            }
            else
            {
                bool hasParam = false;
                if (!hasParam)
                {
                    //忽略大小写
                    string[] arrKeyCopy = { key.ToLower(), key.ToUpper() };
                    foreach (string k in arrKeyCopy)
                    {
                        if (hr[k] != null)
                        {
                            value = hr[k].ToString();
                            hasParam = true;
                            break;
                        }
                    }
                }
                if (!hasParam)
                {
                    string[] arrKey = key.Split(new string[] { ",", "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string k in arrKey)
                    {
                        if (hr[k] != null)
                        {
                            value = hr[k].ToString();
                            hasParam = true;
                            break;
                        }
                    }
                }
                if (!hasParam)
                {
                    //首字母大写
                    string keyCopy = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(key);
                    if (hr[keyCopy] != null)
                    {
                        value = hr[keyCopy].ToString();
                    }
                }
            }
            return value;
        }
        
        private static string GetRequest(string key)
        {
            return GetRequest(HttpContext.Current.Request, key);
        }
        #endregion


        #region  Request
        public static string Request(string key)
        {
            return EscapeBackslash(GetRequest(key).Trim());
        }

        public static string Request(string key, string defaultValue)
        {
            string value = EscapeBackslash(GetRequest(key).Trim());
            return value.Trim().Length > 0 ? value : defaultValue;
        }

        public static string Request(string key, bool isHtmlDecode)
        {
            string value = EscapeBackslash(GetRequest(key).Trim());
            return value.Trim().Length > 0 ? isHtmlDecode ? HttpUtility.HtmlDecode(value) : value : string.Empty;
        }

        public static string Request(string key, bool isHtmlDecode, string defaultValue)
        {
            string value = EscapeBackslash(GetRequest(key).Trim());
            return value.Trim().Length > 0 ? isHtmlDecode ? HttpUtility.HtmlDecode(value) : value : defaultValue;
        }
        
        public static string Request(string key, bool isTrim, bool isEscapeBackslash)
        {
            return Clear(GetRequest(key), isTrim, isEscapeBackslash);
        }

        public static string Request(string key, bool isTrim, bool isEscapeBackslash, string defaultValue)
        {
            string value = Clear(GetRequest(key), isTrim, isEscapeBackslash);
            return value.Trim().Length > 0 ? value : defaultValue;
        }
        #endregion

        #region  Request (int)
        public static int Request(string key, int defaultValue)
        {
            string value = GetRequest(key);
            return IsNumber(value) ? Convert.ToInt32(value.Split('.')[0]) : defaultValue;
        }
        #endregion

        #region  Request (float)
        public static float Request(string key, float defaultValue)
        {
            string value = GetRequest(key);
            return IsNumber(value) ? Convert.ToSingle(value) : defaultValue;
        }
        #endregion

        #region  Request (double)
        public static double Request(string key, double defaultValue)
        {
            string value = GetRequest(key);
            return IsNumber(value) ? Convert.ToDouble(value) : defaultValue;
        }
        #endregion

    }
}