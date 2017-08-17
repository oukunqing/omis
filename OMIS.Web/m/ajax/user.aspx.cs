using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class m_ajax_user : System.Web.UI.Page
{

    protected string action = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";

        this.InitialData();
    }

    protected void InitialData()
    {
        try
        {
            this.action = Public.Request("action");
            switch (this.action)
            {
                case"userLogin":
                    break;
                default:
                    Response.Write(Public.ToJsonHello());
                    break;
            }
        }
        catch (Exception ex)
        {
            ServerLog.WriteErrorLog(ex);
            Response.Write(Public.ToExceptionResult(ex));
        }
    }

    #region  用户登录
    public string UserLogin(string data)
    {
        try
        {
            Dictionary<string, object> par = Public.Deserialize(data);
            string user = Public.ConvertValue(par, "User|user");
            string pwd = Public.ConvertValue(par, "Pwd|pwd");

            return this.UserLogin("", "", "");
        }
        catch (Exception ex) { throw (ex); }
    }

    public string UserLogin(string user, string pwd, string crc)
    {
        try
        {
            return "";
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

}