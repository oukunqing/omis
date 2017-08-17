﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorDeviceVersionManage : OMIS.DAL.Sensor.SensorDeviceVersionDBA
    {

        #region  获得单个传感器设备版本信息
        public SensorDeviceVersionInfo GetSensorDeviceVersionInfo(int versionId)
        {
            try
            {
                DataSet ds = this.GetSensorDeviceVersion(versionId).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorDeviceVersionInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器设备版本信息
        public List<SensorDeviceVersionInfo> GetSensorDeviceVersionInfo(string versionIdList)
        {
            try
            {
                List<SensorDeviceVersionInfo> list = new List<SensorDeviceVersionInfo>();

                DataSet ds = this.GetSensorDeviceVersion(versionIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorDeviceVersionInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器设备版本信息
        public List<Dictionary<string, object>> GetSensorDeviceVersionInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SensorDeviceVersionInfo info = this.FillSensorDeviceVersionInfo(dr);
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

        public List<Dictionary<string, object>> GetSensorDeviceVersionInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;
                return GetSensorDeviceVersionInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        public List<SensorDeviceVersionInfo> GetSensorDeviceVersionInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorDeviceVersionInfo> list = new List<SensorDeviceVersionInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorDeviceVersionInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorDeviceVersionInfo> GetSensorDeviceVersionInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorDeviceVersionInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}