using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{
    public class SensorChannelModeDBA : DataAccess
    {

        #region  获得单个传感器通道类型
        public DataResult GetSensorChannelMode(int modeId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_channel_mode` d ");
                sql.Append(String.Format(" where d.`mode_id` = {0} ", modeId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道类型
        public DataResult GetSensorChannelMode(string modeIdList)
        {
            try
            {
                if (!CheckIdList(ref modeIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_channel_mode` d ");
                sql.Append(String.Format(" where d.`mode_id` in({0}) ", modeIdList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道类型
        public DataResult GetSensorChannelMode(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `dev_sensor_channel_mode` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.`mode_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`mode_id`) as dataCount from `dev_sensor_channel_mode` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增传感器通道类型
        public DataResult AddSensorChannelMode(SensorChannelModeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_channel_mode`(");
                sql.Append("`mode_name`,`mode_code`,`enabled`,`sort_order`");
                sql.Append(")values(");
                sql.Append("?ModeName,?ModeCode,?Enabled,?SortOrder");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?ModeName", "?ModeCode", "?Enabled", "?SortOrder"
				};

                List<object> value = new List<object>() {
					o.ModeName, o.ModeCode, o.Enabled, o.SortOrder
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_channel_mode", "mode_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器通道类型
        public DataResult UpdateSensorChannelMode(SensorChannelModeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_channel_mode` set ");
                sql.Append("`mode_name` = ?ModeName,`mode_code` = ?ModeCode,`enabled` = ?Enabled,`sort_order` = ?SortOrder");
                sql.Append(" where `mode_id` = ?ModeId;");

                List<string> name = new List<string>() {
					"?ModeName", "?ModeCode", "?Enabled", "?SortOrder", "?ModeId"
				};

                List<object> value = new List<object>() {
					o.ModeName, o.ModeCode, o.Enabled, o.SortOrder, o.ModeId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 删除传感器通道类型
        public DataResult DeleteSensorChannelMode(int modeId)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_channel_mode` where `mode_id` = {0};", modeId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorChannelModeInfo FillSensorChannelModeInfo(DataRow dr)
        {
            try
            {
                SensorChannelModeInfo o = new SensorChannelModeInfo();

                o.ModeId = DataConvert.ConvertValue(dr["mode_id"], 0);
                o.ModeName = dr["mode_name"].ToString();
                o.ModeCode = dr["mode_code"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorChannelModeInfo FillSensorChannelModeInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorChannelModeInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}