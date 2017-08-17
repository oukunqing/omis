using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class modules_print_print1 : System.Web.UI.Page
{
    protected string strData = string.Empty;
    protected string strCss = string.Empty;
    protected string strTitle = string.Empty;
    protected bool isAuto = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.strData = Public.Request("data|Data_Print");
            this.strCss = Public.Request("css|Css_Print");
            this.strTitle = Server.UrlDecode(Public.Request("title|Title_Print"));
            this.isAuto = Public.Request("auto|Auto_Print", 0) == 1;

            this.divData.InnerHtml = Server.UrlDecode(strData);

            this.cssPrint.InnerHtml = Server.UrlDecode(strCss);
        }
    }
}