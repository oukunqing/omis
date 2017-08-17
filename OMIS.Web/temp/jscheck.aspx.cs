using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;

public partial class tools_jscheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        StringBuilder prompt = new StringBuilder();

        string[] arr = this.txtCode.Text.Trim().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        int i = 0;
        foreach (string str in arr)
        {
            i++;
            Regex reg = new Regex(@"^//\w+\n$");
            string s = reg.Replace(str.Trim(), "");
            if (s.Length > 0)
            {
                if (!(s.EndsWith("}") || s.EndsWith(";") || s.EndsWith("{") || s.EndsWith("[") || s.EndsWith("+") || s.EndsWith("||")
                    || s.EndsWith("/*") || s.EndsWith("*/") || s.StartsWith("//") || s.EndsWith("['") || s.EndsWith(",")))
                {
                    prompt.Append(String.Format("第{0}行: {1}<br />", i, s));
                }
            }

        }
        this.lblPrompt.Text = prompt.ToString();
    }
}