using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

/// <summary>
///Public 的摘要说明
/// </summary>
public class Public : OMIS.DBA.Common
{
    public Public()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //        
    }
        
    #region  过滤SQL特殊字符
    public static string ReplaceSqlSpecialChar(string strContent)
    {
        return strContent.Replace(";", "").Replace("\\", "\\\\").Replace("'", "\'");
    }

    public static string FilterSqlSpecialChar(string strContent)
    {
        Regex reg = new Regex("(--)|([';\"])|(/[*])|([*]/)");
        return reg.Replace(strContent, "");
    }

    public static string FilterSqlKeywords(string strContent)
    {
        Regex reg = new Regex("(drop|alter|insert|delete|update|select|exec|execute|convert)", RegexOptions.IgnoreCase);
        return reg.Replace(strContent, "");
    }
    #endregion

    #region  验证主机域名是否正确
    public static bool CheckWebHost(HttpRequest hr, string strWebHost)
    {
        string strHost = hr.Url.Host;
        if (strHost.ToLower().Equals("localhost") || strHost.Equals("127.0.0.1") || strWebHost.Equals(string.Empty))
        {
            return true;
        }
        if (hr.Url.Port > 0 && hr.Url.Port != 80)
        {
            strHost += ":" + hr.Url.Port;
        }
        return strHost.Equals(strWebHost);
    }
    #endregion

    #region  替换URL中的主机域名
    public static string ReplaceWebHost(HttpRequest hr, string strWebHost)
    {
        string strUrl = hr.Url.ToString().Replace("http://", "");
        int li = strUrl.IndexOf('/');
        if (li >= 0)
        {
            strUrl = strUrl.Substring(li);
        }
        return "http://" + strWebHost + strUrl;
    }
    #endregion

    #region  JS方式跳转到登录页面
    public static void WriteJsToLoginUrl(string strPageUrl)
    {
        string strText = "<script type=\"text/javascript\">top.location.href='{0}';</script>";
        HttpContext.Current.Response.Write(String.Format(strText, strPageUrl));
    }
    #endregion
    
    #region  替换字符串为查询语句条件
    public static string ReplaceStringToCondition(string strValue)
    {
        if (strValue.Equals(string.Empty) || strValue.IndexOf("'") >= 0 || strValue.IndexOf("','") >= 0)
        {
            return strValue;
        }
        else
        {
            return String.Format("'{0}'", strValue.Replace(",", "','"));
        }
    }
    #endregion

    #region  检测字符串是否包含在内容中
    /// <summary>
    /// 检测字符串是否包含在内容中
    /// </summary>
    /// <param name="strMatch">匹配值</param>
    /// <param name="strContent">多个内容，以英文逗号“,”分隔</param>
    /// <returns></returns>
    public static bool CheckStringInArray(string strMatch, string strContent)
    {
        string[] arr = strContent.Split(',');
        return CheckStringInArray(strMatch, arr);
    }

    /// <summary>
    /// 检测字符串是否包含在内容中
    /// </summary>
    /// <param name="strMatch">匹配值</param>
    /// <param name="arrContent">内容数组</param>
    /// <returns></returns>
    public static bool CheckStringInArray(string strMatch, string[] arrContent)
    {
        foreach (string str in arrContent)
        {
            if (strMatch.Equals(str))
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region  创建文件名
    public static string BuildFileName()
    {
        return String.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), new Random().Next(0, 9));
    }
    #endregion

    #region  获取图片尺寸及大小
    public static string GetImageSizeToJs(string strImagePath)
    {
        string strRealPath = HttpContext.Current.Server.MapPath(Config.WebDir + strImagePath);

        System.Drawing.Image img = System.Drawing.Image.FromFile(strRealPath);

        int[] result = new int[] { img.Width, img.Height };

        img.Dispose();

        return String.Format("{{width:{0},height:{1}}}", result[0], result[1]);
    }
    #endregion
    

    #region  清除文件缓存
    public static string NoCache()
    {
        return DateTime.Now.ToString("mmss");
    }
    #endregion

    
    #region  生成新的临时ID
    public static int BuildTempId()
    {
        return -Convert.ToInt32(DateTime.Now.ToString("HHmmssfff"));
    }
    #endregion



    #region  UrlParam

    #region  BuildUrlParamConnector (& or ?)
    public static string BuildUrlParamConnector(string url)
    {
        return url.IndexOf('?') >= 0 ? "&" : "?";
    }
    #endregion

    #region  GetRawUrlDir
    public static string GetRawUrlDir(string rawUrl)
    {
        return rawUrl.Substring(0, rawUrl.LastIndexOf('/') + 1);
    }
    #endregion

    #endregion
    
    #region  BuildEmptyList
    public static List<int> BuildEmptyList()
    {
        return new List<int>();
    }
    #endregion


    #region  加载JS文件
    public static string LoadJs(string file, string dir, bool isAddWebDir)
    {
        StringBuilder js = new StringBuilder();
        string[] arr = file.Split(new string[] { ",", "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        int n = 0;
        string _cache = NoCache();
        foreach (string s in arr)
        {
            if (s.Split('?')[0].ToLower().EndsWith(".js"))
            {
                string fs = dir + String.Format(s, _cache);
                fs = (isAddWebDir ? (!s.StartsWith("http://") && !s.StartsWith(Config.WebDir) ? Config.WebDir : "") : "") + fs;
                //js.Append(String.Format("{0}<script type=\"text/javascript\" src=\"{1}\"></script>", n++ > 0 ? "\r\n" : "", fs));
                js.Append(String.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", fs));
            }
        }
        return js.ToString();
    }

    public static string LoadJs(string file, string dir)
    {
        return LoadJs(file, dir, true);
    }

    public static string LoadJs(string file)
    {
        return LoadJs(file, string.Empty, false);
    }
    #endregion

    #region  加载JS文件代码
    public static string LoadJsCode(string file)
    {
        return LoadJsCode(file, string.Empty);
    }

    public static string LoadJsCode(string file, string dir)
    {
        StringBuilder js = new StringBuilder();
        string[] arr = file.Split(new string[] { ",", "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string s in arr)
        {
            if (s.Split('?')[0].ToLower().EndsWith(".js") && !s.StartsWith("http://"))
            {
                string p = (dir.Trim().Length > 0 ? (!s.StartsWith(Config.WebDir) ? Config.WebDir : "") + dir : "") + s.Split('?')[0];
                p = HttpContext.Current.Server.MapPath(p);

                if (File.Exists(p))
                {
                    StreamReader sr = new StreamReader(p, Encoding.UTF8);
                    js.Append(String.Format("<script type=\"text/javascript\">{0}</script>", sr.ReadToEnd()));
                    sr.Close();
                }
            }
        }
        return js.ToString();
    }
    #endregion

}