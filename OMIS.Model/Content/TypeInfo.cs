using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Content
{
    public class TypeInfo
    {

        #region  字段属性

        /// <summary>
        /// 分类ID
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 分类编号
        /// </summary>
        public int TypeNumber { get; set; }

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
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        #endregion

        #region  构造函数
        public TypeInfo()
        {
            this.TypeId = 0;
            this.CategoryId = 0;
            this.TypeName = string.Empty;
            this.TypeCode = string.Empty;
            this.TypeNumber = 0;
            this.Level = 0;
            this.ParentId = 0;
            this.ParentTree = string.Empty;
            this.Enabled = 0;
            this.SortOrder = 0;

            this.CreateTime = string.Empty;
            this.UpdateTime = string.Empty;
        }
        #endregion

    }
}