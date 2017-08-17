using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class RoleModuleMenuDBA : DataAccess
    {

        #region  获得单个角色-模块菜单
        public DataResult GetRoleModuleMenu(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_role_module_menu` d ");
                sql.Append(String.Format(" where d.`id` = {0} ", id));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个角色-模块菜单
        public DataResult GetRoleModuleMenu(string idList)
        {
            try
            {
                if (!CheckIdList(ref idList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_role_module_menu` d ");
                sql.Append(String.Format(" where d.`id` in({0}) ", idList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得角色-模块菜单
        public DataResult GetRoleModuleMenu(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_role_module_menu` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增角色-模块菜单
        public DataResult AddRoleModuleMenu(RoleModuleMenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_role_module_menu`(");
                sql.Append("`role_id`,`menu_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?RoleId,?MenuId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?RoleId", "?MenuId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.RoleId, o.MenuId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_role_module_menu", "id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新角色-模块菜单
        public DataResult UpdateRoleModuleMenu(RoleModuleMenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_role_module_menu` set ");
                sql.Append("`role_id` = ?RoleId,`menu_id` = ?MenuId");
                sql.Append(" where `id` = ?Id;");

                List<string> name = new List<string>() {
					"?RoleId", "?MenuId", "?Id"
				};

                List<object> value = new List<object>() {
					o.RoleId, o.MenuId, o.Id
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        

        #region  获得角色-模块菜单配置
        public DataResult GetRoleModuleMenuConfig(Dictionary<string, object> dic)
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
                    //菜单
                    sql.Append(" select d.menu_id,d.menu_name,d.menu_code,d.level,d.parent_id,d.enabled,d.menu_url,d.menu_type ");
                    sql.Append(" from `sys_module_menu` d ");
                    sql.Append(" order by d.`level`,d.`sort_order` desc,d.`menu_id` ");
                    sql.Append(";");
                    //角色-菜单
                    sql.Append(" select r.role_id,d.menu_id from sys_role_module_menu rm,sys_role r,sys_module_menu d ");
                    sql.Append(" where rm.role_id = r.role_id and rm.menu_id = d.menu_id ");
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

        #region  批量新增角色-模块菜单
        public DataResult BatchAddRoleModuleMenu(List<RoleModuleMenuInfo> list)
        {
            try
            {
                if (list.Count > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into `sys_role_module_menu`(");
                    sql.Append("`role_id`,`menu_id`,`operator_id`,`create_time`");
                    sql.Append(")values");

                    int n = 0;
                    foreach (RoleModuleMenuInfo o in list)
                    {
                        sql.Append(n++ > 0 ? "," : "");
                        sql.Append("(");
                        sql.Append(String.Format("{0},{1},{2},'{3}'",
                        o.RoleId, o.MenuId, o.OperatorId, o.CreateTime));
                        sql.Append(")");
                    }
                    sql.Append(";");

                    return new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString()));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }

        public DataResult BatchAddRoleModuleMenu(int roleId, string menuIdList, int operatorId, string createTime)
        {
            try
            {
                if (roleId > 0 && CheckIdList(ref menuIdList))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into `sys_role_module_menu`(");
                    sql.Append("`role_id`,`menu_id`,`operator_id`,`create_time`");
                    sql.Append(")values");

                    string[] list = menuIdList.Split(',');
                    int n = 0;
                    foreach (string id in list)
                    {
                        sql.Append(n++ > 0 ? "," : "");
                        sql.Append("(");
                        sql.Append(String.Format("{0},{1},{2},'{3}'", roleId, id, operatorId, createTime));
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

        #region 删除角色-模块菜单
        public DataResult DeleteRoleModuleMenu(int id)
        {
            try
            {
                string sql = String.Format("delete from `sys_role_module_menu` where `id` = {0};", id);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }

        public DataResult DeleteRoleModuleMenu(int roleId, int menuId)
        {
            try
            {
                if (roleId > 0 || menuId > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("delete from `sys_role_module_menu` where 1 = 1 ");
                    sql.Append(roleId > 0 ? String.Format(" and role_id = {0} ", roleId) : "");
                    sql.Append(menuId > 0 ? String.Format(" and menu_id = {0} ", menuId) : "");

                    return new DataResult(sql.ToString(), Delete(DBConnString, sql.ToString()));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public RoleModuleMenuInfo FillRoleModuleMenuInfo(DataRow dr)
        {
            try
            {
                RoleModuleMenuInfo o = new RoleModuleMenuInfo();

                o.Id = DataConvert.ConvertValue(dr["id"], 0);
                o.RoleId = DataConvert.ConvertValue(dr["role_id"], 0);
                o.MenuId = DataConvert.ConvertValue(dr["menu_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public RoleModuleMenuInfo FillRoleModuleMenuInfo(DataRowView drv)
        {
            try
            {
                return this.FillRoleModuleMenuInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}