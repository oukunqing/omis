using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace OMIS.Common
{
    public class RemoteRequest
    {

        public RemoteRequest()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region  服务器端POST提交
        /// <summary>
        /// 服务器端POST提交
        /// </summary>
        /// <param name="strUrl">URL地址</param>
        /// <param name="strRequestData">数据内容</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string PostRequest(string strUrl, string strRequestData, Encoding encoding)
        {
            try
            {
                WebRequest req = WebRequest.Create(new Uri(strUrl));
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";

                using (Stream reqStream = req.GetRequestStream())
                {
                    byte[] buffer = encoding.GetBytes(strRequestData.ToString());
                    reqStream.Write(buffer, 0, buffer.Length);
                }

                WebResponse rsp = req.GetResponse();
                StringBuilder strResponse = new StringBuilder();
                using (StreamReader reader = new StreamReader(rsp.GetResponseStream(), encoding))
                {
                    strResponse.Append(reader.ReadToEnd());
                }
                if (rsp != null)
                {
                    rsp.Close();
                    rsp = null;
                }
                if (req != null)
                {
                    req = null;
                }

                return strResponse.ToString();
            }
            catch (Exception ex) { throw (ex); }
        }

        /// <summary>
        /// 服务器端POST提交
        /// </summary>
        /// <param name="strUrl">URL地址</param>
        /// <param name="strRequestData">数据内容</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间，单位：毫秒</param>
        /// <returns></returns>
        public static string PostRequest(string strUrl, string strRequestData, Encoding encoding, int timeout)
        {
            try
            {
                WebRequest req = WebRequest.Create(new Uri(strUrl));
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";
                req.Timeout = timeout;

                using (Stream reqStream = req.GetRequestStream())
                {
                    byte[] buffer = encoding.GetBytes(strRequestData.ToString());
                    reqStream.Write(buffer, 0, buffer.Length);
                }

                WebResponse rsp = req.GetResponse();
                StringBuilder strResponse = new StringBuilder();
                using (StreamReader reader = new StreamReader(rsp.GetResponseStream(), encoding))
                {
                    strResponse.Append(reader.ReadToEnd());
                }
                if (rsp != null)
                {
                    rsp.Close();
                    rsp = null;
                }
                if (req != null)
                {
                    req = null;
                }

                return strResponse.ToString();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得WEB客户端页面输出
        /// <summary>
        ///  获得WEB客户端页面输出
        /// </summary>
        /// <param name="strUrl">URL地址</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string GetWebContent(string strUrl, Encoding encoding)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers.Set("User-Agent", "Microsoft Internet Explorer");//增加的代码伪装

                Stream resStream = wc.OpenRead(new Uri(strUrl));
                StringBuilder strResponse = new StringBuilder();
                using (StreamReader reader = new StreamReader(resStream, encoding))
                {
                    strResponse.Append(reader.ReadToEnd());
                }

                resStream.Close();
                wc.Dispose();

                return strResponse.ToString();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}