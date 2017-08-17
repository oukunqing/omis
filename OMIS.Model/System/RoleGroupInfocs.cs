using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  角色组别信息

    /// <summary>
    /// 角色组别信息
    /// </summary>
    public class RoleGroupInfo
    {

        #region  字段属性

        /// <summary>
        /// 组别ID
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 组别名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 组别编码
        /// </summary>
        public string GroupCode { get; set; }

        #endregion

        #region  扩展属性

        public Dictionary<string, object> Extend { get; set; }

        #endregion

        #region  构造函数
        public RoleGroupInfo()
        {
            this.GroupId = 0;
            this.GroupName = string.Empty;
            this.GroupCode = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}
