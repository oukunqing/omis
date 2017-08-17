using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器通道信息

    /// <summary>
    /// 传感器通道信息
    /// </summary>
    public class SensorChannelInfo
    {

        #region  字段属性

        /// <summary>
        /// 通道ID
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        public int ChannelNo { get; set; }

        /// <summary>
        /// 通道分类
        /// </summary>
        public int ChannelType { get; set; }

        /// <summary>
        /// 通道组
        /// </summary>
        public int ChannelGroup { get; set; }

        /// <summary>
        /// 通道类型ID
        /// </summary>
        public int ModeId { get; set; }

        /// <summary>
        /// 原始值类型ID
        /// </summary>
        public int OriTypeId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
        public SensorChannelInfo()
        {
            this.ChannelId = 0;
            this.ChannelNo = 0;
            this.ChannelType = 0;
            this.ChannelGroup = 0;
            this.ModeId = 0;
            this.OriTypeId = 0;
            this.Remark = string.Empty;
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