using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Security;
using System.Security.Cryptography;

namespace OMIS.Common
{
    #region  哈希加密格式
    /// <summary>
    /// 哈希加密格式
    /// </summary>
    public enum EncryptFormat
    {
        /// <summary>
        /// SHA1加密
        /// </summary>
        SHA1,
        /// <summary>
        /// MD5加密
        /// </summary>
        MD5,
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        ShortMd5,
        HMAC,
    }
    #endregion

    /// <summary>
    /// 加密与解密
    /// </summary>
    public class Encrypt
    {

        #region  MD5加密
        public static byte[] MD5Encrypt(byte[] buffer)
        {
            return new MD5CryptoServiceProvider().ComputeHash(buffer);
        }
        public static string MD5Encrypt(string plain)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.UTF8.GetBytes(plain));
            return BitConverter.ToString(md5.Hash).Replace("-", "");
        }
        #endregion

        #region  哈希加密
        /// <summary>
        /// 哈希加密 MD5
        /// </summary>
        /// <param name="plain">明文</param>
        /// <returns></returns>
        public static string HashEncrypt(string plain)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(plain, EncryptFormat.MD5.ToString());
        }
        /// <summary>
        /// 哈希加密
        /// </summary>
        /// <param name="plain">明文</param>
        /// <param name="format">加密格式</param>
        /// <returns>密文 大写</returns>
        public static string HashEncrypt(string plain, EncryptFormat format)
        {
            string encrypt = FormsAuthentication.HashPasswordForStoringInConfigFile(plain, format.ToString());
            return format != EncryptFormat.ShortMd5 ? encrypt : encrypt.Substring(8, 16);
        }
        #endregion

        #region  AES加密解密
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string AESKey
        {
            get { return @"cf8366cabe1c6e37df12b59e31a3fd7b"; }
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string AESIV
        {
            get { return @"be1c6e37df12b59e"; }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plain">明文字符串</param>
        /// <returns>密文</returns>
        public static string AESEncode(string plain)
        {
            return AESEncode(plain, AESKey, AESIV);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plain">明文字符串</param>
        /// <param name="Key">密钥 32位</param>
        /// <param name="Vector">向量 16位</param>
        /// <returns>密文</returns>
        public static string AESEncode(string plain, string Key, string Vector)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(Vector);
            byte[] byteArray = Encoding.UTF8.GetBytes(plain);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return encrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="strEncrypt">密文字符串</param>
        /// <returns>明文</returns>
        public static string AESDecode(string encrypt)
        {
            return AESDecode(encrypt, AESKey, AESIV);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="strEncrypt">密文字符串</param>
        /// <param name="Key">密钥 32位</param>
        /// <param name="Vector">向量 16位</param>
        /// <returns>明文</returns>
        public static string AESDecode(string encrypt, string Key, string Vector)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(Vector);
            byte[] byteArray = Convert.FromBase64String(encrypt);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return decrypt;
        }

        #endregion

        #region  AES加密解密(256)
        public static string AESEncode256(string plain, string key)
        {
            try
            {
                // 256-AES key      
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plain);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string AESDecode256(string code, string key)
        {
            try
            {
                // 256-AES key      
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                byte[] toEncryptArray = Convert.FromBase64String(code);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  DES加密解密
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string DES_Key
        {
            get { return @"6e37df12"; }
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string DES_IV
        {
            get { return @"be1c6e37df12b59e"; }
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="plain">明文字符串</param>
        /// <returns>密文</returns>
        public static string DESEncode(string plain)
        {
            return DESEncode(plain, DES_Key, DES_IV);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="plain">明文字符串</param>
        /// <param name="Key">密钥 8位</param>
        /// <param name="Vector">向量 16位</param>
        /// <returns>密文</returns>
        public static string DESEncode(string plain, string Key, string Vector)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(Vector);
            byte[] byteArray = Encoding.UTF8.GetBytes(plain);

            string encrypt = null;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            des.Clear();

            return encrypt;
        }


        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="strEncrypt">密文字符串</param>
        /// <param name="Key">密钥 8位</param>
        /// <param name="Vector">向量 16位</param>
        /// <returns>明文</returns>
        public static string DESDecode(string encrypt)
        {
            return DESDecode(encrypt, DES_Key, DES_IV);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="strEncrypt">密文字符串</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>明文</returns>
        public static string DESDecode(string encrypt, string Key, string Vector)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(Vector);
            byte[] byteArray = Convert.FromBase64String(encrypt);

            string decrypt = null;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            des.Clear();

            return decrypt;
        }
        #endregion

        #region  TEA加密解密

        #endregion

    }
}