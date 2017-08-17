using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class modules_emap_track_default : System.Web.UI.Page
{

    protected string action = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.action = Public.Request("action");
        if (this.action.Equals(string.Empty))
        {
            //Response.End();
        }
    }
}