using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace OMIS.Common
{

    public class FormRequest
    {

        #region  模拟提交FORM表单
        public static string PostForm(string url, List<FormParam> param)
        {
            try
            {
                string boundary = BuildBoundary();
                WebRequest req = WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = String.Format("multipart/form-data; boundary={0}", boundary);

                StringBuilder data = new StringBuilder();
                string fileName = string.Empty;
                bool hasFile = false;

                foreach (FormParam fp in param)
                {
                    data.Append(String.Format("--{0}", boundary));
                    data.Append("\r\n");
                    if (fp.IsFile)
                    {
                        data.Append(String.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", fp.Name, fp.FileName));
                        data.Append("\r\n");
                        data.Append(String.Format("Content-Type: {0}", fp.ContentType.ToString().Replace("_", "/")));
                        data.Append("\r\n\r\n");

                        hasFile = true;
                        fileName = fp.FileName;
                    }
                    else
                    {
                        data.Append(String.Format("Content-Disposition: form-data; name=\"{0}\"", fp.Name));
                        data.Append("\r\n\r\n");
                        data.Append(fp.Content);
                        data.Append("\r\n");
                    }
                }

                string head = data.ToString();
                byte[] form_data = Encoding.UTF8.GetBytes(head);
                //结尾 
                byte[] foot_data = Encoding.UTF8.GetBytes(String.Format("\r\n--{0}--\r\n", boundary));
                FileStream fileStream = null;

                if (hasFile)
                {
                    //文件 
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //post总长度 
                    long length = form_data.Length + fileStream.Length + foot_data.Length;
                    req.ContentLength = length;
                }

                Stream reqStream = req.GetRequestStream();
                //发送表单参数 
                reqStream.Write(form_data, 0, form_data.Length);


                if (hasFile && fileStream != null)
                {
                    //文件内容 
                    byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        reqStream.Write(buffer, 0, bytesRead);
                    }
                }
                //结尾 
                reqStream.Write(foot_data, 0, foot_data.Length);
                reqStream.Close();

                //响应 
                WebResponse rsp = req.GetResponse();
                StreamReader sr = new StreamReader(rsp.GetResponseStream(), Encoding.UTF8);
                string response = sr.ReadToEnd().Trim();
                sr.Close();
                if (rsp != null)
                {
                    rsp.Close();
                    rsp = null;
                }
                if (req != null)
                {
                    req = null;
                }
                return response;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  创建分隔线
        public static string BuildBoundary()
        {
            StringBuilder separator = new StringBuilder();
            for (int i = 0; i < 27; i++)
            {
                separator.Append("-");
            }
            return String.Format("{0}{1}", separator.ToString(), DateTime.Now.Ticks.ToString("x"));
        }
        #endregion

    }


    public enum FormParamContentType
    {
        text_html,
        text_plain,
        text_richtext,
        image_gif,
        image_jpeg,
        image_pjpeg,
        image_png,
    }

    #region  FORM表单参数
    public class FormParam
    {

        public FormParamContentType ContentType { get; set; }

        public bool IsFile { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string FileName { get; set; }

        public FormParam()
        {
            this.ContentType = FormParamContentType.text_html;
            this.IsFile = false;
        }
    }
    #endregion

}