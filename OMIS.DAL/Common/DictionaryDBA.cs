using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Common;

namespace OMIS.DAL.Common
{
    public class DictionaryDBA : DataAccess
    {

        #region  获得单个分类字典
        public DataResult GetDictionary(int dictionaryId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,t.type_name from `com_dictionary` d ");
                sql.Append(" left outer join `com_dictionary_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(String.Format(" where d.`dictionary_id` = {0} ", dictionaryId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个分类字典
        public DataResult GetDictionary(string dictionaryIdList)
        {
            try
            {
                if (!CheckIdList(ref dictionaryIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,t.type_name from `com_dictionary` d ");
                sql.Append(" left outer join `com_dictionary_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(String.Format(" where d.`dictionary_id` in({0}) ", dictionaryIdList));
                sql.Append(" order by d.type_id, d.`sort_order` desc, d.`dictionary_id` ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得分类字典
        /// <summary>
        /// 获得分类字典
        /// </summary>
        /// <param name="dic">
        /// RootId: int, TypeId : int, GetSubset : int, TypeCode : string, Enabled : int, Number : int,
        /// Keywords : string, SearchField : string, PageIndex : int, PageSize : int
        /// </param>
        /// <returns></returns>
        public DataResult GetDictionary(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                int rootId = ConvertValue(dic, "RootId", 0);
                con.Append(rootId > 0 ? String.Format(" and t.parent_tree like '%({0})%' ", rootId) : "");

                int typeId = ConvertValue(dic, "TypeId", 0);
                if (typeId > 0)
                {
                    bool isGetSubset = ConvertValue(dic, "GetSubset", 0) == 1;
                    con.Append(String.Format(isGetSubset ? " and t.parent_tree like '%({0})%' " : " and d.`type_id` = {0} ", typeId));
                }

                string typeCode = ConvertValue(dic, "TypeCode");
                con.Append(Filter(ref typeCode).Length > 0 ? String.Format(" and t.type_code = '{0}' ", typeCode) : "");

                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                int number = ConvertValue(dic, "Number", 0);
                con.Append(number > 0 ? String.Format(" and d.`dictionary_number` = {0} ", number) : "");

                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`dictionary_id` in ({0}) ", keywords) : "");
                            break;
                        case "Name":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`dictionary_name` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,t.type_name from `com_dictionary` d ");
                sql.Append(" left outer join `com_dictionary_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());

                sql.Append(" order by d.type_id, d.`sort_order` desc, d.`dictionary_id` ");

                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.dictionary_id) as dataCount from `com_dictionary` d ");
                sql.Append(" left outer join `com_dictionary_type` t on d.`type_id` = t.`type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  检测名称是否存在
        public bool CheckNameIsExist(string dictionaryName, int dictionaryId)
        {
            try
            {
                return CheckDataIsExist(DBConnString, "com_dictionary", "dictionary_id", "dictionary_name", dictionaryName, dictionaryId);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  检测编码是否存在
        public bool CheckCodeIsExist(string dictionaryCode, int dictionaryId)
        {
            try
            {
                return CheckDataIsExist(DBConnString, "com_dictionary", "dictionary_id", "dictionary_code", dictionaryCode, dictionaryId);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  检测编号是否存在
        public bool CheckNumberIsExist(string dictionaryNumber, int dictionaryId, int typeId)
        {
            try
            {
                return CheckDataIsExist(DBConnString, "com_dictionary", "dictionary_id", "dictionary_number", dictionaryNumber, dictionaryId, new Dictionary<string, int>() { { "type_id", typeId } });
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        
        #region 新增分类字典
        public DataResult AddDictionary(DictionaryInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `com_dictionary`(");
                sql.Append("`type_id`,`dictionary_number`,`dictionary_name`,`dictionary_code`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?TypeId,?DictionaryNumber,?DictionaryName,?DictionaryCode,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?TypeId", "?DictionaryNumber", "?DictionaryName", "?DictionaryCode", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.TypeId, o.DictionaryNumber, o.DictionaryName, o.DictionaryCode, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dictionary", "dictionary_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新分类字典
        public DataResult UpdateDictionary(DictionaryInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `com_dictionary` set ");
                sql.Append("`type_id` = ?TypeId,`dictionary_number` = ?DictionaryNumber,`dictionary_name` = ?DictionaryName,`dictionary_code` = ?DictionaryCode,`enabled` = ?Enabled");
                sql.Append(",`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `dictionary_id` = ?DictionaryId;");

                List<string> name = new List<string>() {
					"?TypeId", "?DictionaryNumber", "?DictionaryName", "?DictionaryCode", "?Enabled", "?SortOrder", "?UpdateTime", "?DictionaryId"
				};

                List<object> value = new List<object>() {
					o.TypeId, o.DictionaryNumber, o.DictionaryName, o.DictionaryCode, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.DictionaryId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除分类字典
        public DataResult DeleteDictionary(int dictionaryId)
        {
            try
            {
                string sql = String.Format("delete from `com_dictionary` where `dictionary_id` = {0};", dictionaryId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public DictionaryInfo FillDictionaryInfo(DataRow dr)
        {
            try
            {
                DictionaryInfo o = new DictionaryInfo();

                o.DictionaryId = DataConvert.ConvertValue(dr["dictionary_id"], 0);
                o.TypeId = DataConvert.ConvertValue(dr["type_id"], 0);
                o.DictionaryNumber = DataConvert.ConvertValue(dr["dictionary_number"], 0);
                o.DictionaryName = dr["dictionary_name"].ToString();
                o.DictionaryCode = dr["dictionary_code"].ToString();
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

        public DictionaryInfo FillDictionaryInfo(DataRowView drv)
        {
            try
            {
                return this.FillDictionaryInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}