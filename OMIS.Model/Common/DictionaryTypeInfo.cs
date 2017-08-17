using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Common
{

    #region  字典分类信息

    /// <summary>
    /// 字典分类信息
    /// </summary>
    public class DictionaryTypeInfo
    {

        #region  字段属性

        /// <summary>
        /// 
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 分类层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 父级目录树
        /// </summary>
        public string ParentTree { get; set; }

        /// <summary>
        /// 分类字典当前最大编号
        /// </summary>
        public int MaxNumber { get; set; }

        /// <summary>
        /// 是否允许多选
        /// </summary>
        public int MultiSelect { get; set; }

        /// <summary>
        /// 多选数量限制
        /// </summary>
        public int MultiSelectLimit { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
        public DictionaryTypeInfo()
        {
            this.TypeId = 0;
            this.TypeName = string.Empty;
            this.TypeCode = string.Empty;
            this.Level = 0;
            this.ParentId = 0;
            this.ParentTree = string.Empty;
            this.MaxNumber = 0;
            this.MultiSelect = 0;
            this.MultiSelectLimit = 0;
            this.Enabled = 0;

            this.Remark = string.Empty;
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