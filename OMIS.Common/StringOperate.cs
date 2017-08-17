using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace OMIS.Common
{
    /// <summary>
    /// StringOperate 的摘要说明
    /// </summary>
    public class StringOperate
    {
        public StringOperate()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 汉字转换为Unicode编码
        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnicode(string content)
        {
            byte[] bts = Encoding.Unicode.GetBytes(content);
            StringBuilder result = new StringBuilder();
            for (int i = 0, c = bts.Length; i < c; i += 2)
            {
                result.Append(String.Format("\\u{0}{1}", bts[i + 1].ToString("x").PadLeft(2, '0'), bts[i].ToString("x").PadLeft(2, '0')));
            }
            return result.ToString();
        }
        #endregion

        #region Unicode编码转换为汉字
        public static string ToGB2312(string unicode)
        {
            StringBuilder result = new StringBuilder();
            MatchCollection mc = Regex.Matches(unicode, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
                result.Append(Encoding.Unicode.GetString(bts));
            }
            return result.ToString();
        }
        #endregion
        
        #region  截取字符串
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="count">要截取的字节量</param>
        /// <returns>返回截取的字符串</returns>
        public static string SubString(string content, int count)
        {
            return SubString(content, 0, count, true, true);
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="offset">从0开始的字节偏移量</param>
        /// <param name="count">要截取的字节量</param>
        /// <returns>返回截取的字符串</returns>
        public static string SubString(string content, int offset, int count)
        {
            return SubString(content, offset, count, true, true);
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="offset">从0开始的字节偏移量</param>
        /// <param name="count">要截取的字节量</param>
        /// <param name="isFiltrate">是否过滤HTML标签</param>
        /// <param name="isShowEllipsis">是否显示省略号</param>
        /// <returns>返回截取的字符串</returns>
        public static string SubString(string content, int offset, int count, bool isFiltrate, bool isShowEllipsis)
        {
            if (isFiltrate)
            {
                content = Regex.Replace(content, "<[^>]*>|&nbsp;", "");
            }
            if (0 == count)
            {
                return content;
            }
            else
            {
                int intLen = content.Length;
                int start = offset;
                int end = count;
                int single = 0;
                char[] chars = content.ToCharArray();
                for (int i = 0, c = chars.Length; i < c; i++)
                {
                    if (System.Convert.ToInt32(chars[i]) > 255)
                    {
                        start += 2;
                    }
                    else
                    {
                        start += 1;
                        single++;
                    }
                    if (start >= count)
                    {
                        end = end % 2 == 0 ? (single % 2 == 0 ? i + 1 : i) : i + 1;
                        break;
                    }
                }
                if (intLen <= end)
                {
                    return content;
                }
                else
                {
                    string result = content.Substring(0, end);
                    string after = content.Remove(0, end);
                    return String.Format("{0}{1}", result, isShowEllipsis ? "..." : "");
                }
            }
        }
        #endregion

        #region  获取字符串长度
        /// <summary>
        /// 获得字符串长度
        /// </summary>
        /// <param name="content">字符串内容</param>
        /// <returns></returns>
        public static int GetStringLength(string content)
        {
            return GetStringLength(content, true);
        }
        /// <summary>
        /// 获得字符串长度
        /// </summary>
        /// <param name="content">字符串内容</param>
        /// <param name="isFiltrate">是否过滤HTML标签</param>
        /// <returns></returns>
        public static int GetStringLength(string content, bool isFiltrate)
        {
            if (isFiltrate)
            {
                content = Regex.Replace(content, "<[^>]*>|&nbsp;", "");
            }
            int len = 0;
            char[] chars = content.ToCharArray();
            foreach (char c in chars)
            {
                len += Convert.ToInt32(c) > 255 ? 2 : 1;
            }
            return len;
        }
        #endregion

        #region  替换过滤Script
        public static string RemoveScript(string content)
        {
            string pattern = @"/<script(.|\n)*?>(.|\n)*?<\/script(.|\n)*?>/ig";
            return new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(content, string.Empty);
        }
        #endregion

        #region  替换过滤Iframe
        public static string RemoveIframe(string content)
        {
            string pattern = @"/<iframe(.|\n)*?>(.|\n)*?<\/iframe(.|\n)*?>/ig";
            return new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(content, string.Empty);
        }
        #endregion

        #region  过滤程序引用
        public static string RemoveCodeQuote(string content)
        {
            string strExtension = ".aspx|.asp|.php|.jsp|.html|.htm|.shtml|.do|.action|.js";
            //清除JS引用、IFrame框架引用、IMG网页引用
            string pattern = "<img(.|\n)*?(" + strExtension + ")(.|\n)*?>"
                + "|<link(.|\n)*?>"
                + "|<style(.|\n)*?>(.|\n)*?</style(.|\n)*?>"
                + "|<iframe(.|\n)*?>(.|\n)*?</iframe(.|\n)*?>"
                + "|<scr(.|\n)*?ipt(.|\n)*?(" + strExtension + ")(.|\n)*?>(.|\n)*?</script(.|\n)*?>";

            Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

            return reg.Replace(content, string.Empty);
        }
        #endregion

        #region  替换HTML标记
        public static string RemoveHtml(string html)
        {
            #region
            //删除脚本

            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML

            html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
            //Htmlstring =System.Text.RegularExpressions. Regex.Replace(html,@"<A>.*</A>","");
            //Htmlstring =System.Text.RegularExpressions. Regex.Replace(html,@"<[a-zA-Z]*=\.[a-zA-Z]*\?[a-zA-Z]+=\d&\w=%[a-zA-Z]*|[A-Z0-9]","");
            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            html = html.Replace("<", "");
            html = html.Replace(">", "");
            html = html.Replace("\r\n", "");

            #endregion

            return html;
        }
        #endregion

        #region  提取内容中的图片路径
        public static string[] DistillImageUrl(string html)
        {
            Regex regObj = new Regex("<img(.|\n)*?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            string[] arr = new string[regObj.Matches(html).Count];
            int i = 0;
            foreach (Match m in regObj.Matches(html))
            {
                string imgUrl = GetImgUrl(m.Value);
                if (!imgUrl.Equals(string.Empty))
                {
                    arr[i] = imgUrl;
                    i++;
                }
            }
            return arr;
        }

        private static string GetImgUrl(string imgTagStr)
        {
            string str = string.Empty;
            Regex regObj = new Regex("http://.+.(?:jpg|gif|bmp|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match m in regObj.Matches(imgTagStr))
            {
                str = m.Value;
            }
            return str;
        }
        #endregion
        
        #region  过滤JS、过滤IFrame
        public static string FiltrateInputContent(string content)
        {
            //清除JS引用、IFrame框架引用、IMG网页引用
            string pattern = "<img(.|\n)*?(.aspx|.html|.asp|.php|.jsp|.htm|.js|.do)(.|\n)*?>|<link(.|\n)*?>"
                + "|<style(.|\n)*?>(.|\n)*?</style(.|\n)*?>|<iframe(.|\n)*?>(.|\n)*?</iframe(.|\n)*?>"
                + "|<scr(.|\n)*?ipt(.|\n)*?(.aspx|.html|.asp|.php|.jsp|.htm|.js|.do)(.|\n)*?>(.|\n)*?</script(.|\n)*?>";
            Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = reg.Replace(content, "");

            pattern = "<scr(.|\n)*?ipt(.|\n)*?>";
            reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = reg.Replace(content, "&lt;script&gt;");

            pattern = "</scr(.|\n)*?ipt(.|\n)*?>";
            reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = reg.Replace(content, "&lt;/script&gt;");

            pattern = "rel(.|\n)*?=(.|\n)*?nofollow(.|\n)*?";
            reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = reg.Replace(content, "");

            /*
            //给A链接加上 rel="nofollow"
            pattern = "<a(.|\n)*?href=";
            reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = reg.Replace(content, "<a rel=\"nofollow\" href=");

            pattern = "</a>";
            reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = reg.Replace(content, "</a>");
            */
            return content;
        }
        #endregion

        #region  给A链接标签加rel标签
        public static string AnchorAppendRelAttribute(string content)
        {
            //给A链接加上 rel="nofollow"
            string pattern = "<a(.|\n)*?href=";
            Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = reg.Replace(content, "<a rel=\"nofollow\" href=");

            pattern = "</a>";
            reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = reg.Replace(content, "</a>");

            return content;
        }
        #endregion

        #region  数字转汉字
        public static string NumberConvertChinese(int number)
        {
            if (number < 0 || number > 100)
            {
                return string.Empty;
            }
            string[] arrChinese = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            if (number <= 10)
            {
                return arrChinese[number];
            }
            else if (100 == number)
            {
                return "一百";
            }
            else
            {
                int td = number / 10;
                int sd = number % 10;
                return (td > 1 ? arrChinese[td] : "") + arrChinese[10] + (sd > 0 ? arrChinese[sd] : "");
            }
        }
        #endregion

    }
}