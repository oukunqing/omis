using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.User
{
    public class RoleDBA : DataAccess
    {



        #region 新增用户角色
        public DataResult AddRole(RoleInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_role`(");
                sql.Append("`role_name`,`role_code`,`role_desc`,`role_level`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?RoleName,?RoleCode,?RoleDesc,?RoleLevel,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?RoleName", "?RoleCode", "?RoleDesc", "?RoleLevel", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.RoleName, o.RoleCode, o.RoleDesc, o.RoleLevel, o.Enabled, o.SortOrder, o.OperatorId, o.CreateTime
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_role", "role_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新用户角色
        public DataResult UpdateRole(RoleInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_role` set ");
                sql.Append("`role_name` = ?RoleName,`role_code` = ?RoleCode,`role_desc` = ?RoleDesc,`role_level` = ?RoleLevel,`enabled` = ?Enabled");
                sql.Append(",`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `role_id` = ?RoleId;");

                List<string> name = new List<string>() {
					"?RoleName", "?RoleCode", "?RoleDesc", "?RoleLevel", "?Enabled", "?SortOrder", "?UpdateTime", "?RoleId"
				};

                List<object> value = new List<object>() {
					o.RoleName, o.RoleCode, o.RoleDesc, o.RoleLevel, o.Enabled, o.SortOrder, o.UpdateTime, o.RoleId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除用户角色
        public DataResult DeleteRole(int roleId)
        {
            try
            {
                string sql = String.Format("delete from `sys_role` where `role_id` = {0};", roleId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public RoleInfo FillRoleInfo(DataRow dr)
        {
            try
            {
                RoleInfo o = new RoleInfo();

                o.RoleId = DataConvert.ConvertValue(dr["role_id"], 0);
                o.RoleName = dr["role_name"].ToString();
                o.RoleCode = dr["role_code"].ToString();
                o.RoleDesc = dr["role_desc"].ToString();
                o.RoleLevel = DataConvert.ConvertValue(dr["role_level"], 0);
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public RoleInfo FillRoleInfo(DataRowView drv)
        {
            try
            {
                return this.FillRoleInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}