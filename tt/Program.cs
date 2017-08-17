using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace tt
{
    public delegate bool ControlCtrlDelegate(int CtrlType);
    class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);

        static ControlCtrlDelegate newDelegate = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:
                    Console.WriteLine("0工具被强制关闭"); //Ctrl+C关闭  
                    break;
                case 2:
                    Console.WriteLine("2工具被强制关闭");//按控制台关闭按钮关闭  
                    break;
            }
            
            System.Threading.Thread.Sleep(5000);

            return false;
        }

        static void Main(string[] args)
        {
            bool bRet = SetConsoleCtrlHandler(newDelegate, true);

            //这后面写程序该做的事情  
            while (bRet)
            {
                Console.WriteLine("请关闭！");

                Thread.Sleep(5000);
            }
        }
    }
}