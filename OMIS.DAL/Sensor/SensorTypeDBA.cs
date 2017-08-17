using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.DAL.Sensor
{
    public class SensorTypeDBA : DataAccess
    {

        #region  获得单个传感器分类
        public DataResult GetSensorType(int typeId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,pd.type_name as parent_name,count(distinct cd.sensor_id) as data_count ");
                sql.Append(" from `dev_sensor_type` d ");
                sql.Append(" left outer join `dev_sensor_type` pd on d.`parent_id` = pd.`sensor_type_id` ");
                sql.Append(" left outer join `dev_sensor_device_channel` cd on cd.`sensor_type_id` = d.`sensor_type_id` ");
                sql.Append(String.Format(" where d.`sensor_type_id` = {0} ", typeId));
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器分类
        public DataResult GetSensorType(string typeIdList)
        {
            try
            {
                if (!CheckIdList(ref typeIdList))
                {
                    return new DataResult();
                }
                StringBuilder sql = new StringBuilder();
                sql.Append(" select d.*,pd.type_name as parent_name,count(distinct cd.sensor_id) as data_count ");
                sql.Append(" from `dev_sensor_type` d ");
                sql.Append(" left outer join `dev_sensor_type` pd on d.`parent_id` = pd.`sensor_type_id` ");
                sql.Append(" left outer join `dev_sensor_device_channel` cd on cd.`sensor_type_id` = d.`sensor_type_id` ");
                sql.Append(String.Format(" where d.`sensor_type_id` in({0}) ", typeIdList));
                sql.Append(" group by d.`type_id` ");
                sql.Append(" order by d.level,d.sort_order desc,d.sensor_type_id ");
                sql.Append(";");

                return new DataResult(sql, Select(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
                
        #region  获得传感器分类
        public DataResult GetSensorType(Dictionary<string, object> dic)
        {
            try
            {

                #region  Condition
                StringBuilder con = new StringBuilder();

                int typeId = ConvertValue(dic, "TypeId", 0);
                con.Append(typeId > 0 ? String.Format(" and d.`sensor_type_id` = {0} ", typeId) : "");

                string typeIdList = ConvertValue(dic, "TypeIdList", "");
                con.Append(CheckIdList(ref typeIdList) ? String.Format(" and d.`sensor_type_id` in({0}) ", typeIdList) : "");

                string typeCode = ConvertValue(dic, "TypeCode");
                con.Append(Filter(ref typeCode).Length > 0 ? String.Format(" and d.type_code = '{0}' ", typeCode) : "");

                int enabled = ConvertValue(dic, "Enabled", -1);
                con.Append(enabled >= 0 ? String.Format(" and d.`enabled` = {0} ", enabled) : "");
                
                int parentId = ConvertValue(dic, "ParentId", 0);
                if (parentId > 0)
                {
                    bool isGetSubset = ConvertValue(dic, "GetSubset", 0) == 1;
                    con.Append(String.Format(isGetSubset ? " and d.parent_tree like '%({0})%' " : " and d.`parent_id` = {0} ", parentId));
                }

                int excludeId = ConvertValue(dic, "ExcludeId", 0);
                if (excludeId > 0)
                {
                    con.Append(String.Format(" and d.parent_tree not like '%({0})%' ", excludeId));
                }


                string keywords = ConvertValue(dic, "Keywords");
                if (!keywords.Equals(string.Empty))
                {
                    string searchField = ConvertValue(dic, "SearchField");
                    switch (searchField)
                    {
                        case "Id":
                            con.Append(CheckIdList(ref keywords) ? String.Format(" and d.`type_id` in ({0}) ", keywords) : "");
                            break;
                        case "Name":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`type_name` like '%{0}%' "));
                            break;
                        case "Code":
                            con.Append(DBC.BuildSearchCondition(keywords, " and d.`type_code` like '%{0}%' "));
                            break;
                    }
                }
                //TODO:

                #endregion

                #region  Sql
                StringBuilder sql = new StringBuilder();

                sql.Append(" select d.*,pd.type_name as parent_name,count(distinct cd.sensor_id) as data_count ");
                sql.Append(" from `dev_sensor_type` d ");
                sql.Append(" left outer join `dev_sensor_type` pd on d.`parent_id` = pd.`sensor_type_id` ");
                sql.Append(" left outer join `dev_sensor_device_channel` cd on cd.`sensor_type_id` = d.`sensor_type_id` ");
                sql.Append(" where 1 = 1 ");
                sql.Append(con.ToString());
                sql.Append(" group by d.`sensor_type_id` ");
                sql.Append(" order by d.level,d.sort_order desc,d.sensor_type_id ");
                sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, "PageIndex", 0), ConvertValue(dic, "PageSize", 0)));
                sql.Append(";");

                sql.Append(" select count(distinct d.`sensor_type_id`) as dataCount from `dev_sensor_type` d ");
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
        public bool CheckNameIsExist(string typeName, int sensorTypeId)
        {
            try
            {
                return CheckDataIsExist(DBConnString, "dev_sensor_type", "sensor_type_id", "type_name", typeName, sensorTypeId);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  检测编码是否存在
        public bool CheckCodeIsExist(string typeCode, int sensorTypeId)
        {
            try
            {
                return CheckDataIsExist(DBConnString, "dev_sensor_type", "sensor_type_id", "type_code", typeCode, sensorTypeId);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region 新增传感器分类
        public DataResult AddSensorType(SensorTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into `dev_sensor_type`(");
                sql.Append("`type_name`,`type_code`,`type_func`,`type_desc`,`data_unit`,`level`,`parent_id`,`parent_tree`,`enabled`,`sort_order`");
                sql.Append(",`operator_id`,`create_time`");
                sql.Append(")values(");
                sql.Append("?TypeName,?TypeCode,?TypeFunc,?TypeDesc,?DataUnit,?Level,?ParentId,?ParentTree,?Enabled,?SortOrder");
                sql.Append(",?OperatorId,?CreateTime");
                sql.Append(");");

                List<string> name = new List<string>() {
					"?TypeName", "?TypeCode", "?TypeFunc", "?TypeDesc", "?DataUnit", "?Level", "?ParentId", "?ParentTree", "?Enabled", "?SortOrder", 
					"?OperatorId", "?CreateTime"
				};

                List<object> value = new List<object>() {
					o.TypeName, o.TypeCode, o.TypeFunc, o.TypeDesc, o.DataUnit, o.Level, o.ParentId, o.ParentTree, o.Enabled, o.SortOrder, 
					o.OperatorId, CheckDateTime(o.CreateTime)
				};

                DataResult result = new DataResult(sql.ToString(), Insert(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
                result.Result = result.Result > 0 ? GetMaxId(DBConnString, "dev_sensor_type", "sensor_type_id") : 0;

                return result;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region 更新传感器分类
        public DataResult UpdateSensorType(SensorTypeInfo o)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update `dev_sensor_type` set ");
                sql.Append("`type_name` = ?TypeName,`type_code` = ?TypeCode,`type_func` = ?TypeFunc,`type_desc` = ?TypeDesc,`data_unit` = ?DataUnit");
                sql.Append(",`level` = ?Level,`parent_id` = ?ParentId,`parent_tree` = ?ParentTree,`enabled` = ?Enabled,`sort_order` = ?SortOrder");
                sql.Append(",`update_time` = ?UpdateTime");
                sql.Append(" where `sensor_type_id` = ?SensorTypeId;");

                List<string> name = new List<string>() {
					"?TypeName", "?TypeCode", "?TypeFunc", "?TypeDesc", "?DataUnit", "?Level", "?ParentId", "?ParentTree", "?Enabled", "?SortOrder", 
					"?UpdateTime", "?SensorTypeId"
				};

                List<object> value = new List<object>() {
					o.TypeName, o.TypeCode, o.TypeFunc, o.TypeDesc, o.DataUnit, o.Level, o.ParentId, o.ParentTree, o.Enabled, o.SortOrder, 
					CheckDateTime(o.UpdateTime), o.SensorTypeId
				};

                return new DataResult(sql.ToString(), Update(DBConnString, sql.ToString(), BuildSqlParam(name.Count, name, value)));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得层级
        public int GetLevel(int sensorTypeId)
        {
            return GetTypeLevel(DBConnString, "level", "dev_sensor_type", "sensor_type_id", sensorTypeId);
        }
        #endregion

        #region  更新传感器分类目录树
        public int UpdateParentTree(int sensorTypeId)
        {
            return UpdateParentTree(DBConnString, "dev_sensor_type", "sensor_type_id", sensorTypeId, 0);
        }
        public int UpdateParentTree(int sensorTypeId, int minLevel)
        {
            return UpdateParentTree(DBConnString, "dev_sensor_type", "sensor_type_id", sensorTypeId, minLevel);
        }
        #endregion


        #region  获得传感器分类子类及传感器数量
        public int[] GetSensorDataCount(int typeId)
        {
            try
            {
                return GetChildCountAndDataCount(DBConnString, "dev_sensor_type", "sensor_type_id", typeId, "dev_sensor_device_channel", "sensor_id");
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
        
        #region 删除传感器分类
        public DataResult DeleteSensorType(int typeId)
        {
            try
            {
                string sql = String.Format("delete from `dev_sensor_type` where `sensor_type_id` = {0};", typeId);

                return new DataResult(sql, Delete(DBConnString, sql));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  填充数据
        public SensorTypeInfo FillSensorTypeInfo(DataRow dr)
        {
            try
            {
                SensorTypeInfo o = new SensorTypeInfo();

                o.SensorTypeId = DataConvert.ConvertValue(dr["sensor_type_id"], 0);
                o.TypeName = dr["type_name"].ToString();
                o.TypeCode = dr["type_code"].ToString();
                o.TypeFunc = dr["type_func"].ToString();
                o.TypeDesc = dr["type_desc"].ToString();
                o.DataUnit = dr["data_unit"].ToString();
                o.Level = DataConvert.ConvertValue(dr["level"], 0);
                o.ParentId = DataConvert.ConvertValue(dr["parent_id"], 0);
                o.ParentTree = dr["parent_tree"].ToString();
                o.Enabled = DataConvert.ConvertValue(dr["enabled"], 0);

                o.SortOrder = DataConvert.ConvertValue(dr["sort_order"], 0);
                o.OperatorId = DataConvert.ConvertValue(dr["operator_id"], 0);
                o.CreateTime = dr["create_time"].ToString();
                o.UpdateTime = dr["update_time"].ToString();

                if (CheckColumn(dr, "parent_name"))
                {
                    o.Extend = new Dictionary<string, object>()
                    {
                        {"ParentName", dr["parent_name"].ToString()},
                        {"DataCount", DataConvert.ConvertValue(dr["data_count"], 0)}
                    };
                }

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }

        public SensorTypeInfo FillSensorTypeInfo(DataRowView drv)
        {
            try
            {
                return this.FillSensorTypeInfo(drv.Row);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}