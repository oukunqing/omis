using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;

public partial class tools_jse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Build("2 3(0,1){4 0+1}", 5, "a|b|function|add|return"));
                
    }


    public string Encrypt(string code)
    {
        StringBuilder con = new StringBuilder();
        string copy = new Regex("[\r\n|\n]").Replace(code, "");

        string[] arr = copy.Split(' ');

        con.Append(copy);
        return con.ToString();
    }

    #region  Build
    public string Build(string pattern, int count, string content)
    {
        StringBuilder con = new StringBuilder();
        con.Append("eval(function(p,a,c,k,e,d){e=function(c){");
        //con.Append("return(c<a?\"\":e(parseInt(c/a)))+((c=c%a)>35?String.fromCharCode(c+29):c.toString(36))};");
        con.Append("return(c&lt;a?\"\":e(parseInt(c/a)))+((c=c%a)&gt;35?String.fromCharCode(c+29):c.toString(36))};");
        con.Append("if(!''.replace(/^/,String))");
        con.Append("{while(c--)d[e(c)]=k[c]||e(c);k=[function(e){return d[e]}];e=function(){return'\\w+'};c=1;};");
        con.Append("while(c--)if(k[c])p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c]);return p;}");
        con.Append("(");
        con.Append(String.Format("'{0}',{1},{2},'{3}'.split('|'),0,{{}}", pattern, count, count, content));
        con.Append(")");
        con.Append(")");

        return con.ToString();
    }
    #endregion

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.txtCon.Text = Encrypt(this.txtCode.Text.Trim());
    }
}