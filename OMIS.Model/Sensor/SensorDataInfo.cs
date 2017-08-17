using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器数据信息

    /// <summary>
    /// 传感器数据信息
    /// </summary>
    public class SensorDataInfo
    {

        #region  字段属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        public int ChannelNo { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 调试模式
        /// </summary>
        public int DebugMode { get; set; }

        /// <summary>
        /// 数据编码
        /// </summary>
        public string SensorCode { get; set; }

        /// <summary>
        /// 传感器数据
        /// </summary>
        public decimal SensorValue { get; set; }

        /// <summary>
        /// 原始值
        /// </summary>
        public decimal OriginalValue { get; set; }

        /// <summary>
        /// 数据采集时间
        /// </summary>
        public string CollectTime { get; set; }

        /// <summary>
        /// 数据上传时间
        /// </summary>
        public string UploadTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 计算方式
        /// </summary>
        public int Flag { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public int MsgId { get; set; }

        /// <summary>
        /// 表ID
        /// </summary>
        public int TableId { get; set; }

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public SensorDataInfo()
        {
            this.Id = 0;
            this.DeviceCode = string.Empty;
            this.ChannelNo = 0;
            this.DataType = 0;
            this.DebugMode = 0;
            this.SensorCode = string.Empty;
            this.SensorValue = 0.0m;
            this.OriginalValue = 0.0m;
            this.CollectTime = string.Empty;
            this.UploadTime = string.Empty;

            this.CreateTime = string.Empty;
            this.Flag = 0;
            this.MsgId = 0;
            this.TableId = 0;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion


}
