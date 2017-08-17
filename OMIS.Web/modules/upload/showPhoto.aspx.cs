using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class modules_upload_showPhoto : System.Web.UI.Page
{

    protected string photoPath = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.photoPath = Public.Request("path|photoPath|imagePath");
            if (photoPath.Length > 0)
            {
                if (photoPath.IndexOf("http://") != 0)
                {
                    photoPath = Config.WebDir + photoPath;
                }
            }
        }
    }
}