using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class UserDBA : DataAccess
    {

        #region  获得单个用户
        public DataResult GetUser(int userId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_user` d ");
                sql.Append(String.Format(" where d.`user_id` = {0} ", userId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个用户
        public DataResult GetUser(string userIdList)
        {
            try
            {
                if (!CheckIdList(ref userIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_user` d ");
                sql.Append(String.Format(" where d.`user_id` in({0}) ", userIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得用户
        public DataResult GetUser(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_user` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增用户
        public DataResult AddUser(UserInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_user`(");
                sql.Append("`user_name`,`user_pwd`,`pwd_salt`,`user_status`,`expire_time`,`safety_question`,`safety_answer`,`real_name`,`telephone`,`mobile`");
                sql.Append(",`email`,`qq`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?UserName,?UserPwd,?PwdSalt,?UserStatus,?ExpireTime,?SafetyQuestion,?SafetyAnswer,?RealName,?Telephone,?Mobile");
                sql.Append(",?Email,?Qq,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?UserName", "?UserPwd", "?PwdSalt", "?UserStatus", "?ExpireTime", "?SafetyQuestion", "?SafetyAnswer", "?RealName", "?Telephone", "?Mobile", 
					"?Email", "?Qq", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.UserName, o.UserPwd, o.PwdSalt, o.UserStatus, o.ExpireTime, o.SafetyQuestion, o.SafetyAnswer, o.RealName, o.Telephone, o.Mobile, 
					o.Email, o.Qq, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_user", "user_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新用户
        public DataResult UpdateUser(UserInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_user` set ");
                sql.Append("`user_name` = ?UserName,`user_status` = ?UserStatus,`expire_time` = ?ExpireTime,`real_name` = ?RealName,`telephone` = ?Telephone");
                sql.Append(",`mobile` = ?Mobile,`email` = ?Email,`qq` = ?Qq,`update_time` = ?UpdateTime");
                sql.Append(" where `user_id` = ?UserId;");

                List<string> name = new List<string>() {
					"?UserName", "?UserStatus", "?ExpireTime", "?RealName", "?Telephone", "?Mobile", "?Email", "?Qq", "?UpdateTime", "?UserId"
				};

                List<object> value = new List<object>() {
					o.UserName, o.UserStatus, o.ExpireTime, o.RealName, o.Telephone, o.Mobile, o.Email, o.Qq, CheckDateTime(o.UpdateTime), o.UserId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除用户
        public DataResult DeleteUser(int userId)
        {
            try
            {
                string sql = String.Format("delete from `sys_user` where `user_id` = {0};", userId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public UserInfo FillUserInfo(DataRow dr)
        {
            try
            {
                UserInfo o = new UserInfo();

                o.UserId = DataConvert.ConvertValue(dr["user_id"], 0);
                o.UserName = dr["user_name"].ToString();
                o.UserPwd = dr["user_pwd"].ToString();
                o.PwdSalt = dr["pwd_salt"].ToString();
                o.UserStatus = DataConvert.ConvertValue(dr["user_status"], 0);
                o.ExpireTime = dr["expire_time"].ToString();
                o.LoginTimes = DataConvert.ConvertValue(dr["login_times"], 0);
                o.LastLoginTime = dr["last_login_time"].ToString();
                o.LastLoginIp = dr["last_login_ip"].ToString();
                o.LastFailedTimes = DataConvert.ConvertValue(dr["last_failed_times"], 0);

                o.LoginFailedTime = dr["login_failed_time"].ToString();
                o.LoginLocked = DataConvert.ConvertValue(dr["login_locked"], 0);
                o.LoginLockedTime = dr["login_locked_time"].ToString();
                o.UpdatePwdTime = dr["update_pwd_time"].ToString();
                o.SafetyQuestion = dr["safety_question"].ToString();
                o.SafetyAnswer = dr["safety_answer"].ToString();
                o.RealName = dr["real_name"].ToString();
                o.Telephone = dr["telephone"].ToString();
                o.Mobile = dr["mobile"].ToString();
                o.Email = dr["email"].ToString();

                o.Qq = dr["qq"].ToString();
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public UserInfo FillUserInfo(DataRowView drv)
        {
            try
            {
                return this.FillUserInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}