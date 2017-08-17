using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorChannelManage:OMIS.DAL.Sensor.SensorChannelDBA
    {

        #region  获得单个传感器通道信息
        public SensorChannelInfo GetSensorChannelInfo(int channelId)
        {
            try
            {
                DataSet ds = this.GetSensorChannel(channelId).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorChannelInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道信息
        public List<SensorChannelInfo> GetSensorChannelInfo(string channelIdList)
        {
            try
            {
                List<SensorChannelInfo> list = new List<SensorChannelInfo>();

                DataSet ds = this.GetSensorChannel(channelIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorChannelInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道信息
        public List<Dictionary<string, object>> GetSensorChannelInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SensorChannelInfo info = this.FillSensorChannelInfo(dr);
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

        public List<Dictionary<string, object>> GetSensorChannelInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;
                return GetSensorChannelInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        public List<SensorChannelInfo> GetSensorChannelInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorChannelInfo> list = new List<SensorChannelInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorChannelInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorChannelInfo> GetSensorChannelInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorChannelInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
