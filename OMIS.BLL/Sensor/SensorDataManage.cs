﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorDataManage:OMIS.DAL.Sensor.SensorDataDBA
    {

        #region  获得单个传感器信息
        public SensorDataInfo GetSensorDataInfo(int id)
        {
            try
            {
                DataSet ds = this.GetSensorData(id).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorDataInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器信息
        public List<Dictionary<string, object>> GetSensorDataInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SensorDataInfo info = this.FillSensorDataInfo(dr);
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

        public List<Dictionary<string, object>> GetSensorDataInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;
                return GetSensorDataInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        public List<SensorDataInfo> GetSensorDataInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorDataInfo> list = new List<SensorDataInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorDataInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorDataInfo> GetSensorDataInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorDataInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}