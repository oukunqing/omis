using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Configuration;
using OMIS.DAL;

/// <summary>
///Config 的摘要说明
/// </summary>
public class Config
{
    /// <summary>
    /// 站点目录
    /// </summary>
    public static string WebDir = GetRootDir();

    public static string DBConnectionString = GetDBConnectionString();

    public static string GetDBConnectionString()
    {
        return GetConnectionString("DBConnectionString");
    }

    public static string GetDBConnectionType()
    {
        if (ConfigurationManager.ConnectionStrings["DBConnectionString"] != null)
        {
            return ConfigurationManager.ConnectionStrings["DBConnectionString"].ProviderName;
        }
        return "mysql";
    }

    #region  平台配置
    public static string UnitName = GetAppSetting("UnitName", "信阳供电段");
    /// <summary>
    /// 获得单位名称
    /// </summary>
    /// <returns></returns>
    public static string GetUnitName()
    {
        return GetAppSetting("UnitName", "信阳供电段");
    }

    public static string UnitCode = GetAppSetting("UnitCode", "XingYang");
    /// <summary>
    /// 获得单位编码
    /// </summary>
    /// <returns></returns>
    public static string GetUnitCode()
    {
        return GetAppSetting("UnitCode", "XingYang");
    }

    public static string WebName = GetAppSetting("WebName", "信阳供电段安全生产指挥系统");
    /// <summary>
    /// 获得网站平台名称
    /// </summary>
    /// <returns></returns>
    public static string GetWebName()
    {
        return GetAppSetting("WebName", "信阳供电段安全生产指挥系统");
    }

    public static string GetWebLogo()
    {
        string strImage = GetAppSetting("LogoImage");
        if (strImage.Equals(string.Empty))
        {
            return string.Empty;
        }
        return String.Format("<img src=\"{0}{1}/{2}\" alt=\"{3}\" />", WebDir, ImageDir, strImage, GetWebLogoName()).Replace("//", "/");
    }

    public static string GetWebLogoName()
    {
        return GetAppSetting("LogoName");
    }

    public static string GetCopyright()
    {
        return String.Format(GetAppSetting("Copyright"), DateTime.Now.ToString("yyyy"));
    }
    #endregion


    #region  配置文件
    public static string ConfigFileDir = GetAppSetting("ConfigFileDir");
    public static string GetConfigFileDir()
    {
        return GetAppSetting("ConfigFileDir");
    }

    public static string ConfigFileName = GetAppSetting("ConfigFileName");
    public static string GetConfigFileName()
    {
        return GetAppSetting("ConfigFileName");
    }
    #endregion

    #region  NET框架版本
    public static float NetFrameVersion = GetAppSetting("NetFrameVersion", 4.0f);
    #endregion

    #region  站点图片配置
    /// <summary>
    /// LOGO
    /// </summary>
    public static string Logo = GetAppSetting("Logo", "logo.png");
    public static string GetLogo()
    {
        return GetAppSetting("Logo", "logo.png");
    }
    /// <summary>
    /// 登录页背景图片
    /// </summary>
    public static string LoginBg = GetAppSetting("LoginBg", "login-bg.jpg");
    public static string GetLoginBg()
    {
        return GetAppSetting("LoginBg", "login-bg.jpg");
    }

    public static string GetWelcomeBg()
    {
        return GetAppSetting("WelcomeBg");
    }

    public static string GetTopBanner()
    {
        return GetAppSetting("TopBanner");
    }

    public static string GetIndexTopBanner()
    {
        return GetAppSetting("IndexTopBanner");
    }
    #endregion

    #region  站点页面设置
    /// <summary>
    /// 登录页URL
    /// </summary>
    public static string LoginUrl = GetAppSetting("LoginUrl", "/login.aspx");
    public static string GetLoginUrl()
    {
        return GetAppSetting("LoginUrl", "/login.aspx");
    }
    /// <summary>
    /// 模块页URL
    /// </summary>
    public static string ModuleUrl = GetAppSetting("ModuleUrl", "/module.aspx");
    /// <summary>
    /// 首页URL
    /// </summary>
    public static string HomeUrl = GetAppSetting("HomeUrl", "/default.aspx");
    public static string GetHomeUrl()
    {
        return GetAppSetting("HomeUrl", "/default.aspx");
    }
    /// <summary>
    /// UserNoAuthUrl
    /// </summary>
    public static string UserNoAuthUrl = GetAppSetting("UserNoAuthUrl", "/include/noauth.aspx");
    public static string GetUserNoAuthUrl()
    {
        return GetAppSetting("UserNoAuthUrl", "/include/noauth.aspx");
    }
    #endregion

