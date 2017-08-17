using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  模块配置信息

    /// <summary>
    /// 模块配置信息
    /// </summary>
    public class ModuleInfo
    {

        #region  字段属性

        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 模块描述
        /// </summary>
        public string ModuleDesc { get; set; }

        /// <summary>
        /// 模块层级
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
        public ModuleInfo()
        {
            this.ModuleId = 0;
            this.ModuleName = string.Empty;
            this.ModuleCode = string.Empty;
            this.ModuleDesc = string.Empty;
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