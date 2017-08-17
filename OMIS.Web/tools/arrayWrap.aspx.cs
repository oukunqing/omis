using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;

public partial class test_arrayWrap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ddlCols.Items.Add(new ListItem("不换行", "-1"));

            for (int i = 1; i < 33; i++)
            {
                this.ddlCols.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }


            this.ddlCols.SelectedValue = "10";
        }
    }

    public int GetCols()
    {
        try
        {
            return Convert.ToInt32(this.ddlCols.SelectedValue.Trim());
        }
        catch (Exception ex) { return 10; }
    }

    #region  转换内容
    protected void ConvertContent()
    {

        this.txtResult.Text = "";

        string[] d = { "|", "," };

        string[] content = this.txtContent.Text.Trim().Replace("\r\n", "|").Split(d, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder result = new StringBuilder();

        bool isDistinct = this.chbDistinct.Checked;
        bool isString = this.chbString.Checked;
        string space = this.chbSpace.Checked ? " " : "";

        Hashtable htCon = new Hashtable();
        
        int c = content.Length;
        int n = 0;
        int cols = this.GetCols();
        foreach (string s in content)
        {
            if (s.Trim().Length > 0)
            {
                if (isDistinct && htCon.Contains(s))
                {
                    continue;
                }
                if (!htCon.Contains(s))
                {
                    htCon.Add(s, s);
                }

                if (cols > 0 && n > 0 && n % cols == 0)
                {
                    result.Append("\r\n");
                }
                result.Append(String.Format(isString ? "\"{0}\"" : "{0}", s.Trim()));
                result.Append("," + space);
                n++;
            }
        }
        if (result.Length > 0)
        {
            int pos = result.ToString().LastIndexOf(',');

            this.txtResult.Text = result.ToString().Substring(0, pos);
        }
    }
    #endregion


    protected void btnConvert_Click(object sender, EventArgs e)
    {
        this.ConvertContent();
    }
    
    protected void chbSpace_CheckedChanged(object sender, EventArgs e)
    {
        this.ConvertContent();
    }
    protected void chbString_CheckedChanged(object sender, EventArgs e)
    {
        this.ConvertContent();
    }
    protected void ddlCols_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ConvertContent();
    }
    protected void chbDistinct_CheckedChanged(object sender, EventArgs e)
    {
        this.ConvertContent();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        string s = this.txtContent.Text;
        string s1 = this.txtReplace1.Text;
        string s2 = this.txtReplace2.Text;
        if (s.Length > 0 && s1.Length > 0)
        {
            this.txtContent.Text = s.Replace(s1, s2);
        }
        
    }
    protected void btnDelRow_Click(object sender, EventArgs e)
    {
        this.txtContent.Text = this.txtContent.Text.Replace("\r\n", "");
    }
    protected void btnDelRow1_Click(object sender, EventArgs e)
    {
        this.txtContent.Text = this.txtContent.Text.Replace("\r\n", "|");
    }
    protected void btnDelRow2_Click(object sender, EventArgs e)
    {
        this.txtContent.Text = this.txtContent.Text.Replace("\r\n", ",");
    }
    protected void btnClearSpace_Click(object sender, EventArgs e)
    {
        int pc = Convert.ToInt32(this.txtSpaceCount.Text.Trim());
        string space = " ".PadLeft(pc, ' ');

        Regex reg = new Regex(@"\s+");
        this.txtContent.Text = reg.Replace(this.txtContent.Text, space);
    }
}