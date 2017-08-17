using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Common
{

    #region  分类字典信息

    /// <summary>
    /// 分类字典信息
    /// </summary>
    public class DictionaryInfo
    {

        #region  字段属性

        /// <summary>
        /// 字典ID
        /// </summary>
        public int DictionaryId { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 字典编号
        /// </summary>
        public int DictionaryNumber { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictionaryName { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        public string DictionaryCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 排序编号
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
        public DictionaryInfo()
        {
            this.DictionaryId = 0;
            this.TypeId = 0;
            this.DictionaryNumber = 0;
            this.DictionaryName = string.Empty;
            this.DictionaryCode = string.Empty;
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