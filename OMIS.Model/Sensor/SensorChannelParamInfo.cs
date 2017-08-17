using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器通道-参数配置信息

    /// <summary>
    /// 传感器通道-参数配置信息
    /// </summary>
    public class SensorChannelParamInfo
    {

        #region  字段属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 参数ID
        /// </summary>
        public int ParamId { get; set; }

        /// <summary>
        /// 通道编号
        /// </summary>
        public int ChannelNo { get; set; }

        /// <summary>
        /// 通道组别
        /// </summary>
        public int ChannelGroup { get; set; }

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public SensorChannelParamInfo()
        {
            this.Id = 0;
            this.ParamId = 0;
            this.ChannelNo = 0;
            this.ChannelGroup = 0;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}