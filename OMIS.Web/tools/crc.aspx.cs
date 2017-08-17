using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using OMIS.Common;

public partial class tools_crc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void btnConvert_Click(object sender, EventArgs e)
    {
        string con = this.txtContent.Text.Trim();

        string code = CRC.ToCRC16(con);

        this.txtCrc.Text = code;
    }
}