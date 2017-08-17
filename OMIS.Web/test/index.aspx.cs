using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;

public partial class test_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Test()
    {

        string originalPath = Server.MapPath("123.jpg");
        string targetPath = Server.MapPath("123_wm.jpg");
        string markPath = Server.MapPath("11.png");
        WaterMark(originalPath, targetPath, "Image", markPath, false);

        originalPath = targetPath;
        targetPath = Server.MapPath("123_wm_1.jpg");
        WaterMark(originalPath, targetPath, "Text", "中华人民共和国万岁", false);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.Test();
    }

    #region  图片水印
    /// <summary>
    /// 图片水印
    /// </summary>
    /// <param name="originalImagePath">原始图片路径名称</param>
    /// <param name="thumbnailPath">生成水印图片后的图片路径名称</param>
    /// <param name="type">水印类型：1表示文字水印，2表示图片水印</param>
    /// <param name="wmImg">若水印类型为文字水印，则传水印文字内容；若水印类型为图片水印，则传水印图片路径名称</param>
    public static void WaterMark(string originalImagePath, string thumbnailPath, string type, string wmImg, bool deleteSource)
    {
        System.Drawing.Image image = System.Drawing.Image.FromFile(originalImagePath);
        Graphics g = Graphics.FromImage(image);
        switch (type)
        {
            case "Text":
                //加文字水印，注意，这里的代码和以下加图片水印的代码不能共存
                //System.Drawing.Image image = System.Drawing.Image.FromFile(originalImagePath);
                //Graphics g = Graphics.FromImage(image);
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                Font f = new Font("Verdana", 14);
                Brush b = new SolidBrush(Color.White);
                string addText = wmImg;
                g.DrawString(addText, f, b, 10, 10);
                g.Dispose();
                break;
            case "Image":
                //加图片水印
                //System.Drawing.Image image = System.Drawing.Image.FromFile(originalImagePath);
                System.Drawing.Image copyImage = System.Drawing.Image.FromFile(wmImg);
                //Graphics g = Graphics.FromImage(image);
                g.DrawImage(copyImage, new Rectangle(image.Width/2 - copyImage.Width, image.Height/2 - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                g.Dispose();
                break;
        }
        image.Save(thumbnailPath);
        image.Dispose();

        if (deleteSource && File.Exists(originalImagePath))
        {
            File.Delete(originalImagePath);
        }
    }
    #endregion

}