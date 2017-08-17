using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.IO;

public partial class tools_jsencrypt_jse : System.Web.UI.Page
{

    protected JavaScriptSerializer js = new JavaScriptSerializer();

    protected List<JsFileInfo> fileList = new List<JsFileInfo>();

    protected bool[] arrShow = new bool[3];
    protected int fileId = 1;
    protected string rootPath = "";
    protected string dirPath = "";

    protected string JsonData = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnRead_Click(object sender, EventArgs e)
    {
        string dir = this.txtRootDir.Text.Trim();
        this.rootPath = Server.MapPath(Config.WebDir + dir);
        int pos = dir.LastIndexOf('/');
        int pos1 = this.rootPath.LastIndexOf('\\');
        this.dirPath = Config.WebDir.Equals(string.Empty) ? this.rootPath.Substring(0, pos1) : Server.MapPath(Config.WebDir + dir.Substring(0, pos));
        this.fileList.Add(new JsFileInfo(this.fileId++, "", 0, dir));

        this.arrShow = new bool[] { this.chbJs.Checked, this.chbSource.Checked, this.chbBak.Checked };

        this.JsonData = this.ReadFileList(this.rootPath);
    }

    #region  读取文件
    public string ReadFileList(string fileDir)
    {
        this.ReadDirectoryAndFile(fileDir, 0);
        return js.Serialize(this.fileList);
    }
    #endregion

    public void ReadDirectoryAndFile(string aimPath, int pid)
    {
        try
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
            {
                aimPath += Path.DirectorySeparatorChar;
            }

            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
            string[] fileList = Directory.GetFileSystemEntries(aimPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
                if (Directory.Exists(file))
                {
                    this.fileList.Add(new JsFileInfo(this.fileId++, this.FilterDirPath(aimPath, this.dirPath), 0, this.FilterFilePath(file, this.dirPath)));

                    //递归删除目录下的文件和目录，此处文件目录不需要加 Server.MapPath
                    ReadDirectoryAndFile(aimPath + Path.GetFileName(file), this.fileId);
                }
                // 如果是文件则直接删除文件
                else
                {
                    bool show = (this.arrShow[0] && file.EndsWith(".js") && !file.EndsWith("_source.js") && !file.EndsWith("_bak.js")) 
                        || (this.arrShow[1] && file.EndsWith("_source.js")) 
                        || (this.arrShow[2] && file.EndsWith("_bak.js"));

                    if (show)
                    {
                        this.fileList.Add(new JsFileInfo(this.fileId++, this.FilterDirPath(aimPath, this.dirPath), 1, this.FilterFilePath(file, this.dirPath)));
                    }
                }
            }
        }
        catch (Exception ex) { throw (ex); }
    }

    public string FilterDirPath(string path, string root)
    {
        path = path.Replace(root, "").Replace("\\", "/");

        return path.Substring(0, path.LastIndexOf('/'));
    }

    public string FilterFilePath(string path, string root)
    {
        return path.Replace(root, "").Replace("\\", "/");
    }
}

public class JsFileInfo
{
    public int id { get; set; }
    
    public int type { get; set; }

    public string parent { get; set; }

    public string path { get; set; }

    public JsFileInfo(int id, string parent, int type, string path)
    {
        this.id = id;
        this.parent = parent;
        this.type = type;
        this.path = path;
    }
}