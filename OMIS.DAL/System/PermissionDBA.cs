using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class PermissionDBA : DataAccess
    {

        #region  获得单个操作权限
        public DataResult GetPermission(int permissionId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,t.type_name from `sys_permission` d ");
                sql.Append(" left outer join `sys_permission_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(String.Format(" where d.`permission_id` = {0} ", permissionId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个操作权限
        public DataResult GetPermission(string permissionIdList)
        {
            try
            {
                if (!CheckIdList(ref permissionIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,t.type_name from `sys_permission` d ");
                sql.Append(" left outer join `sys_permission_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(String.Format(" where d.`permission_id` in({0}) ", permissionIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得操作权限
        public DataResult GetPermission(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                int typeId = ConvertValue(dic, "TypeId", 0);
                con.Append(typeId > 0 ? String.Format(" and d.`type_id` = {0} ", typeId) : "");

                string typeCode = ConvertValue(dic, "TypeCode");
                con.Append(Filter(ref typeCode).Length > 0 ? String.Format(" and t.type_code = '{0}' ", typeCode) : "");

                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");
                
                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`permission_id` in ({0}) ", keywords) : "");
                            break;
                        case "Name":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`permission_name` like '%{0}%' "));
                            break;
                        case "Code":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`permission_code` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,t.type_name from `sys_permission` d ");
                sql.Append(" left outer join `sys_permission_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by t.type_id,d.sort_order desc, d.permission_id ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.permission_id) as dataCount from `sys_permission` d ");
                sql.Append(" left outer join `sys_permission_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增操作权限
        public DataResult AddPermission(PermissionInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_permission`(");
                sql.Append("`type_id`,`permission_name`,`permission_code`,`permission_desc`,`permission_prompt`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?TypeId,?PermissionName,?PermissionCode,?PermissionDesc,?PermissionPrompt,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?TypeId", "?PermissionName", "?PermissionCode", "?PermissionDesc", "?PermissionPrompt", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.TypeId, o.PermissionName, o.PermissionCode, o.PermissionDesc, o.PermissionPrompt, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_permission", "permission_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新操作权限
        public DataResult UpdatePermission(PermissionInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_permission` set ");
                sql.Append("`type_id` = ?TypeId,`permission_name` = ?PermissionName,`permission_code` = ?PermissionCode,`permission_desc` = ?PermissionDesc,`permission_prompt` = ?PermissionPrompt");
                sql.Append(",`enabled` = ?Enabled,`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `permission_id` = ?PermissionId;");

                List<string> name = new List<string>() {
					"?TypeId", "?PermissionName", "?PermissionCode", "?PermissionDesc", "?PermissionPrompt", "?Enabled", "?SortOrder", "?UpdateTime", "?PermissionId"
				};

                List<object> value = new List<object>() {
					o.TypeId, o.PermissionName, o.PermissionCode, o.PermissionDesc, o.PermissionPrompt, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.PermissionId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得权限-模块(分配)数量
        public int GetPermissionDataCount(int typeId)
        {
            try
            {
                return GetDataCount(DBConnString, "sys_permission", "permission_id", typeId, "sys_module_permission", "id");
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除操作权限
        public DataResult DeletePermission(int permissionId)
        {
            try
            {
                string sql = String.Format("delete from `sys_permission` where `permission_id` = {0};", permissionId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public PermissionInfo FillPermissionInfo(DataRow dr)
        {
            try
            {
                PermissionInfo o = new PermissionInfo();

                o.PermissionId = DataConvert.ConvertValue(dr["permission_id"], 0);
                o.TypeId = DataConvert.ConvertValue(dr["type_id"], 0);
                o.PermissionName = dr["permission_name"].ToString();
                o.PermissionCode = dr["permission_code"].ToString();
                o.PermissionDesc = dr["permission_desc"].ToString();
                o.PermissionPrompt = dr["permission_prompt"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                o.UpdateTime = dr["update_time"].ToString();

                if (CheckColumn(dr, "type_name"))
                {
                    o.Extend = new Dictionary<string, object>()
                    {
                        {"TypeName", dr["type_name"].ToString()}
                    };
                }

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public PermissionInfo FillPermissionInfo(DataRowView drv)
        {
            try
            {
                return this.FillPermissionInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}