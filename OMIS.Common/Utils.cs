using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace OMIS.Common
{
    public static class Utils
    {
        public static string AscArr2Str(byte[] b)
        {
            return Encoding.Unicode.GetString(Encoding.Convert(Encoding.ASCII, Encoding.Unicode, b));
        }

        public static string Base64Decode(string content)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(content);
                return Encoding.Default.GetString(bytes);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string Base64Encode(string content)
        {
            try
            {
                return Convert.ToBase64String(Encoding.Default.GetBytes(content));
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length *= -1;
                    if ((startIndex - length) < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else if ((length >= 0) && ((length + startIndex) > 0))
            {
                length += startIndex;
                startIndex = 0;
            }
            else
            {
                return "";
            }
            if ((str.Length - startIndex) < length)
            {
                length = str.Length - startIndex;
            }
            return str.Substring(startIndex, length);
        }

        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public static bool IsIntArray(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            Regex regex = new Regex(@"\d{1,9}");
            if (!regex.IsMatch(str))
            {
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    if (!regex.IsMatch(str2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void Log(string msg)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("/api/log.txt"), msg + "\r\n", Encoding.Default);
        }

        public static string MD5(string content)
        {
            byte[] bytes = new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(content));
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return result.ToString();
        }

        public static long PHP_Time()
        {
            DateTime time = new DateTime(0x7b2, 1, 1);
            return ((DateTime.UtcNow.Ticks - time.Ticks) / 0x989680L);
        }

        public static string PHP_UrlEncode(string str)
        {
            string str2 = string.Empty;
            string str3 = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < str.Length; i++)
            {
                string str4 = str.Substring(i, 1);
                if (str3.Contains(str4))
                {
                    str2 = str2 + str4;
                }
                else
                {
                    Encoding encoding = Encoding.GetEncoding("utf-8");
                    foreach (byte num2 in encoding.GetBytes(str4))
                    {
                        str2 = str2 + "%" + num2.ToString("X");
                    }
                }
            }
            return str2;
        }

        public static string RandomString(int lens)
        {
            char[] chArray = new char[] { 
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 
                'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 
                'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 
                'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
             };
            int length = chArray.Length;
            string str = "";
            Random random = new Random();
            for (int i = 0; i < lens; i++)
            {
                str = str + chArray[random.Next(length)];
            }
            return str;
        }

        public static DateTime StringToDatetime(string content)
        {
            return StringToDatetime(content, DateTime.MinValue);
        }

        public static DateTime StringToDatetime(string content, DateTime defaultValue)
        {
            if (!string.IsNullOrEmpty(content))
            {
                DateTime minValue = DateTime.MinValue;
                if (DateTime.TryParse(content, out minValue))
                {
                    return minValue;
                }
            }
            return defaultValue;
        }

        public static int StringToInt(string content)
        {
            return StringToInt(content, 0);
        }

        public static int StringToInt(string content, int defaultValue)
        {
            if (!string.IsNullOrEmpty(content))
            {
                int result = 0;
                if (int.TryParse(content, out result))
                {
                    return result;
                }
            }
            return defaultValue;
        }

        public static short StringToShort(string content)
        {
            return StringToShort(content, 0);
        }

        public static short StringToShort(string content, short defaultValue)
        {
            if (!string.IsNullOrEmpty(content))
            {
                short result = 0;
                if (short.TryParse(content, out result))
                {
                    return result;
                }
            }
            return defaultValue;
        }

        public static bool StrIsNullOrEmpty(string content)
        {
            if ((content != null) && !(content.Trim().Equals(string.Empty)))
            {
                return false;
            }
            return true;
        }

        public static int UnixTimestamp()
        {
            DateTime time = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            string content = DateTime.Parse(DateTime.Now.ToString()).Subtract(time).Ticks.ToString();
            return Convert.ToInt32(content.Substring(0, content.Length - 7));
        }

    }
}