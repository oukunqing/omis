using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorChannelParamManage : OMIS.DAL.Sensor.SensorChannelParamDBA
    {
        #region  获得单个传感器通道-参数配置信息
        public SensorChannelParamInfo GetSensorChannelParamInfo(int id)
        {
            try
            {
                DataSet ds = this.GetSensorChannelParam(id).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorChannelParamInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器通道-参数配置信息
        public List<SensorChannelParamInfo> GetSensorChannelParamInfo(string idList)
        {
            try
            {
                List<SensorChannelParamInfo> list = new List<SensorChannelParamInfo>();

                DataSet ds = this.GetSensorChannelParam(idList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorChannelParamInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器通道-参数配置信息
        public List<SensorChannelParamInfo> GetSensorChannelParamInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorChannelParamInfo> list = new List<SensorChannelParamInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorChannelParamInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorChannelParamInfo> GetSensorChannelParamInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorChannelParamInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion
    }
}