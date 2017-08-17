using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Common
{
    public class JsCodeEncrypt
    {

        public static string Encrypt(string code)
        {
            return "";
        }

        public static string Build(string pattern, int count, string content)
        {
            StringBuilder con = new StringBuilder();
            con.Append("eval(function(p,a,c,k,e,d){e=function(c){");
            con.Append("return(c<a?\"\":e(parseInt(c/a)))+((c=c%a)>35?String.fromCharCode(c+29):c.toString(36))};if(!''.replace(/^/,String))");
            con.Append("{while(c--)d[e(c)]=k[c]||e(c);k=[function(e){return d[e]}];e=function(){return'\\w+'};c=1;};");
            con.Append("while(c--)if(k[c])p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c]);return p;}");
            con.Append("(");
            con.Append(String.Format("'{0}',{1},{2},'{3}'.split('|'),0,{{}}", pattern, count, count, content));
            con.Append(")");
            con.Append(")");

            return con.ToString();
        }

    }
}