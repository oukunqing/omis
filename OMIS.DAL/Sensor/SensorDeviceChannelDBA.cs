using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{

    public class SensorDeviceChannelDBA:DataAccess
    {

        #region  获得单个传感器设备
        public DataResult GetSensorDeviceChannel(int sensorId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_device_channel` d ");
                sql.Append(String.Format(" where d.`sensor_id` = {0} ", sensorId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器设备
        public DataResult GetSensorDeviceChannel(string sensorIdList)
        {
            try
            {
                if (!CheckIdList(ref sensorIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_device_channel` d ");
                sql.Append(String.Format(" where d.`sensor_id` in({0}) ", sensorIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器设备
        public DataResult GetSensorDeviceChannel(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `dev_sensor_device_channel` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.`sensor_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`sensor_id`) as dataCount from `dev_sensor_device_channel` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增传感器设备
        public DataResult AddSensorDeviceChannel(SensorDeviceChannelInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_device_channel`(");
                sql.Append("`device_id`,`channel_no`,`sensor_type_id`,`ori_type_id`,`enabled`,`remark`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?DeviceId,?ChannelNo,?SensorTypeId,?OriTypeId,?Enabled,?Remark,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?DeviceId", "?ChannelNo", "?SensorTypeId", "?OriTypeId", "?Enabled", "?Remark", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.DeviceId, o.ChannelNo, o.SensorTypeId, o.OriTypeId, o.Enabled, o.Remark, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_device_channel", "sensor_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器设备
        public DataResult UpdateSensorDeviceChannel(SensorDeviceChannelInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_device_channel` set ");
                sql.Append("`device_id` = ?DeviceId,`channel_no` = ?ChannelNo,`sensor_type_id` = ?SensorTypeId,`ori_type_id` = ?OriTypeId,`enabled` = ?Enabled");
                sql.Append(",`remark` = ?Remark,`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `sensor_id` = ?SensorId;");

                List<string> name = new List<string>() {
					"?DeviceId", "?ChannelNo", "?SensorTypeId", "?OriTypeId", "?Enabled", "?Remark", "?SortOrder", "?UpdateTime", "?SensorId"
				};

                List<object> value = new List<object>() {
					o.DeviceId, o.ChannelNo, o.SensorTypeId, o.OriTypeId, o.Enabled, o.Remark, o.SortOrder, CheckDateTime(o.UpdateTime), o.SensorId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除传感器设备
        public DataResult DeleteSensorDeviceChannel(int sensorId)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_device_channel` where `sensor_id` = {0};", sensorId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorDeviceChannelInfo FillSensorDeviceChannelInfo(DataRow dr)
        {
            try
            {
                SensorDeviceChannelInfo o = new SensorDeviceChannelInfo();

                o.SensorId = DataConvert.ConvertValue(dr["sensor_id"], 0);
                o.DeviceId = DataConvert.ConvertValue(dr["device_id"], 0);
                o.ChannelNo = DataConvert.ConvertValue(dr["channel_no"], 0);
                o.SensorTypeId = DataConvert.ConvertValue(dr["sensor_type_id"], 0);
                o.OriTypeId = DataConvert.ConvertValue(dr["ori_type_id"], 0);
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.Remark = dr["remark"].ToString();
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();

                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorDeviceChannelInfo FillSensorDeviceChannelInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorDeviceChannelInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}
