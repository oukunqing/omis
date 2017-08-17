using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{
    public class SensorChannelConfigDBA:DataAccess
    {

        #region  获得单个传感器通道配置
        public DataResult GetSensorChannelConfig(int configId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_channel_config` d ");
                sql.Append(String.Format(" where d.`config_id` = {0} ", configId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道配置
        public DataResult GetSensorChannelConfig(string configIdList)
        {
            try
            {
                if (!CheckIdList(ref configIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_channel_config` d ");
                sql.Append(String.Format(" where d.`config_id` in({0}) ", configIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道配置
        public DataResult GetSensorChannelConfig(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `dev_sensor_channel_config` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.`config_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`config_id`) as dataCount from `dev_sensor_channel_config` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增传感器通道配置
        public DataResult AddSensorChannelConfig(SensorChannelConfigInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_channel_config`(");
                sql.Append("`sensor_id`,`config_info`,`enabled`,`create_time`,`crc_code`");
                sql.Append(")values(");
                sql.Append("?SensorId,?ConfigInfo,?Enabled,?CreateTime,?CrcCode");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?SensorId", "?ConfigInfo", "?Enabled", "?CreateTime", "?CrcCode"
				};

                List<object> value = new List<object>() {
					o.SensorId, o.ConfigInfo, o.Enabled, CheckDateTime(o.CreateTime), o.CrcCode
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_channel_config", "config_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器通道配置
        public DataResult UpdateSensorChannelConfig(SensorChannelConfigInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_channel_config` set ");
                sql.Append("`sensor_id` = ?SensorId,`config_info` = ?ConfigInfo,`enabled` = ?Enabled,`update_time` = ?UpdateTime,`crc_code` = ?CrcCode");
                sql.Append(" where `config_id` = ?ConfigId;");

                List<string> name = new List<string>() {
					"?SensorId", "?ConfigInfo", "?Enabled", "?UpdateTime", "?CrcCode", "?ConfigId"
				};

                List<object> value = new List<object>() {
					o.SensorId, o.ConfigInfo, o.Enabled, CheckDateTime(o.UpdateTime), o.CrcCode, o.ConfigId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除传感器通道配置
        public DataResult DeleteSensorChannelConfig(int configId)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_channel_config` where `config_id` = {0};", configId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorChannelConfigInfo FillSensorChannelConfigInfo(DataRow dr)
        {
            try
            {
                SensorChannelConfigInfo o = new SensorChannelConfigInfo();

                o.ConfigId = DataConvert.ConvertValue(dr["config_id"], 0);
                o.SensorId = DataConvert.ConvertValue(dr["sensor_id"], 0);
                o.ConfigInfo = dr["config_info"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorChannelConfigInfo FillSensorChannelConfigInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorChannelConfigInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
