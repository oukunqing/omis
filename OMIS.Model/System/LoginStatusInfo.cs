using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    /// <summary>
    /// 登录状态信息
    /// </summary>
    public class LoginStatusInfo
    {

        /// <summary>
        /// 登录状态：true-成功,false-失败
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        public LoginStatusInfo()
        {
            this.Status = false;
        }

    }
}