    #region  选项卡配置
    public static bool ModuleMultiPage = GetAppSetting("ModuleMultiPage", 0) == 1;
    /// <summary>
    /// 模块是否启用多页面模式
    /// </summary>
    public static bool GetModuleMultiPage()
    {
        return GetAppSetting("ModuleMultiPage", 0) == 1;
    }

    public static bool AllowFirstClose = GetAppSetting("AllowFirstClose", 0) == 1;
    /// <summary>
    /// 是否允许关闭第一个加载的模块选项卡
    /// </summary>
    public static bool GetAllowFirstClose()
    {
        return GetAppSetting("AllowFirstClose", 0) == 1;
    }

    public static int ModuleTabMinCount = GetAppSetting("ModuleTabMinCount", 1);
    /// <summary>
    /// 页面选项卡最小数量
    /// </summary>
    public static int GetModuleTabMinCount()
    {
        return GetAppSetting("ModuleTabMinCount", 1);
    }
    public static int ModuleTabMaxCount = GetAppSetting("ModuleTabMaxCount", 10);
    /// <summary>
    /// 页面选项卡最大数量
    /// </summary>
    public static int GetModuleTabMaxCount()
    {
        return GetAppSetting("ModuleTabMaxCount", 10);
    }

    public static bool MultiPage = GetAppSetting("MultiPage", 0) == 1;
    /// <summary>
    /// 是否启用多页面模式
    /// </summary>
    public static bool GetMultiPage()
    {
        return GetAppSetting("MultiPage", 0) == 1;
    }
    public static int TabMinCount = GetAppSetting("TabMinCount", 10);
    /// <summary>
    /// 页面选项卡最小数量
    /// </summary>
    public static int GetTabMinCount()
    {
        return GetAppSetting("TabMinCount", 1);
    }
    public static int TabMaxCount = GetAppSetting("TabMaxCount", 10);
    /// <summary>
    /// 页面选项卡最大数量
    /// </summary>
    public static int GetTabMaxCount()
    {
        return GetAppSetting("TabMaxCount", 10);
    }

    public static int TabWordMaxLength = GetAppSetting("TabWordMaxLength", 20);
    /// <summary>
    /// 选项卡标签文字显示字数
    /// </summary>
    public static int GetTabWordMaxLength()
    {
        return GetAppSetting("TabWordMaxLength", 20);
    }    
    #endregion
    
    #region  文件目录配置
    /// <summary>
    /// 上传文件目录
    /// </summary>
    public static string UploadFileDir = GetAppSetting("UploadFileDir", "/upfiles");
    /// <summary>
    /// 图片文件目录
    /// </summary>
    public static string ImageDir = GetAppSetting("ImageDir", "/skin/default/imgs");
    /// <summary>
    /// CSS文件目录
    /// </summary>
    public static string CssDir = GetAppSetting("CssDir", "/skin/default/css");
    /// <summary>
    /// JS文件目录
    /// </summary>
    public static string JsDir = GetAppSetting("JsDir", "/js");
    #endregion

    #region  日志相关配置
    /// <summary>
    /// 错误日志目录
    /// </summary>
    public static string ErrorLogDir = GetAppSetting("ErrorLogDir", "/log/error/");
    /// <summary>
    /// 操作日志目录
    /// </summary>
    public static string EventLogDir = GetAppSetting("EventLogDir", "/log/event/");
    /// <summary>
    /// 调试日志目录
    /// </summary>
    public static string DebugLogDir = GetAppSetting("DebugLogDir", "/log/debug/");
    /// <summary>
    /// 临时日志目录
    /// </summary>
    public static string TempLogDir = GetAppSetting("TempLogDir", "/log/temp/");
    /// <summary>
    /// 日志文件大小，单位：M
    /// </summary>
    public static int LogFileSize = GetAppSetting("LogFileSize", 10);
    /// <summary>
    /// 日志保留天数
    /// </summary>
    public static int LogKeepDays = GetAppSetting("LogKeepDays", 7);   
    /// <summary>
    /// 是否保存事件日志
    /// </summary>
    public static bool SaveEventLog = GetAppSetting("SaveEventLog", 0) == 1;
    /// <summary>
    /// 是否保存调试日志
    /// </summary>
    public static bool SaveDebugLog = GetAppSetting("SaveDebugLog", 0) == 1;
    /// <summary>
    /// 是否保存错误日志,默认为保存
    /// </summary>
    public static bool SaveErrorLog = GetAppSetting("SaveErrorLog", 1) == 1;
    /// <summary>
    /// 是否保存临时日志,默认为保存
    /// </summary>
    public static bool SaveTempLog = GetAppSetting("SaveTempLog", 1) == 1;
    #endregion

