using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class ModuleDBA : DataAccess
    {

        #region  获得单个模块配置
        public DataResult GetModule(int moduleId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,pd.module_name as parent_name from `sys_module` d ");
                sql.Append(" left outer join `sys_module` pd on d.`parent_id` = pd.`module_id` ");
                sql.Append(String.Format(" where d.`module_id` = {0} ", moduleId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个模块配置
        public DataResult GetModule(string moduleIdList)
        {
            try
            {
                if (!CheckIdList(ref moduleIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,pd.module_name as parent_name from `sys_module` d ");
                sql.Append(" left outer join `sys_module` pd on d.`parent_id` = pd.`module_id` ");
                sql.Append(String.Format(" where d.`module_id` in({0}) ", moduleIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得模块配置
        public DataResult GetModule(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,pd.module_name as parent_name from `sys_module` d ");
                sql.Append(" left outer join `sys_module` pd on d.`parent_id` = pd.`module_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");

                sql.Append(" select count(distinct d.module_id) as dataCount from `sys_module` d ");
                sql.Append(" left outer join `sys_module` pd on d.`parent_id` = pd.`module_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");

                bool getPermission = ConvertValue(dic, "ShowPermission", 0) == 1;
                if (getPermission)
                {
                    sql.Append(" select d.module_id,p.permission_id,p.permission_name,p.permission_code ");
                    sql.Append(" from `sys_module_permission` mp ");
                    sql.Append(" left outer join `sys_module` d on mp.module_id = d.module_id ");
                    sql.Append(" left outer join `sys_permission` p on mp.permission_id = p.permission_id ");
                    sql.Append(";");
                }
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  解析模块-权限
        public ModuleInfo ParseModulePermission(DataTable dt, ModuleInfo o)
        {
            try
            {
                if (CheckTable(dt))
                {
                    string filter = String.Format("module_id = {0}", o.ModuleId);
                    DataView dv = new DataView(dt, filter, "", DataViewRowState.CurrentRows);

                    List<int> ids = new List<int>();
                    List<string> names = new List<string>();
                    foreach (DataRowView dr in dv)
                    {
                        ids.Add(ConvertValue(dr["permission_id"], 0));
                        names.Add(dr["permission_name"].ToString());
                    }
                    if (ids.Count > 0)
                    {
                        o.Extend.Add("PermissionId", ids);
                        o.Extend.Add("PermissionName", names);
                    }
                }
                return o;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region 新增模块配置
        public DataResult AddModule(ModuleInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_module`(");
                sql.Append("`module_name`,`module_code`,`module_desc`,`level`,`parent_id`,`parent_tree`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?ModuleName,?ModuleCode,?ModuleDesc,?Level,?ParentId,?ParentTree,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?ModuleName", "?ModuleCode", "?ModuleDesc", "?Level", "?ParentId", "?ParentTree", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.ModuleName, o.ModuleCode, o.ModuleDesc, o.Level, o.ParentId, o.ParentTree, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_module", "module_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新模块配置
        public DataResult UpdateModule(ModuleInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_module` set ");
                sql.Append("`module_name` = ?ModuleName,`module_code` = ?ModuleCode,`module_desc` = ?ModuleDesc,`level` = ?Level,`parent_id` = ?ParentId");
                sql.Append(",`parent_tree` = ?ParentTree,`enabled` = ?Enabled,`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `module_id` = ?ModuleId;");

                List<string> name = new List<string>() {
					"?ModuleName", "?ModuleCode", "?ModuleDesc", "?Level", "?ParentId", "?ParentTree", "?Enabled", "?SortOrder", "?UpdateTime", "?ModuleId"
				};

                List<object> value = new List<object>() {
					o.ModuleName, o.ModuleCode, o.ModuleDesc, o.Level, o.ParentId, o.ParentTree, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.ModuleId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region  获得层级
        public int GetLevel(int moduleId)
        {
            return GetTypeLevel(DBConnString, "level", "sys_module", "module_id", moduleId);
        }
        #endregion

        #region  更新菜单目录树
        public int UpdateParentTree(int moduleId)
        {
            return UpdateParentTree(DBConnString, "sys_module", "module_id", moduleId, 0);
        }
        public int UpdateParentTree(int moduleId, int minLevel)
        {
            return UpdateParentTree(DBConnString, "sys_module", "module_id", moduleId, minLevel);
        }
        #endregion

        #region  获得子模块数量
        public int GetModuleChildCount(int moduleId)
        {
            try
            {
                return GetChildCount(DBConnString, "sys_module", "module_id", moduleId);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得模块-模块权限数量
        public int GetModuleDataCount(int moduleId)
        {
            try
            {
                return GetDataCount(DBConnString, "sys_module", "module_id", moduleId, "sys_module_permission", "id");
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除模块配置
        public DataResult DeleteModule(int moduleId)
        {
            try
            {
                string sql = String.Format("delete from `sys_module` where `module_id` = {0};", moduleId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public ModuleInfo FillModuleInfo(DataRow dr)
        {
            try
            {
                ModuleInfo o = new ModuleInfo();

                o.ModuleId = DataConvert.ConvertValue(dr["module_id"], 0);
                o.ModuleName = dr["module_name"].ToString();
                o.ModuleCode = dr["module_code"].ToString();
                o.ModuleDesc = dr["module_desc"].ToString();
                o.Level = DataConvert.ConvertValue(dr["level"], 0);
                o.ParentId = DataConvert.ConvertValue(dr["parent_id"], 0);
                o.ParentTree = dr["parent_tree"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);

                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                if (CheckColumn(dr, "parent_name"))
                {
                    o.Extend = new Dictionary<string, object>()
                    {
                        {"ParentName", dr["parent_name"].ToString()}
                    };
                }

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public ModuleInfo FillModuleInfo(DataRowView drv)
        {
            try
            {
                return this.FillModuleInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}