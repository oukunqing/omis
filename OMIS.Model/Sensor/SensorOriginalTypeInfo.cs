using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器通道原始值类型信息

    /// <summary>
    /// 传感器通道原始值类型信息
    /// </summary>
    public class SensorOriginalTypeInfo
    {

        #region  字段属性

        /// <summary>
        /// 原始值类型ID
        /// </summary>
        public int OriTypeId { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string OriTypeName { get; set; }

        /// <summary>
        /// 类型编码
        /// </summary>
        public string OriTypeCode { get; set; }

        /// <summary>
        /// 默认单位
        /// </summary>
        public string DefaultUnit { get; set; }

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
        public SensorOriginalTypeInfo()
        {
            this.OriTypeId = 0;
            this.OriTypeName = string.Empty;
            this.OriTypeCode = string.Empty;
            this.DefaultUnit = string.Empty;
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
