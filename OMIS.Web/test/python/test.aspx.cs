using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class test_python_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(OMIS.Common.RemoteRequest.GetWebContent("http://localhost:8080/RossWan/Tom", Encoding.UTF8));
        //Response.Write("<br />");
        try
        {
            Response.Write(OMIS.Common.RemoteRequest.GetWebContent("http://localhost:8088/name=admin&pwd=12345", Encoding.UTF8));
        }
        catch (Exception ex) { Response.Write(ex.Message); }
        Response.Write("<br />");
        try
        {
            Response.Write(OMIS.Common.RemoteRequest.GetWebContent("http://60.205.200.126:8080/name=admin&pwd=12345", Encoding.UTF8));
        }
        catch (Exception ex1) { Response.Write(ex1.Message); }
    }
}