using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class masters_mpPage : System.Web.UI.MasterPage
{

    protected string NoAuth = string.Empty;

    protected string PageTitle = string.Empty;
    protected string WebDir = Config.WebDir;

    protected bool isDebug = false;

    protected string WebConfig = "{}";

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");

        this.isDebug = Public.Request("debug|isDebug", 0) == 1;

        Dictionary<string, object> dic = new Dictionary<string, object>()
        {
            {"webDir",Config.WebDir},{"isDebug",isDebug}
        };

        this.WebConfig = Public.Json.Serialize(dic);

        //this.NoAuth = MyForm.BuildHtmlControl("label", "lblNoAuth", "");
    }
}
