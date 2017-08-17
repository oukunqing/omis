using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.Content
{
    public class CategoryInfo
    {

        #region  字段属性

        /// <summary>
        /// 类别ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 类别编码
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 类别简介
        /// </summary>
        public string CategoryDesc { get; set; }

        /// <summary>
        /// 是否需要自定义参数
        /// </summary>
        public int CustomParam { get; set; }

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
        
        #region  构造函数
        public CategoryInfo()
        {
            this.CategoryId = 0;
            this.CategoryName = string.Empty;
            this.CategoryCode = string.Empty;
            this.CategoryDesc = string.Empty;
            this.CustomParam = 0;
            this.Enabled = 0;
            this.SortOrder = 0;
            this.OperatorId = 0;
            this.CreateTime = string.Empty;
            this.UpdateTime = string.Empty;
        }
        #endregion

    }
}