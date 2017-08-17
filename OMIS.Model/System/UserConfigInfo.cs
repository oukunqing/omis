using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    /// <summary>
    /// 用户配置
    /// </summary>
    public class UserConfigInfo
    {

        #region  字段属性

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string Qq { get; set; }

        #endregion

        #region  构造函数
        public UserConfigInfo()
        {
            this.UserId = 0;
            this.RealName = string.Empty;
            this.Telephone = string.Empty;
            this.Mobile = string.Empty;
            this.Email = string.Empty;
            this.Qq = string.Empty;
        }
        #endregion

    }
}