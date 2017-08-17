using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Sensor
{

    #region  传感器分类信息

    /// <summary>
    /// 传感器分类信息
    /// </summary>
    public class SensorTypeInfo
    {

        #region  字段属性

        /// <summary>
        /// 分类ID
        /// </summary>
        public int SensorTypeId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 功能说明
        /// </summary>
        public string TypeFunc { get; set; }

        /// <summary>
        /// 分类描述
        /// </summary>
        public string TypeDesc { get; set; }

        /// <summary>
        /// 数据单位
        /// </summary>
        public string DataUnit { get; set; }

        /// <summary>
        /// 分类层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 上级分类ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 上级分类目录树
        /// </summary>
        public string ParentTree { get; set; }

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
        public SensorTypeInfo()
        {
            this.SensorTypeId = 0;
            this.TypeName = string.Empty;
            this.TypeCode = string.Empty;
            this.TypeFunc = string.Empty;
            this.TypeDesc = string.Empty;
            this.DataUnit = string.Empty;
            this.Level = 0;
            this.ParentId = 0;
            this.ParentTree = string.Empty;
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