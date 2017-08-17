using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{
    public class SensorChannelParamDBA:DataAccess
    {

        #region  获得单个传感器通道-参数配置
        public DataResult GetSensorChannelParam(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_channel_param` d ");
                sql.Append(String.Format(" where d.`id` = {0} ", id));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道-参数配置
        public DataResult GetSensorChannelParam(string idList)
        {
            try
            {
                if (!CheckIdList(ref idList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_channel_param` d ");
                sql.Append(String.Format(" where d.`id` in({0}) ", idList));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道-参数配置
        public DataResult GetSensorChannelParam(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();

                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `dev_sensor_channel_param` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.`id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`id`) as dataCount from `dev_sensor_channel_param` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 新增传感器通道-参数配置
        public DataResult AddSensorChannelParam(SensorChannelParamInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_channel_param`(");
                sql.Append("`param_id`,`channel_no`,`channel_group`");
                sql.Append(")values(");
                sql.Append("?ParamId,?ChannelNo,?ChannelGroup");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?ParamId", "?ChannelNo", "?ChannelGroup"
				};

                List<object> value = new List<object>() {
					o.ParamId, o.ChannelNo, o.ChannelGroup
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_channel_param", "id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器通道-参数配置
        public DataResult UpdateSensorChannelParam(SensorChannelParamInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_channel_param` set ");
                sql.Append("`param_id` = ?ParamId,`channel_no` = ?ChannelNo,`channel_group` = ?ChannelGroup");
                sql.Append(" where `id` = ?Id;");

                List<string> name = new List<string>() {
					"?ParamId", "?ChannelNo", "?ChannelGroup", "?Id"
				};

                List<object> value = new List<object>() {
					o.ParamId, o.ChannelNo, o.ChannelGroup, o.Id
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        

        #region  获得通道-参数配置
        public DataResult GetChannelParamConfig(Dictionary<string, object> dic)
        {
            try
            {
                int channelNo = ConvertValue(dic, "ChannelNo", 0);
                if (channelNo > 0)
                {
                    int paramType = ConvertValue(dic, "ParamType", -1);

                    #region  Sql
                    StringBuilder sql = new StringBuilder();
                    //通道
                    sql.Append(" select r.channel_id,r.channel_no from `dev_sensor_channel` r ");
                    sql.Append(channelNo > 0 ? String.Format(" where r.channel_no = {0} ", channelNo) : "");
                    sql.Append(";");
                    //参数
                    sql.Append(" select d.param_id,d.param_name,d.param_code,d.param_func,d.param_desc,d.param_type,d.param_mode ");
                    sql.Append(" from `dev_sensor_param` d ");
                    sql.Append(" where 1 = 1 ");
                    sql.Append(paramType >= 0 ? String.Format(" and d.param_type = {0} ", paramType) : "");
                    sql.Append(";");
                    //通道-参数
                    sql.Append(" select r.channel_no,d.param_id from dev_sensor_channel_param rm,dev_sensor_channel r,dev_sensor_param d ");
                    sql.Append(" where rm.channel_no = r.channel_no and rm.param_id = d.param_id ");
                    sql.Append(channelNo > 0 ? String.Format(" and r.channel_no = {0} ", channelNo) : "");
                    sql.Append(";");

                    #endregion

                    return new DataResult(sql, Select(DBConnString, sql));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  批量新增通道-参数配置
        public DataResult BatchAddChannelParam(List<SensorChannelParamInfo> list)
        {
            try
            {
                if (list.Count > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into `dev_sensor_channel_param`(");
                    sql.Append("`channel_no`,`channel_no`,`channel_group`");
                    sql.Append(")values");

                    int n = 0;
                    foreach (SensorChannelParamInfo o in list)
                    {
                        sql.Append(n++ > 0 ? "," : "");
                        sql.Append("(");
                        sql.Append(String.Format("{0},{1},{2}", o.ParamId, o.ChannelNo, o.ChannelGroup));
                        sql.Append(")");
                    }
                    sql.Append(";");

                    return new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString()));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }

        public DataResult BatchAddChannelParam(int channelNo, string paramIdList, int channelGroup, string createTime)
        {
            try
            {
                if (channelNo > 0 && CheckIdList(ref paramIdList))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("insert into `dev_sensor_channel_param`(");
                    sql.Append("`channel_no`,`param_id`,`channel_group`,`create_time`");
                    sql.Append(")values");

                    string[] list = paramIdList.Split(',');
                    int n = 0;
                    foreach (string id in list)
                    {
                        sql.Append(n++ > 0 ? "," : "");
                        sql.Append("(");
                        sql.Append(String.Format("{0},{1},{2},'{3}'", channelNo, id, channelGroup, createTime));
                        sql.Append(")");
                    }
                    sql.Append(";");

                    return new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString()));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region 删除传感器通道-参数配置
        public DataResult DeleteSensorChannelParam(int id)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_channel_param` where `id` = {0};", id);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }

        public DataResult DeleteSensorChannelParam(int channelNo, int paramId)
        {
            try
            {
                if (channelNo > 0 || paramId > 0)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("delete from `dev_sensor_channel_param` where 1 = 1 ");
                    sql.Append(channelNo > 0 ? String.Format(" and channel_no = {0} ", channelNo) : "");
                    sql.Append(paramId > 0 ? String.Format(" and param_id = {0} ", paramId) : "");

                    return new DataResult(sql.ToString(), Delete(DBConnString, sql.ToString()));
                }
                return new DataResult();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorChannelParamInfo FillSensorChannelParamInfo(DataRow dr)
        {
            try
            {
                SensorChannelParamInfo o = new SensorChannelParamInfo();

                o.Id = DataConvert.ConvertValue(dr["id"], 0);
                o.ParamId = DataConvert.ConvertValue(dr["param_id"], 0);
                o.ChannelNo = DataConvert.ConvertValue(dr["channel_no"], 0);
                o.ChannelGroup = DataConvert.ConvertValue(dr["channel_group"], 0);

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorChannelParamInfo FillSensorChannelParamInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorChannelParamInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}