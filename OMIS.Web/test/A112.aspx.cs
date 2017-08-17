using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Web;
using System.Web.Script.Serialization;

public partial class test_A112 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.TextBox1.Text = OMIS.Common.JsonXml.XmlFormat(this.TextBox1.Text);
        this.TextBox2.Text = OMIS.Common.JsonXml.XmlToJson(this.TextBox1.Text);

        string json = this.TextBox2.Text;

        this.TextBox3.Text = OMIS.Common.JsonXml.JsonToXml(json);
    }
}
