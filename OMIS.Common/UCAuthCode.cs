using System;
using System.Collections.Generic;
using System.Text;

namespace OMIS.Common
{

    #region  ¼ÓÃÜ¡¢½âÃÜ
    public enum AuthCodeOperation
    {
        encode,
        decode,
    }
    #endregion

    public static class UCAuthCode
    {

        public static string AuthCode(string source, AuthCodeOperation operation, string key, int expiry)
        {
            if (Utils.StrIsNullOrEmpty(source) || Utils.StrIsNullOrEmpty(key))
            {
                return "";
            }
            int length = 4;
            key = Utils.MD5(key);
            string str = Utils.MD5(Utils.CutString(key, 0, 0x10));
            string str2 = Utils.MD5(Utils.CutString(key, 0x10, 0x10));
            string str3 = (length > 0) ? ((operation == AuthCodeOperation.decode) ? Utils.CutString(source, 0, length) : Utils.RandomString(length)) : "";
            string pass = str + Utils.MD5(str + str3);
            if (operation == AuthCodeOperation.decode)
            {
                byte[] buffer;
                try
                {
                    buffer = Convert.FromBase64String(Utils.CutString(source, length));
                }
                catch
                {
                    try
                    {
                        buffer = Convert.FromBase64String(Utils.CutString(source + "=", length));
                    }
                    catch
                    {
                        try
                        {
                            buffer = Convert.FromBase64String(Utils.CutString(source + "==", length));
                        }
                        catch
                        {
                            return "";
                        }
                    }
                }
                string str5 = Encoding.Default.GetString(RC4(buffer, pass));
                if (Utils.CutString(str5, 10, 0x10) == Utils.CutString(Utils.MD5(Utils.CutString(str5, 0x1a) + str2), 0, 0x10))
                {
                    return Utils.CutString(str5, 0x1a);
                }
                return "";
            }
            source = "0000000000" + Utils.CutString(Utils.MD5(source + str2), 0, 0x10) + source;
            byte[] inArray = RC4(Encoding.Default.GetBytes(source), pass);
            return (str3 + Convert.ToBase64String(inArray));
        }

        public static string AuthCodeDecode(string str, string key)
        {
            return AuthCode(str, AuthCodeOperation.decode, key, 0);
        }

        public static string AuthCodeDecode(string str, string key, int expiry)
        {
            return AuthCode(str, AuthCodeOperation.decode, key, expiry);
        }

        public static string AuthCodeEncode(string str, string key)
        {
            return AuthCode(str, AuthCodeOperation.encode, key, 0);
        }

        public static string AuthCodeEncode(string str, string key, int expiry)
        {
            return AuthCode(str, AuthCodeOperation.encode, key, expiry);
        }

        private static byte[] GetKey(byte[] pass, int kLen)
        {
            byte[] buffer = new byte[kLen];
            for (long i = 0L; i < kLen; i += 1L)
            {
                buffer[(int)((IntPtr)i)] = (byte)i;
            }
            long num2 = 0L;
            for (long j = 0L; j < kLen; j += 1L)
            {
                num2 = ((num2 + buffer[(int)((IntPtr)j)]) + pass[(int)((IntPtr)(j % ((long)pass.Length)))]) % ((long)kLen);
                byte num4 = buffer[(int)((IntPtr)j)];
                buffer[(int)((IntPtr)j)] = buffer[(int)((IntPtr)num2)];
                buffer[(int)((IntPtr)num2)] = num4;
            }
            return buffer;
        }

        private static byte[] RC4(byte[] input, string pass)
        {
            if ((input == null) || (pass == null))
            {
                return null;
            }
            Encoding encoding = Encoding.Default;
            byte[] buffer = new byte[input.Length];
            byte[] key = GetKey(encoding.GetBytes(pass), 0x100);
            long num = 0L;
            long num2 = 0L;
            for (long i = 0L; i < input.Length; i += 1L)
            {
                num = (num + 1L) % ((long)key.Length);
                num2 = (num2 + key[(int)((IntPtr)num)]) % ((long)key.Length);
                byte num4 = key[(int)((IntPtr)num)];
                key[(int)((IntPtr)num)] = key[(int)((IntPtr)num2)];
                key[(int)((IntPtr)num2)] = num4;
                byte num5 = input[(int)((IntPtr)i)];
                byte num6 = key[(key[(int)((IntPtr)num)] + key[(int)((IntPtr)num2)]) % key.Length];
                buffer[(int)((IntPtr)i)] = (byte)(num5 ^ num6);
            }
            return buffer;
        }
    }
}