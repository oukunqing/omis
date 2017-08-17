using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OMIS.DBA;

public partial class _default : System.Web.UI.Page
{
    public string DBConnString = "host=127.0.0.1;user id=test123;password=12345;database=testdb;port=3306;charset=utf8;allow zero datetime=no;";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";

        string sql = "select * from user_info";

        DataSet ds = DataAccess.Select(DBConnString, sql);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        //if (Common.CheckTable(ds, 0))
        //{
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        list.Add(Common.FillDataValue(dr, true));
        //    }
        //}

        //Response.Write(Common.ToJsonList(list));

        if (Common.CheckTable(ds, 0))
        {
            list = Common.FillDataValue(ds.Tables[0], true);
        }

        Response.Write(Common.ToJsonList(list));

        Response.End();
    }
}