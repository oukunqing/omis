﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  导航菜单分类信息

    /// <summary>
    /// 导航菜单分类信息
    /// </summary>
    public class MenuTypeInfo
    {

        #region  字段属性

        /// <summary>
        /// 分类ID
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

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public MenuTypeInfo()
        {
            this.TypeId = 0;
            this.TypeName = string.Empty;
            this.TypeCode = string.Empty;
            this.Enabled = 0;
            this.SortOrder = 0;
            this.OperatorId = 0;
            this.CreateTime = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}