using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class RoleGroupDBA : DataAccess
    {

        #region  获得单个角色组别
        public DataResult GetRoleGroup(int groupId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_role_group` d ");
                sql.Append(String.Format(" where d.`group_id` = {0} ", groupId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个角色组别
        public DataResult GetRoleGroup(string groupIdList)
        {
            try
            {
                if (!CheckIdList(ref groupIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_role_group` d ");
                sql.Append(String.Format(" where d.`group_id` in({0}) ", groupIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得角色组别
        public DataResult GetRoleGroup(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                string limitIdList = ConvertValue(dic, "LimitGroupIdList");
                if (CheckIdList(ref limitIdList))
                {
                    con.Append(String.Format(" and d.group_id in ({0}) ", limitIdList));
                }
                string excludeIdList = ConvertValue(dic, "ExcludeGroupIdList");
                if (CheckIdList(ref excludeIdList))
                {
                    con.Append(String.Format(" and d.group_id not in ({0}) ", excludeIdList));
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_role_group` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.`group_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`group_id`) as dataCount from `sys_role_group` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增角色组别
        public DataResult AddRoleGroup(RoleGroupInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_role_group`(");
                sql.Append("`group_name`,`group_code`");
                sql.Append(")values(");
                sql.Append("?GroupName,?GroupCode");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?GroupName", "?GroupCode"
				};

                List<object> value = new List<object>() {
					o.GroupName, o.GroupCode
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_role_group", "group_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新角色组别
        public DataResult UpdateRoleGroup(RoleGroupInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_role_group` set ");
                sql.Append("`group_name` = ?GroupName,`group_code` = ?GroupCode");
                sql.Append(" where `group_id` = ?GroupId;");

                List<string> name = new List<string>() {
					"?GroupName", "?GroupCode", "?GroupId"
				};

                List<object> value = new List<object>() {
					o.GroupName, o.GroupCode, o.GroupId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除角色组别
        public DataResult DeleteRoleGroup(int groupId)
        {
            try
            {
                string sql = String.Format("delete from `sys_role_group` where `group_id` = {0};", groupId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public RoleGroupInfo FillRoleGroupInfo(DataRow dr)
        {
            try
            {
                RoleGroupInfo o = new RoleGroupInfo();

                o.GroupId = DataConvert.ConvertValue(dr["group_id"], 0);
                o.GroupName = dr["group_name"].ToString();
                o.GroupCode = dr["group_code"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public RoleGroupInfo FillRoleGroupInfo(DataRowView drv)
        {
            try
            {
                return this.FillRoleGroupInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}