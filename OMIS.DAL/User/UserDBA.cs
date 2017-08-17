using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;
using OMIS.DBUtility;

namespace OMIS.DAL.User
{
    public class UserDBA : DataAccess
    {

        public UserDBA()
        {
            
        }


        #region  获得用户信息
        public DataResult GetUserInfo(string userName)
        {
            try
            {
                string sql = " select u.* from `sys_user` u where `user_name` = ?UserName limit 1;";
                return new DataResult(sql).Set(Select(DBConnString, sql, BuildSqlParam("?UserName", userName)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region 删除用户
        public DataResult DeleteUser(int userId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(String.Format("delete from `sys_user` where `user_id` = {0};", userId));

                return new DataResult(sql.ToString(), Delete(DBConnString, sql.ToString()));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
