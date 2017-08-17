using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Common;

namespace OMIS.DAL.Common
{
    public class DictionaryTypeDBA : DataAccess
    {

        #region  获得单个字典分类
        public DataResult GetDictionaryType(int typeId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,pd.type_name as parent_name,count(distinct cd.dictionary_id) as data_count ");
                sql.Append(" from `com_dictionary_type` d ");
                sql.Append(" left outer join `com_dictionary_type` pd on d.`parent_id` = pd.`type_id` ");
                sql.Append(" left outer join `com_dictionary` cd on cd.`type_id` = d.`type_id` ");
                sql.Append(String.Format(" where d.`type_id` = {0} ", typeId));
                sql.Append(" group by d.`type_id` ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个字典分类
        public DataResult GetDictionaryType(string typeIdList)
        {
            try
            {                
                if (!CheckIdList(ref typeIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,pd.type_name as parent_name,count(distinct cd.dictionary_id) as data_count ");
                sql.Append(" from `com_dictionary_type` d ");
                sql.Append(" left outer join `com_dictionary_type` pd on d.`parent_id` = pd.`type_id` ");
                sql.Append(" left outer join `com_dictionary` cd on cd.`type_id` = d.`type_id` ");
                sql.Append(String.Format(" where d.`type_id` in({0}) ", typeIdList));
                sql.Append(" group by d.`type_id` ");
                sql.Append(" order by d.`level`, d.`sort_order` desc, d.`type_id` ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得字典分类
        /// <summary>
        /// 获得字典分类
        /// </summary>
        /// <param name="dic">
        /// TypeId : int, TypeIdList : string, TypeCode : string, Enabled : int, ParentId : int
        /// </param>
        /// <returns></returns>
        public DataResult GetDictionaryType(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                int typeId = ConvertValue(dic, "TypeId", 0);
                con.Append(typeId > 0 ? String.Format(" and d.`type_id` = {0} ", typeId) : "");

                string typeIdList = ConvertValue(dic, "TypeIdList", "");
                con.Append(CheckIdList(ref typeIdList) ? String.Format(" and d.`type_id` in({0}) ", typeIdList) : "");

                string typeCode = ConvertValue(dic, "TypeCode");
                con.Append(Filter(ref typeCode).Length > 0 ? String.Format(" and d.type_code = '{0}' ", typeCode) : "");

                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                int multiSelect = ConvertValue(dic, "MultiSelect", -1);
                con.Append(multiSelect >= 0 ? String.Format(" and d.`multi_select` = {0} ", multiSelect) : "");

                int parentId = ConvertValue(dic, "ParentId", 0);
                if (parentId > 0)
                {
                    bool isGetSubset = ConvertValue(dic, "GetSubset", 0) == 1;
                    con.Append(String.Format(isGetSubset ? " and d.parent_tree like '%({0})%' " : " and d.`parent_id` = {0} ", parentId));
                }

                int excludeId = ConvertValue(dic, "ExcludeId", 0);
                if (excludeId > 0)
                {
                    con.Append(String.Format(" and d.parent_tree not like '%({0})%' ", excludeId));
                }

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,pd.type_name as parent_name,count(distinct cd.dictionary_id) as data_count ");
                sql.Append(" from `com_dictionary_type` d ");
                sql.Append(" left outer join `com_dictionary_type` pd on d.`parent_id` = pd.`type_id` ");
                sql.Append(" left outer join `com_dictionary` cd on cd.`type_id` = d.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" group by d.`type_id` ");
                sql.Append(" order by d.`level`, d.`sort_order` desc, d.`type_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.type_id) as dataCount from `com_dictionary_type` d ");
                sql.Append(" left outer join `com_dictionary_type` pd on d.`parent_id` = pd.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增字典分类
        public DataResult AddDictionaryType(DictionaryTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `com_dictionary_type`(");
                sql.Append("`type_name`,`type_code`,`level`,`parent_id`,`parent_tree`,`max_number`,`multi_select`,`multi_select_limit`,`enabled`,`remark`");
                sql.Append(",`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?TypeName,?TypeCode,?Level,?ParentId,?ParentTree,?MaxNumber,?MultiSelect,?MultiSelectLimit,?Enabled,?Remark");
                sql.Append(",?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?TypeName", "?TypeCode", "?Level", "?ParentId", "?ParentTree", "?MaxNumber", "?MultiSelect", "?MultiSelectLimit", "?Enabled", "?Remark", 
					"?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.TypeName, o.TypeCode, o.Level, o.ParentId, o.ParentTree, o.MaxNumber, o.MultiSelect, o.MultiSelectLimit, o.Enabled, o.Remark, 
					o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "com_dictionary_type", "type_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新字典分类
        public DataResult UpdateDictionaryType(DictionaryTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `com_dictionary_type` set ");
                sql.Append("`type_name` = ?TypeName,`type_code` = ?TypeCode,`level` = ?Level,`parent_id` = ?ParentId,`parent_tree` = ?ParentTree");
                //sql.Append(",`max_number` = ?MaxNumber,`multi_select` = ?MultiSelect,`multi_select_limit` = ?MultiSelectLimit,`enabled` = ?Enabled,`remark` = ?Remark");
                sql.Append(",`multi_select` = ?MultiSelect,`multi_select_limit` = ?MultiSelectLimit,`enabled` = ?Enabled,`remark` = ?Remark");
                sql.Append(",`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `type_id` = ?TypeId;");

                List<string> name = new List<string>() {
					//"?TypeName", "?TypeCode", "?Level", "?ParentId", "?ParentTree", "?MaxNumber", "?MultiSelect", "?MultiSelectLimit", "?Enabled", "?Remark", 
					"?TypeName", "?TypeCode", "?Level", "?ParentId", "?ParentTree", "?MultiSelect", "?MultiSelectLimit", "?Enabled", "?Remark", 
					"?SortOrder", "?UpdateTime", "?TypeId"
				};

                List<object> value = new List<object>() {
					//o.TypeName, o.TypeCode, o.Level, o.ParentId, o.ParentTree, o.MaxNumber, o.MultiSelect, o.MultiSelectLimit, o.Enabled, o.Remark, 
					o.TypeName, o.TypeCode, o.Level, o.ParentId, o.ParentTree, o.MultiSelect, o.MultiSelectLimit, o.Enabled, o.Remark, 
					o.SortOrder, CheckDateTime(o.UpdateTime), o.TypeId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  更新最大编号
        public int UpdateMaxNumber(int typeId)
        {
            try
            {
                return Update(DBConnString, String.Format("update `com_dictionary_type` set `max_number` = `max_number` + 1 where `type_id` = {0};", typeId));
            }
            catch (Exception ex) { throw (ex); }
        }

        public int UpdateMaxNumber(int typeId, int maxNumber)
        {
            try
            {
                return Update(DBConnString, String.Format("update `com_dictionary_type` set `max_number` = {0} where `type_id` = {1};", maxNumber, typeId));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得最大编号
        public int GetMaxNumber(int typeId)
        {
            try
            {
                string sql = String.Format("select ifnull(`max_number`,0) from `com_dictionary_type` where `type_id` = {0};", typeId);

                return Convert.ToInt32(Scalar(DBConnString, sql).ToString());
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得层级
        public int GetLevel(int typeId)
        {
            return GetTypeLevel(DBConnString, "level", "com_dictionary_type", "type_id", typeId);
        }
        #endregion

        #region  更新菜单目录树
        public int UpdateParentTree(int typeId)
        {
            return UpdateParentTree(DBConnString, "com_dictionary_type", "type_id", typeId, 0);
        }
        public int UpdateParentTree(int typeId, int minLevel)
        {
            return UpdateParentTree(DBConnString, "com_dictionary_type", "type_id", typeId, minLevel);
        }
        #endregion
        
        #region  获得字典分类子类及字典数量
        public int[] GetDictionaryDataCount(int typeId)
        {
            try
            {
                return GetChildCountAndDataCount(DBConnString, "com_dictionary_type", "type_id", typeId, "com_dictionary", "dictionary_id");
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除字典分类
        public DataResult DeleteDictionaryType(int typeId)
        {
            try
            {
                string sql = String.Format("delete from `com_dictionary_type` where `type_id` = {0};", typeId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public DictionaryTypeInfo FillDictionaryTypeInfo(DataRow dr)
        {
            try
            {
                DictionaryTypeInfo o = new DictionaryTypeInfo();

                o.TypeId = DataConvert.ConvertValue(dr["type_id"], 0);
                o.TypeName = dr["type_name"].ToString();
                o.TypeCode = dr["type_code"].ToString();
                o.Level = DataConvert.ConvertValue(dr["level"], 0);
                o.ParentId = DataConvert.ConvertValue(dr["parent_id"], 0);
                o.ParentTree = dr["parent_tree"].ToString();
                o.MaxNumber = DataConvert.ConvertValue(dr["max_number"], 0);
                o.MultiSelect = DataConvert.ConvertValue(dr["multi_select"], 0);
                o.MultiSelectLimit = DataConvert.ConvertValue(dr["multi_select_limit"], 0);
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);

                o.Remark = dr["remark"].ToString();
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.UpdateTime = dr["update_time"].ToString();

                if (CheckColumn(dr, "parent_name"))
                {
                    o.Extend = new Dictionary<string, object>()
                    {
                        {"ParentName", dr["parent_name"].ToString()},
                        {"DataCount", DataConvert.ConvertValue(dr["data_count"], 0)}
                    };
                }

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public DictionaryTypeInfo FillDictionaryTypeInfo(DataRowView drv)
        {
            try
            {
                return this.FillDictionaryTypeInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}