    #region  用户相关配置
    /// <summary>
    /// 是否保存用户名
    /// </summary>
    public static bool SaveUserName()
    {
        return GetAppSetting("SaveUserName", 0) == 1;
    }

    /// <summary>
    /// 是否保存用户密码
    /// </summary>
    public static bool SaveUserPassword()
    {
        return GetAppSetting("SaveUserPassword", 0) == 1;
    }

    /// <summary>
    /// 是否启用自动登录
    /// </summary>
    public static bool UserAutoLogin()
    {
        return GetAppSetting("UserAutoLogin", 0) == 1;
    }

    /// <summary>
    /// 登录失败多少次后启用验证码，若设置为-1表示不启用验证码，默认为3次
    /// </summary>
    public static int LoginFailedTimes()
    {
        return GetAppSetting("LoginFailedTimes", 3);
    }

    /// <summary>
    /// 登录失败多少次后锁定帐户，若设置为-1表示不锁定，默认为10次
    /// </summary>
    public static int LoginFailedLocked()
    {
        return GetAppSetting("LoginFailedLocked", 10);
    }

    /// <summary>
    /// 登录失败后锁定帐户多少时间，单位：秒，默认为15分钟（900秒）
    /// </summary>
    public static int LoginFailedLockedTime()
    {
        return GetAppSetting("LoginFailedLockedTime", 900);
    }

    /// <summary>
    /// 是否检测用户帐户有效期
    /// </summary>
    public static bool CheckUserExpireTime()
    {
        return GetAppSetting("CheckUserExpireTime", 1) == 1;
    }
    /// <summary>
    /// 是否启用用户自定义首页
    /// </summary>
    public static bool EnabledUserCustomHome()
    {
        return GetAppSetting("EnabledUserCustomHome", 0) == 1;
    }
        
    /// <summary>
    /// 用户登录COOKIE有效时间，单位：分钟，默认为1440分钟（1天）
    /// </summary>
    public static int UserLoginCookieExpireTime()
    {
        return GetAppSetting("UserLoginCookieExpireTime", 1440);
    }

    /// <summary>
    /// 检测用户登录状态最大有效期时间，单位：分钟，默认为15分钟，-1表示不检测有效期
    /// </summary>
    public static int UserLoginStatusMaxExpireTime()
    {
        return GetAppSetting("UserLoginStatusMaxExpireTime", 15);
    }
    
    /// <summary>
    /// 是否启用密码闪现效果（仿手机密码输入效果）
    /// </summary>
    public static int EnabledPasswordFlash()
    {
        return GetAppSetting("EnabledPasswordFlash", 0);
    }    
    #endregion

    #region  用户权限相关
    /// <summary>
    /// 系统管理员（非超级管理员）是否可以控制“系统设置”模块中的设置功能
    /// </summary>
    public static bool AdminEnabledControlSystemConfig()
    {
        return GetAppSetting("AdminEnabledControlSystemConfig", 0) == 1;
    }
    #endregion

    #region  安全相关配置

    /// <summary>
    /// 是否禁用右键菜单（防止查看网页源文件），0-不禁用，1-禁用
    /// </summary>
    public static bool DisabledContextMenu()
    {
        return GetAppSetting("DisabledContextMenu", 1) == 1;
    }

    /// <summary>
    /// 设置浏览器右键菜单是否启用
    /// </summary>
    public static string SetContextMenu()
    {
        return DisabledContextMenu() ? "return false;" : string.Empty;
    }
    /// <summary>
    /// 是否验证POST来源，0-不验证，1-验证
    /// </summary>
    public static bool ValidateUrlReferrer()
    {
        return GetAppSetting("ValidateUrlReferrer", 0) == 1;
    }
    /// <summary>
    /// 是否验证表单提交方式，0-不验证，1-验证：表单提交必须是POST方式
    /// </summary>
    public static bool ValidateRequestType()
    {
        return GetAppSetting("ValidateRequestType", 0) == 1;
    }
    /// <summary>
    /// 是否验证表单请求令牌，0-不验证，1-验证：若令牌错误，则不予提交
    /// </summary>
    public static bool ValidateRequestToken()
    {
        return GetAppSetting("ValidateRequestToken", 0) == 1;
    }

    #endregion

