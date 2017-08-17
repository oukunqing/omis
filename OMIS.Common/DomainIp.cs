using System;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Common
{
    /// <summary>
    /// DomainIp 的摘要说明
    /// </summary>
    public class DomainIp
    {
        public DomainIp()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 获得客户端IP地址
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


        #region  获得服务器IP地址
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

        #region 获得客户端IP地址
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

        #region  根据域名获取IP地址
        /// <summary>
        /// 根据域名获取IP地址
        /// </summary>
        /// <param name="domain">域名</param>
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