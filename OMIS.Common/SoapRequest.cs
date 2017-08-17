using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace OMIS.Common
{
    public class Soap
    {
        public enum SoapVersion
        {
            soap,
            soap11,
            soap12,
        }

        #region  SOAP请求
        public static string SoapRequest(string url, string soapData, Encoding encoding)
        {
            try
            {
                WebRequest webRequest = WebRequest.Create(new Uri(url));
                webRequest.ContentType = "text/xml; charset=utf-8";
                webRequest.Method = "POST";
                using (Stream reqStream = webRequest.GetRequestStream())
                {
                    byte[] buffer = encoding.GetBytes(soapData.ToString());
                    reqStream.Write(buffer, 0, buffer.Length);
                }

                WebResponse webResponse = webRequest.GetResponse();
                StringBuilder response = new StringBuilder();
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(), encoding))
                {
                    response.Append(reader.ReadToEnd());
                }

                return response.ToString();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  创建SOAP数据
        public static string BuildSoapData(string headerContent, string bodyContent)
        {
            return BuildSoapData(SoapVersion.soap, headerContent, bodyContent);
        }

        public static string BuildSoapData(SoapVersion soapVersion, string headerContent, string bodyContent)
        {
            StringBuilder soap = new StringBuilder();
            string version = soapVersion.ToString();

            soap.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            soap.Append(String.Format("<{0}:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:{0}=\"http://schemas.xmlsoap.org/soap/envelope/\">", soapVersion));
            soap.Append(String.Format("<{0}:Header>", version));
            soap.Append(String.Format("{0}", headerContent));
            soap.Append(String.Format("</{0}:Header>", version));
            soap.Append(String.Format("<{0}:Body>", version));
            soap.Append(String.Format("{0}", bodyContent));
            soap.Append(String.Format("</{0}:Body>", version));
            soap.Append(String.Format("</{0}:Envelope>", version));

            return soap.ToString();
        }
        #endregion

    }
}