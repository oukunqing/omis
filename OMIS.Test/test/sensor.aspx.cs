using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZyrhSensor;
using ZyrhSensor1;

public partial class test_sensor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string devCode = "dx01";
        string msg = this.txtData.Text.Trim();
        ZyrhSensor1.ZyrhSensorService ws = new ZyrhSensor1.ZyrhSensorService();
        ws.UploadSensorDataInfo(devCode, new string[] { msg });
        /*
         * ZyrhSensor.ZyrhSensorService ws1 = new ZyrhSensor.ZyrhSensorService();
        ws1.UploadSensorDataInfo(devCode, new string[] { msg });
         */

    }

}