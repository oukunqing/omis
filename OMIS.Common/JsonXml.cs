using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Web;
using System.Web.Script.Serialization;

namespace OMIS.Common
{
    public class JsonXml
    {
        private static JavaScriptSerializer js = new JavaScriptSerializer();


        #region
        private static string BuildItemStart(string key)
        {
            return String.Format("<{0}>", key);
        }
        private static string BuildItemEnd(string key)
        {
            return String.Format("</{0}>", key);
        }

        private static string BuildItem(string key, string value)
        {
            return value.Length == 0 ? String.Format("<{0}/>", key) : String.Format("<{0}>{1}</{0}>", key, HttpUtility.HtmlEncode(value));
        }

        private static object GetValue(Dictionary<string, object> dic, string key, object v)
        {
            return dic.ContainsKey(key) ? dic[key] : v;
        }

        private static Dictionary<string, object> _Deserialize(string json)
        {
            return js.Deserialize<Dictionary<string, object>>(json);
        }

        private static string _Serialize(Dictionary<string, object> dic)
        {
            return js.Serialize(dic);
        }

        private static Dictionary<string, object> _BuildDic()
        {
            return new Dictionary<string, object>();
        }

        private static Dictionary<string, object> _BuildDic(string key)
        {
            return new Dictionary<string, object>() { { key, "" } };
        }


        private static string _CheckXml(string xml)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);

