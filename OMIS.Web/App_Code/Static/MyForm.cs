using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
///Form 的摘要说明
/// </summary>
public class MyForm
{
    public MyForm()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    #region  BuildEnabledSelect
    public static string BuildEnabledSelect()
    {
        return BuildEnabledSelect(-1);
    }

    public static string BuildEnabledSelect(bool isShowAll)
    {
        return BuildEnabledSelect(-1, isShowAll);
    }

    public static string BuildEnabledSelect(int defaultValue)
    {
        StringBuilder str = new StringBuilder();
        str.Append("<option value=\"1\"" + (defaultValue == 1 ? " selected=\"selected\"" : "") + ">启用</option>");
        str.Append("<option value=\"0\"" + (defaultValue == 0 ? " selected=\"selected\"" : "") + ">不启用</option>");
        return str.ToString();
    }

    public static string BuildEnabledSelect(int defaultValue, bool isShowAll)
    {
        StringBuilder str = new StringBuilder();
        str.Append(isShowAll ? "<option value=\"-1\"" + (defaultValue == -1 ? " selected=\"selected\"" : "") + ">请选择</option>" : "");        
        str.Append("<option value=\"1\"" + (defaultValue == 1 ? " selected=\"selected\"" : "") + ">启用</option>");
        str.Append("<option value=\"0\"" + (defaultValue == 0 ? " selected=\"selected\"" : "") + ">不启用</option>");
        return str.ToString();
    }
    #endregion
    
    #region  BuildSelectOption
    public static string BuildSelectOption(int min, int max, int step)
    {
        return BuildSelectOption(min, max, step, -1);
    }

    public static string BuildSelectOption(int min, int max, int step, int val)
    {
        StringBuilder str = new StringBuilder();
        for (int i = min; i <= max; i += step)
        {
            string selected = i == val ? " selected=\"selected\"" : "";
            str.Append(String.Format("<option value=\"{0}\"{1}>{2}</option>", i, selected, i));
        }
        return str.ToString();
    }
    #endregion
    
    #region  BuildSelectOption
    public static string BuildSelectOption(string s)
    {
        return BuildSelectOption(s, "");
    }

    /// <summary>
    /// BuildSelectOption
    /// </summary>
    /// <param name="s">val_text,val_text(val|text,val|text)</param>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string BuildSelectOption(string s, string val)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        string[] arr = s.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string a in arr)
        {
            string[] tmp = a.Split(new string[] { "|", "_" }, StringSplitOptions.RemoveEmptyEntries);
            dic.Add(tmp[0], tmp.Length >= 2 ? tmp[1] : tmp[0]);
        }
        return BuildSelectOption(dic, val);
    }
    #endregion

    #region  BuildSelectOption
    public static string BuildSelectOption(string[] arr)
    {
        return BuildSelectOption(arr, 0);
    }

    public static string BuildSelectOption(string[] arr, int start)
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0, c = arr.Length; i < c; i++)
        {
            str.Append(String.Format("<option value=\"{0}\"{1}>{2}</option>", i + start, "", arr[i]));
        }
        return str.ToString();
    }

    public static string BuildSelectOption(string[] arr, int start, string val)
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0, c = arr.Length; i < c; i++)
        {
            string selected = (i + start).ToString().Equals(val) ? " selected=\"selected\"" : "";
            str.Append(String.Format("<option value=\"{0}\"{1}>{2}</option>", i + start, selected, arr[i]));
        }
        return str.ToString();
    }

    public static string BuildSelectOption(Dictionary<string, object> dic)
    {
        StringBuilder str = new StringBuilder();
        foreach (KeyValuePair<string, object> kv in dic)
        {
            str.Append(String.Format("<option value=\"{0}\"{1}>{2}</option>", kv.Key, "", kv.Value));
        }
        return str.ToString();
    }

    public static string BuildSelectOption(Dictionary<string, object> dic, string val)
    {
        StringBuilder str = new StringBuilder();
        foreach (KeyValuePair<string, object> kv in dic)
        {
            string selected = kv.Key.Equals(val) ? " selected=\"selected\"" : "";
            str.Append(String.Format("<option value=\"{0}\"{1}>{2}</option>", kv.Key, selected, kv.Value));
        }
        return str.ToString();
    }
    #endregion


    #region  BuildHtmlControl
    public static string BuildHtmlControl(string type, string id, string html)
    {
        return String.Format("<{0} id=\"{1}\">{2}</{0}>", type, id, html);
    }
    #endregion


    #region  AppendPrefixPostfix
    public static string AppendPrefix(string prefix, string content)
    {
        if (!content.Equals(string.Empty))
        {
            return prefix + content;
        }
        return content;
    }

    public static string AppendPostfix(string content, string postfix)
    {
        if (!content.Equals(string.Empty))
        {
            return content + postfix;
        }
        return content;
    }

    public static string AppendPrefixPostfix(string prefix, string content, string postfix)
    {
        if (!content.Equals(string.Empty))
        {
            return prefix + content + postfix;
        }
        return content;
    }
    #endregion

}