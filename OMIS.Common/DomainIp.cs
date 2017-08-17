using System;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Common
{
    /// <summary>
    /// DomainIp ��ժҪ˵��
    /// </summary>
    public class DomainIp
    {
        public DomainIp()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        #region ��ÿͻ���IP��ַ
        public static string GetIpAddress()
        {
            string strIp = System.Web.HttpContext.Current.Request.Headers["x-forwarded-for"];
            if (strIp == null || strIp.Length == 0)
            {
                strIp = System.Web.HttpContext.Current.Request.UserHostName;
            }
            return strIp;
        }
        #endregion


        #region  ��÷�����IP��ַ
        public static string GetServerIp()
        {
            return System.Web.HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
        }

        public static string GetServerIp(string url)
        {
            try
            {
                return GetIpByDomain(url);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region ��ÿͻ���IP��ַ
        public static string GetClientIp()
        {
            string strIp = System.Web.HttpContext.Current.Request.Headers["x-forwarded-for"];
            if (strIp == null || strIp.Length == 0)
            {
                strIp = System.Web.HttpContext.Current.Request.UserHostName;
            }
            return strIp;
        }
        #endregion

        #region  ����������ȡIP��ַ
        /// <summary>
        /// ����������ȡIP��ַ
        /// </summary>
        /// <param name="domain">����</param>
        /// <returns></returns>
        public static string GetIpByDomain(string domain)
        {
            if (domain.Trim().Equals(string.Empty))
            {
                return string.Empty;
            }
            try
            {
                domain = new Regex("\\w+://").Replace(domain, "");

                if (domain.IndexOf(':') > 0)
                {
                    domain = domain.Split(':')[0];
                }
                IPHostEntry host = Dns.GetHostEntry(domain);
                return host.AddressList.GetValue(0).ToString();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}