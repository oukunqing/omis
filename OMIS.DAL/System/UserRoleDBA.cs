using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class UserRoleDBA : DataAccess
    {

        #region  获得单个用户-角色
        public DataResult GetUserRole(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_user_role` d ");
                sql.Append(String.Format(" where d.`id` = {0} ", id));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个用户-角色
        public DataResult GetUserRole(string idList)
        {
            try
            {
                if (!CheckIdList(ref idList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_user_role` d ");
                sql.Append(String.Format(" where d.`id` in({0}) ", idList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得用户-角色
        public DataResult GetUserRole(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_user_role` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增用户-角色
        public DataResult AddUserRole(UserRoleInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_user_role`(");
                sql.Append("`user_id`,`role_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?UserId,?RoleId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?UserId", "?RoleId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.UserId, o.RoleId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_user_role", "id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新用户-角色
        public DataResult UpdateUserRole(UserRoleInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_user_role` set ");
                sql.Append("`user_id` = ?UserId,`role_id` = ?RoleId");
                sql.Append(" where `id` = ?Id;");

                List<string> name = new List<string>() {
					"?UserId", "?RoleId", "?Id"
				};

                List<object> value = new List<object>() {
					o.UserId, o.RoleId, o.Id
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除用户-角色
        public DataResult DeleteUserRole(int id)
        {
            try
            {
                string sql = String.Format("delete from `sys_user_role` where `id` = {0};", id);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public UserRoleInfo FillUserRoleInfo(DataRow dr)
        {
            try
            {
                UserRoleInfo o = new UserRoleInfo();

                o.Id = DataConvert.ConvertValue(dr["id"], 0);
                o.UserId = DataConvert.ConvertValue(dr["user_id"], 0);
                o.RoleId = DataConvert.ConvertValue(dr["role_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public UserRoleInfo FillUserRoleInfo(DataRowView drv)
        {
            try
            {
                return this.FillUserRoleInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}