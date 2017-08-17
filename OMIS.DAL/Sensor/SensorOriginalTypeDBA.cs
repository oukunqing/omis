using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{
    public class SensorOriginalTypeDBA:DataAccess
    {

        #region  获得单个传感器通道原始值类型
        public DataResult GetSensorOriginalType(int oriTypeId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_original_type` d ");
                sql.Append(String.Format(" where d.`ori_type_id` = {0} ", oriTypeId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道原始值类型
        public DataResult GetSensorOriginalType(string oriTypeIdList)
        {
            try
            {
                if (!CheckIdList(ref oriTypeIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_original_type` d ");
                sql.Append(String.Format(" where d.`ori_type_id` in({0}) ", oriTypeIdList));
                sql.Append(" order by d.sort_order desc,d.`ori_type_id` ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道原始值类型
        public DataResult GetSensorOriginalType(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();
                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`ori_type_id` in ({0}) ", keywords) : "");
                            break;
                        case "Name":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`ori_type_name` like '%{0}%' "));
                            break;
                        case "Code":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`ori_type_code` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `dev_sensor_original_type` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.sort_order desc,d.`ori_type_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`ori_type_id`) as dataCount from `dev_sensor_original_type` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增传感器通道原始值类型
        public DataResult AddSensorOriginalType(SensorOriginalTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_original_type`(");
                sql.Append("`ori_type_name`,`ori_type_code`,`default_unit`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?OriTypeName,?OriTypeCode,?DefaultUnit,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?OriTypeName", "?OriTypeCode", "?DefaultUnit", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.OriTypeName, o.OriTypeCode, o.DefaultUnit, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_original_type", "ori_type_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器通道原始值类型
        public DataResult UpdateSensorOriginalType(SensorOriginalTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_original_type` set ");
                sql.Append("`ori_type_name` = ?OriTypeName,`ori_type_code` = ?OriTypeCode,`default_unit` = ?DefaultUnit,`enabled` = ?Enabled,`sort_order` = ?SortOrder");
                sql.Append(",`update_time` = ?UpdateTime");
                sql.Append(" where `ori_type_id` = ?OriTypeId;");

                List<string> name = new List<string>() {
					"?OriTypeName", "?OriTypeCode", "?DefaultUnit", "?Enabled", "?SortOrder", "?UpdateTime", "?OriTypeId"
				};

                List<object> value = new List<object>() {
					o.OriTypeName, o.OriTypeCode, o.DefaultUnit, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.OriTypeId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道原始值使用数量
        public int GetSensorOriginalTypeUseCount(int typeId)
        {
            return GetDataCount(DBConnString, "dev_sensor_original_type", "ori_type_id", typeId, "dev_sensor_channel", "channel_id");
        }
        #endregion

        #region 删除传感器通道原始值类型
        public DataResult DeleteSensorOriginalType(int oriTypeId)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_original_type` where `ori_type_id` = {0};", oriTypeId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorOriginalTypeInfo FillSensorOriginalTypeInfo(DataRow dr)
        {
            try
            {
                SensorOriginalTypeInfo o = new SensorOriginalTypeInfo();

                o.OriTypeId = DataConvert.ConvertValue(dr["ori_type_id"], 0);
                o.OriTypeName = dr["ori_type_name"].ToString();
                o.OriTypeCode = dr["ori_type_code"].ToString();
                o.DefaultUnit = dr["default_unit"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorOriginalTypeInfo FillSensorOriginalTypeInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorOriginalTypeInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
