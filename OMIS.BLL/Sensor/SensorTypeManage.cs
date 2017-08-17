using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorTypeManage:OMIS.DAL.Sensor.SensorTypeDBA
    {

        #region  获得单个传感器分类信息
        public SensorTypeInfo GetSensorTypeInfo(int typeId)
        {
            try
            {
                DataSet ds = this.GetSensorType(typeId).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorTypeInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器分类信息
        public List<SensorTypeInfo> GetSensorTypeInfo(string typeIdList)
        {
            try
            {
                List<SensorTypeInfo> list = new List<SensorTypeInfo>();

                DataSet ds = this.GetSensorType(typeIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorTypeInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器分类信息
        public List<Dictionary<string, object>> GetSensorTypeInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SensorTypeInfo info = this.FillSensorTypeInfo(dr);
                        if (info != null)
                        {
                            list.Add(ConvertClassValue(info, dicField, true));
                        }
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<Dictionary<string, object>> GetSensorTypeInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;
                return GetSensorTypeInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        public List<SensorTypeInfo> GetSensorTypeInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorTypeInfo> list = new List<SensorTypeInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorTypeInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorTypeInfo> GetSensorTypeInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorTypeInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}