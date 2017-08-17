using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using OMIS.Service.Common;

namespace OMIS.Service.Device
{
    class DeviceService
    {
        protected TcpClient DevClient = null;
        protected NetworkStream DevStream = null;
        protected readonly int BufferSize = 8192;
        protected byte[] Buffer = null;

        protected string DevCode = string.Empty;
        //设备是否启用
        protected bool DevEnabled = true;
        //是否首次数据
        protected bool IsFirstMsg = true;
        //消息栈
        protected string MsgStack = string.Empty;

        //设备超时时间，单位：秒
        protected int DeviceTimeout = Common.Config.GetAppSetting("DeviceTimeout", 60);

        protected string MsgLog = string.Empty;

        public DeviceService(TcpClient client)
        {
            DevClient = client;
            MsgLog = "Device Connected: "
                + "Device(" + Convert.ToString(client.Client.RemoteEndPoint) + ")"
                + " -> "
                + "Server(" + Convert.ToString(client.Client.LocalEndPoint) + ").";

            Log.ServerLog.WriteEventLog("DeviceConnect", MsgLog);
            
            new Thread(ReceiveClientMessage).Start();
        }


        #region  接收客户端消息报文
        private void ReceiveClientMessage()
        {
            try
            {
                DevStream = DevClient.GetStream();
                DevStream.ReadTimeout = DeviceTimeout * 1000;
                Buffer = new byte[BufferSize];
            }
            catch (Exception ex)
            {
                Log.ServerLog.WriteErrorLog(ex);
                return;
            }

            while (DevEnabled)
            {
                try
                {
                    int rcvData = DevStream.Read(Buffer, 0, BufferSize);
                    if (rcvData > 0)
                    {
                        string rcvMsg = Encoding.Default.GetString(Buffer, 0, rcvData);

                        Log.ServerLog.WriteEventLog("ReceivedMessage", rcvMsg);

                        //需要上传的传感器数据列表
                        List<string> DataList = new List<string>();

                        string rspMsg = string.Empty;

                        List<string> MsgList = MessageCheck.CheckMessageFormat(rcvMsg, ref MsgStack);

                        foreach (string s in MsgList)
                        {
                            MessageInfo o = MessageParse.ParseMessage(s);
                            if (o != null)
                            {
                                if (o.CmdFlag == 1)
                                {
                                    #region  设备主动上传的数据
                                    switch (o.CN)
                                    {
                                        case 9021:
                                            #region  收到设备首个心跳时的处理
                                            //判断是否首个心跳数据
                                            if (IsFirstMsg)
                                            {
                                                //设置当前设备编号
                                                DevCode = o.MN;

                                                //先向设备回复注册报文
                                                SendMessage(MessageParse.BuildRegistResponseMsg(DevCode, o.QN));
                                                //发送校时指令
                                                SendMessage(MessageParse.BuildTimeCalibratMsg(DevCode));

                                                //TODO:
                                                //设备上线
                                                DeviceStatus.AddDevice(DevCode, this.DevClient);

                                                //向WS更新设备状态为在线
                                                SensorService.UpdateDeviceStatus(DevCode, 1);

                                                IsFirstMsg = false;
                                            }
                                            else
                                            {
                                                //回复心跳
                                                SendMessage(MessageParse.BuildHeartbeatResponseMsg(DevCode, o.QN));
                                            }
                                            #endregion

                                            //向WS更新设备心跳
                                            SensorService.UpdateDeviceHeartbeat(DevCode, Program.GetDateTime());
                                            break;
                                        case 2011:
                                            DataList.Add(s);
                                            break;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region  设备应答的数据
                                    //TODO:
                                    #endregion
                                }


                                Log.ServerLog.WriteEventLog("ParseMessage", String.Format("QN:{0},MN:{1},CN:{2},CmdFlag:{3},CP:{4}\r\n",
                                    o.QN, o.MN, o.CN, o.CmdFlag, o.CP));

                                Console.WriteLine("解析报文:" + String.Format("QN:{0},MN:{1},CN:{2},CmdFlag:{3},CP:{4}\r\n",
                                    o.QN, o.MN, o.CN, o.CmdFlag, o.CP));
                            }
                        }
                        //回复客户端消息
                        SendMessage(rspMsg);

                        if (DataList.Count > 0)
                        {
                            //上传数据到WebService
                            SensorService.UploadSensorData(DevCode, DataList);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (System.Exception ex)
                {
                    Log.ServerLog.WriteErrorLog(ex);

                    if (DevClient == null || !DevClient.Client.Connected)
                    {
                        DeviceStatus.DeleteDevice(DevCode);
                        DevEnabled = false;

                        Log.ServerLog.WriteEventLog("ConnectBreak", DevCode + ":设备掉线，断开设备连接，释放资源");
                    }
                }
            }
        }
        #endregion

        #region  回复客户端消息
        protected void SendMessage(string rspMsg)
        {
            try
            {
                if (DevClient != null && DevClient.Client.Connected && rspMsg.Length > 0)
                {
                    Log.ServerLog.WriteEventLog("SendMessage", String.Format("DevId:{0},Msg:{1}", DevCode, rspMsg));

                    byte[] rspData = System.Text.Encoding.GetEncoding("gb2312").GetBytes(rspMsg);
                    DevStream.Write(rspData, 0, rspData.Length);
                }
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}