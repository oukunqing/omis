using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OMIS.Model;
using OMIS.Model.Sensor;

namespace OMIS.BLL.Sensor
{
    public class SensorParamManage : OMIS.DAL.Sensor.SensorParamDBA
    {

        #region  获得单个传感器参数信息
        public SensorParamInfo GetSensorParamInfo(int paramId)
        {
            try
            {
                DataSet ds = this.GetSensorParam(paramId).DataSet;

                return CheckTable(ds, 0) ? this.FillSensorParamInfo(ds.Tables[0].Rows[0]) : null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得多个传感器参数信息
        public List<SensorParamInfo> GetSensorParamInfo(string paramIdList)
        {
            try
            {
                List<SensorParamInfo> list = new List<SensorParamInfo>();

                DataSet ds = this.GetSensorParam(paramIdList).DataSet;
                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorParamInfo(dr));
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得传感器参数信息
        public List<Dictionary<string, object>> GetSensorParamInfo(DataSet ds, Dictionary<string, string> dicField, out int dataCount)
        {
            try
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SensorParamInfo info = this.FillSensorParamInfo(dr);
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

        public List<Dictionary<string, object>> GetSensorParamInfo(DataSet ds, Dictionary<string, string> dicField)
        {
            try
            {
                int dataCount = 0;
                return GetSensorParamInfo(ds, dicField, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        public List<SensorParamInfo> GetSensorParamInfo(DataSet ds, out int dataCount)
        {
            try
            {
                List<SensorParamInfo> list = new List<SensorParamInfo>();

                if (CheckTable(ds, 0))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(this.FillSensorParamInfo(dr));
                    }
                }
                dataCount = ConvertFieldValue(ds, 1, list.Count);

                return list;
            }
            catch (Exception ex) { throw (ex); }
        }

        public List<SensorParamInfo> GetSensorParamInfo(DataSet ds)
        {
            try
            {
                int dataCount = 0;
                return GetSensorParamInfo(ds, out dataCount);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}
