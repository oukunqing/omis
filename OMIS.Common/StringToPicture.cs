using System;
using System.Web;
using System.Web.Security;
using System.Drawing;

namespace OMIS.Common
{
    /// <summary>
    /// StringToPicture 的摘要说明
    /// </summary>
    public class StringToPicture
    {
        public StringToPicture()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 字体大小
        private int fontSize = 12;
        public int FontSize
        {
            get { return this.fontSize; }
            set { this.fontSize = value; }
        }
        #endregion

        #region 边框补(默认1像素)
        private int padding = 0;
        public int Padding
        {
            get { return this.padding; }
            set { this.padding = value; }
        }
        #endregion

        #region 是否输出燥点(默认不输出)
        private bool chaos = false;
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        private Color chaosColor = Color.White;
        public Color ChaosColor
        {
            get { return this.chaosColor; }
            set { this.chaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        private Color backgroundColor = Color.Transparent;
        public Color BackgroundColor
        {
            get { return this.backgroundColor; }
            set { this.backgroundColor = value; }
        }
        #endregion

        #region 自定义字体数组
        private string[] fonts = {"宋体", "Arial", "Georgia" };
        public string[] Fonts
        {
            get { return this.fonts; }
            set { this.fonts = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        private Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        public Color[] Colors
        {
            get { return this.colors; }
            set { this.colors = value; }
        }
        #endregion

        #region 生成校验码图片
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize;
            int txtLength = 0;
            int len = 0;
            int rows = 1;
            //检测内容是否有换行
            if (code.IndexOf("\r\n") >= 0)
            {
                string[] arrCode = code.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                rows = arrCode.Length;
                if (rows > 1)
                {
                    foreach (string str in arrCode)
                    {
                        len = StringOperate.GetStringLength(str);
                        if (len > txtLength)
                        {
                            txtLength = len;
                        }
                    }
                }
            }
            else
            {
                txtLength = StringOperate.GetStringLength(code);
            }
            int imageWidth = (int)(txtLength * fWidth / 2) + 5;
            int imageHeight = (int)((double)fSize * 1.2) * rows;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            g.Clear(BackgroundColor);

            Random rand = new Random();

            //给背景添加随机生成的燥点
            if (this.Chaos)
            {
                Pen pen = new Pen(ChaosColor, 0);
                int c = txtLength * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            Font f = new System.Drawing.Font(Fonts[0], fSize, GraphicsUnit.Pixel);
            Brush b = new System.Drawing.SolidBrush(Colors[0]);

            g.DrawString(code, f, b, 0, 0);
            g.Dispose();

            return image;
        }
        #endregion

        #region 将创建好的图片输出到页面
        public void CreateImageOnPage(string content, HttpContext context)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Bitmap image = this.CreateImageCode(content);

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            context.Response.ClearContent();
            context.Response.ContentType = "image/Jpeg";
            context.Response.BinaryWrite(ms.GetBuffer());

            ms.Close();
            ms = null;
            image.Dispose();
            image = null;
        }
        #endregion

    }
}