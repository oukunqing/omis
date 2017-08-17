using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace OMIS.Device
{
    public partial class frmDevice : Form
    {

        private string DeviceCode = "";
        private string HeartbeatData = "";
        private string MessageData = "";
        
        private TcpClient DevClient;
        private NetworkStream DevStream;
        private int BufferSize = 8192;
        private byte[] Buffer;

        private string ServerIp = string.Empty;
        private int ServerPort = 0;

        private int HeartbeatTimes = 0;

        //消息栈
        protected string MsgStack = string.Empty;

        protected Dictionary<int, string> dicDevCP = new Dictionary<int, string>();

        public frmDevice()
        {
            InitializeComponent();
        }

        private void frmDevice_Load(object sender, EventArgs e)
        {

        }

        #region  连接服务端
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                //若已存在连接，则先断开连接
                if (this.DevClient != null && this.DevClient.Client.Connected)
                {
                    this.DevClient.Client.Dispose();
                    this.DevClient.Client.Close();

                    System.Threading.Thread.Sleep(1000);
                }

                this.ServerIp = this.cmbServerIp.Text.Trim();
                this.ServerPort = DataConvert.ConvertValue(this.cmbServerPort.Text, 0);

                if (this.ServerIp.Length > 0 && this.ServerPort > 0)
                {
                    this.DevClient = new TcpClient();
                    this.DevClient.Connect(IPAddress.Parse(this.ServerIp), this.ServerPort);

                    this.DevStream = this.DevClient.GetStream();
                    this.Buffer = new byte[this.BufferSize];

                    this.PrintLog("连接服务器", String.Format("{0} => {1}", DevClient.Client.LocalEndPoint.ToString(), DevClient.Client.RemoteEndPoint.ToString()));
                    
                    //开始异步接收
                    DevStream.BeginRead(Buffer, 0, BufferSize, new AsyncCallback(ReceiveCallback), DevClient);
                }
                else
                {
                    this.PrintLog("服务器IP、端口设置错误", String.Format("IP:{0}, Port:{1}", this.ServerIp, this.ServerPort));
                }
            }
            catch (Exception ex) { this.PrintErrorLog(ex); }
        }

        private void btnDisconncet_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DevClient != null)
                {
                    string server = this.DevClient.Client.RemoteEndPoint.ToString();

                    this.PrintLog("断开连接", String.Format("与服务器（{0}）的连接已断开。", server));

                    this.DevClient.Client.Dispose();
                    this.DevClient.Client.Close();

                    this.timerHeartbeat.Enabled = false;
                }
            }
            catch (Exception ex) { this.PrintErrorLog(ex); }
        }
        #endregion
        
        #region  发送数据到服务端
        public void SendData(string data)
        {
            try
            {
                if (this.DevClient != null && this.DevClient.Client.Connected)
                {
                    string msg = String.Format("{0}{1}", data, "\r\n");

                    DevStream.Write(Encoding.ASCII.GetBytes(msg), 0, Encoding.ASCII.GetBytes(msg).Length);
                }
            }
            catch (Exception ex) { this.PrintErrorLog(ex); }
        }
        #endregion

        #region  接收服务端数据
        protected void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                if (this.DevClient != null && this.DevClient.Connected)
                {
                    int bytesRead = 0;
                    lock (DevClient)
                    {
                        bytesRead = DevStream.EndRead(result);
                    }
                    if (bytesRead == 0)
                    {
                        throw new Exception("读取到0字节");
                    }

                    string rcvMsg = Encoding.Default.GetString(Buffer, 0, bytesRead);
                    this.PrintLog("收到服务端消息", rcvMsg);

                    ServerLog.WriteEventLog("ReceiveMessage", rcvMsg);

                    List<string> MsgList = MessageCheck.CheckMessageFormat(rcvMsg, ref MsgStack);
                    foreach (string s in MsgList)
                    {
                        MessageInfo o = MessageParse.ParseMessage(s);
                        if (o != null)
                        {
                            if (o.CmdFlag == 1)
                            {
                                this.CacheMessageContent(o);

                                this.SendResponseMessage(this.BuildResponseMessage(o));
                            }
                        }
                    }

                    Array.Clear(Buffer, 0, Buffer.Length); // 清空缓存，避免脏读

                    DevStream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(ReceiveCallback), DevStream);
                }
            }
            catch (Exception ex) { this.PrintErrorLog(ex); }
        }
        #endregion
        
        #region  打印日志
        public void PrintLog(string action, string con)
        {
            try
            {
                string log = String.Format("[{0}]{1}：{2}\r\n\r\n", Common.GetDateTime(), action, con);

                this.txtLog.AppendText(log);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void PrintErrorLog(Exception ex)
        {
            try
            {
                string log = String.Format("[{0}][{1}]：{2}\r\n\r\n", Common.GetDateTime(), "Error", ex.Message);

                this.txtLog.AppendText(log);
            }
            catch (Exception exx) { MessageBox.Show(exx.Message); }
        }
        #endregion


        #region  发送心跳数据

        #region  组装心跳数据
        protected string BuildHeartData()
        {
            try
            {
                this.DeviceCode = this.cmbDevCode.Text.Trim();

                string con = this.txtHeartbeatCon.Text.Trim();
                string qn = this.txtHeartbeatQN.Text.Trim();

                if (HeartbeatTimes++ > 0)
                {
                    qn = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                }

                string heartbeat = String.Format(this.txtHeartbeat.Text.Trim(), qn, DeviceCode, con);

                string len = String.Format("##{0:D4}", heartbeat.Length);
                string crc = CRC.ToCRC16(heartbeat);

                this.txtHeartbeatQN.Text = qn;
                this.txtHeartbeatLen.Text = len;
                this.txtHeartbeatCRC.Text = crc;

                this.HeartbeatData = String.Format("{0}{1}{2}", len, heartbeat, crc);

                this.timerHeartbeat.Enabled = true;
                this.timerHeartbeat.Interval = DataConvert.ConvertValue(this.cmbHeartbeat.Text, 30) * 1000;

                return this.HeartbeatData;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        private void btnHeartbeat_Click(object sender, EventArgs e)
        {
            try
            {
                this.SendHeartbeat();
            }
            catch (Exception ex) { this.PrintErrorLog(ex); }
        }

        private void timerHeartbeat_Tick(object sender, EventArgs e)
        {
            if (this.DevClient != null && this.DevClient.Connected)
            {
                this.SendHeartbeat();
            }
        }

        private void SendHeartbeat()
        {
            try
            {
                this.HeartbeatData = this.BuildHeartData();

                this.PrintLog("发送心跳", this.HeartbeatData);

                ServerLog.WriteEventLog("SendHeartbeat", HeartbeatData);

                this.SendData(this.HeartbeatData);
            }
            catch (Exception ex) { this.PrintErrorLog(ex); }
        }
        #endregion

        #region  发送报文数据
        
        private void btnSendData_Click(object sender, EventArgs e)
        {
            try
            {
                this.SendMessageData();
            }
            catch (Exception ex) { this.PrintErrorLog(ex); }
        }

        #region  发送报文数据
        public void SendMessageData()
        {
            string con = this.txtDataCon.Text.Trim();
            string qn = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            string data = String.Format(this.txtData.Text.Trim(), qn, DeviceCode, con);

            string len = String.Format("##{0:D4}", data.Length);
            string crc = CRC.ToCRC16(data);

            this.txtDataQN.Text = qn;
            this.txtDataLen.Text = len;
            this.txtDataCRC.Text = crc;

            this.MessageData = String.Format("{0}{1}{2}", len, data, crc);

            bool isPack = this.chbPack.Checked;

            int loopTimes = DataConvert.ConvertValue(this.cmbLoopTimes.Text, 1);

            if (isPack)
            {
                StringBuilder datas = new StringBuilder();
                for (int i = 0; i < loopTimes; i++)
                {
                    datas.Append(MessageData);
                    datas.Append(i < loopTimes - 1 ? "\r\n" : "");
                }

                this.PrintLog("发送报文", datas.ToString());

                this.SendData(datas.ToString());
            }
            else
            {
                for (int i = 0; i < loopTimes; i++)
                {
                    this.PrintLog("发送报文", MessageData);

                    this.SendData(MessageData);
                }
            }
        }
        #endregion

        #endregion

        #region  缓存消息内容
        public void SetMessageDic(int cn, string con)
        {
            if (dicDevCP.ContainsKey(cn))
            {
                dicDevCP[cn] = con;
            }
            else
            {
                dicDevCP.Add(cn, con);
            }
        }

        public string GetCacheMessage(int cn, string con)
        {
            string result = con;
            switch (cn)
            {
                case 9013:
                case 9014:
                    result = dicDevCP.ContainsKey(cn) ? dicDevCP[cn].ToString() : con;
                    break;
            }
            return result;
        }

        public void CacheMessageContent(MessageInfo o)
        {
            string cp = o.CP;
            string[] arr = cp.Split('&');

            switch (o.CN)
            {
                case 9015:
                    SetMessageDic(9013, arr[0]);
                    SetMessageDic(9014, cp);
                    break;
            }
        }
        #endregion

        #region  组装回复报文
        public string BuildResponseMessage(MessageInfo o)
        {
            string cp = this.GetCacheMessage(o.CN, "");
            string con = String.Format("QN={0};ST={1};CN={2};PW={3};MN={4};CmdFlag=0;CP=&&{5}&&", o.QN, o.ST, o.CN, "123456", o.MN, cp);
            string len = String.Format("##{0:D4}", con.Length);
            string crc = CRC.ToCRC16(con);

            return String.Format("{0}{1}{2}", len, con, crc);
        }
        #endregion

        #region  发送回复报文
        public void SendResponseMessage(string rspMsg)
        {
            if (!rspMsg.Equals(string.Empty))
            {
                this.PrintLog("回复服务端消息：", rspMsg);

                ServerLog.WriteEventLog("ResponseMessage", rspMsg);
                this.SendData(rspMsg);
            }
        }
        #endregion

        #region  清除内容
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清除内容吗？", "清除内容", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.txtLog.Text = String.Format("CLS.{0}\r\n\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
        }
        #endregion

    }
}
