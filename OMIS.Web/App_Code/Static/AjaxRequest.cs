using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMIS.BLL;
using OMIS.BLL.System;

/// <summary>
///AjaxRequest 的摘要说明
/// </summary>
public class AjaxRequest
{
	public AjaxRequest()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    #region  检测是否已登录
    private static bool CheckLogin()
    {
        return new UserCenter(Config.DBConnectionString).IsLogin;
    }

    public static void CheckLogin(string action, HttpContext hc)
    {
        if (!action.Equals(string.Empty))
        {
            if (!CheckLogin())
            {
                hc.Response.Write(Public.ToMessageResult(0, "NoAuth"));
                hc.Response.End();
            }
        }
    }
    #endregion

    #region  检查AJAX请求
    public static void CheckRequest(string action, HttpContext hc)
    {
        CheckRequest(action, hc, true, true, true, true);
    }

    public static void CheckRequest(string action, HttpContext hc, bool isCheckLogin)
    {
        CheckRequest(action, hc, isCheckLogin, true, true, true);
    }

    public static void CheckRequest(string action, HttpContext hc, bool isCheckLogin, bool isCheckRequestType)
    {
        CheckRequest(action, hc, isCheckLogin, isCheckRequestType, true, true);
    }

    public static void CheckRequest(string action, HttpContext hc, 
        bool isCheckLogin, bool isCheckRequestType, bool isCheckUrlReferrer)
    {
        CheckRequest(action, hc, isCheckLogin, isCheckRequestType, isCheckUrlReferrer, true);
    }

    public static void CheckRequest(string action, HttpContext hc, 
        bool isCheckLogin, bool isCheckRequestType, bool isCheckUrlReferrer, bool isCheckToken)
    {
        if (!action.Equals(string.Empty))
        {
            if (isCheckLogin)
            {
                if (!CheckLogin())
                {
                    hc.Response.Write(Public.ToMessageResult(0, "NoAuth"));
                    hc.Response.End();
                }
            }

            if (isCheckRequestType)
            {
                if (Config.ValidateRequestType() && !hc.Request.RequestType.Equals("POST"))
                {
                    hc.Response.Write(Public.ToMessageResult(0, "RequestType Invalid"));
                    hc.Response.End();
                }
            }

            if (isCheckUrlReferrer)
            {
                if (Config.ValidateUrlReferrer())
                {
                    if (hc.Request.UrlReferrer == null || !hc.Request.UrlReferrer.Host.Equals(hc.Request.Url.Host))
                    {
                        hc.Response.Write(Public.ToMessageResult(0, "UrlReferrer Invalid"));
                        hc.Response.End();
                    }
                }
            }

            if (isCheckToken)
            {
                if (Config.ValidateRequestToken())
                {
                    if (!Token.CheckToken(Public.Request("token")))
                    {
                        hc.Response.Write(Public.ToMessageResult(0, "Token Invalid"));
                        hc.Response.End();
                    }
                }
            }
        }
    }
    #endregion
    
}