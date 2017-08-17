using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  操作权限信息

    /// <summary>
    /// 操作权限信息
    /// </summary>
    public class PermissionInfo
    {

        #region  字段属性

        /// <summary>
        /// 权限ID
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// 权限分类ID
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string PermissionCode { get; set; }

        /// <summary>
        /// 权限描述
        /// </summary>
        public string PermissionDesc { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string PermissionPrompt { get; set; }

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
        public PermissionInfo()
        {
            this.PermissionId = 0;
            this.TypeId = 0;
            this.PermissionName = string.Empty;
            this.PermissionCode = string.Empty;
            this.PermissionDesc = string.Empty;
            this.PermissionPrompt = string.Empty;
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