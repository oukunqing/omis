using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OMIS.Service.Client
{
    class ClientService
    {
        private TcpClient client;
        private NetworkStream streamToClient;
        private readonly int BufferSize = 8192;
        private byte[] buffer;
        private string remaindata = String.Empty;
        protected string strLog = string.Empty;

        public ClientService(TcpClient client)
        {
            this.client = client;
            this.strLog = "Client Connected: "
                + "Client(" + Convert.ToString(client.Client.RemoteEndPoint) + ")"
                + " -> "
                + "Server(" + Convert.ToString(client.Client.LocalEndPoint) + ").";
            Log.ServerLog.WriteEventLog("ClientConnect", this.strLog);
            // 打印连接到的客户端信息
            if (Program.isPrint)
            {
                Console.WriteLine(this.strLog);
            }

            // 获得流
            streamToClient = client.GetStream();
            buffer = new byte[BufferSize];

            // 在构造函数中就开始准备读取
            AsyncCallback callBack = new AsyncCallback(ReadComplete);
            streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null);
        }

        // 再读取完成时进行回调
        private void ReadComplete(IAsyncResult result)
        {
            int bytesRead = 0;
            try
            {
                lock (streamToClient)
                {
                    bytesRead = streamToClient.EndRead(result);
                }
                if (bytesRead == 0)
                {
                    throw new Exception("读取到0字节");
                }

                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                //显示WEB下发的每一条指令
                Log.ServerLog.WriteEventLog("ReceiveData", "收到web报文:" + msg);
                if (Program.isPrint)
                {
                    Console.WriteLine("收到web报文:" + msg);
                }

                Array.Clear(buffer, 0, buffer.Length); // 清空缓存，避免脏读
                // 遍历获得到的字符串

                TcpClient devClient = Device.DeviceStatus.GetDeviceClient("100088888");
                if (devClient != null)
                {
                    if (Program.isPrint)
                    {
                        Console.WriteLine("发送报文到设备:" + msg);
                    }
                    NetworkStream nets = devClient.GetStream();
                    byte[] bSend = UnicodeEncoding.UTF8.GetBytes(msg);
                    nets.Write(bSend, 0, bSend.Length);
                }

                string strRsp = "Response:Success";
                //byte[] bRsp = UnicodeEncoding.ASCII.GetBytes(strRsp);
                byte[] bRsp = UnicodeEncoding.UTF8.GetBytes(strRsp);
                streamToClient.Write(bRsp, 0, bRsp.Length);

                // 再次调用BeginRead()，完成时调用自身，形成无限循环
                lock (streamToClient)
                {
                    AsyncCallback callBack = new AsyncCallback(ReadComplete);
                    streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null);
                }
            }
            catch (IOException ioex)
            {
                if (streamToClient != null)
                    streamToClient.Dispose();
                if (client != null)
                    client.Close();
                Log.ServerLog.WriteErrorLog(ioex);
            }
            catch (Exception ex)
            {
                Log.ServerLog.WriteErrorLog(ex);
            }
        }
    }
}