    #region  页面相关设置
    public static bool SetTableMinWidth = GetAppSetting("SetTableMinWidth", 1) == 1;
    /// <summary>
    /// 是否设置表格最小宽度（为了表格不变形）
    /// </summary>
    /// <returns></returns>
    public static bool IsSetTableMinWidth()
    {
        return GetAppSetting("SetTableMinWidth", 1) == 1;
    }

    public static int ListPageSize = GetAppSetting("ListPageSize", 10);
    /// <summary>
    /// 数据列表默认显示行数
    /// </summary>
    /// <returns></returns>
    public static int GetListPageSize()
    {
        return GetAppSetting("ListPageSize", 10);
    }
    #endregion

    #region  页面列表分页设置
    public static string PageMarkType()
    {
        return GetAppSetting("PageMarkType", "Chinese");
    }
    public static int PageMarkCount()
    {
        return GetAppSetting("PageMarkCount", 10);
    }
    public static bool HidePageUrl()
    {
        return GetAppSetting("HidePageUrl", 1) == 1;
    }
    public static bool ShowPageCount()
    {
        return GetAppSetting("ShowPageCount", 1) == 1;
    }
    public static bool ShowDataCount()
    {
        return GetAppSetting("ShowDataCount", 1) == 1;
    }
    public static bool ShowPageJump()
    {
        return GetAppSetting("ShowPageJump", 1) == 1;
    }

    #endregion

    #region  GIS地图配置
    public static string CenterLatLng()
    {
        return GetAppSetting("CenterLatLng", "{lat:32.13924, lng:114.06560, zoom:4, name:'信阳供电段', type:'GDD'}");
    }
    #endregion

    #region  获得网站配置信息

    #region  获得配置值
    private static string GetWebConfigValue(object obj, string defaultValue)
    {
        return obj != null ? obj.ToString() : defaultValue;
    }

    private static int GetWebConfigValue(object obj, int defaultValue)
    {
        return obj != null && IsNumber(obj) ? Convert.ToInt32(obj.ToString()) : defaultValue;
    }

    private static int GetWebConfigValue(object obj, int defaultValue, int minValue)
    {
        if (obj != null && IsNumber(obj))
        {
            int result = Convert.ToInt32(obj.ToString());
            return result < minValue ? minValue : result;
        }
        return defaultValue;
    }

    private static int GetWebConfigValue(object obj, int defaultValue, int minValue, int maxValue)
    {
        if (obj != null && IsNumber(obj))
        {
            int result = Convert.ToInt32(obj.ToString());
            return result < minValue ? minValue : result > maxValue ? maxValue : result;
        }
        return defaultValue;
    }

    private static float GetWebConfigValue(object obj, float defaultValue)
    {
        return obj != null && IsNumber(obj) ? Convert.ToSingle(obj.ToString()) : defaultValue;
    }

    private static float GetWebConfigValue(object obj, float defaultValue, float minValue)
    {
        if (obj != null && IsNumber(obj))
        {
            float result = Convert.ToSingle(obj.ToString());
            return result < minValue ? minValue : result;
        }
        return defaultValue;
    }

    private static float GetWebConfigValue(object obj, float defaultValue, float minValue, float maxValue)
    {
        if (obj != null && IsNumber(obj))
        {
            float result = Convert.ToSingle(obj.ToString());
            return result < minValue ? minValue : result > maxValue ? maxValue : result;
        }
        return defaultValue;
    }
    #endregion

    #region AppSettings
    public static string GetAppSetting(string key)
    {
        return GetAppSetting(key, string.Empty);
    }

    public static string GetAppSetting(string key, string defaultValue)
    {
        return GetWebConfigValue(ConfigurationManager.AppSettings[key], defaultValue);
    }

    public static int GetAppSetting(string key, int defaultValue)
    {
        return GetWebConfigValue(ConfigurationManager.AppSettings[key], defaultValue);
    }

    public static int GetAppSetting(string key, int defaultValue, int minValue)
    {
        return GetWebConfigValue(ConfigurationManager.AppSettings[key], defaultValue, minValue);
    }

    public static int GetAppSetting(string key, int defaultValue, int minValue, int maxValue)
    {
        return GetWebConfigValue(ConfigurationManager.AppSettings[key], defaultValue, minValue, maxValue);
    }

    public static float GetAppSetting(string key, float defaultValue)
    {
        return GetWebConfigValue(ConfigurationManager.AppSettings[key], defaultValue);
    }

    public static float GetAppSetting(string key, float defaultValue, float minValue)
    {
        return GetWebConfigValue(ConfigurationManager.AppSettings[key], defaultValue, minValue);
    }

