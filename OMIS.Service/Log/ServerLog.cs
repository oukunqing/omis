using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace OMIS.Service.Log
{
    public class ServerLog
    {
        public static bool isSplit = true;
        public static int LogFileSize = GetAppSetting("LogFileSize", 10);    //10MB
        public static int maxFileLength = 1024 * LogFileSize;

        public static string eventLogName = String.Format("ev{0}{1}.log", DateTime.Now.ToString("yyyyMMdd"), string.Empty);
        public static string errorLogName = String.Format("er{0}{1}.log", DateTime.Now.ToString("yyyyMMdd"), string.Empty);
        public static string debugLogName = String.Format("de{0}{1}.log", DateTime.Now.ToString("yyyyMMdd"), string.Empty);

        public static bool isSaveEventLog = GetAppSetting("SaveEventLog", 1) == 1;
        public static bool isSaveDebugLog = GetAppSetting("SaveDebugLog", 1) == 1;
        public static bool isSaveErrorLog = GetAppSetting("SaveErrorLog", 1) == 1;

        public static int keepDays = GetAppSetting("LogKeepDays", 7);

        public static string RootDir = AppDomain.CurrentDomain.BaseDirectory;
        public static string ErrorLogDir = RootDir + GetAppSetting("ErrorLogDir", "log/error/");
        public static string DebugLogDir = RootDir + GetAppSetting("DebugLogDir", "log/debug/");
        public static string EventLogDir = RootDir + GetAppSetting("EventLogDir", "log/event/");

        #region  验证是否是数字
        public static bool IsNumber(string strNumber)
        {
            string strPattern = @"^\-?(0+)?(\d+)(.\d+)?$";

            return new Regex(strPattern).IsMatch(strNumber);
        }
        #endregion

        #region  获得配置信息
        public static int GetAppSetting(string key, int defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                if (IsNumber(ConfigurationManager.AppSettings[key].ToString()))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings[key].ToString());
                }
                else
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        public static string GetAppSetting(string key, string defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            return defaultValue;
        }
        #endregion

        #region  创建日志目录、文件名
        public static string BuildFileDir()
        {
            //return DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/";

            return DateTime.Now.ToString("yyyyMM") + "/";
        }

        public static string BuildFileName(string prefix, string postfix)
        {
            return String.Format("{0}{1}{2}.log", prefix, DateTime.Now.ToString("yyyyMMdd"), postfix);
        }

        public static FileInfo[] SearchFile(string fileDir, string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(fileDir);
            return dir.Exists ? dir.GetFiles(fileName) : null;
        }

        public static string GetFileNamePostfix(FileInfo[] arrFile, int maxLength, string separate)
        {
            try
            {
                int c = arrFile.Length;
                if (c > 0)
                {
                    int max = 0;
                    FileInfo fiLast = arrFile[0];

                    string[] delimiter = { separate };
                    foreach (FileInfo fi in arrFile)
                    {
                        string[] postfix = Path.GetFileNameWithoutExtension(fi.Name).Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                        if (postfix.Length > 1)
                        {
                            int cur = Convert.ToInt32(postfix[1]);
                            if (cur > max)
                            {
                                max = cur;
                                fiLast = fi;
                            }
                        }
                    }
                    double flen = Convert.ToDouble(fiLast.Length) / 1024;
                    return max > 0 ? separate + (max + (flen > maxLength ? 1 : 0)).ToString() : (flen > maxLength ? separate + "1" : "");
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string BuildLogFileName(string prefix, string fileDir)
        {
            return BuildFileName(prefix, isSplit ? GetFileNamePostfix(SearchFile(fileDir, BuildFileName(prefix, "*")), maxFileLength, "_") : "");
        }
        #endregion

        #region  写入错误日志
        public static void WriteErrorLog(Exception ex)
        {
            try
            {
                if (isSaveErrorLog)
                {
                    string fileDir = ErrorLogDir + BuildFileDir();
                    string fileName = BuildLogFileName("er", fileDir);
                    Common.FileOperate.WriteErrorLog(ex, fileDir, fileName, true, Encoding.Default);
                }
                else
                {
                    DeleteAllLogFile(ErrorLogDir);
                }
            }
            catch (Exception exx) { }
        }
        #endregion

        #region  写入服务器交互事件日志
        public static void WriteEventLog(string eventName, string eventContent)
        {
            try
            {
                WriteEventLog(string.Empty, eventName, eventContent, false);
            }
            catch (Exception exx) { }
        }

        public static void WriteEventLog(string eventName, string eventContent, bool isSpaceLine)
        {
            try
            {
                WriteEventLog(string.Empty, eventName, eventContent, isSpaceLine);
            }
            catch (Exception exx) { }
        }

        public static void WriteEventLog(string fileName, string eventName, string eventContent)
        {
            try
            {
                WriteEventLog(fileName, eventName, eventContent, false);
            }
            catch (Exception exx) { }
        }

        public static void WriteEventLog(string fileName, string eventName, string eventContent, bool isSpaceLine)
        {
            try
            {
                if (isSaveEventLog)
                {
                    DeleteLogFile(EventLogDir, GetKeepMonths(keepDays), GetKeepDays(keepDays));

                    string fileDir = EventLogDir + BuildFileDir();
                    if (fileName.Trim().Equals(string.Empty))
                    {
                        fileName = BuildLogFileName("ev", fileDir);
                    }
                    Common.FileOperate.WriteEventLog(fileDir, fileName, eventName, eventContent, true, Encoding.Default, isSpaceLine);
                }
                else
                {
                    DeleteAllLogFile(EventLogDir);
                }
            }
            catch (Exception exx) { }
        }
        #endregion

        #region  写入调试日志
        public static void WriteDebugLog(string eventName, string eventContent)
        {
            try
            {
                if (isSaveDebugLog)
                {
                    string fileDir = DebugLogDir + BuildFileDir();
                    string fileName = BuildLogFileName("de", fileDir);
                    Common.FileOperate.WriteEventLog(fileDir, fileName, eventName, eventContent, true, Encoding.Default);
                }
                else
                {
                    DeleteAllLogFile(DebugLogDir);
                }
            }
            catch (Exception exx) { }
        }
        #endregion

        #region  删除全部日志文件
        public static void DeleteAllLogFile(string rootDir)
        {
            try
            {
                if (!rootDir.Equals(string.Empty))
                {
                    string strTargetDir = rootDir;
                    strTargetDir = strTargetDir.Replace("/", "\\").Replace("\\\\", "\\");

                    Common.FileOperate.DeleteDirectory(strTargetDir);
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region  删除日志文件
        public static void DeleteLogFile(string rootDir, ArrayList arrMonth, ArrayList arrDay)
        {
            try
            {
                if (!rootDir.Equals(string.Empty))
                {
                    string strTargetDir = rootDir;
                    strTargetDir = strTargetDir.Replace("/", "\\").Replace("\\\\", "\\");

                    DeleteDirectory(strTargetDir, strTargetDir, arrMonth, arrDay);

                    DeleteFile(strTargetDir, strTargetDir, arrDay);
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region  删除子目录及所有文件
        /// <summary>
        /// 删除子目录及所有文件
        /// </summary>
        /// <param name="aimPath"></param>
        /// <param name="rootDir"></param>
        /// <param name="arrMonth"></param>
        /// <param name="arrDay"></param>
        /// <returns></returns>
        public static bool DeleteDirectory(string aimPath, string rootDir, ArrayList arrMonth, ArrayList arrDay)
        {
            try
            {
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath += Path.DirectorySeparatorChar;
                }
                string[] fileList = Directory.GetFileSystemEntries(aimPath);
                foreach (string file in fileList)
                {
                    if (Directory.Exists(file))
                    {
                        string dirMonth = file.Substring(file.LastIndexOf('\\') + 1);
                        if (arrMonth.IndexOf(dirMonth) < 0)
                        {
                            DeleteDirectory(aimPath + Path.GetFileName(file), rootDir, arrMonth, arrDay);
                        }
                    }
                    else
                    {
                        File.Delete(aimPath + Path.GetFileName(file));
                    }
                }

                if (!aimPath.Equals(rootDir))
                {
                    Directory.Delete(aimPath, true);
                }
                return true;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="aimPath"></param>
        /// <param name="rootDir"></param>
        /// <param name="arrDay"></param>
        /// <returns></returns>
        public static bool DeleteFile(string aimPath, string rootDir, ArrayList arrDay)
        {
            try
            {
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath += Path.DirectorySeparatorChar;
                }
                string[] fileList = Directory.GetFileSystemEntries(aimPath);
                foreach (string file in fileList)
                {
                    if (Directory.Exists(file))
                    {
                        DeleteFile(aimPath + Path.GetFileName(file), rootDir, arrDay);
                    }
                    else
                    {
                        Regex reg = new Regex("[ev|er|de]");
                        string[] delimiter = { "_", "." };
                        string fileDay = reg.Replace(Path.GetFileNameWithoutExtension(file), "").Split(delimiter, StringSplitOptions.RemoveEmptyEntries)[0];

                        if (arrDay.IndexOf(fileDay) < 0)
                        {
                            File.Delete(aimPath + Path.GetFileName(file));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得要保留的月份
        public static ArrayList GetKeepMonths(int days)
        {
            days -= 1;

            DateTime dtNow = DateTime.Now;
            DateTime dtStart = DateTime.Now.AddDays(-days);
            int months = (dtNow.Year - dtStart.Year) * 12 + (dtNow.Month - dtStart.Month);

            ArrayList arrMonth = new ArrayList();
            for (int i = 0; i < months; i++)
            {
                arrMonth.Add(DateTime.Now.AddMonths(-i).ToString("yyyyMM"));
            }

            arrMonth.Add(dtStart.ToString("yyyyMM"));

            return arrMonth;
        }
        #endregion

        #region  获得要保留的日期
        public static ArrayList GetKeepDays(int days)
        {
            ArrayList arrDay = new ArrayList();
            for (int i = 0; i < days; i++)
            {
                arrDay.Add(DateTime.Now.AddDays(-i).ToString("yyyyMMdd"));
            }

            return arrDay;
        }
        #endregion

    }
}