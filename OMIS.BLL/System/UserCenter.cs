using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMIS.Common;
using OMIS.Model.System;

namespace OMIS.BLL.System
{

    public enum RoleGroupEnum
    {
        Founder = 0,
        Admin = 1,
        Manage = 2,
        App = 3,
        Test = 4,
        Guest = 5,
    }

    public class UserCenter
    {

        #region  属性

        /// <summary>
        /// 登录用户ID
        /// </summary>
        public int LoginUserId { get; set; }

        /// <summary>
        /// 是否已登录
        /// </summary>
        public bool IsLogin { get; set; }

        /// <summary>
        /// 登录用户角色组别ID（取最高组别）
        /// </summary>
        public int RoleGroupId { get; set; }

        /// <summary>
        /// 是否为来宾帐户
        /// </summary>
        public bool IsGuest { get; set; }

        public RoleGroupEnum RoleGroup { get; set; }

        #endregion

        public UserCenter()
        {
            UserInfo o = this.GetLoginUserInfo();
            this.LoginUserId = o.UserId;
        }

        public UserCenter(string dbConnString)
        {

        }

        #region  获得当前登录用户信息
        public UserInfo GetLoginUserInfo()
        {
            try
            {
                UserInfo o = new UserInfo();

                string userName = "";

                return CheckUserInfo(o, userName) ? o : new UserInfo();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  检测用户是否正确
        public bool CheckUserInfo(UserInfo o, string userName)
        {
            return o != null && !userName.Trim().Equals(string.Empty) && o.UserName.Equals(userName);
        }

        public bool CheckUserInfo(UserInfo o, string userName, string userPwd)
        {
            if (userName.Trim().Equals(string.Empty) || userPwd.Trim().Equals(string.Empty))
            {
                return false;
            }
            return o != null && o.UserName.Equals(userName) && o.UserPwd.Equals(BuildUserPwd(o, ref userPwd));
        }
        #endregion

        #region  生成用户密码
        public string BuildUserPwd(UserInfo o, ref string userPwd)
        {
            if (userPwd.Length != 32)
            {
                userPwd = Encrypt.MD5Encrypt(userPwd).ToLower();
            }
            userPwd = Encrypt.MD5Encrypt(String.Format("{0}{1}{2}", userPwd, o.PwdSalt, o.UserName)).ToLower();

            return userPwd;
        }
        #endregion

        #region  比较当前用户与数据用户的权限等级

        #endregion


    }
}