    public static float GetAppSetting(string key, float defaultValue, float minValue, float maxValue)
    {
        return GetWebConfigValue(ConfigurationManager.AppSettings[key], defaultValue, minValue, maxValue);
    }
    #endregion

    #region  ConnectionStrings
    public static string GetConnectionString(string name)
    {
        return GetConnectionString(name, string.Empty);
    }

    public static string GetConnectionString(string name, string defaultValue)
    {
        return GetWebConfigValue(ConfigurationManager.ConnectionStrings[name], defaultValue);
    }

    public static int GetConnectionString(string name, int defaultValue)
    {
        return GetWebConfigValue(ConfigurationManager.ConnectionStrings[name], defaultValue);
    }

    public static int GetConnectionString(string name, int defaultValue, int minValue)
    {
        return GetWebConfigValue(ConfigurationManager.ConnectionStrings[name], defaultValue, minValue);
    }

    public static int GetConnectionString(string name, int defaultValue, int minValue, int maxValue)
    {
        return GetWebConfigValue(ConfigurationManager.ConnectionStrings[name], defaultValue, minValue, maxValue);
    }

    public static float GetConnectionString(string name, float defaultValue)
    {
        return GetWebConfigValue(ConfigurationManager.ConnectionStrings[name], defaultValue);
    }

    public static float GetConnectionString(string name, float defaultValue, float minValue)
    {
        return GetWebConfigValue(ConfigurationManager.ConnectionStrings[name], defaultValue, minValue);
    }

    public static float GetConnectionString(string name, float defaultValue, float minValue, float maxValue)
    {
        return GetWebConfigValue(ConfigurationManager.ConnectionStrings[name], defaultValue, minValue, maxValue);
    }
    #endregion

    #endregion

    #region  验证是否是数字
    public static bool IsNumber(string number)
    {
        string pattern = @"^\-?(0+)?(\d+)(.\d+)?$";

        return new Regex(pattern).IsMatch(number);
    }
    public static bool IsNumber(object number)
    {
        string pattern = @"^\-?(0+)?(\d+)(.\d+)?$";

        return new Regex(pattern).IsMatch(number.ToString());
    }
    #endregion

    /*
    #region  CheckIsNumber
    public static bool IsNumber(string number)
    {
        return new Regex(@"^[-+]?[0-9]+(\.[0-9]+)?$").IsMatch(number.Trim());
    }

    public static bool IsIntNumber(string number)
    {
        return new Regex(@"^[-+]?[0-9]+$").IsMatch(number.Trim());
    }
    #endregion
    */


    #region  获得网站根目录
    /// <summary>
    /// 获得网站根目录
    /// </summary>
    /// <returns></returns>
    public static string GetRootUrl()
    {
        try
        {
            return GetRootUrl(HttpContext.Current.Request);
        }
        catch (Exception ex) { return string.Empty; }
    }

    /// <summary>
    /// 获得网站根目录
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <returns></returns>
    public static string GetRootUrl(HttpRequest httpRequest)
    {
        string strAppPath = "";
        if (httpRequest != null)
        {
            string strUrlAuthority = httpRequest.Url.GetLeftPart(UriPartial.Authority);
            if (httpRequest.ApplicationPath == null || httpRequest.ApplicationPath.Equals("/"))
            {
                //直接安装在独立WEB站点
                strAppPath = strUrlAuthority;
            }
            else
            {
                //安装在虚拟子目录下
                strAppPath = strUrlAuthority + httpRequest.ApplicationPath;
            }
        }
        return strAppPath;
    }

    /// <summary>
    /// 获得网站根目录(相对根目录)
    /// </summary>
    /// <returns></returns>
    public static string GetRootDir()
    {
        try
        {
            string strAppPath = "";
            HttpContext httpCurrent = HttpContext.Current;
            HttpRequest httpRequest;
            if (httpCurrent != null)
            {
                httpRequest = httpCurrent.Request;
                if (httpRequest.ApplicationPath != null && !httpRequest.ApplicationPath.Equals("/"))
                {
                    //安装在虚拟子目录下
                    strAppPath = httpRequest.ApplicationPath;
                }
            }
            return strAppPath;
        }
        catch (Exception ex) { return string.Empty; }
    }
    #endregion
        
    #region  清除文件缓存
    public static string NoCache()
    {
        return DateTime.Now.ToString("mmss");
    }
    #endregion

    #region 人员默认照片
    /// <summary>
    /// 人员默认照片
    /// </summary>
    public static string PersonDefaultPhoto = "/skin/default/imgs/photo/nophoto.png";
    #endregion

}