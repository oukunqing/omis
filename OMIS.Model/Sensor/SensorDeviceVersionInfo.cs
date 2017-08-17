using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器设备版本信息

    /// <summary>
    /// 传感器设备版本信息
    /// </summary>
    public class SensorDeviceVersionInfo
    {

        #region  字段属性

        /// <summary>
        /// 版本ID
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// 版本编码
        /// </summary>
        public string VersionCode { get; set; }

        /// <summary>
        /// 版本配置
        /// </summary>
        public string VersionConfig { get; set; }

        /// <summary>
        /// 版本描述
        /// </summary>
        public string VersionDesc { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 排序次序
        /// </summary>
        public int SortOrder { get; set; }

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
        public SensorDeviceVersionInfo()
        {
            this.VersionId = 0;
            this.VersionCode = string.Empty;
            this.VersionConfig = string.Empty;
            this.VersionDesc = string.Empty;
            this.Enabled = 0;
            this.SortOrder = 0;
            this.CreateTime = string.Empty;
            this.UpdateTime = string.Empty;
            this.CrcCode = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}
