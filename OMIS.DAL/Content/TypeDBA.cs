using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Content;

namespace OMIS.DAL.Content
{
    public class TypeDBA : DataAccess
    {

        #region  获得单个内容分类
        public DataResult GetType(int typeId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `con_type` d ");
                sql.Append(String.Format(" where d.`type_id` = {0} ", typeId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个内容分类
        public DataResult GetType(string typeIdList)
        {
            try
            {
                if (!CheckIdList(ref typeIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `con_type` d ");
                sql.Append(String.Format(" where d.`type_id` in({0}) ", typeIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增内容分类
        public DataResult AddType(TypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `con_type`(");
                sql.Append("`category_id`,`type_name`,`type_code`,`type_number`,`level`,`parent_id`,`parent_tree`,`enabled`,`sort_order`,`create_time`");
                sql.Append(",`update_time`");
                sql.Append(")values(");
                sql.Append("?CategoryId,?TypeName,?TypeCode,?TypeNumber,?Level,?ParentId,?ParentTree,?Enabled,?SortOrder,?CreateTime");
                sql.Append(",?UpdateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?CategoryId", "?TypeName", "?TypeCode", "?TypeNumber", "?Level", "?ParentId", "?ParentTree", "?Enabled", "?SortOrder", "?CreateTime", 
					"?UpdateTime"
				};

                List<object> value = new List<object>() {
					o.CategoryId, o.TypeName, o.TypeCode, o.TypeNumber, o.Level, o.ParentId, o.ParentTree, o.Enabled, o.SortOrder, CheckDateTime(o.CreateTime), 
					CheckDateTime(o.UpdateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "con_type", "type_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新内容分类
        public DataResult UpdateType(TypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `con_type` set ");
                sql.Append("`category_id` = ?CategoryId,`type_name` = ?TypeName,`type_code` = ?TypeCode,`type_number` = ?TypeNumber,`level` = ?Level");
                sql.Append(",`parent_id` = ?ParentId,`parent_tree` = ?ParentTree,`enabled` = ?Enabled,`sort_order` = ?SortOrder,`create_time` = ?CreateTime");
                sql.Append(",`update_time` = ?UpdateTime");
                sql.Append(" where `type_id` = ?TypeId;");

                List<string> name = new List<string>() {
					"?CategoryId", "?TypeName", "?TypeCode", "?TypeNumber", "?Level", "?ParentId", "?ParentTree", "?Enabled", "?SortOrder", "?CreateTime", 
					"?UpdateTime", "?TypeId"
				};

                List<object> value = new List<object>() {
					o.CategoryId, o.TypeName, o.TypeCode, o.TypeNumber, o.Level, o.ParentId, o.ParentTree, o.Enabled, o.SortOrder, CheckDateTime(o.CreateTime), 
					CheckDateTime(o.UpdateTime), o.TypeId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除内容分类
        public DataResult DeleteType(int typeId)
        {
            try
            {
                string sql = String.Format("delete from `con_type` where `type_id` = {0};", typeId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public TypeInfo FillTypeInfo(DataRow dr)
        {
            try
            {
                TypeInfo o = new TypeInfo();

                o.TypeId = DataConvert.ConvertValue(dr["type_id"], 0);
                o.CategoryId = DataConvert.ConvertValue(dr["category_id"], 0);
                o.TypeName = dr["type_name"].ToString();
                o.TypeCode = dr["type_code"].ToString();
                o.TypeNumber = DataConvert.ConvertValue(dr["type_number"], 0);
                o.Level = DataConvert.ConvertValue(dr["level"], 0);
                o.ParentId = DataConvert.ConvertValue(dr["parent_id"], 0);
                o.ParentTree = dr["parent_tree"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);

                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public TypeInfo FillTypeInfo(DataRowView drv)
        {
            try
            {
                return this.FillTypeInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}