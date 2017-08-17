using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZyrhGprs;

public partial class test_gprs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ZyrhWebService ws = new ZyrhWebService();
        DataInfo o = new DataInfo();
        o.DeviceCode = "31005";
        o.DataCode = "019";
        o.DataContent = "1";
        o.ChannelNo = 1;
        o.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        bool result = ws.UploadDeviceData(o);

        Response.Write(result.ToString());
    }
}