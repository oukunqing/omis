using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Common
{

    /// <summary>
    /// CommonJsonModel 的摘要说明
    /// </summary>
    public class JsonModel : JsonModelAnalyzer
    {
        private string rawjson;
        private bool isValue = false;
        private bool isModel = false;
        private bool isCollection = false;

        //internal JsonModel(string rawjson)
        //{
        //    this.rawjson = rawjson;
        //    if (string.IsNullOrEmpty(rawjson))
        //        throw new Exception("missing rawjson");
        //    rawjson = rawjson.Trim();
        //    if (rawjson.StartsWith("{"))
        //    {
        //        isModel = true;
        //    }
        //    else if (rawjson.StartsWith("["))
        //    {
        //        isCollection = true;
        //    }
        //    else
        //    {
        //        isValue = true;
        //    }
        //}

        public JsonModel(string rawjson)
        {
            this.rawjson = rawjson;
            if (string.IsNullOrEmpty(rawjson))
                throw new Exception("missing rawjson");
            rawjson = rawjson.Trim();
            if (rawjson.StartsWith("{"))
            {
                isModel = true;
            }
            else if (rawjson.StartsWith("["))
            {
                isCollection = true;
            }
            else
            {
                isValue = true;
            }
        }

        /// <summary>
        /// JSON数据
        /// </summary>
        public string Rawjson
        {
            get { return rawjson; }
        }

        public bool IsValue()
        {
            return isValue;
        }
        public bool IsValue(string key)
        {
            if (!isModel)
                return false;
            if (string.IsNullOrEmpty(key))
                return false;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                JsonModel model = new JsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    JsonModel submodel = new JsonModel(model.Value);
                    return submodel.IsValue();
                }
            }
            return false;
        }
        public bool IsModel()
        {
            return isModel;
        }
        public bool IsModel(string key)
        {
            if (!isModel)
                return false;
            if (string.IsNullOrEmpty(key))
                return false;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                JsonModel model = new JsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    JsonModel submodel = new JsonModel(model.Value);
                    return submodel.IsModel();
                }
            }
            return false;
        }
        public bool IsCollection()
        {
            return isCollection;
        }
        public bool IsCollection(string key)
        {
            if (!isModel)
                return false;
            if (string.IsNullOrEmpty(key))
                return false;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                JsonModel model = new JsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    JsonModel submodel = new JsonModel(model.Value);
                    return submodel.IsCollection();
                }
            }
            return false;
        }

        /// <summary>
        /// 当模型是对象，返回拥有的key
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeys()
        {
            if (!isModel)
                return null;
            List<string> list = new List<string>();
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                string key = new JsonModel(subjson).Key;
                if (!string.IsNullOrEmpty(key))
                    list.Add(key);
            }
            return list;
        }
        /// <summary>
        /// 当模型是对象，key对应是值，则返回key对应的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (!isModel)
                return null;
            if (string.IsNullOrEmpty(key))
                return null;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                JsonModel model = new JsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                    return model.Value;
            }
            return null;
        }
        /// <summary>
        /// 模型是对象，key对应是对象，返回key对应的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonModel GetModel(string key)
        {
            if (!isModel)
                return null;
            if (string.IsNullOrEmpty(key))
                return null;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                JsonModel model = new JsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    JsonModel submodel = new JsonModel(model.Value);
                    if (!submodel.IsModel())
                        return null;
                    else
                        return submodel;
                }
            }
            return null;
        }
        /// <summary>
        /// 模型是对象，key对应是集合，返回集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonModel GetCollection(string key)
        {
            if (!isModel)
                return null;
            if (string.IsNullOrEmpty(key))
                return null;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                JsonModel model = new JsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    JsonModel submodel = new JsonModel(model.Value);
                    if (!submodel.IsCollection())
                        return null;
                    else
                        return submodel;
                }
            }
            return null;
        }
        /// <summary>
        /// 模型是集合，返回自身
        /// </summary>
        /// <returns></returns>
        public List<JsonModel> GetCollection()
        {
            List<JsonModel> list = new List<JsonModel>();
            if (IsValue())
                return list;
            foreach (string subjson in base._GetCollection(rawjson))
            {
                list.Add(new JsonModel(subjson));
            }
            return list;
        }


        /// <summary>
        /// 当模型是值对象，返回key
        /// </summary>
        private string Key
        {
            get
            {
                if (IsValue())
                    return base._GetKey(rawjson);
                return null;
            }
        }
        /// <summary>
        /// 当模型是值对象，返回value
        /// </summary>
        private string Value
        {
            get
            {
                if (!IsValue())
                    return null;
                return base._GetValue(rawjson);
            }
        }

        /// <summary>
        /// 替换内容
        /// 清除单引号
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string ReplaceValue(string strValue)
        {
            string pattern = "^'";
            Regex reg = new Regex(pattern);
            if (reg.IsMatch(strValue))
            {
                return strValue.Substring(1, strValue.Length - 2);
            }
            return strValue;
        }

    }
}
