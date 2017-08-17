using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class ModulePermissionDBA : DataAccess
    {

        #region  获得单个模块-权限
        public DataResult GetModulePermission(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_module_permission` d ");
                sql.Append(String.Format(" where d.`id` = {0} ", id));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个模块-权限
        public DataResult GetModulePermission(string idList)
        {
            try
            {
                if (!CheckIdList(ref idList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_module_permission` d ");
                sql.Append(String.Format(" where d.`id` in({0}) ", idList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得模块-权限
        public DataResult GetModulePermission(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_module_permission` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增模块-权限
        public DataResult AddModulePermission(ModulePermissionInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_module_permission`(");
                sql.Append("`module_id`,`permission_id`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?ModuleId,?PermissionId,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?ModuleId", "?PermissionId", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.ModuleId, o.PermissionId, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_module_permission", "id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新模块-权限
        public DataResult UpdateModulePermission(ModulePermissionInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_module_permission` set ");
                sql.Append("`module_id` = ?ModuleId,`permission_id` = ?PermissionId");
                sql.Append(" where `id` = ?Id;");

                List<string> name = new List<string>() {
					"?ModuleId", "?PermissionId", "?Id"
				};

                List<object> value = new List<object>() {
					o.ModuleId, o.PermissionId, o.Id
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region  获得模块-权限配置
        public DataResult GetModulePermissionConfig(Dictionary<string, object> dic)
        {
            try
            {
                int moduleId = ConvertValue(dic, "ModuleId", 0);

                #region  Sql
                StringBuilder sql = new StringBuilder();

                //模块
                sql.Append(" select d.module_id,d.module_name,d.module_code from `sys_module` d ");
                sql.Append(moduleId > 0 ? String.Format(" where d.module_id = {0} ", moduleId) : "");
                sql.Append(";");

                //权限分类
                sql.Append(" select t.type_id,t.type_name from `sys_permission_type` t; ");
                //权限
                sql.Append(" select p.permission_id,p.permission_name,p.permission_code,p.type_id from `sys_permission` p; ");

                //模块-权限
                sql.Append(" select mp.module_id,mp.permission_id ");
                sql.Append(" from `sys_module_permission` mp ");
                sql.Append(" left outer join `sys_module` d on mp.module_id = d.module_id ");
                sql.Append(" where 1 = 1 ");
                sql.Append(moduleId > 0 ? String.Format(" and d.module_id = {0} ", moduleId) : "");
                sql.Append(";");

                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  批量新增模块-权限
        public DataResult BatchAddModulePermission(List<ModulePermissionInfo> list)
        {
            try
            {
                if (list.Count > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into `sys_module_permission`(");
                    sql.Append("`module_id`,`permission_id`,`operator_id`,`create_time`");
                    sql.Append(")values");

                    int n = 0;
                    foreach (ModulePermissionInfo o in list)
                    {
                        sql.Append(n++ > 0 ? "," : "");
                        sql.Append("(");
                        sql.Append(String.Format("{0},{1},{2},'{3}'", o.ModuleId, o.PermissionId, o.OperatorId, o.CreateTime));
                        sql.Append(")");
                    }
                    sql.Append(";");

                    return new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString()));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }

        public DataResult BatchAddModulePermission(int moduleId, string permissionIdList, int operatorId, string createTime)
        {
            try
            {
                if (moduleId > 0 && CheckIdList(ref permissionIdList))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into `sys_module_permission`(");
                    sql.Append("`module_id`,`permission_id`,`operator_id`,`create_time`");
                    sql.Append(")values");

                    string[] list = permissionIdList.Split(',');
                    int n = 0;
                    foreach (string id in list)
                    {
                        sql.Append(n++ > 0 ? "," : "");
                        sql.Append("(");
                        sql.Append(String.Format("{0},{1},{2},'{3}'",  moduleId, id, operatorId, createTime));
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


        #region 删除模块-权限
        public DataResult DeleteModulePermission(int id)
        {
            try
            {
                string sql = String.Format("delete from `sys_module_permission` where `id` = {0};", id);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }

        public DataResult DeleteModulePermission(int moduleId, int permissionId)
        {
            try
            {
                if (moduleId > 0 || permissionId > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("delete from `sys_module_permission` where 1 = 1 ");
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
        public ModulePermissionInfo FillModulePermissionInfo(DataRow dr)
        {
            try
            {
                ModulePermissionInfo o = new ModulePermissionInfo();

                o.Id = DataConvert.ConvertValue(dr["id"], 0);
                o.ModuleId = DataConvert.ConvertValue(dr["module_id"], 0);
                o.PermissionId = DataConvert.ConvertValue(dr["permission_id"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public ModulePermissionInfo FillModulePermissionInfo(DataRowView drv)
        {
            try
            {
                return this.FillModulePermissionInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}