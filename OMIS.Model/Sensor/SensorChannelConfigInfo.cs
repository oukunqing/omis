using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器通道配置信息

    /// <summary>
    /// 传感器通道配置信息
    /// </summary>
    public class SensorChannelConfigInfo
    {

        #region  字段属性

        /// <summary>
        /// 配置ID
        /// </summary>
        public int ConfigId { get; set; }

        /// <summary>
        /// 传感器ID
        /// </summary>
        public int SensorId { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public string ConfigInfo { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// CRC校验码
        /// </summary>
        public string CrcCode { get; set; }

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public SensorChannelConfigInfo()
        {
            this.ConfigId = 0;
            this.SensorId = 0;
            this.ConfigInfo = string.Empty;
            this.Enabled = 0;
            this.CreateTime = string.Empty;
            this.UpdateTime = string.Empty;
            this.CrcCode = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}