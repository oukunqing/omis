using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{
    public class SensorParamDBA : DataAccess
    {

        #region  获得单个传感器参数
        public DataResult GetSensorParam(int paramId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_param` d ");
                sql.Append(String.Format(" where d.`param_id` = {0} ", paramId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器参数
        public DataResult GetSensorParam(string paramIdList)
        {
            try
            {
                if (!CheckIdList(ref paramIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_param` d ");
                sql.Append(String.Format(" where d.`param_id` in({0}) ", paramIdList));
                sql.Append(" order by d.sort_order desc,d.param_id ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器参数
        public DataResult GetSensorParam(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                int paramType = ConvertValue(dic, "ParamType", -1);
                con.Append(paramType >= 0 ? String.Format(" and d.`param_type` = {0} ", paramType) : "");
                int paramMode = ConvertValue(dic, "ParamMode", -1);
                con.Append(paramMode >= 0 ? String.Format(" and d.`param_mode` = {0} ", paramMode) : "");
                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`param_id` in ({0}) ", keywords) : "");
                            break;
                        case "Name":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`param_name` like '%{0}%' "));
                            break;
                        case "Code":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`param_code` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `dev_sensor_param` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.sort_order desc,d.`param_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`param_id`) as dataCount from `dev_sensor_param` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增传感器参数
        public DataResult AddSensorParam(SensorParamInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_param`(");
                sql.Append("`param_name`,`param_code`,`param_func`,`param_desc`,`param_type`,`param_mode`,`config_show`,`value_type`,`char_length`,`value_option`");
                sql.Append(",`default_value`,`required`,`value_sample`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?ParamName,?ParamCode,?ParamFunc,?ParamDesc,?ParamType,?ParamMode,?ConfigShow,?ValueType,?CharLength,?ValueOption");
                sql.Append(",?DefaultValue,?Required,?ValueSample,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?ParamName", "?ParamCode", "?ParamFunc", "?ParamDesc", "?ParamType", "?ParamMode", "?ConfigShow", "?ValueType", "?CharLength", "?ValueOption", 
					"?DefaultValue", "?Required", "?ValueSample", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.ParamName, o.ParamCode, o.ParamFunc, o.ParamDesc, o.ParamType, o.ParamMode, o.ConfigShow, o.ValueType, o.CharLength, o.ValueOption, 
					o.DefaultValue, o.Required, o.ValueSample, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_param", "param_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器参数
        public DataResult UpdateSensorParam(SensorParamInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_param` set ");
                sql.Append("`param_name` = ?ParamName,`param_code` = ?ParamCode,`param_func` = ?ParamFunc,`param_desc` = ?ParamDesc,`param_type` = ?ParamType");
                sql.Append(",`param_mode` = ?ParamMode,`config_show` = ?ConfigShow,`value_type` = ?ValueType,`char_length` = ?CharLength,`value_option` = ?ValueOption");
                sql.Append(",`default_value` = ?DefaultValue,`required` = ?Required,`value_sample` = ?ValueSample,`enabled` = ?Enabled,`sort_order` = ?SortOrder");
                sql.Append(",`update_time` = ?UpdateTime");
                sql.Append(" where `param_id` = ?ParamId;");

                List<string> name = new List<string>() {
					"?ParamName", "?ParamCode", "?ParamFunc", "?ParamDesc", "?ParamType", "?ParamMode", "?ConfigShow", "?ValueType", "?CharLength", "?ValueOption", 
					"?DefaultValue", "?Required", "?ValueSample", "?Enabled", "?SortOrder", "?UpdateTime", "?ParamId"
				};

                List<object> value = new List<object>() {
					o.ParamName, o.ParamCode, o.ParamFunc, o.ParamDesc, o.ParamType, o.ParamMode, o.ConfigShow, o.ValueType, o.CharLength, o.ValueOption, 
					o.DefaultValue, o.Required, o.ValueSample, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.ParamId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器参数使用数量
        public int GetSensorParamUseCount(int paramId)
        {
            return GetDataCount(DBConnString, "dev_sensor_param", "param_id", paramId, "dev_sensor_channel_param", "id");
        }
        #endregion

        #region 删除传感器参数
        public DataResult DeleteSensorParam(int paramId)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_param` where `param_id` = {0};", paramId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorParamInfo FillSensorParamInfo(DataRow dr)
        {
            try
            {
                SensorParamInfo o = new SensorParamInfo();

                o.ParamId = DataConvert.ConvertValue(dr["param_id"], 0);
                o.ParamName = dr["param_name"].ToString();
                o.ParamCode = dr["param_code"].ToString();
                o.ParamFunc = dr["param_func"].ToString();
                o.ParamDesc = dr["param_desc"].ToString();
                o.ParamType = DataConvert.ConvertValue(dr["param_type"], 0);
                o.ParamMode = DataConvert.ConvertValue(dr["param_mode"], 0);
                o.ConfigShow = DataConvert.ConvertValue(dr["config_show"], 0);
                o.ValueType = DataConvert.ConvertValue(dr["value_type"], 0);
                o.CharLength = DataConvert.ConvertValue(dr["char_length"], 0);

                o.ValueOption = dr["value_option"].ToString();
                o.DefaultValue = dr["default_value"].ToString();
                o.Required = DataConvert.ConvertValue(dr["required"], 0);
                o.ValueSample = dr["value_sample"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorParamInfo FillSensorParamInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorParamInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}