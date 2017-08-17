using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;

namespace OMIS.Server
{    
    public delegate bool ControlCtrlDelegate(int CtrlType);
    
    class Program
    {
        //捕捉关闭事件
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        static ControlCtrlDelegate newDelegate = new ControlCtrlDelegate(HandlerRoutine);
                 
        //禁用关闭按钮
        [DllImport("User32.dll ", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll ", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll ", EntryPoint = "RemoveMenu")]
        extern static int RemoveMenu(IntPtr hMenu, int nPos, int flags);

        public static bool isDisabledCloseButton = Common.Config.GetAppSetting("DisabledCloseButton", 0) == 1;
        public static bool isEnabledCommandClose = Common.Config.GetAppSetting("EnabledCommandClose", isDisabledCloseButton ? 1 : 0) == 1;
        //等待接受退出指令
        public bool isWaitExit = false;
        //等待接受调试指令
        public bool isWaitDebug = false;

        /// <summary>
        /// 是否启用调试
        /// </summary>
        public static bool isDebug = false;
        /// <summary>
        /// 是否打印内容到屏幕
        /// </summary>
        public static bool isPrint = true;

        public int clsTimes = 0;
        public List<string> lstConsole = new List<string>();

        public string strStartTime = GetLogDateTime();

        public int DevicePort = Common.Config.GetAppSetting("DevicePort", 0);
        public int DevicePort_1 = Common.Config.GetAppSetting("DevicePort_1", 0);
        public int ClientPort = Common.Config.GetAppSetting("ClientPort", 0);

        public System.Timers.Timer timer;
        public int timerInterval = Common.Config.GetAppSetting("TimerInterval", 0);

        #region  捕捉关闭事件
        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:
                    //Ctrl+C关闭  
                    Log.ServerLog.WriteEventLog("ServerClose", "应用程序被Ctrl+C强制关闭");
                    break;
                case 2:
                    //按控制台关闭按钮关闭  
                    Log.ServerLog.WriteEventLog("ServerClose", "应用程序被强制关闭");
                    break;
            }

            //在应用程序被关闭之前需要处理一些事务
            new Program().ToDoBeforeExit(false);

