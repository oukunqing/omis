using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model.System
{

    #region  用户信息

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {

        #region  字段属性

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// 密码后缀
        /// </summary>
        public string PwdSalt { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int UserStatus { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public string ExpireTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTimes { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public string LastLoginTime { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 登录失败次数
        /// </summary>
        public int LastFailedTimes { get; set; }

        /// <summary>
        /// 登录失败最后记录时间
        /// </summary>
        public string LoginFailedTime { get; set; }

        /// <summary>
        /// 是否因登录失败次数超限被锁定
        /// </summary>
        public int LoginLocked { get; set; }

        /// <summary>
        /// 登录失败超限锁定的时间
        /// </summary>
        public string LoginLockedTime { get; set; }

        /// <summary>
        /// 最后修改密码时间
        /// </summary>
        public string UpdatePwdTime { get; set; }

        /// <summary>
        /// 安全问题
        /// </summary>
        public string SafetyQuestion { get; set; }

        /// <summary>
        /// 安全问题答案
        /// </summary>
        public string SafetyAnswer { get; set; }

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
        public UserInfo()
        {
            this.UserId = 0;
            this.UserName = string.Empty;
            this.UserPwd = string.Empty;
            this.PwdSalt = string.Empty;
            this.UserStatus = 0;
            this.ExpireTime = string.Empty;
            this.LoginTimes = 0;
            this.LastLoginTime = string.Empty;
            this.LastLoginIp = string.Empty;
            this.LastFailedTimes = 0;

            this.LoginFailedTime = string.Empty;
            this.LoginLocked = 0;
            this.LoginLockedTime = string.Empty;
            this.UpdatePwdTime = string.Empty;
            this.SafetyQuestion = string.Empty;
            this.SafetyAnswer = string.Empty;
            this.RealName = string.Empty;
            this.Telephone = string.Empty;
            this.Mobile = string.Empty;
            this.Email = string.Empty;

            this.Qq = string.Empty;
            this.OperatorId = 0;
            this.CreateTime = string.Empty;
            this.UpdateTime = string.Empty;

            this.Extend = new Dictionary<string, object>();
        }
        #endregion

    }

    #endregion

}