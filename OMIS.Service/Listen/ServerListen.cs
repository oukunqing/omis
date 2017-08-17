using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OMIS.Service.Listen
{
    class ServerListen
    {

        #region  监听设备请求
        public void BeginListenDeviceRequest(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            Log.ServerLog.WriteEventLog("BeginListen", "Device Server:" + port + " is start listening.");

            try
            {
                listener.Start();
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    new Device.DeviceService(client);

                    //记录日志

                }
            }
                
            catch (Exception ex)
            {
                Console.WriteLine("错误:" + ex.Message);
                Log.ServerLog.WriteErrorLog(ex);

                listener.Stop();
                Thread.Sleep(1000);
                //重新开启监听
                BeginListenDeviceRequest(port);
            }
        }
        #endregion

        #region  监听客户端请求
        public void BeginListenClientRequest(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            Log.ServerLog.WriteEventLog("BeginListen", "Client Server:" + port + " is start listening.");

            try
            {
                listener.Start();
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    new Client.ClientService(client);

                    //记录日志
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误:" + ex.Message);
                Log.ServerLog.WriteErrorLog(ex);

                listener.Stop();
                Thread.Sleep(1000);
                //重新开启监听
                BeginListenClientRequest(port);
            }
        }
        #endregion

    }
}