            return false;
        }
        #endregion

        #region  主程入口
        static void Main(string[] args)
        {
            Program pr = new Program();

            string strLog = "Server is running. " + pr.strStartTime;
            Console.WriteLine(strLog);
            pr.lstConsole.Add(strLog);

            Log.ServerLog.WriteEventLog("ServerStart", "Server start.");

            ////获取应用程序名称（包含路径）
            //Console.WriteLine(Process.GetCurrentProcess().MainModule.FileName);
            
            try
            {
                new Thread(pr.DeviceRun).Start();
                new Thread(pr.DeviceRun_1).Start();
                new Thread(pr.ClientRun).Start();
                
                new Thread(pr.TimerRun).Start();

                #region  关闭事件、命令行
                Thread.Sleep(5000);

                new Thread(pr.CatchCloseEvent).Start();
                new Thread(pr.CommandInput).Start();

                //是否禁用关闭按钮
                if (isDisabledCloseButton)
                {
                    pr.DisabledCloseButton();
                }
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine("错误：" + ex.Message);

                Log.ServerLog.WriteErrorLog(ex);
            }
        }
        #endregion
        
        #region  启用监听
        public void DeviceRun()
        {
            if (DevicePort > 0)
            {
                string strLog = "Device Server:" + DevicePort + " is start listening.";
                Console.WriteLine(strLog);
                lstConsole.Add(strLog);

                new Listen.ServerListen().BeginListenDeviceRequest(DevicePort);
            }
        }

        public void DeviceRun_1()
        {
            if (DevicePort_1 > 0)
            {
                string strLog = "Device Server:" + DevicePort_1 + " is start listening.";
                Console.WriteLine(strLog);
                lstConsole.Add(strLog);

                new Listen.ServerListen().BeginListenDeviceRequest(DevicePort_1);
            }
        }

        public void ClientRun()
        {
            if (ClientPort > 0)
            {
                string strLog = "Client Server:" + ClientPort + " is start listening.";
                Console.WriteLine(strLog);
                lstConsole.Add(strLog);

                new Listen.ServerListen().BeginListenClientRequest(ClientPort);
            }
        }
        #endregion

        
        #region  捕捉关闭事件
        public void CatchCloseEvent()
        {
            //捕捉关闭事件
            bool bRet = SetConsoleCtrlHandler(newDelegate, true);

        }
        #endregion

        #region  程序退出之前需要做的事务
        public void ToDoBeforeExit(bool isCommand)
        {
            try
            {
                Console.WriteLine("正在退出...");

                Log.ServerLog.WriteEventLog("ExitBegin", "Exit begin.");

                //退出之前要处理的事务
                //TODO:

                Log.ServerLog.WriteEventLog("ExitEnd", "Exit end.");
            }
            catch (Exception ex)
            {
                Log.ServerLog.WriteErrorLog(ex);
                //程序出错，退出程序
                Environment.Exit(0);
            }
        }
        #endregion

        #region 定时器
        public void TimerRun()
        {
            if (timerInterval > 0)
            {
                //当配置的定时器时间间隔大于0时，启用定时器
                timer = new System.Timers.Timer();
                timer.Elapsed += new ElapsedEventHandler(TimerdEvent);
                timer.Interval = timerInterval * 1000;
                timer.Enabled = true;

                //先执行一遍任务
                TimerTask();
            }
        }
        #endregion

        #region  定时器事件
        private static void TimerdEvent(object source, ElapsedEventArgs e)
        {
            TimerTask();
        }
        #endregion

        #region  定时执行的任务
        private static void TimerTask()
        {
            try
            {
                //Console.WriteLine("TimerInterval. " + GetLogDateTime());
            }
            catch (Exception ex)
            {
                Log.ServerLog.WriteErrorLog(ex);
                if (isPrint)
                {
                    Console.WriteLine("错误：" + ex.Message);
                }
            }
        }
        #endregion

        #region  禁用关闭按钮
        public void DisabledCloseButton()
        {
            //与控制台标题名一样的路径
            //string fullPath = System.Environment.CurrentDirectory + "\\ConsoleApplication1.exe";
            string fullPath = Process.GetCurrentProcess().MainModule.FileName;
            //根据控制台标题找控制台
            int WINDOW_HANDLER = FindWindow(null, fullPath);
            //找关闭按钮
            IntPtr CLOSE_MENU = GetSystemMenu((IntPtr)WINDOW_HANDLER, IntPtr.Zero);
            int SC_CLOSE = 0xF060;
            //关闭按钮禁用
            RemoveMenu(CLOSE_MENU, SC_CLOSE, 0x0);
        }
        #endregion
        
        #region  命令输入
        public void CommandInput()
        {
            //使用命令关闭
            while (true)
            {
                string strInput = Console.ReadLine().ToUpper();

                /*
                 * //直接退出，不提问
                if (strInput.Equals("EXIT") || strInput.Equals("QUIT"))
                {
                    
                    //Console.WriteLine("按任意键退出...");
                    //Console.ReadKey(false);
                    //Console.WriteLine("");
                    
                    Log.ServerLog.WriteEventLog("ExitBefore", String.Format("应用程序接受退出程序指令[{0}]。", strInput));

                    //在应用程序被关闭之前需要处理一些事务
                    ToDoBeforeExit(true);

                    //退出程序
                    Environment.Exit(0);
                }
                */

                #region  等待指令确定
                if (isWaitExit)
                {
                    //等待退出
                    if (strInput.Equals("Y"))
                    {
                        //在应用程序被关闭之前需要处理一些事务
                        ToDoBeforeExit(true);

                        //退出程序
                        Environment.Exit(0);
                    }
                    else
                    {
                        isWaitExit = false;
                        Console.WriteLine("退出指令已取消");
                        Log.ServerLog.WriteEventLog("ExitCancel", "退出指令取消。");
                    }
                }
                else if (isWaitDebug)
                {
                    //等待调试
                    if (strInput.Equals("Y"))
                    {
                        isDebug = true;
                        Console.WriteLine("Enabled Debug. " + GetLogDateTime());
                    }
                    else
                    {
                        isDebug = false;
                    }
                    isWaitDebug = false;
                }
                #endregion

                #region  各种指令
                switch (strInput)
                {
                    case "EXIT":
                    case "QUIT":
                        //命令行启用状态下，判断是否收到退出指令
                        if (isEnabledCommandClose)
                        {
                            Console.WriteLine("确定要退出程序吗？(Y/N)");
                            //等待接受退出指令
                            isWaitExit = true;
                            //取消等待调试指令
                            isWaitDebug = false;
                            Log.ServerLog.WriteEventLog("ExitBefore", String.Format("应用程序接受退出程序指令[{0}]。", strInput));
                        }
                        break;
                    case "CLEAR":
                        Console.Clear();
                        Console.WriteLine(String.Format("CLS. ({0}) {1}", ++clsTimes, GetLogDateTime()));
                        //清屏后，输出初始内容
                        WriteInitialData();
                        break;
                    case "OPEN DEBUG":
                        Console.WriteLine("确定要启用调试吗？(Y/N)");
                        //等待接受调试指令
                        isWaitDebug = true;
                        //取消等待退出指令
                        isWaitExit = false;
                        break;
                    case "CLOSE DEBUG":
                        //等待接受调试指令
                        isWaitDebug = false;
                        //取消调试
                        isDebug = false;
                        Console.WriteLine("Disabled Debug. " + GetLogDateTime());
                        break;
                    case "OPEN PRINT":
                        isPrint = true;
                        Console.WriteLine("Enabled Print. " + GetLogDateTime());
                        break;
                    case "CLOSE PRINT":
                        isPrint = false;
                        Console.WriteLine("Disabled Print. " + GetLogDateTime());
                        break;
                }
                #endregion

            }
        }
        #endregion

        #region  清屏后输出内容
        public void WriteInitialData()
        {
            foreach (string str in lstConsole)
            {
                Console.WriteLine(str);
            }
            if (isDebug)
            {
                Console.WriteLine("Enabled Debug. ");
            }
            if (!isPrint)
            {
                Console.WriteLine("Disabled Print. ");
            }
        }
        #endregion

        #region  打印命令行日志
        public static void PrintConsoleLog(string con)
        {
            if (Program.isPrint)
            {
                Console.WriteLine(con);
            }
        }
        #endregion

        #region  获得当前时间
        public static string GetLogDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

    }
}