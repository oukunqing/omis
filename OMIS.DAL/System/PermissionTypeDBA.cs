using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.System;

namespace OMIS.DAL.System
{
    public class PermissionTypeDBA : DataAccess
    {

        #region  获得单个权限分类
        public DataResult GetPermissionType(int typeId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,count(distinct cd.permission_id) as data_count ");
                sql.Append(" from `sys_permission_type` d ");
                sql.Append(" left outer join `sys_permission` cd on cd.`type_id` = d.`type_id` ");
                sql.Append(String.Format(" where d.`type_id` = {0} ", typeId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个权限分类
        public DataResult GetPermissionType(string typeIdList)
        {
            try
            {
                if (!CheckIdList(ref typeIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,count(distinct cd.permission_id) as data_count ");
                sql.Append(" from `sys_permission_type` d ");
                sql.Append(" left outer join `sys_permission` cd on cd.`type_id` = d.`type_id` ");
                sql.Append(String.Format(" where d.`type_id` in({0}) ", typeIdList));
                sql.Append(" group by d.`type_id` ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得权限分类
        public DataResult GetPermissionType(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                int typeId = ConvertValue(dic, "TypeId", 0);
                con.Append(typeId > 0 ? String.Format(" and d.`type_id` = {0} ", typeId) : "");

                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`type_id` in ({0}) ", keywords) : "");
                            break;
                        case "Name":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`type_name` like '%{0}%' "));
                            break;
                        case "Code":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`type_code` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,count(distinct cd.permission_id) as data_count ");
                sql.Append(" from `sys_permission_type` d ");
                sql.Append(" left outer join `sys_permission` cd on cd.`type_id` = d.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" group by d.`type_id` ");
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增权限分类
        public DataResult AddPermissionType(PermissionTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `sys_permission_type`(");
                sql.Append("`type_name`,`type_code`,`type_desc`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?TypeName,?TypeCode,?TypeDesc,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?TypeName", "?TypeCode", "?TypeDesc", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.TypeName, o.TypeCode, o.TypeDesc, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "sys_permission_type", "type_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新权限分类
        public DataResult UpdatePermissionType(PermissionTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `sys_permission_type` set ");
                sql.Append("`type_name` = ?TypeName,`type_code` = ?TypeCode,`type_desc` = ?TypeDesc,`enabled` = ?Enabled,`sort_order` = ?SortOrder");
                sql.Append(",`update_time` = ?UpdateTime");
                sql.Append(" where `type_id` = ?TypeId;");

                List<string> name = new List<string>() {
					"?TypeName", "?TypeCode", "?TypeDesc", "?Enabled", "?SortOrder", "?UpdateTime", "?TypeId"
				};

                List<object> value = new List<object>() {
					o.TypeName, o.TypeCode, o.TypeDesc, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.TypeId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得权限分类权限数量
        public int GetPermissionDataCount(int typeId)
        {
            try
            {
                return GetDataCount(DBConnString, "sys_permission_type", "type_id", typeId, "sys_permission", "permission_id");
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除权限分类
        public DataResult DeletePermissionType(int typeId)
        {
            try
            {
                string sql = String.Format("delete from `sys_permission_type` where `type_id` = {0};", typeId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public PermissionTypeInfo FillPermissionTypeInfo(DataRow dr)
        {
            try
            {
                PermissionTypeInfo o = new PermissionTypeInfo();

                o.TypeId = DataConvert.ConvertValue(dr["type_id"], 0);
                o.TypeName = dr["type_name"].ToString();
                o.TypeCode = dr["type_code"].ToString();
                o.TypeDesc = dr["type_desc"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                if (CheckColumn(dr, "data_count"))
                {
                    o.Extend = new Dictionary<string, object>()
                    {
                        {"DataCount", DataConvert.ConvertValue(dr["data_count"], 0)}
                    };
                }

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public PermissionTypeInfo FillPermissionTypeInfo(DataRowView drv)
        {
            try
            {
                return this.FillPermissionTypeInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}