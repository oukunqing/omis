using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器通道类型信息

    /// <summary>
    /// 传感器通道类型信息
    /// </summary>
    public class SensorChannelModeInfo
    {

        #region  字段属性

        /// <summary>
        /// 类型ID
        /// </summary>
        public int ModeId { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string ModeName { get; set; }

        /// <summary>
        /// 类型编码
        /// </summary>
        public string ModeCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 排序次序
        /// </summary>
        public int SortOrder { get; set; }

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public SensorChannelModeInfo()
        {
            this.ModeId = 0;
            this.ModeName = string.Empty;
            this.ModeCode = string.Empty;
            this.Enabled = 0;
            this.SortOrder = 0;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}
