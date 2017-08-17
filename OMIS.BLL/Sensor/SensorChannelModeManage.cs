using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorChannelModeManage : OMIS.DAL.Sensor.SensorChannelModeDBA
    {

        #region  获得单个传感器通道类型信息
        public SensorChannelModeInfo GetSensorChannelModeInfo(int modeId)
        {
            try
            {
                DataSet ds = this.GetSensorChannelMode(modeId).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorChannelModeInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道类型信息
        public List<SensorChannelModeInfo> GetSensorChannelModeInfo(string modeIdList)
        {
            try
            {
                List<SensorChannelModeInfo> list = new List<SensorChannelModeInfo>();

                DataSet ds = this.GetSensorChannelMode(modeIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorChannelModeInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道类型信息
        public List<Dictionary<string, object>> GetSensorChannelModeInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SensorChannelModeInfo info = this.FillSensorChannelModeInfo(dr);
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

        public List<Dictionary<string, object>> GetSensorChannelModeInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;
                return GetSensorChannelModeInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        public List<SensorChannelModeInfo> GetSensorChannelModeInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorChannelModeInfo> list = new List<SensorChannelModeInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorChannelModeInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorChannelModeInfo> GetSensorChannelModeInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorChannelModeInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}