using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器参数信息

    /// <summary>
    /// 传感器参数信息
    /// </summary>
    public class SensorParamInfo
    {

        #region  字段属性

        /// <summary>
        /// 参数ID
        /// </summary>
        public int ParamId { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

        /// <summary>
        /// 参数功能
        /// </summary>
        public string ParamFunc { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public string ParamDesc { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public int ParamType { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public int ParamMode { get; set; }

        /// <summary>
        /// 配置时是否显示
        /// </summary>
        public int ConfigShow { get; set; }

        /// <summary>
        /// 参数值类型
        /// </summary>
        public int ValueType { get; set; }

        /// <summary>
        /// 字符长度
        /// </summary>
        public int CharLength { get; set; }

        /// <summary>
        /// 参数值选项
        /// </summary>
        public string ValueOption { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public int Required { get; set; }

        /// <summary>
        /// 值示例
        /// </summary>
        public string ValueSample { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 排序次序
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public SensorParamInfo()
        {
            this.ParamId = 0;
            this.ParamName = string.Empty;
            this.ParamCode = string.Empty;
            this.ParamFunc = string.Empty;
            this.ParamDesc = string.Empty;
            this.ParamType = 0;
            this.ParamMode = 0;
            this.ConfigShow = 0;
            this.ValueType = 0;
            this.CharLength = 0;

            this.ValueOption = string.Empty;
            this.DefaultValue = string.Empty;
            this.Required = 0;
            this.ValueSample = string.Empty;
            this.Enabled = 0;
            this.SortOrder = 0;
            this.OperatorId = 0;
            this.CreateTime = string.Empty;
            this.UpdateTime = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}
