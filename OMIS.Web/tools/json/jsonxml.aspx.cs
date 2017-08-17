using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OMIS.Common;

public partial class test_jsonxml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string json = this.txtJson1.Text.Trim();

        this.txtXml1.Text = JsonXml.JsonToXml(json, "", true);

        this.txtXml11.Text = JsonXml.XmlFormat(this.txtXml1.Text);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string xml = this.txtXml2.Text.Trim();

        this.txtJson2.Text = JsonXml.XmlToJson(xml);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        this.txtXml11.Text = JsonXml.XmlFormat(this.txtXml1.Text);
    }
}