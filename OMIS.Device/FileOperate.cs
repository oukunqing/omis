using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace OMIS.Device
{
    class FileOperate
    {

        #region  获得日期时间毫秒数
        private static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        #endregion

        #region  创建目录
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="strFileDir"></param>
        /// <returns></returns>
        public static bool CreateDir(string strFileDir)
        {
            try
            {
                if (!Directory.Exists(strFileDir))
                {
                    Directory.CreateDirectory(strFileDir);
                }
                return Directory.Exists(strFileDir);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region  删除子目录及所有文件
        /// <summary>
        /// 删除子目录及所有文件
        /// 请谨慎使用，以免误删除造成数据损失
        /// </summary>
        /// <param name="aimPath"></param>
        /// <returns></returns>
        public static bool DeleteDirectory(string aimPath)
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
                        DeleteDirectory(aimPath + Path.GetFileName(file));
                    }
                    // 如果是文件则直接删除文件
                    else
                    {
                        File.Delete(aimPath + Path.GetFileName(file));
                    }
                }
                //删除文件夹
                Directory.Delete(aimPath, true);

                return !Directory.Exists(aimPath);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region  读取文件
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadFile(string strFilePath, Encoding encoding)
        {
            try
            {
                if (File.Exists(strFilePath))
                {
                    StreamReader sr = new StreamReader(strFilePath, encoding);
                    string strResult = sr.ReadToEnd();
                    sr.Close();
                    return strResult;
                }
                else
                {
                    return strFilePath + " file does not exist.";
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region  写入文件
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strContent"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool WriteFile(string strFilePath, string strContent, bool append, Encoding encoding)
        {
            try
            {
                StreamWriter sw = new StreamWriter(strFilePath, append, encoding);
                sw.Write(strContent);
                sw.Close();
                return File.Exists(strFilePath);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region  写入错误日志
        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="strFileDir"></param>
        /// <param name="strFileName"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool WriteErrorLog(Exception ex, string strFileDir, string strFileName, bool append, Encoding encoding)
        {
            try
            {
                CreateDir(strFileDir);

                string strText = "{0} Message: {1}\r\nSource: {2}\r\nStackTrace: {3}\r\nTargetSite: {4}\r\n\r\n";
                string strContent = String.Format(strText, GetDateTime(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite);

                return WriteFile(strFileDir + strFileName, strContent, append, encoding);
            }
            catch (Exception exx)
            {
                throw (exx);
            }
        }
        #endregion

        #region  写入事件日志
        /// <summary>
        /// 写入事件日志
        /// </summary>
        /// <param name="strFileDir"></param>
        /// <param name="strFileName"></param>
        /// <param name="strEventName"></param>
        /// <param name="strEventContent"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool WriteEventLog(string strFileDir, string strFileName, string strEventName, string strEventContent, bool append, Encoding encoding)
        {
            try
            {
                CreateDir(strFileDir);

                string strText = "{0} [{1}] {2}\r\n\r\n";
                string strContent = String.Format(strText, GetDateTime(), strEventName, strEventContent);

                return WriteFile(strFileDir + strFileName, strContent, append, encoding);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 写入事件日志
        /// </summary>
        /// <param name="strFileDir"></param>
        /// <param name="strFileName"></param>
        /// <param name="strEventName"></param>
        /// <param name="strEventContent"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <param name="isSpaceLine"></param>
        /// <returns></returns>
        public static bool WriteEventLog(string strFileDir, string strFileName, string strEventName, string strEventContent, bool append, Encoding encoding, bool isSpaceLine)
        {
            try
            {
                CreateDir(strFileDir);

                string strText = "{0} [{1}] {2}\r\n" + (isSpaceLine ? "\r\n" : "");
                string strContent = String.Format(strText, GetDateTime(), strEventName, strEventContent);

                return WriteFile(strFileDir + strFileName, strContent, append, encoding);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 写入事件日志
        /// </summary>
        /// <param name="strFileDir"></param>
        /// <param name="strFileName"></param>
        /// <param name="strEventName"></param>
        /// <param name="strEventContent"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <param name="isSpaceLine"></param>
        /// <param name="showRawUrl"></param>
        /// <returns></returns>
        public static bool WriteEventLog(string strFileDir, string strFileName, string strEventName, string strEventContent, bool append, Encoding encoding, bool isSpaceLine, bool showRawUrl)
        {
            try
            {
                CreateDir(strFileDir);

                string strContent = "";
                if (showRawUrl)
                {
                    strContent = String.Format("{0} [{1}] {2}\r\n", GetDateTime(), strEventName, strEventContent);
                }
                else
                {
                    strContent = String.Format("{0} [{1}] {2}\r\n", GetDateTime(), strEventName, strEventContent);
                }
                if (isSpaceLine)
                {
                    strContent += "\r\n";
                }
                return WriteFile(strFileDir + strFileName, strContent, append, encoding);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

    }
}