using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class modules_upload_sourcePhoto : System.Web.UI.Page
{

    protected string photoPath = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.photoPath = Public.Request("path");

            this.ResponsePhoto(this.photoPath);
        }
    }

    protected void ResponsePhoto(string photoPath)
    {
        try
        {
            string local = this.GetUrlHead(this.Request.Url.ToString());
            string localPhoto = this.GetUrlHead(photoPath);
            if (localPhoto.Length > 0)
            {
                if (local.Equals(localPhoto))
                {
                    photoPath = photoPath.Replace("http://", "").Split('?')[0];
                    //本地图片
                    int pos = photoPath.IndexOf('/');
                    string strPath = photoPath.Substring(pos);

                    System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(strPath), false);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    this.Response.ClearContent();
                    this.Response.ContentType = "image/Jpeg";

                    //输出图像
                    this.Response.BinaryWrite(ms.GetBuffer());

                    ms.Close();
                    ms = null;
                    img.Dispose();
                    img = null;
                }
                else
                {
                    //远程图片，跳转到图片地址
                    Response.Redirect(photoPath);
                }
            }
        }
        catch (Exception ex) { Response.Write(ex.Message); }
    }

    public string GetUrlHead(string url)
    {
        if (url.Length > 0)
        {
            url = url.Replace("http://", "");
            int pos = url.IndexOf('/');
            return url.Substring(0, pos);
        }
        return "";
    }
}