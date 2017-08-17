using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class RoleModulePermissionDBA : DataAccess
    {

        #region  获得单个角色-模块-权限
        public DataResult GetRoleModulePermission(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_role_module_permission` d ");
                sql.Append(String.Format(" where d.`id` = {0} ", id));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个角色-模块-权限
        public DataResult GetRoleModulePermission(string idList)
        {
            try
            {
                if (!CheckIdList(ref idList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_role_module_permission` d ");
                sql.Append(String.Format(" where d.`id` in({0}) ", idList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得角色-模块-权限
        public DataResult GetRoleModulePermission(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_role_module_permission` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增角色-模块-权限
        public DataResult AddRoleModulePermission(RoleModulePermissionInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_role_module_permission`(");
                sql.Append("`role_id`,`module_id`,`permission_id`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?RoleId,?ModuleId,?PermissionId,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?RoleId", "?ModuleId", "?PermissionId", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.RoleId, o.ModuleId, o.PermissionId, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_role_module_permission", "id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新角色-模块-权限
        public DataResult UpdateRoleModulePermission(RoleModulePermissionInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_role_module_permission` set ");
                sql.Append("`role_id` = ?RoleId,`module_id` = ?ModuleId,`permission_id` = ?PermissionId");
                sql.Append(" where `id` = ?Id;");

                List<string> name = new List<string>() {
					"?RoleId", "?ModuleId", "?PermissionId", "?Id"
				};

                List<object> value = new List<object>() {
					o.RoleId, o.ModuleId, o.PermissionId, o.Id
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得角色-模块-权限配置
        public DataResult GetRoleModulePermissionConfig(Dictionary<string, object> dic)
        {
            try
            {
                int roleId = ConvertValue(dic, "RoleId", 0);
                if (roleId > 0)
                {
                    #region  Sql
                    StringBuilder sql = new StringBuilder();
                    //角色
                    sql.Append(" select r.role_id,r.role_name,r.role_code from `sys_role` r ");
                    sql.Append(roleId > 0 ? String.Format(" where r.role_id = {0} ", roleId) : "");
                    sql.Append(";");
                    //模块
                    sql.Append(" select d.module_id,d.module_name,d.module_code,d.level,d.parent_id from `sys_module` d ;");
                    //模块-权限
                    sql.Append(" select m.module_id,p.permission_id,p.permission_name,p.permission_code from sys_module_permission mp,sys_module m,sys_permission p ");
                    sql.Append(" where mp.module_id = m.module_id and mp.permission_id = p.permission_id;");
                    //角色-模块-权限
                    sql.Append(" select rmp.role_id,rmp.module_id,rmp.permission_id ");
                    sql.Append(" from `sys_role_module_permission` rmp,`sys_role` r,`sys_module` d ");
                    sql.Append(" where 1 = 1 and rmp.role_id = r.role_id and rmp.module_id = d.module_id");
                    sql.Append(roleId > 0 ? String.Format(" and r.role_id = {0} ", roleId) : "");
                    sql.Append(";");

                    #endregion

                    return new DataResult(sql, Select(DBConnString, sql));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  批量新增角色-模块-权限
        public DataResult BatchAddRoleModulePermission(List<RoleModulePermissionInfo> list)
        {
            try
            {
                if (list.Count > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into `sys_role_module_permission`(");
                    sql.Append("`role_id`,`module_id`,`permission_id`,`operator_id`,`create_time`");
                    sql.Append(")values");

                    int n = 0;
                    foreach (RoleModulePermissionInfo o in list)
                    {
                        sql.Append(n++ > 0 ? "," : "");
                        sql.Append("(");
                        sql.Append(String.Format("{0},{1},{2},{3},'{4}'",
                        o.RoleId, o.ModuleId, o.PermissionId, o.OperatorId, o.CreateTime));
                        sql.Append(")");
                    }
                    sql.Append(";");

                    return new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString()));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }

        public DataResult BatchAddRoleModulePermission(int roleId, string moduleIdList, string permissionIdList, int operatorId, string createTime)
        {
            try
            {
                if (roleId > 0 && CheckIdList(ref moduleIdList) && CheckIdList(ref permissionIdList))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into `sys_role_module_permission`(");
                    sql.Append("`role_id`,`module_id`,`permission_id`,`operator_id`,`create_time`");
                    sql.Append(")values");

                    string[] ms = moduleIdList.Split(',');
                    string[] ps = permissionIdList.Split(',');
                    int n = 0;
                    for (int i = 0, c = ms.Length; i < c; i++)
                    {
                        sql.Append(n++ > 0 ? "," : "");
                        sql.Append("(");
                        sql.Append(String.Format("{0},{1},{2},{3},'{4}'", roleId, ms[i], ps[i], operatorId, createTime));
                        sql.Append(")");
                    }
                    sql.Append(";");

                    return new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString()));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除角色-模块-权限
        public DataResult DeleteRoleModulePermission(int id)
        {
            try
            {
                string sql = String.Format("delete from `sys_role_module_permission` where `id` = {0};", id);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }

        public DataResult DeleteRoleModulePermission(int roleId, int moduleId, int permissionId)
        {
            try
            {
                if (roleId > 0 || moduleId > 0 || permissionId > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("delete from `sys_role_module_permission` where 1 = 1 ");
                    sql.Append(roleId > 0 ? String.Format(" and role_id = {0} ", roleId) : "");
                    sql.Append(moduleId > 0 ? String.Format(" and module_id = {0} ", moduleId) : "");
                    sql.Append(permissionId > 0 ? String.Format(" and permission_id = {0} ", permissionId) : "");

                    return new DataResult(sql.ToString(), Delete(DBConnString, sql.ToString()));

                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public RoleModulePermissionInfo FillRoleModulePermissionInfo(DataRow dr)
        {
            try
            {
                RoleModulePermissionInfo o = new RoleModulePermissionInfo();

                o.Id = DataConvert.ConvertValue(dr["id"], 0);
                o.RoleId = DataConvert.ConvertValue(dr["role_id"], 0);
                o.ModuleId = DataConvert.ConvertValue(dr["module_id"], 0);
                o.PermissionId = DataConvert.ConvertValue(dr["permission_id"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public RoleModulePermissionInfo FillRoleModulePermissionInfo(DataRowView drv)
        {
            try
            {
                return this.FillRoleModulePermissionInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}