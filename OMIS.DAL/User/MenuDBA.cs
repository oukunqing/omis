using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.User
{
    public class MenuDBA : DataAccess
    {
     

        #region 新增导航菜单
        public DataResult AddMenu(MenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_menu`(");
                sql.Append("`type_id`,`menu_name`,`menu_code`,`menu_url`,`menu_pic`,`open_type`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?TypeId,?MenuName,?MenuCode,?MenuUrl,?MenuPic,?OpenType,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?TypeId", "?MenuName", "?MenuCode", "?MenuUrl", "?MenuPic", "?OpenType", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.TypeId, o.MenuName, o.MenuCode, o.MenuUrl, o.MenuPic, o.OpenType, o.Enabled, o.SortOrder, o.OperatorId, o.CreateTime
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_menu", "menu_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新导航菜单
        public DataResult UpdateMenu(MenuInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_menu` set ");
                sql.Append("`type_id` = ?TypeId,`menu_name` = ?MenuName,`menu_code` = ?MenuCode,`menu_url` = ?MenuUrl,`menu_pic` = ?MenuPic");
                sql.Append(",`open_type` = ?OpenType,`enabled` = ?Enabled,`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `menu_id` = ?MenuId;");

                List<string> name = new List<string>() {
					"?TypeId", "?MenuName", "?MenuCode", "?MenuUrl", "?MenuPic", "?OpenType", "?Enabled", "?SortOrder", "?UpdateTime", "?MenuId"
				};

                List<object> value = new List<object>() {
					o.TypeId, o.MenuName, o.MenuCode, o.MenuUrl, o.MenuPic, o.OpenType, o.Enabled, o.SortOrder, o.UpdateTime, o.MenuId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除导航菜单
        public DataResult DeleteMenu(int menuId)
        {
            try
            {
                string sql = String.Format("delete from `sys_menu` where `menu_id` = {0};", menuId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public MenuInfo FillMenuInfo(DataRow dr)
        {
            try
            {
                MenuInfo o = new MenuInfo();

                o.MenuId = ConvertValue(dr["menu_id"], 0);
                o.TypeId = ConvertValue(dr["type_id"], 0);
                o.MenuName = dr["menu_name"].ToString();
                o.MenuCode = dr["menu_code"].ToString();
                o.MenuUrl = dr["menu_url"].ToString();
                o.MenuPic = dr["menu_pic"].ToString();
                o.OpenType = ConvertValue(dr["open_type"], 0);
                o.Enabled = ConvertValue(dr["enabled"], 0);
                o.SortOrder = ConvertValue(dr["sort_order"], 0);
                o.OperatorId = ConvertValue(dr["operator_id"], 0);

                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public MenuInfo FillMenuInfo(DataRowView drv)
        {
            try
            {
                return this.FillMenuInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
    }
}