using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  角色-模块-权限信息

    /// <summary>
    /// 角色-模块-权限信息
    /// </summary>
    public class RoleModulePermissionInfo
    {

        #region  字段属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public int PermissionId { get; set; }

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
        public RoleModulePermissionInfo()
        {
            this.Id = 0;
            this.RoleId = 0;
            this.ModuleId = 0;
            this.PermissionId = 0;
            this.OperatorId = 0;
            this.CreateTime = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}