using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using OMIS.DBUtility;
using OMIS.Common;

public partial class tools_pinyin : System.Web.UI.Page
{

    protected JavaScriptSerializer js = new JavaScriptSerializer();

    protected List<PinyinClass> lstPinyin = new List<PinyinClass>();
    protected List<PinyinClass> lstUnknown = new List<PinyinClass>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitialData();
            this.ReadFile();
        }
    }

    protected void InitialData()
    {
        string[] PinyinList = new string[] {
            "a", "ai", "an", "ang", "ao", "ba", "bai", "ban", "bang", "bao", "bei", "ben", "beng", "bi", "bian", "biao", 
            "bie", "bin", "bing", "bo", "bu", "ca", "cai", "can", "cang", "cao", "ce", "ceng", "cha", "chai", "chan", "chang", 
            "chao", "che", "chen", "cheng", "chi", "chong", "chou", "chu", "chuai", "chuan", "chuang", "chui", "chun", "chuo", "ci", "cong", 
            "cou", "cu", "cuan", "cui", "cun", "cuo", "da", "dai", "dan", "dang", "dao", "de", "deng", "di", "dian", "diao", 
            "die", "ding", "diu", "dong", "dou", "du", "duan", "dui", "dun", "duo", "e", "en", "er", "fa", "fan", "fang", 
            "fei", "fen", "feng", "fo", "fou", "fu", "ga", "gai", "gan", "gang", "gao", "ge", "gei", "gen", "geng", "gong", 
            "gou", "gu", "gua", "guai", "guan", "guang", "gui", "gun", "guo", "ha", "hai", "han", "hang", "hao", "he", "hei", 
            "hen", "heng", "hong", "hou", "hu", "hua", "huai", "huan", "huang", "hui", "hun", "huo", "ji", "jia", "jian", "jiang", 
            "jiao", "jie", "jin", "jing", "jiong", "jiu", "ju", "juan", "jue", "jun", "ka", "kai", "kan", "kang", "kao", "ke", 
            "ken", "keng", "kong", "kou", "ku", "kua", "kuai", "kuan", "kuang", "kui", "kun", "kuo", "la", "lai", "lan", "lang", 
            "lao", "le", "lei", "leng", "li", "lia", "lian", "liang", "liao", "lie", "lin", "ling", "liu", "long", "lou", "lu", 
            "lv", "luan", "lue", "lun", "luo", "ma", "mai", "man", "mang", "mao", "me", "mei", "men", "meng", "mi", "mian", 
            "miao", "mie", "min", "ming", "miu", "mo", "mou", "mu", "na", "nai", "nan", "nang", "nao", "ne", "nei", "nen", 
            "neng", "ni", "nian", "niang", "niao", "nie", "nin", "ning", "niu", "nong", "nu", "nv", "nuan", "nue", "nuo", "o", 
            "ou", "pa", "pai", "pan", "pang", "pao", "pei", "pen", "peng", "pi", "pian", "piao", "pie", "pin", "ping", "po", 
            "pu", "qi", "qia", "qian", "qiang", "qiao", "qie", "qin", "qing", "qiong", "qiu", "qu", "quan", "que", "qun", "ran", 
            "rang", "rao", "re", "ren", "reng", "ri", "rong", "rou", "ru", "ruan", "rui", "run", "ruo", "sa", "sai", "san", 
            "sang", "sao", "se", "sen", "seng", "sha", "shai", "shan", "shang", "shao", "she", "shen", "sheng", "shi", "shou", "shu", 
            "shua", "shuai", "shuan", "shuang", "shui", "shun", "shuo", "si", "song", "sou", "su", "suan", "sui", "sun", "suo", "ta", 
            "tai", "tan", "tang", "tao", "te", "teng", "ti", "tian", "tiao", "tie", "ting", "tong", "tou", "tu", "tuan", "tui", 
            "tun", "tuo", "wa", "wai", "wan", "wang", "wei", "wen", "weng", "wo", "wu", "xi", "xia", "xian", "xiang", "xiao", 
            "xie", "xin", "xing", "xiong", "xiu", "xu", "xuan", "xue", "xun", "ya", "yan", "yang", "yao", "ye", "yi", "yin", 
            "ying", "yo", "yong", "you", "yu", "yuan", "yue", "yun", "za", "zai", "zan", "zang", "zao", "ze", "zei", "zen", 
            "zeng", "zha", "zhai", "zhan", "zhang", "zhao", "zhe", "zhen", "zheng", "zhi", "zhong", "zhou", "zhu", "zhua", "zhuai", "zhuan", 
            "zhuang", "zhui", "zhun", "zhuo", "zi", "zong", "zou", "zu", "zuan", "zui", "zun", "zuo"
        };

        foreach (string s in PinyinList)
        {
            this.ddlPinyin.Items.Add(new ListItem(s, s));
        }
    }

    protected void btnConvert_Click(object sender, EventArgs e)
    {
        this.txtCode.Text = "";
        this.txtPinyin.Text = "";
        this.txtUnKnown.Text = "";
        this.txtMultiPinyin.Text = "";

        this.ConvertPinyin();
    }

    #region  转换拼音
    protected void ConvertPinyin()
    {
        this.ReadFile();

        try
        {
            List<int> code = new List<int>();
            List<string> pinyin = new List<string>();
            List<string> unknown = new List<string>();

            Regex reg = new Regex("/(\r\n|\n)/g");
            string text = reg.Replace(this.txtText.Text.Trim(), "");
            char[] chars = text.ToCharArray();
            foreach (char c in chars)
            {
                int cd = ChinesePinyin.ConvertCode(c);
                code.Add(cd);
                string p = ChinesePinyin.Show(ChinesePinyin.ConvertPinyin(c, cd), true);
                pinyin.Add(p);
                if (p.Length == 0)
                {
                    PinyinClass pyc = new PinyinClass(0, cd, "", c.ToString());
                    lstPinyin.Add(pyc);
                    lstUnknown.Add(pyc);
                }
            }

            this.txtUnKnown.Text = this.ShowContent(js.Serialize(lstUnknown));

            StringBuilder sb_c = new StringBuilder();
            foreach (int i in code)
            {
                sb_c.Append(i.ToString());
            }
            this.txtCode.Text = sb_c.ToString();

            StringBuilder sb_p = new StringBuilder();
            int k = 0;
            foreach (string p in pinyin)
            {
                sb_p.Append(k++ > 0 ? "-" : "");
                sb_p.Append(p);
            }
            this.txtPinyin.Text = sb_p.ToString();

            if (text.Trim().Length == 1)
            {
                this.txtMultiPinyin.Text = ChinesePinyin.FindMultiPinyin(text.ToCharArray()[0]);
            }
        }
        catch (Exception ex)
        {
            Response.Write("ConvertError:" + ex.Message);
        }
    }
    #endregion

    protected void btnWrite_Click(object sender, EventArgs e)
    {
        this.WriteFile();
    }

    #region  记录到文件
    public void WriteFile()
    {
        try
        {
            string tmpPinyin = this.ddlPinyin.Text.Trim();
            int tmpIndex = Convert.ToInt32(this.txtPinyinIndex.Text.Trim());

            string content = this.txtUnKnown.Text.Trim();
            List<PinyinClass> lstPinyin = js.Deserialize<List<PinyinClass>>(content);

            List<string> lstSql = new List<string>();

            string tableName = this.txtTableName.Text.Trim();
            foreach (PinyinClass p in lstPinyin)
            {
                if (p.index > 0 && p.pinyin.Length > 0)
                {
                    lstSql.Add(String.Format("insert into `{0}`(`index`,`code`,`pinyin`,`text`)values({1},{2},'{3}','{4}');",
                        tableName, p.index, p.code, p.pinyin, p.text));
                }
            }
            string dbconn  =this.txtConnString.Text.Trim();
            foreach (string sql in lstSql)
            {
                try
                {
                    MysqlHelper.ExecuteNonQuery(dbconn, sql);
                }
                catch (Exception ex) { }
            }

            this.ReadFile();

            this.txtError.Text = "";
        }
        catch (Exception ex)
        {
            this.txtError.Text = "WriteError:" + ex.Message;
        }
    }

    public void ReadFile()
    {
        try
        {
            string dbconn = this.txtConnString.Text.Trim();
            string tableName = this.txtTableName.Text.Trim();
            string orderBy = this.ddlAsc.SelectedValue.ToString();
            string sql = String.Format(" select * from `{0}` order by `{1}` ", tableName, orderBy);

            List<PinyinClass> lstInfo = new List<PinyinClass>();

            DataSet ds = MysqlHelper.ExecuteDataSet(dbconn, sql);
            if (ds != null && ds.Tables[0] != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    PinyinClass p = new PinyinClass();
                    p.index = Convert.ToInt32(dr["index"].ToString());
                    p.code = Convert.ToInt32(dr["code"].ToString());
                    p.pinyin = dr["pinyin"].ToString();
                    p.text = dr["text"].ToString();

                    lstInfo.Add(p);
                }
            }

            this.txtSaveUn.Text = this.ShowContent(js.Serialize(lstInfo));

            this.txtError.Text = "";

            this.lblCount.Text = lstInfo.Count.ToString();
        }
        catch (Exception ex)
        {
            this.txtError.Text = "ReadError:" + ex.Message;
        }
    }
    #endregion

    protected void btnRead_Click(object sender, EventArgs e)
    {
        this.ReadFile();
    }

    #region  显示内容
    protected string ShowContent(string content)
    {
        return content.Replace("[", "[\r\n").Replace("]", "\r\n]").Replace("},", "},\r\n").Replace(",", ", ").Replace(":", ": ");
    }
    #endregion
    
    protected void btnBuild_Click(object sender, EventArgs e)
    {
        string content = this.txtSaveUn.Text.Trim();
        List<PinyinClass> lstPinyin = js.Deserialize<List<PinyinClass>>(content);

        StringBuilder strCode = new StringBuilder();
        StringBuilder strPinyin = new StringBuilder();
        StringBuilder strText = new StringBuilder();

        int cols = Convert.ToInt32(this.txtCols.Text.Trim());
        int c = lstPinyin.Count;
        int m = 0;
        int n = 0;
        
        foreach (PinyinClass p in lstPinyin)
        {
            if (p.index > 0)
            {
                if (n > 0 && n % cols == 0)
                {
                    strCode.Append("\r\n");
                    strPinyin.Append("\r\n");
                    strText.Append("\r\n");
                    /*
                     * if (n >= 10)
                    {
                        strCode.Append("\t\t\t");
                        strPinyin.Append("\t\t\t");
                        strText.Append("\t\t\t");
                    }*/
                }
                strCode.Append(String.Format("{0}", p.code));
                strPinyin.Append(String.Format("\"{0}\"", p.pinyin));
                strText.Append(String.Format("\"{0}\"", p.text));

                if (m < c - 1)
                {
                    strCode.Append(", ");
                    strPinyin.Append(", ");
                    strText.Append(", ");
                }
                n++;
            }
            m++;
        }

        this.txtSpecialCode.Text = strCode.ToString();
        this.txtSpecialPinyin.Text = strPinyin.ToString();
        this.txtSpecialText.Text = strText.ToString();
    }

    protected void ddlPinyin_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtPinyinIndex.Text = (this.ddlPinyin.SelectedIndex - 1).ToString();

        this.txtPinyinCopy.Text = this.ddlPinyin.SelectedItem.Text.ToString();
    }

    protected void ddlAsc_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ReadFile();
    }

}

public class PinyinClass
{
    public int index { get; set; }
    public int code { get; set; }
    public string pinyin { get; set; }
    public string text { get; set; }

    public PinyinClass() { }

    public PinyinClass(int idx, int code, string pinyin, string text)
    {
        this.index = idx;
        this.code = code;
        this.pinyin = pinyin;
        this.text = text;
    }
}