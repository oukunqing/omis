using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading;

namespace OMIS.Service
{
    public partial class OMISService : ServiceBase
    {
        public System.Timers.Timer timer1;
        public static string MyServiceName = "OMISService";

        public int DevicePort = Common.Config.GetAppSetting("DevicePort", 0);
        public int DevicePort_1 = Common.Config.GetAppSetting("DevicePort_1", 0);
        public int ClientPort = Common.Config.GetAppSetting("ClientPort", 0);

        public OMISService()
        {
            InitializeComponent();
            if (!EventLog.SourceExists(MyServiceName))
            {
                EventLog.CreateEventSource(MyServiceName, MyServiceName + "Log");
            }
            evl.Source = MyServiceName;
            evl.Log = string.Empty;
        }

        protected override void OnStart(string[] args)
        {
            //evl.WriteEntry(MyServiceName + " Start.");
            Log.ServerLog.WriteEventLog("Start", "");
            TimerRun();

            new Thread(DeviceRun).Start();
            new Thread(DeviceRun_1).Start();
            new Thread(ClientRun).Start();

            //StartProcess();
        }

        protected void StartProcess()
        {
            //启动外部程序
            //Process.Start(AppDomain.CurrentDomain.BaseDirectory + "OMIS.Message.exe");

            Process p = new Process();
            p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "OMIS.Message.exe";

            //设置命令行参数 可以设置多个参数，每个参数之间以空格分隔
            p.StartInfo.Arguments = "WindowsService调用WinForm程序" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");
            p.Start();
        }

        protected override void OnStop()
        {
            //evl.WriteEntry(MyServiceName + " Stop.");
            Log.ServerLog.WriteEventLog("Stop", "");
            timer1.Enabled = false;
        }

        #region  定时器
        private void TimerRun()
        {
            timer1 = new System.Timers.Timer();
            timer1.Interval = Common.Config.GetAppSetting("TimerInterval", 30) * 1000;
            timer1.Enabled = true;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);

            TimerTask();
        }

        private void timer1_Elapsed(object sender, EventArgs e)
        {
            TimerTask();
        }

        private void TimerTask()
        {
            try
            {
                //TODO:
            }
            catch (Exception ex) { Log.ServerLog.WriteErrorLog(ex); }
        }
        #endregion
        
        #region  启用监听
        public void DeviceRun()
        {
            if (DevicePort > 0)
            {
                string strLog = "Device Server:" + DevicePort + " is start listening.";

                new Listen.ServerListen().BeginListenDeviceRequest(DevicePort);
            }
        }

        public void DeviceRun_1()
        {
            if (DevicePort_1 > 0)
            {
                string strLog = "Device Server:" + DevicePort_1 + " is start listening.";

                new Listen.ServerListen().BeginListenDeviceRequest(DevicePort_1);
            }
        }

        public void ClientRun()
        {
            if (ClientPort > 0)
            {
                string strLog = "Client Server:" + ClientPort + " is start listening.";

                new Listen.ServerListen().BeginListenClientRequest(ClientPort);
            }
        }
        #endregion

    }
}