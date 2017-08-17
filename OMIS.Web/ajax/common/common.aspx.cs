using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using OMIS.BLL.Common;
using OMIS.Model.Common;
using OMIS.Common;

public partial class ajax_common_common : System.Web.UI.Page
{

    protected string action = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";

        this.InitialData();
    }

    protected void InitialData()
    {
        this.action = Public.Request("action");
        switch (this.action)
        {
            case "getDictionaryType":
                Response.Write(this.GetDictionaryType(
                    Public.Request("typeId", 0)
                ));
                break;
            case "getDictionary":
                Response.Write(this.GetDictionary(
                    Public.Request("dictionaryId", 0),
                    Public.Request("data", true)
                ));
                break;
            case "getPinyin":
                Response.Write(this.GetPinyin(Public.Request("content|text")));
                break;
            case "getPinyinInitial":
                Response.Write(this.GetPinyinInitial(Public.Request("content|text")));
                break;
            default:
                Response.Write(Public.ToJsonHello());
                break;
        }
    }

    #region  GetDictionaryType
    public string GetDictionaryType(int typeId)
    {
        try
        {
            DictionaryTypeInfo info1 = new DictionaryTypeInfo();

            DictionaryTypeManage dm = new DictionaryTypeManage();
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) { 
                {"TypeId",""},{"TypeIdList",""},{"TypeCode","Test's"},{"enabled",1},{"parentId",-1}
            };

            //Hashtable dic = new Hashtable(StringComparer.OrdinalIgnoreCase) {
            //    {"TypeId",""},{"TypeIdList","1,2,3"},{"enabled",1}
            //};

            int dataCount = 0;

            return Public.ToJsonList(dm.GetDictionaryTypeInfo(dm.GetDictionaryType(dic).DataSet, out dataCount), dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion


    #region  GetDictionary
    public string GetDictionary(int dictionaryId, string dicData)
    {
        try
        {
            DictionaryManage dm = new DictionaryManage();
            Dictionary<string, object> dic = Public.CheckIsJsonData(dicData) ? 
                Public.Json.Deserialize<Dictionary<string, object>>(dicData) : 
                new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) { 
                    {"TypeId",3},{"GetSubset",1},{"Keywords",""},{"SearchField", "Name"},{"PageIndex",0},{"PageSize",0}
                };

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            DataSet ds = dm.GetDictionary(dic).DataSet;

            if (Public.CheckTable(ds, 0))
            {
                Dictionary<string, string> key = new Dictionary<string, string>() { 
                    {"DictionaryId","id"},{"TypeId","tid"},{"DictionaryName","name"},{"DictionaryCode","code"}
                };
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(Public.ConvertClassValue(dm.FillDictionaryInfo(dr), key, true));
                }
            }
            int dataCount = Public.ConvertFieldValue(ds, 1, 0);

            //return Public.ToJsonList(dm.GetDictionaryInfo(dm.GetDictionary(dic).DataSet, out dataCount), dataCount);

            return Public.ToJsonList(list, dataCount);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得拼音
    public string GetPinyin(string text)
    {
        return Public.ToJsonResult("pinyin", ChinesePinyin.Show(ChinesePinyin.ConvertPinyin(text)));
    }
    #endregion

    #region  获得拼音首字母
    public string GetPinyinInitial(string text)
    {
        return Public.ToJsonResult("pinyin", ChinesePinyin.ShowInitial(ChinesePinyin.ConvertPinyin(text)));
    }
    #endregion

}