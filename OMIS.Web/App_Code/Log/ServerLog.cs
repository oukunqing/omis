using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using OMIS.Common;

/// <summary>
///ServerLog 的摘要说明
/// </summary>
public class ServerLog
{
    public ServerLog()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public static bool isSplit = true;
    public static int LogFileSize = Config.LogFileSize;    //10MB
    public static int maxFileLength = 1024 * LogFileSize;

    public static string date = DateTime.Now.ToString("yyyyMMdd");
    public static string strEventLogName = String.Format("ev{0}{1}.log", date, string.Empty);
    public static string strErrorLogName = String.Format("er{0}{1}.log", date, string.Empty);
    public static string strDebugLogName = String.Format("de{0}{1}.log", date, string.Empty);
    public static string strTempLogName = String.Format("tmp{0}{1}.log", date, string.Empty);

    public static bool isSaveEventLog = Config.SaveEventLog;
    public static bool isSaveDebugLog = Config.SaveDebugLog;
    public static bool isSaveErrorLog = Config.SaveErrorLog;
    public static bool isSaveTempLog = Config.SaveTempLog;

    public static int keepDays = Config.LogKeepDays;
    
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
        DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Request.PhysicalApplicationPath + fileDir);
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
                    string[] strPostfix = Path.GetFileNameWithoutExtension(fi.Name).Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                    if (strPostfix.Length > 1)
                    {
                        int cur = Convert.ToInt32(strPostfix[1]);
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
    #endregion

    #region  写入错误日志
    public static void WriteErrorLog(Exception ex)
    {
        try
        {
            WriteErrorLog(ex, HttpContext.Current.Request, string.Empty);
        }
        catch (Exception exx) { }
    }
    public static void WriteErrorLog(Exception ex, string eventName)
    {
        try
        {
            WriteErrorLog(ex, HttpContext.Current.Request, eventName);
        }
        catch (Exception exx) { }
    }

    public static void WriteErrorLog(Exception ex, HttpRequest hr)
    {
        try
        {
            WriteErrorLog(ex, hr, string.Empty);
        }
        catch (Exception exx) { }
    }

    public static void WriteErrorLog(Exception ex, HttpRequest hr, string eventName)
    {
        try
        {
            if (ex is System.Threading.ThreadAbortException)
            {
                //如果是进程中断错误，则不记录日志
            }
            else
            {
                if (isSaveErrorLog)
                {
                    //ex.Source.IndexOf("mscorlib") >= 0
                    if (ex.TargetSite.ToString().IndexOf("Void AbortInternal()") >= 0 || ex.Message.IndexOf("正在中止线程") >= 0)
                    {
                        //页面跳转，忽略
                    }
                    else
                    {
                        string strFileDir = Config.ErrorLogDir + BuildFileDir();
                        string fileName = BuildFileName("er", isSplit ? GetFileNamePostfix(SearchFile(strFileDir, BuildFileName("er", "*")), maxFileLength, "_") : "");
                        FileOperate.WriteErrorLog(ex, hr, strFileDir, fileName, eventName, true, Encoding.Default);
                    }
                }
                else
                {
                    DeleteAllLogFile(hr, Config.ErrorLogDir);
                }
            }
        }
        catch (Exception exx) { }
    }

    public static void WriteErrorLog(Exception ex, HttpContext hc)
    {
        try
        {
            WriteErrorLog(ex, hc.Request);
        }
        catch (Exception exx) { }
    }
    #endregion

    #region  写入事件日志
    public static void WriteEventLog(HttpRequest hr, string eventName, string eventContent)
    {
        try
        {
            WriteEventLog(hr, string.Empty, eventName, eventContent, false, true);
        }
        catch (Exception exx) { }
    }

    public static void WriteEventLog(HttpRequest hr, string eventName, string eventContent, bool isSpaceLine)
    {
        try
        {
            WriteEventLog(hr, string.Empty, eventName, eventContent, isSpaceLine, true);
        }
        catch (Exception exx) { }
    }

    public static void WriteEventLog(HttpRequest hr, string eventName, string eventContent, bool isSpaceLine, bool showRawUrl)
    {
        try
        {
            WriteEventLog(hr, string.Empty, eventName, eventContent, isSpaceLine, showRawUrl);
        }
        catch (Exception exx) { }
    }

    public static void WriteEventLog(HttpRequest hr, string fileName, string eventName, string eventContent)
    {
        try
        {
            WriteEventLog(hr, fileName, eventName, eventContent, false, true);
        }
        catch (Exception exx) { }
    }

    public static void WriteEventLog(HttpRequest hr, string fileName, string eventName, string eventContent, bool isSpaceLine)
    {
        try
        {
            WriteEventLog(hr, fileName, eventName, eventContent, isSpaceLine, true);
        }
        catch (Exception exx) { }
    }

    public static void WriteEventLog(HttpRequest hr, string fileName, string eventName, string eventContent, bool isSpaceLine, bool showRawUrl)
    {
        try
        {
            if (isSaveEventLog)
            {
                DeleteLogFile(hr, Config.EventLogDir, GetKeepMonths(keepDays), GetKeepDays(keepDays));

                string strFileDir = Config.EventLogDir + BuildFileDir();
                if (fileName.Trim().Equals(string.Empty))
                {
                    fileName = BuildFileName("ev", isSplit ? GetFileNamePostfix(SearchFile(strFileDir, BuildFileName("ev", "*")), maxFileLength, "_") : "");
                }
                FileOperate.WriteEventLog(hr, strFileDir, fileName, eventName, eventContent, true, Encoding.Default, isSpaceLine, showRawUrl);
            }
            else
            {
                DeleteAllLogFile(hr, Config.EventLogDir);
            }
        }
        catch (Exception exx) { }
    }
    #endregion

    #region  写入调试日志
    public static void WriteDebugLog(HttpRequest hr, string eventName, string eventContent)
    {
        try
        {
            if (isSaveDebugLog)
            {
                string fileDir = Config.DebugLogDir + BuildFileDir();
                string fileName = BuildFileName("de", isSplit ? GetFileNamePostfix(SearchFile(fileDir, BuildFileName("de", "*")), maxFileLength, "_") : "");
                FileOperate.WriteEventLog(hr, fileDir, fileName, eventName, eventContent, true, Encoding.Default);
            }
            else
            {
                DeleteAllLogFile(hr, Config.DebugLogDir);
            }
        }
        catch (Exception exx) { }
    }
    #endregion

    #region  写入临时日志
    public static void WriteTempLog(HttpRequest hr, string eventName, string eventContent)
    {
        try
        {
            if (isSaveTempLog)
            {
                string fileDir = Config.TempLogDir + BuildFileDir();
                string fileName = BuildFileName("tmp", isSplit ? GetFileNamePostfix(SearchFile(fileDir, BuildFileName("tmp", "*")), maxFileLength, "_") : "");
                FileOperate.WriteEventLog(hr, fileDir, fileName, eventName, eventContent, true, Encoding.Default);
            }
            else
            {
                DeleteAllLogFile(hr, Config.TempLogDir);
            }
        }
        catch (Exception exx) { }
    }
    #endregion

    #region  删除全部日志文件
    public static void DeleteAllLogFile(HttpRequest hr, string rootDir)
    {
        try
        {
            if (!rootDir.Equals(string.Empty))
            {
                string targetDir = hr.PhysicalApplicationPath + rootDir;
                targetDir = targetDir.Replace("/", "\\").Replace("\\\\", "\\");

                FileOperate.DeleteDirectory(targetDir);
            }
        }
        catch (Exception ex) { }
    }
    #endregion

    #region  删除日志文件
    public static void DeleteLogFile(HttpRequest hr, string rootDir, ArrayList arrMonth, ArrayList arrDay)
    {
        try
        {
            if (!rootDir.Equals(string.Empty))
            {
                string targetDir = hr.PhysicalApplicationPath + rootDir;
                targetDir = targetDir.Replace("/", "\\").Replace("\\\\", "\\");

                DeleteDirectory(targetDir, targetDir, arrMonth, arrDay);

                DeleteFile(targetDir, targetDir, arrDay);
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
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
            {
                aimPath += Path.DirectorySeparatorChar;
            }
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
            string[] fileList = Directory.GetFileSystemEntries(aimPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
                if (Directory.Exists(file))
                {
                    string dirMonth = file.Substring(file.LastIndexOf('\\') + 1);
                    if (arrMonth.IndexOf(dirMonth) < 0)
                    {
                        //递归删除目录下的文件和目录，此处文件目录不需要加 Server.MapPath
                        DeleteDirectory(aimPath + Path.GetFileName(file), rootDir, arrMonth, arrDay);
                    }
                }
                // 如果是文件则直接删除文件
                else
                {
                    File.Delete(aimPath + Path.GetFileName(file));
                }
            }

            if (!aimPath.Equals(rootDir))
            {
                //删除文件夹
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
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
            {
                aimPath += Path.DirectorySeparatorChar;
            }
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
            string[] fileList = Directory.GetFileSystemEntries(aimPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
                if (Directory.Exists(file))
                {
                    //递归删除目录下的文件和目录，此处文件目录不需要加 Server.MapPath
                    DeleteFile(aimPath + Path.GetFileName(file), rootDir, arrDay);
                }
                // 如果是文件则直接删除文件
                else
                {
                    Regex reg = new Regex("[ev|er|de]");
                    string[] strDelimiter = { "_", "." };
                    string fileDay = reg.Replace(Path.GetFileNameWithoutExtension(file), "").Split(strDelimiter, StringSplitOptions.RemoveEmptyEntries)[0];

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