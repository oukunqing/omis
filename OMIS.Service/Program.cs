using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;

namespace OMIS.Service
{
    static class Program
    {
        /// <summary>
        /// 是否启用调试
        /// </summary>
        public static bool isDebug = false;
        /// <summary>
        /// 是否打印内容到屏幕
        /// </summary>
        public static bool isPrint = true;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new OMISService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
        
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