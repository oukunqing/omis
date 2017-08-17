using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{
    public class SensorChannelDBA:DataAccess
    {

        #region  获得单个传感器通道
        public DataResult GetSensorChannel(int channelId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,m.mode_name,t.ori_type_name from `dev_sensor_channel` d ");
                sql.Append(" left outer join `dev_sensor_channel_mode` m on d.`mode_id` = m.`mode_id` ");
                sql.Append(" left outer join `dev_sensor_original_type` t on d.`ori_type_id` = t.`ori_type_id` ");
                sql.Append(String.Format(" where d.`channel_id` = {0} ", channelId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道
        public DataResult GetSensorChannel(string channelIdList)
        {
            try
            {
                if (!CheckIdList(ref channelIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,m.mode_name,t.ori_type_name from `dev_sensor_channel` d ");
                sql.Append(" left outer join `dev_sensor_channel_mode` m on d.`mode_id` = m.`mode_id` ");
                sql.Append(" left outer join `dev_sensor_original_type` t on d.`ori_type_id` = t.`ori_type_id` ");
                sql.Append(String.Format(" where d.`channel_id` in({0}) ", channelIdList));
                sql.Append(" order by d.sort_order desc,d.`channel_id` ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道
        public DataResult GetSensorChannel(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                int channelType = ConvertValue(dic, "ChannelType", -1);
                con.Append(channelType >= 0 ? String.Format(" and d.`channel_type` = {0} ", channelType) : "");
                int modeId = ConvertValue(dic, "ModeId", -1);
                con.Append(modeId >= 0 ? String.Format(" and d.`mode_id` = {0} ", modeId) : "");
                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");

                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`channel_id` in ({0}) ", keywords) : "");
                            break;
                        case "Number":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`channel_no` in ({0}) ", keywords) : "");
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,m.mode_name,t.ori_type_name from `dev_sensor_channel` d ");
                sql.Append(" left outer join `dev_sensor_channel_mode` m on d.`mode_id` = m.`mode_id` ");
                sql.Append(" left outer join `dev_sensor_original_type` t on d.`ori_type_id` = t.`ori_type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.sort_order desc,d.`channel_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`channel_id`) as dataCount from `dev_sensor_channel` d ");
                sql.Append(" left outer join `dev_sensor_channel_mode` m on d.`mode_id` = m.`mode_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增传感器通道
        public DataResult AddSensorChannel(SensorChannelInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_channel`(");
                sql.Append("`channel_no`,`channel_type`,`channel_group`,`mode_id`,`ori_type_id`,`remark`,`enabled`,`sort_order`,`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?ChannelNo,?ChannelType,?ChannelGroup,?ModeId,?OriTypeId,?Remark,?Enabled,?SortOrder,?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?ChannelNo", "?ChannelType", "?ChannelGroup", "?ModeId", "?OriTypeId", "?Remark", "?Enabled", "?SortOrder", "?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.ChannelNo, o.ChannelType, o.ChannelGroup, o.ModeId, o.OriTypeId, o.Remark, o.Enabled, o.SortOrder, o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_channel", "channel_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器通道
        public DataResult UpdateSensorChannel(SensorChannelInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_channel` set ");
                sql.Append("`channel_no` = ?ChannelNo,`channel_type` = ?ChannelType,`channel_group` = ?ChannelGroup,`mode_id` = ?ModeId,`ori_type_id` = ?OriTypeId");
                sql.Append(",`remark` = ?Remark,`enabled` = ?Enabled,`sort_order` = ?SortOrder,`update_time` = ?UpdateTime");
                sql.Append(" where `channel_id` = ?ChannelId;");

                List<string> name = new List<string>() {
					"?ChannelNo", "?ChannelType", "?ChannelGroup", "?ModeId", "?OriTypeId", "?Remark", "?Enabled", "?SortOrder", "?UpdateTime", "?ChannelId"
				};

                List<object> value = new List<object>() {
					o.ChannelNo, o.ChannelType, o.ChannelGroup, o.ModeId, o.OriTypeId, o.Remark, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.ChannelId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region 删除传感器通道
        public DataResult DeleteSensorChannel(int channelId)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_channel` where `channel_id` = {0};", channelId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorChannelInfo FillSensorChannelInfo(DataRow dr)
        {
            try
            {
                SensorChannelInfo o = new SensorChannelInfo();

                o.ChannelId = DataConvert.ConvertValue(dr["channel_id"], 0);
                o.ChannelNo = DataConvert.ConvertValue(dr["channel_no"], 0);
                o.ChannelType = DataConvert.ConvertValue(dr["channel_type"], 0);
                o.ChannelGroup = DataConvert.ConvertValue(dr["channel_group"], 0);
                o.ModeId = DataConvert.ConvertValue(dr["mode_id"], 0);
                o.OriTypeId = DataConvert.ConvertValue(dr["ori_type_id"], 0);
                o.Remark = dr["remark"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);

                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();
                
                if (CheckColumn(dr, "mode_name"))
                {
                    o.Extend.Add("ModeName", dr["mode_name"].ToString());
                }
                if (CheckColumn(dr, "ori_type_name"))
                {
                    o.Extend.Add("OriTypeName", dr["ori_type_name"].ToString());
                }

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorChannelInfo FillSensorChannelInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorChannelInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
