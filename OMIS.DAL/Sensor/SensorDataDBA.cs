using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{
    public class SensorDataDBA : DataAccess
    {


        #region  获得单个传感器
        public DataResult GetSensorData(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_data` d ");
                sql.Append(String.Format(" where d.`id` = {0} ", id));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器
        public DataResult GetSensorData(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();
                string devCode = ConvertValue(dic, "DeviceCode");
                con.Append(!devCode.Equals(string.Empty) ? String.Format(" and d.`device_code` = '{0}' ", Filter(devCode)) : "");

                int channelNo = ConvertValue(dic, "ChannelNo", -1);
                con.Append(channelNo > 0 ? String.Format(" and d.`channel_no` = {0} ", channelNo) : "");

                int dataType = ConvertValue(dic, "DataType", -1);
                con.Append(dataType >= 0 ? String.Format(" and d.`data_type` = {0} ", dataType) : "");

                string sensorCode = ConvertValue(dic, "SensorCode");
                con.Append(!sensorCode.Equals(string.Empty) ? String.Format(" and d.`sensor_code` = '{0}' ", Filter(sensorCode)) : "");

                string startTime = ConvertValue(dic, "StartTime");
                string endTime = ConvertValue(dic, "EndTime");
                if (startTime.Length > 0 && endTime.Length > 0)
                {
                    con.Append(String.Format(" and d.`collect_time` between '{0}' and '{1}' ", startTime, endTime));
                }
                else if (startTime.Length > 0)
                {
                    con.Append(String.Format(" and d.`collect_time` >= '{0}' ", startTime));
                }
                else if (endTime.Length > 0)
                {
                    con.Append(String.Format(" and d.`collect_time` <= '{0}' ", endTime));
                }

                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "ChannelNo":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`channel_no` in ({0}) ", keywords) : "");
                            break;
                        case "DeviceCode":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`device_code` like '%{0}%' "));
                            break;
                        case "SensorCode":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`sensor_code` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,dt.type_name,dt.data_unit from `dev_sensor_data` d ");
                sql.Append(" left outer join `dev_sensor_type` dt on d.sensor_code = dt.type_code ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.`collect_time` desc,d.`id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`id`) as dataCount from `dev_sensor_data` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增传感器数据
        public DataResult AddSensorData(SensorDataInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_data`(");
                sql.Append("`device_code`,`channel_no`,`data_type`,`sensor_code`,`sensor_value`,`original_value`,`collect_time`,`upload_time`,`create_time`");
                sql.Append(")values(");
                sql.Append("?DeviceCode,?ChannelNo,?DataType,?SensorCode,?SensorValue,?OriginalValue,?CollectTime,?UploadTime,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?DeviceCode", "?ChannelNo", "?DataType", "?SensorCode", "?SensorValue", "?OriginalValue", "?CollectTime", "?UploadTime", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.DeviceCode, o.ChannelNo, o.DataType, o.SensorCode, o.SensorValue, o.OriginalValue, o.CollectTime, o.UploadTime, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_data", "id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  批量新增传感器数据
        public int BatchAddSensorData(List<SensorDataInfo> list)
        {
            try
            {
                if (list.Count <= 0)
                {
                    return 0;
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_data`(");
                sql.Append("`device_code`,`channel_no`,`data_type`,`sensor_code`,`sensor_value`,`original_value`,`collect_time`,`upload_time`,`create_time`");
                sql.Append(")values");

                int n = 0;
                foreach (SensorDataInfo o in list)
                {
                    sql.Append(n++ > 0 ? "," : "");
                    sql.Append("(");
                    sql.Append(String.Format("'{0}',{1},{2},'{3}',{4},{5},'{6}','{7}','{8}'",
                        Filter(o.DeviceCode), o.ChannelNo, o.DataType, Filter(o.SensorCode), o.SensorValue, o.OriginalValue, 
                        CheckDateTime(o.CollectTime), CheckDateTime(o.UploadTime), CheckDateTime(o.CreateTime)));
                    sql.Append(")");
                }
                sql.Append(";");

                return Insert(DBConnString, sql.ToString());
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorDataInfo FillSensorDataInfo(DataRow dr)
        {
            try
            {
                SensorDataInfo o = new SensorDataInfo();

                o.Id = DataConvert.ConvertValue(dr["id"], 0);
                o.DeviceCode = dr["device_code"].ToString();
                o.ChannelNo = DataConvert.ConvertValue(dr["channel_no"], 0);
                o.DataType = DataConvert.ConvertValue(dr["data_type"], 0);
                o.SensorCode = dr["sensor_code"].ToString();
                o.SensorValue = DataConvert.ConvertValue(dr["sensor_value"], 0.0m);
                o.OriginalValue = DataConvert.ConvertValue(dr["original_value"], 0.0m);
                o.CollectTime = dr["collect_time"].ToString();
                o.UploadTime = dr["upload_time"].ToString();
                o.CreateTime = dr["create_time"].ToString();

                if (CheckColumn(dr, "type_name"))
                {
                    o.Extend.Add("TypeName", dr["type_name"].ToString());
                }

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorDataInfo FillSensorDataInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorDataInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}