using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  网络线路信息

    /// <summary>
    /// 网络线路信息
    /// </summary>
    public class NetworkLineInfo
    {

        #region  字段属性

        /// <summary>
        /// 线路ID
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 线路编码
        /// </summary>
        public string LineCode { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 线路描述
        /// </summary>
        public string LineDesc { get; set; }

        /// <summary>
        /// 默认IP(或域名)
        /// </summary>
        public string DefaultIp { get; set; }

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
        public NetworkLineInfo()
        {
            this.LineId = 0;
            this.LineName = string.Empty;
            this.LineCode = string.Empty;
            this.LineNumber = 0;
            this.LineDesc = string.Empty;
            this.DefaultIp = string.Empty;
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