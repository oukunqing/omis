using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

public partial class m_ajax_gps : System.Web.UI.Page
{

    protected string action = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitialData();
        }
    }

    #region  初始化数据
    protected void InitialData()
    {
        try
        {
            this.action = Public.Request("action");
            switch (this.action)
            {
                case "uploadGps":
                    Response.Write(this.UploadGps(Public.Request("data", true, "{}")));
                    break;
                case "getGps":
                    Response.Write(this.GetGpsList(Public.Request("data", true, "{}")));
                    break;
                default:
                    Response.Write(Public.ToJsonHello());
                    break;
            }
        }
        catch (Exception ex)
        {
            ServerLog.WriteErrorLog(ex);
            Response.Write(Public.ToExceptionResult(ex));
        }
    }
    #endregion

    #region  上传GPS
    public string UploadGps(string data)
    {
        try
        {
            ServerLog.WriteDebugLog(HttpContext.Current.Request, "UploadGps", data);

            Dictionary<string, object> par = Public.Deserialize(data);
            if (!par.ContainsKey("Latitude") || !par.ContainsKey("Longitude"))
            {
                return Public.ToJsonMessage("数据不完整");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("insert into `tmp_gps`(group_id,latitude,longitude,speed,altitude,bearing,accuracy,time,create_time)values(");
            sql.Append(String.Format("{0},{1},{2},{3},{4},{5},{6},'{7}','{8}'",
                Public.ConvertValue(par, "GroupId", 0),
                Public.ConvertValue(par, "Latitude", 0m),
                Public.ConvertValue(par, "Longitude", 0m),
                Public.ConvertValue(par, "Speed", 0m),
                Public.ConvertValue(par, "Altitude", 0m),
                Public.ConvertValue(par, "Bearing", 0m),
                Public.ConvertValue(par, "Accuracy", 0m),
                Public.ConvertValue(par, "Time", Public.GetDateTime()),
                Public.GetDateTime()
            ));
            sql.Append(");");

            int result = OMIS.DAL.DataAccess.Insert(OMIS.DAL.DataAccess.DBConnString, sql.ToString());

            return Public.ToJsonResult(result);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  获得GPS
    public string GetGpsList(string data)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from `tmp_gps` ");
            sql.Append(" order by id desc ");
            sql.Append(" limit 0,20 ");
            sql.Append(";");

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            DataSet ds = OMIS.DAL.DataAccess.Select(OMIS.DAL.DataAccess.DBConnString, sql.ToString());
            if (Public.CheckTable(ds, 0))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(Public.FillDataValue(dr));
                }
            }
            return Public.ToJsonList(list);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

}