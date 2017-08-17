using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;
using OMIS.Common;

namespace OMIS.DAL.Sensor
{
    public class SensorDeviceVersionDBA : DataAccess
    {

        #region  获得单个传感器设备版本
        public DataResult GetSensorDeviceVersion(int versionId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_device_version` d ");
                sql.Append(String.Format(" where d.`version_id` = {0} ", versionId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器设备版本
        public DataResult GetSensorDeviceVersion(string versionIdList)
        {
            try
            {
                if (!CheckIdList(ref versionIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.* from `dev_sensor_device_version` d ");
                sql.Append(String.Format(" where d.`version_id` in({0}) ", versionIdList));
                sql.Append(" order by d.`version_id` ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器设备版本
        public DataResult GetSensorDeviceVersion(Dictionary<string, object> dic)
        {
            try
            {
                #region  Condition
                StringBuilder con = new StringBuilder();


                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`version_id` in ({0}) ", keywords) : "");
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.* from `dev_sensor_device_version` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" order by d.`version_id` ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`version_id`) as dataCount from `dev_sensor_device_version` d ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(";");
                #endregion

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
                
        #region 新增传感器设备版本
        public DataResult AddSensorDeviceVersion(SensorDeviceVersionInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_device_version`(");
                sql.Append("`version_code`,`version_config`,`version_desc`,`enabled`,`sort_order`,`create_time`,`crc_code`");
                sql.Append(")values(");
                sql.Append("?VersionCode,?VersionConfig,?VersionDesc,?Enabled,?SortOrder,?CreateTime,?CrcCode");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?VersionCode", "?VersionConfig", "?VersionDesc", "?Enabled", "?SortOrder", "?CreateTime", "?CrcCode"
				};

                BuildCrcCode(o, true);

                List<object> value = new List<object>() {
					o.VersionCode, o.VersionConfig, o.VersionDesc, o.Enabled, o.SortOrder, CheckDateTime(o.CreateTime), o.CrcCode
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_device_version", "version_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器设备版本
        public DataResult UpdateSensorDeviceVersion(SensorDeviceVersionInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_device_version` set ");
                sql.Append("`version_code` = ?VersionCode,`version_config` = ?VersionConfig,`version_desc` = ?VersionDesc,`enabled` = ?Enabled,`sort_order` = ?SortOrder");
                sql.Append(",`update_time` = ?UpdateTime,`crc_code` = ?CrcCode");
                sql.Append(" where `version_id` = ?VersionId;");

                List<string> name = new List<string>() {
					"?VersionCode", "?VersionConfig", "?VersionDesc", "?Enabled", "?SortOrder", "?UpdateTime", "?CrcCode", "?VersionId"
				};

                BuildCrcCode(o, true);

                List<object> value = new List<object>() {
					o.VersionCode, o.VersionConfig, o.VersionDesc, o.Enabled, o.SortOrder, CheckDateTime(o.UpdateTime), o.CrcCode, o.VersionId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
                        
        #region 删除传感器设备版本
        public DataResult DeleteSensorDeviceVersion(int versionId)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_device_version` where `version_id` = {0};", versionId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorDeviceVersionInfo FillSensorDeviceVersionInfo(DataRow dr)
        {
            try
            {
                SensorDeviceVersionInfo o = new SensorDeviceVersionInfo();

                o.VersionId = DataConvert.ConvertValue(dr["version_id"], 0);
                o.VersionCode = dr["version_code"].ToString();
                o.VersionConfig = dr["version_config"].ToString();
                o.VersionDesc = dr["version_desc"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);
                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();
                o.CrcCode = dr["crc_code"].ToString();

                return CheckCrcCode(o, o.CrcCode) ? o : DataCheck.AlwaysPass ? o : null;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorDeviceVersionInfo FillSensorDeviceVersionInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorDeviceVersionInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  BuildCrcCode
        private string BuildCrcCode(SensorDeviceVersionInfo o, bool isEncrypt)
        {
            if (o != null)
            {
                o.VersionCode = isEncrypt ? UCAuthCode.AuthCodeEncode(o.VersionCode, KeyApi.OMIS_KEY) : o.VersionCode;
                o.VersionConfig = isEncrypt ? UCAuthCode.AuthCodeEncode(o.VersionConfig, KeyApi.OMIS_KEY) : o.VersionConfig;
                o.VersionDesc = isEncrypt ? UCAuthCode.AuthCodeEncode(o.VersionDesc, KeyApi.OMIS_KEY) : o.VersionDesc;

                string con = String.Format("{0}-{1}-{2}-ZYRH", o.VersionCode, o.VersionConfig, o.Enabled);
                o.CrcCode = CRC.ToCRC16(con, Encoding.UTF8);

                return o.CrcCode;
            }
            return string.Empty;
        }
        #endregion

        #region  CheckCrcCode
        private bool CheckCrcCode(SensorDeviceVersionInfo o, string crcCode)
        {
            bool pass = !crcCode.Trim().Equals(string.Empty) && this.BuildCrcCode(o, false).Equals(crcCode);
            
            o.VersionCode = UCAuthCode.AuthCodeDecode(o.VersionCode, KeyApi.OMIS_KEY);
            o.VersionConfig = UCAuthCode.AuthCodeDecode(o.VersionConfig, KeyApi.OMIS_KEY);
            o.VersionDesc = UCAuthCode.AuthCodeDecode(o.VersionDesc, KeyApi.OMIS_KEY);

            return pass;
        }
        #endregion

    }
}