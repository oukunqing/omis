using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OMIS.Common;

public partial class test_demo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Write(CRC.ToCRC16("123456780", true));
        Response.Write("<br />");

        Response.Write(CRC.ToCRC16("123456780", false));
        Response.Write("<br />");

        Response.Write(CRC.ToModbusCRC16("123456780", true));
        Response.Write("<br />");

        Response.Write(CRC.ToModbusCRC16("123456780", false));
        Response.Write("<br />");

    }
}