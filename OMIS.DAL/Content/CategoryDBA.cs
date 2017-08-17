using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Content;

namespace OMIS.DAL.Content
{
    public class CategoryDBA : DataAccess
    {

        #region  获得单个内容类别
        public DataResult GetCategory(int categoryId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `con_category` d ");
                sql.Append(String.Format(" where d.`category_id` = {0} ", categoryId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个内容类别
        public DataResult GetCategory(string categoryIdList)
        {
            try
            {
                if (!CheckIdList(ref categoryIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `con_category` d ");
                sql.Append(String.Format(" where d.`category_id` in({0}) ", categoryIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得内容类别
        public DataResult GetCategory(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `con_category` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增内容类别
        public DataResult AddCategory(CategoryInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `con_category`(");
                sql.Append("`category_name`,`category_code`,`category_desc`,`custom_param`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?CategoryName,?CategoryCode,?CategoryDesc,?CustomParam,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?CategoryName", "?CategoryCode", "?CategoryDesc", "?CustomParam", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.CategoryName, o.CategoryCode, o.CategoryDesc, o.CustomParam, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "con_category", "category_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新内容类别
        public DataResult UpdateCategory(CategoryInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `con_category` set ");
                sql.Append("`category_name` = ?CategoryName,`category_code` = ?CategoryCode,`category_desc` = ?CategoryDesc,`custom_param` = ?CustomParam,`enabled` = ?Enabled");
                sql.Append(",`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `category_id` = ?CategoryId;");

                List<string> name = new List<string>() {
					"?CategoryName", "?CategoryCode", "?CategoryDesc", "?CustomParam", "?Enabled", "?SortOrder", "?UpdateTime", "?CategoryId"
				};

                List<object> value = new List<object>() {
					o.CategoryName, o.CategoryCode, o.CategoryDesc, o.CustomParam, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.CategoryId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除内容类别
        public DataResult DeleteCategory(int categoryId)
        {
            try
            {
                string sql = String.Format("delete from `con_category` where `category_id` = {0};", categoryId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public CategoryInfo FillCategoryInfo(DataRow dr)
        {
            try
            {
                CategoryInfo o = new CategoryInfo();

                o.CategoryId = DataConvert.ConvertValue(dr["category_id"], 0);
                o.CategoryName = dr["category_name"].ToString();
                o.CategoryCode = dr["category_code"].ToString();
                o.CategoryDesc = dr["category_desc"].ToString();
                o.CustomParam = DataConvert.ConvertValue(dr["custom_param"], 0);
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public CategoryInfo FillCategoryInfo(DataRowView drv)
        {
            try
            {
                return this.FillCategoryInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion



    }
}