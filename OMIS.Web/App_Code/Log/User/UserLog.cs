using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
///UserLog 的摘要说明
/// </summary>
public class UserLog
{
	public UserLog()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
    }
    
    #region  记录登录日志
    public static bool SaveLoginLog(HttpRequest hr, string strUserName, string strClientIp, string strResult)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.Append(String.Format("User:{0},Ip:{1},Result:{2}", strUserName, strClientIp, strResult));
        ServerLog.WriteEventLog(hr, "UserLogin", strLog.ToString());
        return true;
    }
    #endregion

    #region  记录登出日志
    public static bool SaveLogoutLog(HttpRequest hr, string strUserName, string strResult)
    {
        StringBuilder strLog = new StringBuilder();
        strLog.Append(String.Format("User:{0},Result:{1}", strUserName, strResult));
        ServerLog.WriteEventLog(hr, "UserLogout", strLog.ToString());
        return true;
    }
    #endregion

}