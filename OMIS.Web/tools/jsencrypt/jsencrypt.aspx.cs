using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

public partial class tools_jsencrypt_jsencrypt : System.Web.UI.Page
{

    protected bool isEncrypt = false;
    protected string filePath = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitialData();
        }
    }

    public void InitialData()
    {
        this.filePath = Public.Request("filePath|path");
        this.txtOldFilePath.Text = this.filePath;
        if (this.filePath.Length > 0)
        {
            string code = this.ReadJsCode(this.filePath);
            
            this.input.Value = code;

            this.isEncrypt = code.StartsWith("eval(function");

            this.BuildFilePath(filePath);
        }
    }

    protected void BuildFilePath(string filePath)
    {
        if (filePath.Trim().Length == 0)
        {
            this.lblPrompt.Text = "请输入JS旧文件路径";
            this.txtOldFilePath.Focus();
            return;
        }
        string fileDir = Path.GetDirectoryName(filePath);
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string ext = Path.GetExtension(filePath).ToLower();

        this.txtOldFilePath.Text = filePath;

        if (fileName.ToLower().EndsWith("_source"))
        {
            this.txtSourceFilePath.Text = filePath;
            int pos = filePath.IndexOf("_source");
            this.txtNewFilePath.Text = filePath.Substring(0, pos) + ext;
            this.txtBakFilePath.Text = "";
        }
        else
        {
            this.txtNewFilePath.Text = filePath;
            this.txtBakFilePath.Text = String.Format("{0}/{1}{2}{3}", fileDir.Replace("\\", "/"), fileName, "_bak", ext);
            this.txtSourceFilePath.Text = String.Format("{0}/{1}{2}{3}", fileDir.Replace("\\", "/"), fileName, "_source", ext);
        }
    }

    #region  读取JS文件
    protected void btnRead_Click(object sender, EventArgs e)
    {
        string filePath = this.txtOldFilePath.Text.Trim();
        this.input.Value = this.ReadJsCode(filePath);
        this.output.Value = "";

        this.BuildFilePath(filePath);
    }

    public string ReadJsCode(string filePath)
    {
        string realPath = Server.MapPath(Config.WebDir + filePath);
        bool fileExist = false;

        return this.ReadFile(realPath, out fileExist);
    }
    #endregion

    #region  生成JS源文件
    protected void btnSource_Click(object sender, EventArgs e)
    {
        try
        {
            string code = this.input.Value.Trim();

            this.filePath = this.txtSourceFilePath.Text.Trim();

            if (!code.StartsWith("eval(function"))
            {
                if (this.filePath.Length == 0)
                {
                    this.lblPrompt.Text = "请填写要保存的源文件路径";
                    return;
                }
                string realPath = Server.MapPath(Config.WebDir + filePath);

                this.lblPrompt.Text = String.Format(this.WriteFile(realPath, code) ? "JS源文件[{0}]已生成。" : "JS源文件{0}]生成失败。", filePath);
            }
            else
            {
                this.lblPrompt.Text = "请先对JS源代码进行解码";
            }
        }
        catch (Exception ex) { this.lblPrompt.Text = ex.Message; }
    }
    #endregion

    #region  生成JS文件
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string code = this.output.Value.Trim();

            this.filePath = this.txtNewFilePath.Text.Trim();

            this.lblPrompt.Text = "";

            if (code.Length > 0)
            {
                //if (code.StartsWith("eval(function"))
                //{
                if (this.filePath.Length == 0)
                {
                    this.lblPrompt.Text = "请填写要保存的文件路径";
                    return;
                }

                string sourcePath = this.txtSourceFilePath.Text.Trim();
                if (!File.Exists(Server.MapPath(Config.WebDir + sourcePath)))
                {
                    this.lblPrompt.Text += "源文件尚未生成，不能生成文件!";
                    return;
                }
                else
                {
                    if (this.chbBak.Checked)
                    {
                        string bakFilePath = this.txtBakFilePath.Text.Trim();
                        if (bakFilePath.Length > 0)
                        {
                            string sourceCode = this.input.Value.Trim();
                            string realBakFilePath = Server.MapPath(Config.WebDir + bakFilePath);
                            this.lblPrompt.Text = String.Format(this.WriteFile(realBakFilePath, sourceCode) ? "JS备份文件[{0}]已生成。<br/>" : "JS备份文件[{0}]生成失败。<br/>", bakFilePath);
                        }
                    }
                    string realPath = Server.MapPath(Config.WebDir + filePath);

                    this.lblPrompt.Text += String.Format(this.WriteFile(realPath, code) ? "JS文件[{0}]已生成。" : "JS文件{0}]生成失败。", filePath);
                }
            }
            else
            {
                this.lblPrompt.Text = "请先对JS代码进行编码";
            }
        }
        catch (Exception ex) { this.lblPrompt.Text = ex.Message; }
    }
    #endregion


    #region  读取文件
    public string ReadFile(string filePath, out bool fileExist)
    {
        try
        {
            fileExist = File.Exists(filePath);

            if (fileExist)
            {
                StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
                string content = sr.ReadToEnd();
                sr.Close();

                return content;
            }
            return "错误：文件不存在!";
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  写入文件
    public bool WriteFile(string filePath, string content)
    {
        try
        {
            StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
            sw.Write(content);
            sw.Close();

            return File.Exists(filePath);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

}