using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace OMIS.Server.Device
{
    class DeviceStatus
    {
        public static Dictionary<string, TcpClient> dicDev = new Dictionary<string, TcpClient>();

        public static void AddDevice(string strDevCode, TcpClient client)
        {
            if (!dicDev.ContainsKey(strDevCode))
            {
                dicDev.Add(strDevCode, client);
            }
        }

        public static void DeleteDevice(string strDevCode)
        {
            if (!dicDev.ContainsKey(strDevCode))
            {
                dicDev.Remove(strDevCode);
            }
        }

        public static TcpClient GetDeviceClient(string strDevCode)
        {
            if (dicDev.ContainsKey(strDevCode))
            {
                return dicDev[strDevCode];
            }
            return null;
        }
    }
}
