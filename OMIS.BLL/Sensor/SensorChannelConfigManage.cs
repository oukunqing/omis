using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorChannelConfigManage:OMIS.DAL.Sensor.SensorChannelConfigDBA
    {

        #region  获得单个传感器通道配置信息
        public SensorChannelConfigInfo GetSensorChannelConfigInfo(int configId)
        {
            try
            {
                DataSet ds = this.GetSensorChannelConfig(configId).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorChannelConfigInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道配置信息
        public List<SensorChannelConfigInfo> GetSensorChannelConfigInfo(string configIdList)
        {
            try
            {
                List<SensorChannelConfigInfo> list = new List<SensorChannelConfigInfo>();

                DataSet ds = this.GetSensorChannelConfig(configIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorChannelConfigInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道配置信息
        public List<SensorChannelConfigInfo> GetSensorChannelConfigInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorChannelConfigInfo> list = new List<SensorChannelConfigInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorChannelConfigInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorChannelConfigInfo> GetSensorChannelConfigInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorChannelConfigInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
    }
}
