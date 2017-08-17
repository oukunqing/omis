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
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        #region  ��������POST�ύ
        /// <summary>
        /// ��������POST�ύ
        /// </summary>
        /// <param name="strUrl">URL��ַ</param>
        /// <param name="strRequestData">��������</param>
        /// <param name="encoding">�ַ�����</param>
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
        /// ��������POST�ύ
        /// </summary>
        /// <param name="strUrl">URL��ַ</param>
        /// <param name="strRequestData">��������</param>
        /// <param name="encoding">�ַ�����</param>
        /// <param name="timeout">��ʱʱ�䣬��λ������</param>
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

        #region  ���WEB�ͻ���ҳ�����
        /// <summary>
        ///  ���WEB�ͻ���ҳ�����
        /// </summary>
        /// <param name="strUrl">URL��ַ</param>
        /// <param name="encoding">�ַ�����</param>
        /// <returns></returns>
        public static string GetWebContent(string strUrl, Encoding encoding)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers.Set("User-Agent", "Microsoft Internet Explorer");//���ӵĴ���αװ

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