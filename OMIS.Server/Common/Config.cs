using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

namespace OMIS.Server.Common
{
    class Config
    {

        #region  验证是否是数字
        public static bool IsNumber(string strNumber)
        {
            string strPattern = @"^\-?(0+)?(\d+)(.\d+)?$";

            return new Regex(strPattern).IsMatch(strNumber);
        }
        #endregion
        
        #region  获得配置信息
        public static int GetAppSetting(string strKey, int defaultValue)
        {
            if (ConfigurationManager.AppSettings[strKey] != null)
            {
                if (IsNumber(ConfigurationManager.AppSettings[strKey].ToString()))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings[strKey].ToString());
                }
                else
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
        #endregion

        #region  获得配置信息
        public static string GetAppSetting(string strKey, string defaultValue)
        {
            if (ConfigurationManager.AppSettings[strKey] != null)
            {
                return ConfigurationManager.AppSettings[strKey].ToString();
            }
            return defaultValue;
        }

        public static string GetAppSetting(string strKey)
        {
            if (ConfigurationManager.AppSettings[strKey] != null)
            {
                return ConfigurationManager.AppSettings[strKey].ToString();
            }
            return string.Empty;
        }
        #endregion


        #region  获得配置信息
        public static int GetConnectionString(string strName, int defaultValue)
        {
            if (ConfigurationManager.ConnectionStrings[strName] != null)
            {
                if (IsNumber(ConfigurationManager.ConnectionStrings[strName].ToString()))
                {
                    return Convert.ToInt32(ConfigurationManager.ConnectionStrings[strName].ToString());
                }
                else
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
        #endregion

        #region  获得配置信息
        public static string GetConnectionString(string strName, string defaultValue)
        {
            if (ConfigurationManager.ConnectionStrings[strName] != null)
            {
                return ConfigurationManager.ConnectionStrings[strName].ToString();
            }
            return defaultValue;
        }

        public static string GetConnectionString(string strName)
        {
            if (ConfigurationManager.ConnectionStrings[strName] != null)
            {
                return ConfigurationManager.ConnectionStrings[strName].ToString();
            }
            return string.Empty;
        }
        #endregion


        #region  获取服务端口
        public static List<ServerPort> GetServerPort()
        {
            try
            {
                List<ServerPort> lstPort = new List<ServerPort>();
                string strServerPort = GetAppSetting("ServerPortList", string.Empty);
                if (!strServerPort.Equals(string.Empty))
                {
                    string[] arrCfg = strServerPort.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in arrCfg)
                    {
                        string[] arrPort = str.Split(':');
                        if (arrPort.Length >= 2)
                        {
                            ServerPort sp = new ServerPort();
                            sp.Server = arrPort[0];
                            sp.Port = Convert.ToInt32(arrPort[1]);

                            lstPort.Add(sp);
                        }
                    }
                }
                return lstPort;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}