                return xml;
            }
            catch (Exception ex)
            {
                return String.Format("<{0}>{1}</{0}>", "Root", xml);
            }
        }

        private static string _FilterXml(string xml)
        {
            Regex reg = new Regex(@"<!-[\s\S]*?-->");

            return reg.Replace(xml, "");
        }
        #endregion


        #region  JsonToXml
        public static string JsonToXml(string json)
        {
            return SerializeToXml(_Deserialize(json));
        }

        public static string JsonToXml(string json, string rootKey)
        {
            return SerializeToXml(_Deserialize(json), rootKey);
        }

        public static string JsonToXml(string json, string rootKey, bool addVersion)
        {
            return SerializeToXml(_Deserialize(json), rootKey, addVersion);
        }
        #endregion

        #region  SerializeToXml
        public static string SerializeToXml(Dictionary<string, object> dic)
        {
            return SerializeToXml(dic, string.Empty);
        }

        public static string SerializeToXml(Dictionary<string, object> dic, string rootKey)
        {
            return SerializeToXml(dic, rootKey, false);
        }

        public static string SerializeToXml(Dictionary<string, object> dic, string rootKey, bool addVersion)
        {
            bool hasRoot = !rootKey.Equals(string.Empty);

            StringBuilder xml = new StringBuilder();
            xml.Append(addVersion ? "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" : "");

            xml.Append(hasRoot ? BuildItemStart(rootKey) : string.Empty);

            _SerializeToXml(dic, xml);

            xml.Append(hasRoot ? BuildItemEnd(rootKey) : string.Empty);

            return hasRoot ? xml.ToString() : _CheckXml(xml.ToString());
        }

        private static string _SerializeToXml(Dictionary<string, object> dic, StringBuilder xml)
        {
            foreach (KeyValuePair<string, object> kv in dic)
            {
                if (kv.Value != null)
                {
                    Type t = kv.Value.GetType();
                    if (t.Name.IndexOf("Dictionary") >= 0)
                    {
                        xml.Append(BuildItemStart(kv.Key));
                        _SerializeToXml((Dictionary<string, object>)kv.Value, xml);
                        xml.Append(BuildItemEnd(kv.Key));
                    }
                    //数组的处理方式
                    else if (t.Name.IndexOf("ArrayList") >= 0)
                    {
                        xml.Append(BuildItemStart(kv.Key));
                        foreach (object o in (ArrayList)kv.Value)
                        {
                            foreach (KeyValuePair<string, object> dv in (Dictionary<string, object>)o)
                            {
                                xml.Append(BuildItem(dv.Key, dv.Value.ToString()));
                            }
                        }
                        xml.Append(BuildItemEnd(kv.Key));
                    }
                    else
                    {
                        xml.Append(BuildItem(kv.Key, kv.Value.ToString()));
                    }
                }
            }
            return xml.ToString();
        }
        #endregion

        #region XmlToJson
        public static string XmlToJson(string xml)
        {
            try
            {
                return _Serialize(ParseXml(xml));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  ParseXml
        public static Dictionary<string, object> ParseXml(string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_FilterXml(xml.Trim()));

                XmlNode root = (XmlNode)doc.DocumentElement;

                return (root is XmlElement) ? _ParseXml(root.ChildNodes, root.Name, _BuildDic(root.Name)) : _BuildDic();
            }
            catch (Exception ex) { throw (ex); }
        }

        private static Dictionary<string, object> _ParseXml(XmlNodeList list, string rootName, Dictionary<string, object> dic)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();

            List<Dictionary<string, object>> ld = new List<Dictionary<string, object>>();

            foreach (XmlNode n in list)
            {
                if (n.HasChildNodes && n.FirstChild.NodeType == XmlNodeType.Element)
                {
                    _ParseXml(n.ChildNodes, n.Name, d);
                }
                else
                {
                    /*
                     * 判断字典中是否已有关键字KEY，若有，则表示为数组，以数组方式解析
                     */
                    if (d.ContainsKey(n.Name))
                    {
                        //将数组第一个元素，添加到结果数组中
                        if (ld.Count == 0)
                        {
                            ld.Add(d);
                        }
                        ld.Add(new Dictionary<string, object>() { { n.Name, n.InnerText } });
                    }
                    else
                    {
                        d[n.Name] = n.InnerText;
                    }
                }
            }

            if (ld.Count > 0)
            {
                dic[rootName] = ld;
            }
            else
            {
                dic[rootName] = d;
            }

            return dic;
        }


        private static string _ParseAttribute(XmlNode xn)
        {
            StringBuilder xas = new StringBuilder();
            XmlAttributeCollection xac = xn.Attributes;
            foreach (XmlAttribute a in xac)
            {
                xas.Append(String.Format(" {0}=\"{1}\"", a.Name, HttpUtility.HtmlEncode(a.Value)));
            }
            return xas.ToString();
        }

        #endregion

        #region  XmlFormat
        public static string XmlFormat(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_FilterXml(xml.Trim()));

            XmlNode root = (XmlNode)doc.DocumentElement;
            if (root is XmlElement)
            {
                StringBuilder _xml = new StringBuilder();
                _xml.Append(String.Format("<{0}{1}>\r\n", root.Name, _ParseAttribute(root)));
                _XmlFormat(root.ChildNodes, _xml, 1);
                _xml.Append(String.Format("</{0}>\r\n", root.Name));
                return _xml.ToString();
            }
            return xml;
        }

        private static string _XmlFormat(XmlNodeList list, StringBuilder xml, int level)
        {
            string sep = "".PadLeft(level, '\t');
            foreach (XmlNode n in list)
            {
                if (n.HasChildNodes && n.FirstChild.NodeType == XmlNodeType.Element)
                {
                    xml.Append(String.Format("{0}<{1}{2}>\r\n", sep, n.Name, _ParseAttribute(n)));
                    _XmlFormat(n.ChildNodes, xml, ++level);
                    xml.Append(String.Format("{0}</{1}>\r\n", sep, n.Name));
                }
                else
                {
                    xml.Append(String.Format("{0}<{1}{2}>{3}</{1}>\r\n", sep, n.Name, _ParseAttribute(n), n.InnerText));
                }
            }
            return xml.ToString();
        }
        #endregion


        #region  CheckIsXml
        public static bool CheckIsXml(string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                return true;
            }
            catch (XmlException xex) { return false; }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }

}