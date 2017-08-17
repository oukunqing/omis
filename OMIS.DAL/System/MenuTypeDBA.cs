using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class MenuTypeDBA : DataAccess
    {
        
        #region  获得单个导航菜单分类
        public DataResult GetMenuType(int typeId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_menu_type` d ");
                sql.Append(String.Format(" where d.`type_id` = {0} ", typeId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个导航菜单分类
        public DataResult GetMenuType(string typeIdList)
        {
            try
            {
                if (!CheckIdList(ref typeIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `sys_menu_type` d ");
                sql.Append(String.Format(" where d.`type_id` in({0}) ", typeIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得导航菜单分类
        public DataResult GetMenuType(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `sys_menu_type` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增导航菜单分类
        public DataResult AddMenuType(MenuTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_menu_type`(");
                sql.Append("`type_name`,`type_code`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?TypeName,?TypeCode,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?TypeName", "?TypeCode", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.TypeName, o.TypeCode, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_menu_type", "type_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新导航菜单分类
        public DataResult UpdateMenuType(MenuTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_menu_type` set ");
                sql.Append("`type_name` = ?TypeName,`type_code` = ?TypeCode,`enabled` = ?Enabled,`sort_order` = ?SortOrder");
                sql.Append(" where `type_id` = ?TypeId;");

                List<string> name = new List<string>() {
					"?TypeName", "?TypeCode", "?Enabled", "?SortOrder", "?TypeId"
				};

                List<object> value = new List<object>() {
					o.TypeName, o.TypeCode, o.Enabled, o.SortOrder, o.TypeId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除导航菜单分类
        public DataResult DeleteMenuType(int typeId)
        {
            try
            {
                string sql = String.Format("delete from `sys_menu_type` where `type_id` = {0};", typeId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public MenuTypeInfo FillMenuTypeInfo(DataRow dr)
        {
            try
            {
                MenuTypeInfo o = new MenuTypeInfo();

                o.TypeId = DataConvert.ConvertValue(dr["type_id"], 0);
                o.TypeName = dr["type_name"].ToString();
                o.TypeCode = dr["type_code"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public MenuTypeInfo FillMenuTypeInfo(DataRowView drv)
        {
            try
            {
                return this.FillMenuTypeInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}