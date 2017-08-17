using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMIS.Common;

namespace OMIS.DAL
{
    public class KeyApi
    {

        public static string OMIS_KEY = Encrypt.MD5Encrypt("OMIS-AUTH-KEY");

        public static string OMIS_AES_KEY = Encrypt.MD5Encrypt("OMIS_AES256_KEY");

    }

    public class AuthCode
    {

        public static string AuthCodeEncode(string str)
        {
            return UCAuthCode.AuthCodeEncode(str, KeyApi.OMIS_KEY);
        }

        public static string AuthCodeEncode(string str, string key)
        {
            return UCAuthCode.AuthCodeEncode(str, key);
        }

        public static string AuthCodeDecode(string code)
        {
            try
            {
                return UCAuthCode.AuthCodeDecode(code, KeyApi.OMIS_KEY);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string AuthCodeDecode(string code, string key)
        {
            try
            {
                return UCAuthCode.AuthCodeDecode(code, key);
            }
            catch (Exception ex) { throw (ex); }
        }
    }

}