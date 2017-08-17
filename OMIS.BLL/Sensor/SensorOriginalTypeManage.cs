using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorOriginalTypeManage:OMIS.DAL.Sensor.SensorOriginalTypeDBA
    {

        #region  获得单个传感器通道原始值类型信息
        public SensorOriginalTypeInfo GetSensorOriginalTypeInfo(int oriTypeId)
        {
            try
            {
                DataSet ds = this.GetSensorOriginalType(oriTypeId).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorOriginalTypeInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道原始值类型信息
        public List<SensorOriginalTypeInfo> GetSensorOriginalTypeInfo(string oriTypeIdList)
        {
            try
            {
                List<SensorOriginalTypeInfo> list = new List<SensorOriginalTypeInfo>();

                DataSet ds = this.GetSensorOriginalType(oriTypeIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorOriginalTypeInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道原始值类型信息
        public List<Dictionary<string, object>> GetSensorOriginalTypeInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SensorOriginalTypeInfo info = this.FillSensorOriginalTypeInfo(dr);
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

        public List<Dictionary<string, object>> GetSensorOriginalTypeInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;
                return GetSensorOriginalTypeInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        public List<SensorOriginalTypeInfo> GetSensorOriginalTypeInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorOriginalTypeInfo> list = new List<SensorOriginalTypeInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorOriginalTypeInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorOriginalTypeInfo> GetSensorOriginalTypeInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorOriginalTypeInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
