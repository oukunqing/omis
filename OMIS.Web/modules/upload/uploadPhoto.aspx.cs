using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using OMIS.Common;

public partial class modules_upload_uploadPhoto : System.Web.UI.Page
{

    protected string action = string.Empty;
    protected string name = string.Empty;
    protected string title = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        if (!LoginUser.CheckLogin() || LoginUser.CheckUserIsGuest())
        {
            Response.End();
        }
        */
        if (!IsPostBack)
        {
            this.action = Public.Request("action");
            this.name = Public.Request("name", "图片");
            this.title = Public.Request("title", "上传图片");
            //photo, icon
            this.txtType.Value = Public.Request("type", "photo");
            this.txtDir.Value = Public.Request("dir");
            this.txtWidth.Value = Public.Request("w|width", "400");
            this.txtHeight.Value = Public.Request("h|height", "400");
            this.txtThumbWidth.Value = Public.Request("tw|thumbWidth", "200");
            this.txtThumbHeight.Value = Public.Request("th|thumbHeight", "200");
            this.txtDeleteSource.Value = Public.Request("deleteSource", 0).ToString();
            this.txtThumbMode.Value = Public.Request("mode|thumbMode", "HW").ToUpper();

            this.chbThumb.Checked = Public.Request("thumb", 1) == 1;
        
            if (this.action.Equals("deletePhoto"))
            {
                Response.ContentType = "text/plain";

                Response.Write(this.DeletePhoto(Public.Request("filePath|path")));

                Response.End();
            }

        }
    }

    #region  上传图片
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            string type = this.txtType.Value.Trim();
            string oldName = this.fuPhoto.PostedFile.FileName;
            string ext = Path.GetExtension(oldName).ToLower();
            string dir = this.txtDir.Value.Trim();
            DateTime dtNow = DateTime.Now;

            if (dir.Equals(string.Empty))
            {
                return;
            }

            FileExtension[] fx = new FileExtension[] { FileExtension.PNG, FileExtension.GIF, FileExtension.JPG, FileExtension.BMP };
            string imgExt = ".jpg,.jpeg,.gif,.png,.bmp";
            if (imgExt.IndexOf(ext) < 0)
            {
                string error = "图片格式错误，允许上传的图片格式为：" + imgExt;
                this.lblResponse.InnerHtml = String.Format("<script>showErrorInfo('{0}');</script>", error);
                return;
            }
            else if (!FileOperate.ValidateFileExtension(this.fuPhoto, fx))
            {
                string error = "文件格式错误，允许上传的图片格式为：" + imgExt;
                this.lblResponse.InnerHtml = String.Format("<script>showErrorInfo('{0}');</script>", error);
                return;
            }

            string fileName = dtNow.ToString("yyyyMMddHHmmssfff");

            string fileDir = String.Format("{0}/{1}/", Config.UploadFileDir, dir);
            if (type.Equals("photo"))
            {
                fileDir += String.Format("{0}/{1}/{2}/", "photo", dtNow.ToString("yyyy"), dtNow.ToString("MMdd"));
            }

            if (!Directory.Exists(Server.MapPath(Config.WebDir + fileDir)))
            {
                Directory.CreateDirectory(Server.MapPath(Config.WebDir + fileDir));
            }

            string filePathSource = String.Format("{0}{1}_source{2}", fileDir, fileName, ext);
            string filePath = String.Format("{0}{1}{2}", fileDir, fileName, ext);

            bool isThumb = this.chbThumb.Checked;
            int width = Convert.ToInt32(this.txtWidth.Value);
            int height = Convert.ToInt32(this.txtHeight.Value);

            if (isThumb)
            {
                //先上传源图片
                this.fuPhoto.PostedFile.SaveAs(Server.MapPath(Config.WebDir + filePathSource));
                //获得上传的图片尺寸大小
                int[] size = this.GetPhotoSize(filePathSource);

                //压缩源图片，生成新的图片 按指定大小生成
                string pathSource = Server.MapPath(Config.WebDir + filePathSource);
                string path = Server.MapPath(Config.WebDir + filePath);
                if (size[0] > width || size[1] > height)
                {
                    MakeThumbnailMode thumbMode = MakeThumbnailMode.HW;
                    string model = this.txtThumbMode.Value.Trim();
                    switch (model)
                    {
                        case "HW": thumbMode = MakeThumbnailMode.HW; break;
                        case "HWR": thumbMode = MakeThumbnailMode.HWR; break;
                        case "Cut": thumbMode = MakeThumbnailMode.Cut; break;
                    }
                    FileOperate.MakeThumbnail(pathSource, path, width, height, thumbMode);
                }
                else
                {
                    File.Copy(pathSource, path);
                }                

                if (type.Equals("photo"))
                {
                    string filePathThumb = String.Format("{0}{1}_thumb{2}", fileDir, fileName, ext);
                    //再生成一张缩略图
                    string pathThumb = Server.MapPath(Config.WebDir + filePathThumb);

                    int thumbWidth = Convert.ToInt32(this.txtThumbWidth.Value);
                    int thumbHeight = Convert.ToInt32(this.txtThumbHeight.Value);

                    if (size[0] > thumbWidth || size[1] > thumbHeight)
                    {
                        FileOperate.MakeThumbnail(pathSource, pathThumb, thumbWidth, thumbHeight, MakeThumbnailMode.HWR);
                    }
                    else
                    {
                        File.Copy(pathSource, pathThumb);
                    }  
                }

                bool isDeleteSource = Public.ConvertValue(this.txtDeleteSource.Value, 0) == 1;
                if (isDeleteSource && File.Exists(pathSource))
                {
                    File.Delete(pathSource);
                }
            }
            else
            {
                //上传图片
                this.fuPhoto.PostedFile.SaveAs(Server.MapPath(Config.WebDir + filePath));
            }

            this.lblResponse.InnerHtml = String.Format("<script>uploadCallback('{0}');</script>", filePath);
        }
        catch (Exception ex) { this.lblResponse.InnerHtml = ex.Message; }
    }
    #endregion

    #region  获取图片尺寸
    public int[] GetPhotoSize(string photoPath)
    {
        string realPath = HttpContext.Current.Server.MapPath(Config.WebDir + photoPath);

        System.Drawing.Image img = System.Drawing.Image.FromFile(realPath);

        int[] result = new int[] { img.Width, img.Height };

        img.Dispose();

        return result;
    }
    #endregion

    #region  删除照片
    public string DeletePhoto(string filePath)
    {
        try
        {
            try
            {
                string[] paths = filePath.Split('|');
                foreach (string path in paths)
                {
                    if (path.Length > 0)
                    {
                        string fileFullName = Path.GetFileName(path);
                        string fileName = Path.GetFileNameWithoutExtension(path);
                        string ext = Path.GetExtension(path);

                        if (File.Exists(Server.MapPath(Config.WebDir + path)))
                        {
                            File.Delete(Server.MapPath(Config.WebDir + path));
                        }

                        string filePathSource = path.Replace(fileFullName, fileName + "_source" + ext);
                        string filePathThumb = path.Replace(fileFullName, fileName + "_thumb" + ext);

                        if (File.Exists(Server.MapPath(Config.WebDir + filePathSource)))
                        {
                            File.Delete(Server.MapPath(Config.WebDir + filePathSource));
                        }

                        if (File.Exists(Server.MapPath(Config.WebDir + filePathThumb)))
                        {
                            File.Delete(Server.MapPath(Config.WebDir + filePathThumb));
                        }
                    }
                }                
            }
            catch (Exception exx) { }

            return "{\"result\":1}";
        }
        catch (Exception ex) { return Public.ToExceptionResult(ex); }
    }
    #endregion

}