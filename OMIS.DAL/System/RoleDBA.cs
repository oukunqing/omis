using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class RoleDBA : DataAccess
    {

        #region  获得单个用户角色
        public DataResult GetRole(int roleId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,g.group_name,g.group_code from `sys_role` d ");
                sql.Append(" left outer join `sys_role_group` g on d.group_id = g.group_id ");
                sql.Append(String.Format(" where d.`role_id` = {0} ", roleId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个用户角色
        public DataResult GetRole(string roleIdList)
        {
            try
            {
                if (!CheckIdList(ref roleIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,g.group_name,g.group_code from `sys_role` d ");
                sql.Append(" left outer join `sys_role_group` g on d.group_id = g.group_id ");
                sql.Append(String.Format(" where d.`role_id` in({0}) ", roleIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得用户角色
        public DataResult GetRole(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                string limitIdList = ConvertValue(dic, "LimitGroupIdList");
                if (CheckIdList(ref limitIdList))
                {
                    con.Append(String.Format(" and g.group_id in ({0}) ", limitIdList));
                }
                string excludeIdList = ConvertValue(dic, "ExcludeGroupIdList");
                if (CheckIdList(ref excludeIdList))
                {
                    con.Append(String.Format(" and g.group_id not in ({0}) ", excludeIdList));
                }

                int groupId = ConvertValue(dic, "GroupId", 0);
                con.Append(groupId > 0 ? String.Format(" and d.`group_id` = {0} ", groupId) : "");

                string groupCode = ConvertValue(dic, "GroupCode");
                con.Append(Filter(ref groupCode).Length > 0 ? String.Format(" and g.group_code = '{0}' ", groupCode) : "");

                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`role_id` in ({0}) ", keywords) : "");
                            break;
                        case "Name":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`role_name` like '%{0}%' "));
                            break;
                        case "Code":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`role_code` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,g.group_name,g.group_code from `sys_role` d ");
                sql.Append(" left outer join `sys_role_group` g on d.group_id = g.group_id ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by g.group_id,d.sort_order desc, d.role_id ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.role_id) as dataCount from `sys_role` d ");
                sql.Append(" left outer join `sys_role_group` g on d.`group_id` = g.`group_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增用户角色
        public DataResult AddRole(RoleInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_role`(");
                sql.Append("`group_id`,`role_name`,`role_code`,`role_desc`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?GroupId,?RoleName,?RoleCode,?RoleDesc,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?GroupId", "?RoleName", "?RoleCode", "?RoleDesc", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.GroupId, o.RoleName, o.RoleCode, o.RoleDesc, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
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
                sql.Append("`group_id` = ?GroupId,`role_name` = ?RoleName,`role_code` = ?RoleCode,`role_desc` = ?RoleDesc,`enabled` = ?Enabled");
                sql.Append(",`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `role_id` = ?RoleId;");

                List<string> name = new List<string>() {
					"?GroupId", "?RoleName", "?RoleCode", "?RoleDesc", "?Enabled", "?SortOrder", "?UpdateTime", "?RoleId"
				};

                List<object> value = new List<object>() {
					o.GroupId, o.RoleName, o.RoleCode, o.RoleDesc, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.RoleId
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
                o.GroupId = DataConvert.ConvertValue(dr["group_id"], 0);
                o.RoleName = dr["role_name"].ToString();
                o.RoleCode = dr["role_code"].ToString();
                o.RoleDesc = dr["role_desc"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                if (CheckColumn(dr, "group_name"))
                {
                    o.Extend = new Dictionary<string, object>()
                    {
                        {"GroupName", dr["group_name"].ToString()}
                    };
                }
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