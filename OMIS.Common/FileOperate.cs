using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Drawing;

namespace OMIS.Common
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public class FileOperate
    {

        #region  变量属性
        private string errorCode = "";
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode
        {
            get
            {
                return this.errorCode;
            }
            set
            {
                this.errorCode = value;
            }
        }
        private string returnValue = "";
        /// <summary>
        /// 返回结果
        /// </summary>
        public string ReturnValue
        {
            get
            {
                return this.returnValue;
            }
            set
            {
                this.returnValue = value;
            }
        }
        #endregion

        #region  替换文件目录
        /// <summary>
        /// 替换文件目录
        ///  "/" 替换为 "\"，以"\"结尾，适用于不带文件名的目录地址
        /// </summary>
        /// <param name="fileDir">要替换的文件目录</param>
        /// <returns></returns>
        public static string ReplaceFileDir(string fileDir)
        {
            fileDir = fileDir.Replace("/", @"\");
            return fileDir.EndsWith("\\") ? fileDir : fileDir + "\\";
        }
        /// <summary>
        /// 替换文件名称
        ///  "/" 替换为 "\"
        /// </summary>
        /// <param name="fileName">要替换的文件名称</param>
        /// <returns></returns>
        public static string ReplaceFileName(string fileName)
        {
            return fileName.Replace("/", @"\");
        }
        /// <summary>
        /// 替换网页地址为物理路径地址
        /// "/" 替换为 "\"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplaceUrlToPath(string path)
        {
            return path.Replace("/", @"\");
        }
        /// <summary>
        /// 替换物理路径地址为网页地址
        /// "\" 替换为 "/"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplacePathToUrl(string path)
        {
            return path.Replace(@"\", "/");
        }
        #endregion

        #region  读取文件内容
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="hr"></param>
        /// <param name="fileDir">文件保存目录 以 / 或 \ 结尾</param>
        /// <param name="fileName">文件名（不包含目录）</param>
        /// <param name="encoding">文件编码</param>
        /// <returns></returns>
        public static string ReadFile(HttpRequest hr, string fileDir, string fileName, Encoding encoding)
        {
            try
            {
                string filePath = hr.PhysicalApplicationPath + fileDir + fileName;

                return ReadFile(filePath, encoding);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  生成静态页面
        /// <summary>
        /// 生成静态页面
        /// 执行ASP.NET动态页面生成HTML静态页面
        /// </summary>
        /// <param name="hr"></param>
        /// <param name="sourcePage">源页面 相对目录</param>
        /// <param name="fileDir">保存目录 相对于网站根目录 以 / 或 \ 结尾</param>
        /// <param name="fileName">HTML页面名称</param>
        /// <param name="append">是否追加</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public bool BuildHtml(HttpRequest hr, string sourcePage, string fileDir, string fileName, bool append, Encoding encoding)
        {
            try
            {
                StringWriter myWrite = new StringWriter();
                System.Web.UI.Page myPage = new Page();
                myPage.Server.Execute(sourcePage, myWrite);

                fileDir = hr.PhysicalApplicationPath + fileDir;
                CreateDir(fileDir);

                return WriteFile(fileDir + fileName, myWrite.ToString(), append, encoding);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

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
        /// <param name="fileDir"></param>
        /// <returns></returns>
        public static bool CreateDir(string fileDir)
        {
            try
            {
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                return Directory.Exists(fileDir);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  读取文件
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    StreamReader sr = new StreamReader(filePath, encoding);
                    string strResult = sr.ReadToEnd();
                    sr.Close();
                    return strResult;
                }
                else
                {
                    return filePath + " file does not exist.";
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
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool WriteFile(string filePath, string content, bool append, Encoding encoding)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filePath, append, encoding);
                sw.Write(content);
                sw.Close();
                return File.Exists(filePath);
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
        /// <param name="hr"></param>
        /// <param name="fileDir"></param>
        /// <param name="fileName"></param>
        /// <param name="eventName"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool WriteErrorLog(Exception ex, HttpRequest hr, string fileDir, string fileName, string eventName, bool append, Encoding encoding)
        {
            try
            {
                fileDir = hr.PhysicalApplicationPath + fileDir;
                CreateDir(fileDir);

                string strText = "{0}{1} Message: {2}\r\nSource: {3}\r\nStackTrace: {4}\r\nTargetSite: {5}\r\nRawUrl: {6}\r\n\r\n";
                eventName = eventName.Equals(string.Empty) ? "" : String.Format(" [{0}]", eventName);
                string content = String.Format(strText, GetDateTime(), eventName, ex.Message, ex.Source, ex.StackTrace, ex.TargetSite, hr.RawUrl);

                return WriteFile(fileDir + fileName, content, append, encoding);
            }
            catch (Exception exx)
            {
                throw (exx);
            }
        }

        public static bool WriteErrorLog(Exception ex, HttpRequest hr, string fileDir, string fileName, bool append, Encoding encoding)
        {
            try
            {
                fileDir = hr.PhysicalApplicationPath + fileDir;
                CreateDir(fileDir);

                string strText = "{0} Message: {1}\r\nSource: {2}\r\nStackTrace: {3}\r\nTargetSite: {4}\r\nRawUrl: {5}\r\n\r\n";
                string content = String.Format(strText, GetDateTime(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite, hr.RawUrl);

                return WriteFile(fileDir + fileName, content, append, encoding);
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
        /// <param name="hr"></param>
        /// <param name="fileDir"></param>
        /// <param name="fileName"></param>
        /// <param name="eventName"></param>
        /// <param name="eventContent"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool WriteEventLog(HttpRequest hr, string fileDir, string fileName, string eventName, string eventContent, bool append, Encoding encoding)
        {
            try
            {
                fileDir = hr.PhysicalApplicationPath + fileDir;
                CreateDir(fileDir);

                string strText = "{0} [{1}] {2}\r\nRawUrl:{3}\r\n\r\n";
                string content = String.Format(strText, GetDateTime(), eventName, eventContent, hr.RawUrl);

                return WriteFile(fileDir + fileName, content, append, encoding);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 写入事件日志
        /// </summary>
        /// <param name="hr"></param>
        /// <param name="fileDir"></param>
        /// <param name="fileName"></param>
        /// <param name="eventName"></param>
        /// <param name="eventContent"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <param name="isSpaceLine"></param>
        /// <returns></returns>
        public static bool WriteEventLog(HttpRequest hr, string fileDir, string fileName, string eventName, string eventContent, bool append, Encoding encoding, bool isSpaceLine)
        {
            try
            {
                fileDir = hr.PhysicalApplicationPath + fileDir;
                CreateDir(fileDir);

                string strText = "{0} [{1}] {2}\r\nRawUrl:{3}\r\n" + (isSpaceLine ? "\r\n" : "");
                string content = String.Format(strText, GetDateTime(), eventName, eventContent, hr.RawUrl);

                return WriteFile(fileDir + fileName, content, append, encoding);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 写入事件日志
        /// </summary>
        /// <param name="hr"></param>
        /// <param name="fileDir"></param>
        /// <param name="fileName"></param>
        /// <param name="eventName"></param>
        /// <param name="eventContent"></param>
        /// <param name="append"></param>
        /// <param name="encoding"></param>
        /// <param name="isSpaceLine"></param>
        /// <param name="showRawUrl"></param>
        /// <returns></returns>
        public static bool WriteEventLog(HttpRequest hr, string fileDir, string fileName, string eventName, string eventContent, bool append, Encoding encoding, bool isSpaceLine, bool showRawUrl)
        {
            try
            {
                fileDir = hr.PhysicalApplicationPath + fileDir;
                CreateDir(fileDir);

                string content = "";
                if (showRawUrl)
                {
                    content = String.Format("{0} [{1}] {2}\r\nRawUrl:{3}\r\n", GetDateTime(), eventName, eventContent, hr.RawUrl);
                }
                else
                {
                    content = String.Format("{0} [{1}] {2}\r\n", GetDateTime(), eventName, eventContent);
                }
                if (isSpaceLine)
                {
                    content += "\r\n";
                }
                return WriteFile(fileDir + fileName, content, append, encoding);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region  删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="hr"></param>
        /// <param name="fileDir">文件保存目录 以 / 或 \ 结尾</param>
        /// <param name="fileName">文件名（不包含目录）</param>
        /// <returns></returns>
        public static bool DeleteFile(HttpRequest hr, string fileDir, string fileName)
        {
            try
            {
                string filePath = hr.PhysicalApplicationPath + fileDir + fileName;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return !File.Exists(filePath);
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

        #region  下载HTML代码中的远程图片并替换HTML内容
        /// <summary>
        /// 获取HTML代码中图片地址
        /// </summary>
        /// <param name="html"></param>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static List<String> GetImgTag(string html, string url)
        {
            Regex regObj = new Regex("<img(.|\n)*?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            List<String> arrImg = new List<string>();

            foreach (Match matchItem in regObj.Matches(html))
            {
                string imgUrl = GetImgUrl(matchItem.Value);
                if (!imgUrl.Equals(string.Empty) && (imgUrl.IndexOf(url) < 0 || url.Equals(string.Empty)))
                {
                    arrImg.Add(imgUrl);
                }
            }
            return arrImg;
        }

        private static string GetImgUrl(string imgTag)
        {
            string str = string.Empty;
            //Regex regObj = new Regex("http://.+.(?:jpg|gif|bmp|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regObj = new Regex(@"src=(""|\').+.(?:jpg|gif|bmp|png)(""|\')", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match matchItem in regObj.Matches(imgTag))
            {
                str = matchItem.Value;
                str = str.Substring(str.IndexOf('=') + 1).Replace("\"", "").Replace("'", "");
            }
            return str;
        }

        /// <summary>
        /// 下载远程图片
        /// </summary>
        /// <param name="html"></param>
        /// <param name="hr"></param>
        /// <param name="dir">图片保存目录</param>
        /// <param name="webUrl">网站本地域名网址，如果图片网址不是本地网址，则需要下载图片</param>
        /// <param name="newUrl">下载后的图片域名网址</param>
        /// <param name="markThumb">是否生成缩略图</param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static string DownloadRemotePicture(string html, HttpRequest hr, string dir, string webUrl, string newUrl, bool markThumb, int w, int h)
        {
            string strSource = html;
            string strError = string.Empty;
            List<String> arrImgUrl = GetImgTag(html, webUrl);
            string saveDir = ReplaceFileDir(dir);

            try
            {
                if (!Directory.Exists(hr.PhysicalApplicationPath + saveDir))
                {
                    Directory.CreateDirectory(hr.PhysicalApplicationPath + saveDir);
                }

                for (int i = 0; i < arrImgUrl.Count; i++)
                {
                    try
                    {
                        if (arrImgUrl[i] != null)
                        {
                            WebRequest req = WebRequest.Create(arrImgUrl[i]);
                            string strName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (i + 1) + Path.GetExtension(arrImgUrl[i]).ToLower();
                            string strCurUrl = arrImgUrl[i];

                            string strSavePath = hr.PhysicalApplicationPath + saveDir + strName;
                            string strImgPath = newUrl + dir + strName;

                            WebClient wc = new WebClient();
                            if (markThumb)
                            {
                                string strTempPath = hr.PhysicalApplicationPath + saveDir + "temp_" + strName;

                                wc.DownloadFile(arrImgUrl[i], strTempPath);

                                MakeThumbnail(strTempPath, strSavePath, w, h, MakeThumbnailMode.W);

                                if (File.Exists(strSavePath))
                                {
                                    File.Delete(strTempPath);
                                }
                            }
                            else
                            {
                                wc.DownloadFile(arrImgUrl[i], strSavePath);
                            }
                            html = html.Replace(arrImgUrl[i], strImgPath);
                        }
                    }
                    catch (Exception exx)
                    {
                        strError = exx.Message;
                        continue;
                    }
                }

                return html;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return strSource;
            }
        }
        #endregion

        #region  生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, float width, float height, MakeThumbnailMode mode)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(originalImagePath, false);

            string format = Path.GetExtension(originalImagePath);
            if (format.ToLower().Equals(".gif"))
            {
                int frameCount = img.GetFrameCount(System.Drawing.Imaging.FrameDimension.Time);
                if (frameCount > 1)
                {
                    new GifOperate().MakeThumbnail(originalImagePath, (int)width, thumbnailPath);
                }
                else
                {
                    BuildMakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, MakeThumbnailFileFormat.Jpg);
                }
            }
            else
            {
                BuildMakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, MakeThumbnailFileFormat.Jpg);
            }
            img.Dispose();
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        /// <param name="fileFormat">生成缩略图的保存格式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, float width, float height,
            MakeThumbnailMode mode, MakeThumbnailFileFormat fileFormat)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(originalImagePath, false);
            string format = Path.GetExtension(originalImagePath);
            if (format.ToLower().Equals(".gif"))
            {
                int frameCount = img.GetFrameCount(System.Drawing.Imaging.FrameDimension.Time);
                if (frameCount > 1)
                {
                    new GifOperate().MakeThumbnail(originalImagePath, (int)width, thumbnailPath);
                }
                else
                {
                    BuildMakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, fileFormat);
                }
            }
            else
            {
                BuildMakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, fileFormat);
            }
            img.Dispose();

        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="thumbnailPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <param name="fileFormat"></param>
        private static void BuildMakeThumbnail(string originalImagePath, string thumbnailPath, float width, float height,
            MakeThumbnailMode mode, MakeThumbnailFileFormat fileFormat)
        {
            string strError = string.Empty;
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath, false);
            float newWidth = width;
            float newHeight = height;
            float x = 0;
            float y = 0;
            float oldWidth = (float)originalImage.Width;
            float oldHeight = (float)originalImage.Height;
            
            if (oldWidth <= width && oldHeight <= height)
            {
                originalImage.Dispose();
                try
                {
                    File.Copy(originalImagePath, thumbnailPath);
                }
                catch (Exception exx) { strError = exx.Message; }
            }
            else
            {
                #region
                switch (mode)
                {
                    case MakeThumbnailMode.HW://指定高宽缩放(按比例)
                    default:
                        if (oldWidth > newWidth || oldHeight > newHeight)
                        {
                            if (oldWidth > oldHeight)
                            {
                                /*计算图片宽度缩小后的比率*/
                                float ratio = newWidth / oldWidth;
                                /*计算新的高度（根据比率）*/
                                newHeight = oldHeight * ratio;
                            }
                            else
                            {
                                /*计算图片高度缩小后的比率*/
                                float ratio = newHeight / oldHeight;
                                /*计算新的宽度（根据比率）*/
                                newWidth = oldWidth * ratio;
                            }
                        }
                        else
                        {
                            newWidth = oldWidth;
                            newHeight = oldHeight;
                        }
                        break;
                    case MakeThumbnailMode.HWR://指定高宽缩放(不按比例)
                        newWidth = width;
                        newHeight = height;
                        break;
                    case MakeThumbnailMode.W://指定宽，高按比例
                        newWidth = oldWidth < width ? oldWidth : width;
                        newHeight = oldHeight * newWidth / oldWidth;
                        break;
                    case MakeThumbnailMode.H://指定高，宽按比例
                        newHeight = oldHeight < height ? oldHeight : height;
                        newWidth = oldWidth * newHeight / oldHeight;
                        break;
                    case MakeThumbnailMode.Cut://指定高宽裁减（不变形）                
                        if ((double)oldWidth / (double)oldHeight > (double)newWidth / (double)newHeight)
                        {
                            oldHeight = originalImage.Height;
                            oldWidth = originalImage.Height * newWidth / newHeight;
                            y = 0;
                            x = (originalImage.Width - oldWidth) / 2;
                        }
                        else
                        {
                            oldWidth = originalImage.Width;
                            oldHeight = originalImage.Width * height / newWidth;
                            x = 0;
                            y = (originalImage.Height - oldHeight) / 2;
                        }
                        break;
                }
                #endregion

                //新建一个bmp图片
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                //新建一个画板
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(System.Drawing.Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, (int)newWidth, (int)newHeight),
                    new System.Drawing.Rectangle((int)x, (int)y, (int)oldWidth, (int)oldHeight),
                    System.Drawing.GraphicsUnit.Pixel);
                try
                {
                    string strFormat = fileFormat == MakeThumbnailFileFormat.None ? Path.GetExtension(originalImagePath) : "." + fileFormat.ToString().ToLower();
                    switch (strFormat)
                    {
                        case ".jpg":
                        case ".jpeg":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case ".gif":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case ".png":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case ".bmp":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case ".ico":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Icon);
                            break;
                        case ".tiff":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Tiff);
                            break;
                        default:
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                    }
                }
                catch (System.Exception e)
                {
                    throw (e);
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
            }
        }
        #endregion

        #region  图片水印
        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="originalImagePath">原始图片路径名称</param>
        /// <param name="thumbnailPath">生成水印图片后的图片路径名称</param>
        /// <param name="type">水印类型：1表示文字水印，2表示图片水印</param>
        /// <param name="wmImg">若水印类型为文字水印，则传水印文字内容；若水印类型为图片水印，则传水印图片路径名称</param>
        public static void WaterMark(string originalImagePath, string thumbnailPath, WaterMarkType type, string wmImg, bool deleteSource)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(originalImagePath);
            Graphics g = Graphics.FromImage(image);
            switch (type)
            {
                case WaterMarkType.Text:
                    //加文字水印，注意，这里的代码和以下加图片水印的代码不能共存
                    //System.Drawing.Image image = System.Drawing.Image.FromFile(originalImagePath);
                    //Graphics g = Graphics.FromImage(image);
                    g.DrawImage(image, 0, 0, image.Width, image.Height);
                    Font f = new Font("Verdana", 32);
                    Brush b = new SolidBrush(Color.White);
                    string addText = wmImg;
                    g.DrawString(addText, f, b, 10, 10);
                    g.Dispose();
                    break;
                case WaterMarkType.Image:
                    //加图片水印
                    //System.Drawing.Image image = System.Drawing.Image.FromFile(originalImagePath);
                    System.Drawing.Image copyImage = System.Drawing.Image.FromFile(wmImg);
                    //Graphics g = Graphics.FromImage(image);
                    g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                    break;
            }
            image.Save(thumbnailPath);
            image.Dispose();

            if (deleteSource && File.Exists(originalImagePath))
            {
                File.Delete(originalImagePath);
            }
        }
        #endregion

        #region  比较两张图片是否一致
        /// <summary>
        /// 比较两张图片是否一致
        /// </summary>
        /// <param name="firstImage">图片一</param>
        /// <param name="secondImage">图片二</param>
        /// <returns></returns>
        public static bool ImageCompareString(Bitmap firstImage, Bitmap secondImage)
        {
            MemoryStream ms = new MemoryStream();
            firstImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String firstBitmap = Convert.ToBase64String(ms.ToArray());
            ms.Position = 0;

            secondImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String secondBitmap = Convert.ToBase64String(ms.ToArray());

            if (firstBitmap.Equals(secondBitmap))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region  获取文件扩展名 (导出文件时用)
        private static string GetFileExtension(string fileName, string defaultExt)
        {
            try
            {
                string ext = Path.GetExtension(fileName).ToLower();
                if (ext.Equals(string.Empty))
                {
                    ext = defaultExt;
                }
                return ext;
            }
            catch (Exception ex) { return defaultExt; }
        }
        #endregion

        #region  导出Excel文件
        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">编码：UTF-8,GB2312</param>
        public static void ExportExcel(string fileName, string content, Encoding encoding)
        {
            try
            {
                ExportExcel(fileName, content, encoding, true);
            }
            catch (Exception ex) { throw (ex); }
        }

        public static void ExportExcel(string fileName, string content, Encoding encoding, bool isIE)
        {
            try
            {
                ExportExcel(fileName, "Sheet1", content, encoding, true);
            }
            catch (Exception ex) { throw (ex); }
        }

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">编码：UTF-8,GB2312</param>
        /// <param name="isIE">是否为IE浏览器，若是IE浏览器，中文文件名需要转换编码</param>
        public static void ExportExcel(string fileName, string sheetName, string content, Encoding encoding, bool isIE)
        {
            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = encoding.ToString();
                HttpContext.Current.Response.ContentEncoding = encoding;
                fileName = isIE ? HttpUtility.UrlEncode(fileName, Encoding.UTF8) : fileName;
                string ext = GetFileExtension(fileName, ".xls");
                fileName = Path.GetFileNameWithoutExtension(fileName) + ext;

                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                HttpContext.Current.Response.ContentType = "Application/ms-excel";

                StringBuilder head = new StringBuilder();
                head.Append("<html");
                head.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
                head.Append(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
                head.Append(" xmlns=\"http://www.w3.org/TR/REC-html40\"");
                head.Append(">");
                head.Append("<head>");
                head.Append("<!--[if gte mso 9]>");
                head.Append("<xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>");
                head.Append("<x:Name>");
                head.Append(sheetName);
                head.Append("</x:Name>");
                head.Append("<x:WorksheetOptions><x:Print><x:ValidPrinterInfo /></x:Print></x:WorksheetOptions>");
                head.Append("</x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml>");
                head.Append("<![endif]-->");
                head.Append("</head><body>");

                StringBuilder foot = new StringBuilder();
                foot.Append("</body></html>");

                HttpContext.Current.Response.Write(head.ToString() + content + foot.ToString());
                HttpContext.Current.Response.Flush();
                //HttpContext.Current.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  导出Word文件
        /// <summary>
        /// 导出Word文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">编码：UTF-8,GB2312</param>
        public static void ExportWord(string fileName, string content, Encoding encoding)
        {
            try
            {
                ExportWord(fileName, content, encoding, true);
            }
            catch (Exception ex) { throw (ex); }
        }

        /// <summary>
        /// 导出Word文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">编码：UTF-8,GB2312</param>
        /// <param name="isIE">是否为IE浏览器，若是IE浏览器，中文文件名需要转换编码</param>
        public static void ExportWord(string fileName, string content, Encoding encoding, bool isIE)
        {
            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = encoding.ToString();
                HttpContext.Current.Response.ContentEncoding = encoding;
                fileName = isIE ? HttpUtility.UrlEncode(fileName, Encoding.UTF8) : fileName;
                string ext = GetFileExtension(fileName, ".doc");
                fileName = Path.GetFileNameWithoutExtension(fileName) + ext;

                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                HttpContext.Current.Response.ContentType = "Application/ms-word";

                StringBuilder head = new StringBuilder();
                head.Append("<html");
                head.Append(" xmlns:v=\"urn:schemas-microsoft-com:vml\"");
                head.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
                head.Append(" xmlns:w=\"urn:schemas-microsoft-com:office:word\"");
                head.Append(" xmlns:dt=\"uuid:C2F41010-65B3-11d1-A29F-00AA00C14882\"");
                head.Append(" xmlns=\"http://www.w3.org/TR/REC-html40\"");
                head.Append(">");
                head.Append("<head>");
                head.Append("<!--[if gte mso 9]>");
                head.Append("<xml><w:WordDocument>");
                head.Append("<w:View>Print</w:View>");
                head.Append("</w:WordDocument></xml>");
                head.Append("<![endif]-->");
                head.Append("</head><body>");

                StringBuilder foot = new StringBuilder();
                foot.Append("</body></html>");

                HttpContext.Current.Response.Write(head.ToString() + content + foot.ToString());
                HttpContext.Current.Response.Flush();
                //HttpContext.Current.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  导出TXT文件
        public static void ExportTxt(string fileName, string content, Encoding encoding, bool isIE)
        {
            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = encoding.ToString();
                HttpContext.Current.Response.ContentEncoding = encoding;
                fileName = isIE ? HttpUtility.UrlEncode(fileName, Encoding.UTF8) : fileName;
                string ext = GetFileExtension(fileName, ".txt");
                fileName = Path.GetFileNameWithoutExtension(fileName) + ext;

                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                HttpContext.Current.Response.ContentType = "text/plain";

                HttpContext.Current.Response.Write(content);
                HttpContext.Current.Response.Flush();
                //HttpContext.Current.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  下载远程图片到本地
        /// <summary>
        /// 下载远程图片到本地
        /// </summary>
        /// <param name="remotePath">远程图片路径URL</param>
        /// <param name="localPath">要保存的本地图片物理路径名称</param>
        /// <returns></returns>
        public static bool DownloadRemoteImage(string remotePath, string localPath)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(remotePath, localPath);

                return File.Exists(localPath);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  下载图片文件
        public static void DownloadImage(string filePath)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                string extension = Path.GetExtension(filePath).ToLower();
                string contentType = "image/jpeg";

                switch (extension)
                {
                    case "jpg":
                    case "jpeg":
                        contentType = "image/jpeg";
                        break;
                    case "gif":
                        contentType = "image/gif";
                        break;
                    case "png":
                        contentType = "image/png";
                        break;
                    case "bmp":
                        contentType = "image/bmp";
                        break;
                    default:
                        contentType = "image/jpeg";
                        break;
                }

                HttpContext.Current.Response.ContentType = contentType;
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.TransmitFile(filePath);
                //HttpContext.Current.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region  验证文件类型扩展名
        public static bool ValidateFileExtension(string fileEx, FileExtension[] arrFileEx)
        {
            try
            {
                if (fileEx.Equals(string.Empty))
                {
                    return false;
                }
                foreach (FileExtension fe in arrFileEx)
                {
                    //if (Convert.ToInt32(strFileEx) == fe.GetHashCode())
                    //{
                    //    return true;
                    //}
                    if (CheckFileExtensionCode(Convert.ToInt32(fileEx), fe))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static bool ValidateFileExtension(System.Web.UI.HtmlControls.HtmlInputFile fu, FileExtension[] arrFileEx)
        {
            try
            {
                string strError = string.Empty;
                string strFileEx = string.Empty;
                int filelen = fu.PostedFile.ContentLength;
                byte[] arrFileByte = new byte[filelen];

                fu.PostedFile.InputStream.Read(arrFileByte, 0, filelen);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(arrFileByte);
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);

                try
                {
                    strFileEx += br.ReadByte().ToString();
                    strFileEx += br.ReadByte().ToString();
                }
                catch (Exception ex) { strError = ex.Message; }
                br.Close();
                ms.Close();

                return ValidateFileExtension(strFileEx, arrFileEx);
            }
            catch (Exception ex) { throw (ex); }
        }
        public static bool ValidateFileExtension(System.Web.UI.WebControls.FileUpload fu, FileExtension[] arrFileEx)
        {
            try
            {
                string strError = string.Empty;
                string strFileEx = string.Empty;
                int filelen = fu.PostedFile.ContentLength;
                byte[] arrFileByte = new byte[filelen];

                fu.PostedFile.InputStream.Read(arrFileByte, 0, filelen);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(arrFileByte);
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);

                try
                {
                    strFileEx += br.ReadByte().ToString();
                    strFileEx += br.ReadByte().ToString();
                }
                catch (Exception ex) { strError = ex.Message; }
                br.Close();
                ms.Close();

                return ValidateFileExtension(strFileEx, arrFileEx);
            }
            catch (Exception ex) { throw (ex); }
        }
        public static bool ValidateFileExtension(System.Web.HttpPostedFile fu, FileExtension[] arrFileEx)
        {
            try
            {
                string strError = string.Empty;
                string strFileEx = string.Empty;
                int filelen = fu.ContentLength;
                byte[] arrFileByte = new byte[filelen];

                fu.InputStream.Read(arrFileByte, 0, filelen);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(arrFileByte);
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);

                try
                {
                    strFileEx += br.ReadByte().ToString();
                    strFileEx += br.ReadByte().ToString();
                }
                catch (Exception ex) { strError = ex.Message; }
                br.Close();
                ms.Close();

                return ValidateFileExtension(strFileEx, arrFileEx);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  读取文件扩展名
        public static string ReadFileExtension(byte[] arrFileByte)
        {
            string strFileEx = string.Empty;

            System.IO.MemoryStream ms = new System.IO.MemoryStream(arrFileByte);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            try
            {
                strFileEx += br.ReadByte().ToString();
                strFileEx += br.ReadByte().ToString();

                return strFileEx;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                br.Close();
                ms.Close();
            }
        }
        #endregion

        #region 获得文件格式编码
        public static bool CheckFileExtensionCode(int curCode, FileExtension fe)
        {
            bool result = false;
            int[] arrCode;
            switch (fe)
            {
                case FileExtension.XLS:
                    arrCode = new int[] { fe.GetHashCode(), FileExtension.XLS_HTML.GetHashCode() };
                    foreach (int num in arrCode)
                    {
                        result = curCode == num;
                        if (result)
                        {
                            break;
                        }
                    }
                    break;
                default:
                    result = curCode == fe.GetHashCode();
                    break;
            }
            return result;
        }
        #endregion

    }

    #region  枚举

    #region  文件写入方式
    public enum WriteType
    {
        Create,
        Append,
    }
    #endregion

    public enum MakeThumbnailMode
    {
        HW,
        HWR,
        W,
        H,
        Cut,
    }
    public enum MakeThumbnailFileFormat
    {
        None,
        Jpg,
        Jpeg,
        Gif,
        Png,
        Bmp,
        Tiff,
        Icon,
    }
    public enum WaterMarkType
    {
        None,
        Text,
        Image,
    }
    #endregion

    #region  文件扩展名
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        PNG = 13780,
        BMP = 6677,

        SWF = 6787,
        RAR = 8297,

        ZIP = 8075,
        DOCX = 8075, //DOCX实际上是一个ZIP文件
        XLSX = 8075,

        PDF = 3780, // .pdf文件
        

        DOC = 208207,
        WPS = 208207,
        XLS = 208207,
        ET = 208207, //WPS表格

        HTML = 60104,
        XLS_HTML = 60104,

        XML = 6063,
        PHP = 6063,

        DWF = 4068, // .dwf图纸
        DWG = 6567, // .dwg图纸

        _7Z = 55122,

        TXT = 102100
    }
    #endregion

}