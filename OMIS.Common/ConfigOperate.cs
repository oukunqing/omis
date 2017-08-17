using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;

namespace OMIS.Common
{

    /// <summary>
    /// 网站配置文件操作
    /// </summary>
    public class ConfigOperate
    {

        public enum ConfigRootName
        {
            appSettings,
            connectionStrings,
        }

        #region  属性

        public string[] AppSettingsKey { get; set; }

        public string[] ConnectStringKey { get; set; }

        #endregion
        
        public ConfigOperate()
        {
            this.AppSettingsKey = new string[] { "key", "value" };
            this.ConnectStringKey = new string[] { "name", "connectionString" };
        }

        #region  获得网站配置文件信息
        /// <summary>
        /// 获得AppSettings
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAppSettings(string fileName)
        {
            return this.GetWebConfig(fileName, ConfigRootName.appSettings, this.AppSettingsKey);
        }

        /// <summary>
        /// 获得ConnectionString
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetConnectionString(string fileName)
        {
            return this.GetWebConfig(fileName, ConfigRootName.connectionStrings, this.ConnectStringKey);
        }

        /// <summary>
        /// 获得配置文件信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="rootName"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetWebConfig(string fileName, ConfigRootName rootName, string[] keyValue)
        {
            try
            {
                Dictionary<string, string> config = new Dictionary<string, string>();

                XmlDocument xml = new XmlDocument();

                xml.Load(HttpContext.Current.Server.MapPath(fileName));

                XmlNode root = xml.SelectSingleNode(rootName.ToString());

                XmlNodeList childlist = root.ChildNodes;

                foreach (XmlNode xn in childlist)
                {
                    if (xn.Name.Equals("add") && xn.Attributes.Count >= 2)
                    {
                        string strKey = xn.Attributes[keyValue[0]].Value.Trim();
                        string strValue = xn.Attributes[keyValue[1]].Value.Trim();

                        if (config.ContainsKey(strKey))
                        {
                            config.Remove(strKey);
                        }
                        config.Add(strKey, strValue);
                    }
                }
                return config;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region  更新网站配置文件信息
        /// <summary>
        /// 更新AppSettings
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool UpdateAppSettings(string fileName, Dictionary<string, string> config)
        {
            return this.UpdateWebConfig(fileName, ConfigRootName.appSettings, this.AppSettingsKey, config);
        }

        /// <summary>
        /// 更新ConnectionString
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool UpdateConnectionString(string fileName, Dictionary<string, string> config)
        {
            return this.UpdateWebConfig(fileName, ConfigRootName.connectionStrings, this.ConnectStringKey, config);
        }

        /// <summary>
        /// 更新网站配置文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="rootName"></param>
        /// <param name="keyValue"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private bool UpdateWebConfig(string fileName, ConfigRootName rootName, string[] keyValue, Dictionary<string, string> config)
        {
            try
            {
                XmlDocument xml = new XmlDocument();

                xml.Load(HttpContext.Current.Server.MapPath(fileName));

                XmlNode root = xml.SelectSingleNode(rootName.ToString());

                XmlNodeList childlist = root.ChildNodes;
                foreach (XmlNode xn in childlist)
                {
                    if (xn.Name.Equals("add") && xn.Attributes.Count >= 2)
                    {
                        string strKey = xn.Attributes[keyValue[0]].Value.Trim();
                        string strValue = xn.Attributes[keyValue[1]].Value.Trim();

                        if (config.ContainsKey(strKey))
                        {
                            if (!strValue.Equals(config[strKey].ToString()))
                            {
                                xn.Attributes[keyValue[1]].Value = config[strKey].ToString();
                            }
                            config.Remove(strKey);
                        }
                    }
                }
                foreach (KeyValuePair<string, string> objKV in config)
                {
                    XmlElement newNode = xml.CreateElement("add");

                    newNode.SetAttribute("key", objKV.Key.ToString());
                    newNode.SetAttribute("value", objKV.Value.ToString());

                    xml.DocumentElement.AppendChild(newNode);
                }

                xml.Save(HttpContext.Current.Server.MapPath(fileName)); //保存
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

    }
}