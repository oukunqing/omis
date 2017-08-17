using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorDeviceChannelManage:OMIS.DAL.Sensor.SensorDeviceChannelDBA
    {

        #region  获得单个传感器设备信息
        public SensorDeviceChannelInfo GetSensorDeviceChannelInfo(int sensorId)
        {
            try
            {
                DataSet ds = this.GetSensorDeviceChannel(sensorId).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorDeviceChannelInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器设备信息
        public List<SensorDeviceChannelInfo> GetSensorDeviceChannelInfo(string sensorIdList)
        {
            try
            {
                List<SensorDeviceChannelInfo> list = new List<SensorDeviceChannelInfo>();

                DataSet ds = this.GetSensorDeviceChannel(sensorIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorDeviceChannelInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器设备信息
        public List<SensorDeviceChannelInfo> GetSensorDeviceChannelInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorDeviceChannelInfo> list = new List<SensorDeviceChannelInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorDeviceChannelInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorDeviceChannelInfo> GetSensorDeviceChannelInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorDeviceChannelInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}