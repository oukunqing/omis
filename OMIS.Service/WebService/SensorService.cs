using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMIS.Service.SensorWS;

namespace OMIS.Service
{
    public class SensorService
    {

        #region  更新设备状态
        public static bool UpdateDeviceStatus(string DeviceCode, int Status)
        {
            try
            {
                return new ZyrhSensorService().UpdateDeviceStatus(new string[] { DeviceCode }, Status);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static bool UpdateDeviceStatus(List<string> DeviceCodeList, int Status)
        {
            try
            {
                return new ZyrhSensorService().UpdateDeviceStatus(DeviceCodeList.ToArray(), Status);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  更新设备心跳
        public static bool UpdateDeviceHeartbeat(string DeviceCode, string Time)
        {
            try
            {
                return new ZyrhSensorService().UpdateDeviceHeartbeatTime(DeviceCode, Time);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  上传传感器数据
        public static bool UploadSensorData(string DeviceCode, List<string> DataList)
        {
            try
            {
                return new ZyrhSensorService().UploadSensorDataInfo(DeviceCode, DataList.ToArray());
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


    }
}
