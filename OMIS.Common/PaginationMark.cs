using System;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Common
{
    /// <summary>
    /// Pagination 的摘要说明
    /// </summary>
    public class PaginationMark
    {
        protected PaginationParam pageParam;
        public int DataCount = 0;
        public int PageCount = 0;
        public string PageString = string.Empty;
        /// <summary>
        /// 是否隐藏URL链接，若隐藏，则将 href="URL" 改为 onclick="location=URL;"方式
        /// </summary>
        protected bool HideUrl = false;
        /// <summary>
        /// 是否隐藏分页码为0的分页标记
        /// </summary>
        protected bool HideZeroPage = true;

        public PaginationMark()
        {
            this.pageParam = new PaginationParam();
        }

        public PaginationMark(PaginationParam param)
        {
            this.pageParam = param;
        }

        #region  创建分页
        public string BuildPaginationString(int dataCount, int pageSize, int pageIndex, int pageStart, string targetUrl, string parameter)
        {
            PaginationParam p = new PaginationParam();
            p.DataCount = dataCount;
            p.PageSize = pageSize;
            p.PageIndex = pageIndex;
            p.PageStart = pageStart;
            p.TargetUrl = targetUrl;
            p.Parameter = parameter;
            p.TextStyle = PaginationTextStyle.None;

            return this.BuildPaginationString(p);
        }
        public string BuildPaginationString(int dataCount, int pageSize, int pageIndex, int pageStart, string targetUrl,
            string parameter, PaginationTextStyle textStyle, string pageName)
        {
            PaginationParam p = new PaginationParam();
            p.DataCount = dataCount;
            p.PageSize = pageSize;
            p.PageIndex = pageIndex;
            p.PageStart = pageStart;
            p.TargetUrl = targetUrl;
            p.Parameter = parameter;
            p.TextStyle = textStyle;
            p.PageName = pageName;

            return this.BuildPaginationString(p);
        }

        public string BuildPaginationString(int dataCount, int pageSize, int pageIndex, int pageStart, string targetUrl,
            string parameter, PaginationTextStyle textStyle, string pageName,
            bool isUrlRewrite, string connector, string extension, string lastParameter)
        {
            PaginationParam p = new PaginationParam();
            p.DataCount = dataCount;
            p.PageSize = pageSize;
            p.PageIndex = pageIndex;
            p.PageStart = pageStart;
            p.TargetUrl = targetUrl;
            p.Parameter = parameter;
            p.TextStyle = textStyle;
            p.PageName = pageName;
            p.UrlRewrite = isUrlRewrite;
            p.Connector = connector;
            p.Extension = extension;
            p.LastParameter = lastParameter;

            return this.BuildPaginationString(p);
        }

        public string BuildPaginationString(int dataCount, int pageSize, int pageIndex, int pageStart, string targetUrl,
            string parameter, PaginationTextStyle textStyle, string pageName,
            bool isUrlRewrite, string connector, string extension, string lastParameter,
            PaginationMarkType markType, int markCount,
            bool showInvalid, bool showDataCount, bool showPageCount, bool showDataStat)
        {
            PaginationParam p = new PaginationParam();
            p.DataCount = dataCount;
            p.PageSize = pageSize;
            p.PageIndex = pageIndex;
            p.PageStart = pageStart;
            p.TargetUrl = targetUrl;
            p.Parameter = parameter;
            p.TextStyle = textStyle;
            p.PageName = pageName;
            p.UrlRewrite = isUrlRewrite;
            p.Connector = connector;
            p.Extension = extension;
            p.LastParameter = lastParameter;
            p.MarkType = markType;
            p.MarkCount = markCount;
            p.ShowInvalid = showInvalid;
            p.ShowDataCount = showDataCount;
            p.ShowPageCount = showPageCount;
            p.ShowDataStat = showDataStat;

            return this.BuildPaginationString(p);
        }
        #endregion

        #region  创建分页
        public string BuildPaginationString(PaginationParam p)
        {
            StringBuilder page = new StringBuilder();
            StringBuilder url = new StringBuilder();
            PaginationTextMark textMarker = p.TextMark != null ? p.TextMark : this.BuildTextMarker(p.MarkType);

            ///样式名
            string textStyle = " class=\"pgts\"";
            string textStyleLink = " class=\"pgtls\"";
            string textStyleCurrentLink = " class=\"pgtcls\"";
            string textStyleTextLink = " class=\"pgtxts\"";

            if (p.DataCount <= 0 || p.PageSize <= 0)
            {
                this.DataCount = 0;
                this.PageCount = 0;
                this.PageString = string.Empty;
                return string.Empty;
            }
            else if (p.TargetUrl.Equals(string.Empty))
            {
                if (p.UrlRewrite)
                {
                    this.PageString = string.Empty;
                    return string.Empty;
                }
                string pageUrl = System.Web.HttpContext.Current.Request.Url.ToString();
                Regex reg = new Regex(@"[&]?(page=|pageIndex=)\d+");
                p.TargetUrl = reg.Replace(pageUrl.Substring(pageUrl.LastIndexOf('/') + 1), string.Empty);
                if (p.TargetUrl.EndsWith("?"))
                {
                    p.TargetUrl = p.TargetUrl.Substring(0, p.TargetUrl.Length - 1);
                }
                reg = new Regex(@"[&]{2,}");
                p.TargetUrl = reg.Replace(p.TargetUrl, "&");
            }

            this.DataCount = p.DataCount;
            this.PageCount = this.GetPageCount(p);
            this.HideUrl = p.HideUrl;
            this.HideZeroPage = p.HideZeroPage;

            int[] arrStartEnd = this.GetStartEndNum(this.PageCount, p.PageStart, p.PageIndex, p.MarkCount);
            int startNum = arrStartEnd[0];
            int endNum = arrStartEnd[1];
            int minuend = Math.Abs(p.PageStart - 1);
            int num = 0;
            string text = string.Empty;

            //检测扩展名，防止出现双重扩展名
            p.Extension = this.CheckExtension(p.TargetUrl, p.Extension);

            #region  创建分页标记
            if (p.UrlRewrite)
            {
                #region  创建分页码 数字
                for (int i = startNum; i < endNum; i++)
                {
                    num++;
                    if (i > (this.PageCount - minuend) || num > p.MarkCount) break;
                    string strNum = (i + minuend).ToString();

                    url.Length = 0;
                    if (i == p.PageIndex)
                    {
                        url.Append(String.Format("<li{0}><em>{1}</em></li>", textStyleCurrentLink, strNum));
                    }
                    else
                    {
                        //href="TargetUrl + Connector + Parameter + Num + Extension + LastParameter"
                        text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}{5}';\">{6}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}{5}\">{6}</a></li>";
                        url.Append(String.Format(text, textStyleLink, p.TargetUrl, p.Parameter,
                            this.BuildPageNum(i, startNum, p.Connector, string.Empty), p.Extension, p.LastParameter, strNum));
                    }
                    page.Append(url.ToString());
                }
                #endregion
            }
            else
            {
                #region  创建分页码 数字
                for (int i = startNum; i < endNum; i++)
                {
                    num++;
                    if (i > (this.PageCount - minuend) || num > p.MarkCount) break;
                    string strNum = (i + minuend).ToString();

                    url.Length = 0;
                    if (i == p.PageIndex)
                    {
                        url.Append(String.Format("<li{0}><em>{1}</em></li>", textStyleCurrentLink, strNum));
                    }
                    else
                    {
                        text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}';\">{5}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}\">{5}</a></li>";
                        url.Append(String.Format(text, textStyleLink, p.TargetUrl, p.Extension,
                            this.BuildPageNum(i, startNum, p.TargetUrl, p.Extension, p.Parameter, p.PageName, string.Empty),
                            p.LastParameter, strNum));
                    }
                    page.Append(url.ToString());
                }
                #endregion
            }

            int quickNum = 0;
            if (p.UrlRewrite)
            {
                #region  创建上、下页标签
                if (p.PageIndex != this.PageCount - minuend)
                {
                    //下一页
                    url.Length = 0;
                    text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}{5}{6}';\">{7}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}{5}{6}\">{7}</a></li>";
                    url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Parameter, p.Connector,
                        p.PageIndex + 1, p.Extension, p.LastParameter, textMarker.Next));
                    page.Append(url.ToString());

                    //显示省略号快进按钮
                    if (p.ShowEllipsis)
                    {
                        url.Length = 0;
                        quickNum = p.PageIndex + p.MarkCount < this.PageCount - minuend ? p.PageIndex + p.MarkCount : this.PageCount - minuend;
                        text = this.HideUrl ? "<li><a onclick=\"location.href='{1}{2}{3}{4}{5}{6}';\">{7}</a></li>" : "<li><a href=\"{1}{2}{3}{4}{5}{6}\">{7}</a></li>";
                        url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Parameter, p.Connector,
                            quickNum, p.Extension, p.LastParameter, "…"));
                        page.Append(url.ToString());
                    }

                    if (p.ShowFirstLastTextMark)
                    {
                        //末页
                        url.Length = 0;
                        //text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}{5}{6}';\">{7}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}{5}{6}\">{7}</a></li>";
                        url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Parameter, p.Connector,
                            this.PageCount - minuend, p.Extension, p.LastParameter, textMarker.Last));
                        page.Append(url.ToString());
                    }
                }
                else if (p.ShowInvalid)
                {
                    page.Append(String.Format("<li{0}><em>{1}</em></li>", textStyle, textMarker.Next));
                    if (p.ShowFirstLastTextMark)
                    {
                        page.Append(String.Format("<li{0}><em>{1}</em></li>", textStyle, textMarker.Last));
                    }
                }

                if (p.PageIndex != startNum)
                {
                    //上一页
                    url.Length = 0;
                    text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}{5}';\">{6}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}{5}\">{6}</a></li>";
                    url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Parameter,
                        this.BuildPageText(p.PageIndex - 1, startNum, p.Connector),
                        p.Extension, p.LastParameter, textMarker.Previous));
                    page.Insert(0, url.ToString());

                    //显示省略号快退按钮
                    if (p.ShowEllipsis)
                    {
                        url.Length = 0;
                        quickNum = p.PageIndex - p.MarkCount > p.PageStart ? p.PageIndex - p.MarkCount : p.PageStart;
                        text = this.HideUrl ? "<li><a onclick=\"location.href='{1}{2}{3}{4}{5}';\">{6}</a></li>" : "<li><a href=\"{1}{2}{3}{4}{5}\">{6}</a></li>";
                        url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Parameter,
                            (1 == p.PageStart || quickNum != p.PageStart ? p.Connector + quickNum : string.Empty),
                            p.Extension, p.LastParameter, "…"));
                        page.Insert(0, url.ToString());
                    }

                    if (p.ShowFirstLastTextMark)
                    {
                        //首页
                        url.Length = 0;
                        //text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}{5}';\">{6}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}{5}\">{6}</a></li>";
                        url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Parameter,
                            (1 == p.PageStart ? p.Connector + p.PageStart.ToString() : string.Empty),
                            p.Extension, p.LastParameter, textMarker.First));
                        page.Insert(0, url.ToString());
                    }
                }
                else if (p.ShowInvalid)
                {
                    page.Insert(0, String.Format("<li{0}><em>{1}</em></li>", textStyle, textMarker.Previous));
                    if (p.ShowFirstLastTextMark)
                    {
                        page.Insert(0, String.Format("<li{0}><em>{1}</em></li>", textStyle, textMarker.First));
                    }
                }
                #endregion
            }
            else
            {
                #region  创建上、下页标签
                if (p.PageIndex != this.PageCount - minuend)
                {
                    //下一页
                    url.Length = 0;
                    text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}';\">{5}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}\">{5}</a></li>";
                    url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Extension,
                        this.BuildPageNum(p.PageIndex + 1, startNum, p.TargetUrl, p.Extension, p.Parameter, p.PageName, string.Empty),
                        p.LastParameter, textMarker.Next));
                    page.Append(url.ToString());

                    //显示省略号快进按钮
                    if (p.ShowEllipsis)
                    {
                        url.Length = 0;
                        quickNum = p.PageIndex + p.MarkCount < this.PageCount - minuend ? p.PageIndex + p.MarkCount : this.PageCount - minuend;
                        text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}';\">{5}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}\">{5}</a></li>";
                        url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Extension,
                            this.BuildPageNum(quickNum, startNum, p.TargetUrl, p.Extension, p.Parameter, p.PageName, string.Empty),
                            p.LastParameter, "…"));
                        page.Append(url.ToString());
                    }

                    if (p.ShowFirstLastTextMark)
                    {
                        //末页
                        url.Length = 0;
                        //text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}';\">{5}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}\">{5}</a></li>";
                        url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Extension,
                            this.BuildPageNum(this.PageCount - minuend, startNum, p.TargetUrl, p.Extension, p.Parameter, p.PageName, string.Empty),
                            p.LastParameter, textMarker.Last));
                        page.Append(url.ToString());
                    }
                }
                else if (p.ShowInvalid)
                {
                    page.Append(String.Format("<li{0}><em>{1}</em></li>", textStyle, textMarker.Next));
                    if (p.ShowFirstLastTextMark)
                    {
                        page.Append(String.Format("<li{0}><em>{1}</em></li>", textStyle, textMarker.Last));
                    }
                }

                if (p.PageIndex != startNum)
                {
                    //上一页
                    url.Length = 0;
                    text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}';\">{5}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}\">{5}</a></li>";
                    url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Extension,
                        this.BuildPageText(p.PageIndex - 1, startNum, p.TargetUrl, p.Extension, p.Parameter, p.PageName),
                        p.LastParameter, textMarker.Previous));
                    page.Insert(0, url.ToString());

                    //显示省略号快退按钮
                    if (p.ShowEllipsis)
                    {
                        url.Length = 0;
                        quickNum = p.PageIndex - p.MarkCount > p.PageStart ? p.PageIndex - p.MarkCount : p.PageStart;
                        text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}';\">{5}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}\">{5}</a></li>";
                        url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Extension,
                            this.BuildPageText(quickNum, startNum, p.TargetUrl, p.Extension, p.Parameter, p.PageName),
                            p.LastParameter, "…"));
                        page.Insert(0, url.ToString());
                    }

                    if (p.ShowFirstLastTextMark)
                    {
                        //首页
                        url.Length = 0;
                        //text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}{2}{3}{4}';\">{5}</a></li>" : "<li{0}><a href=\"{1}{2}{3}{4}\">{5}</a></li>";
                        url.Append(String.Format(text, textStyleTextLink, p.TargetUrl, p.Extension,
                            this.BuildPageText(p.PageStart, startNum, p.TargetUrl, p.Extension, p.Parameter, p.PageName),
                            p.LastParameter, textMarker.First));
                        page.Insert(0, url.ToString());
                    }
                }
                else if (p.ShowInvalid)
                {
                    page.Insert(0, String.Format("<li{0}><em>{1}</em></li>", textStyle, textMarker.Previous));
                    if (p.ShowFirstLastTextMark)
                    {
                        page.Insert(0, String.Format("<li{0}><em>{1}</em></li>", textStyle, textMarker.First));
                    }
                }
                #endregion
            }
            #endregion

            //显示数据总数
            page.Insert(0, p.ShowDataCount ? String.Format("<li>({0})</li>", p.DataCount) : string.Empty);
            //显示刷新按钮
            text = this.HideUrl ? "<li{0}><a onclick=\"location.href='{1}';\">{2}</a></li>" : "<li{0}><a href=\"{1}\">{2}</a></li>";
            page.Append(!p.ShowRefurbishButton ? string.Empty : String.Format(text,
                "", this.BuildCurrentPageUrl(p, string.Empty), (p.MarkType == PaginationMarkType.Enblish ? "Refresh" : "刷新")));
            //显示总页数
            page.Append(!p.ShowPageCount ? string.Empty : this.BuildPageCount(p.MarkType, this.PageCount));
            //显示跳转文本框
            page.Append(!p.ShowPageJump ? string.Empty : this.BuildPageJump(p));
            //显示数据统计
            page.Append(!p.ShowDataStat ? string.Empty : this.BuildDataStat(p));

            page.Insert(0, String.Format("<div class=\"{0}\"><ul>", p.CssName));
            page.Append("</ul></div>");

            //创建样式
            page.Insert(0, this.BuildPaginationStyle(p.TextStyle, p.CssName));

            this.PageString = new Regex(@"[&]{2,}").Replace(page.ToString(), "&");

            return this.PageString.Replace("?&", "?");
        }
        #endregion

        #region  获得总页数
        protected int GetPageCount(PaginationParam param)
        {
            return param.DataCount % param.PageSize == 0 ? param.DataCount / param.PageSize : (param.DataCount / param.PageSize) + 1;
        }
        #endregion

        #region  获得开始结束页码
        protected int[] GetStartEndNum(int pageCount, int pageStart, int pageIndex, int markCount)
        {
            int startNum = pageStart;
            int endNum = 0;
            if (pageCount < markCount)
            {
                endNum = markCount;
            }
            else
            {
                int mf = (int)Math.Floor((double)markCount / 2);
                int mc = (int)Math.Ceiling((double)markCount / 2);
                if (pageIndex > mf)
                {
                    startNum = pageIndex - mf;
                }
                endNum = (pageIndex + mc) > pageCount ? pageCount : (pageIndex + mc);
                if (pageStart > 0) endNum += pageStart;
                int c = endNum - startNum;

                if (c < markCount)
                {
                    if (startNum < markCount)
                    {
                        endNum += markCount - c;
                    }
                    else if (startNum >= markCount)
                    {
                        startNum -= markCount - c;
                    }
                }
            }
            return new int[2] { startNum, endNum };
        }
        #endregion

        #region  检测扩展名
        protected string CheckExtension(string strTargetUrl, string strExtension)
        {
            if (strTargetUrl.IndexOf('?') >= 0)
            {
                return string.Empty;
            }
            else
            {
                string[] arr = new string[] { 
                    ".aspx",".asp",".html",".htm",".php",".jsp",".shtml"
                };
                foreach (string str in arr)
                {
                    if (strTargetUrl.EndsWith(str))
                    {
                        return string.Empty;
                    }
                }
            }
            return strExtension;
        }
        #endregion

        #region  创建页码数字文本
        /// <summary>
        /// 创建页码数字文本（URL重写）
        /// </summary>
        /// <param name="i"></param>
        /// <param name="startNum"></param>
        /// <param name="strConnector"></param>
        /// <returns></returns>
        protected string BuildPageNum(int i, int startNum, string connector, string pageIndex)
        {
            if (pageIndex.Equals(string.Empty))
            {
                return i == startNum ? (startNum > 0 ? connector + i.ToString() : string.Empty) : connector + i.ToString();
            }
            else if (0 == this.pageParam.PageStart)
            {
                return connector + pageIndex; //启用页面跳转输入框时，当前是第0（索引从0开始）页也可以跳转
            }
            else
            {
                return i == startNum ? (startNum > 0 ? connector + pageIndex : string.Empty) : connector + pageIndex;
            }
        }
        /// <summary>
        /// 创建上、下页文字页码
        /// </summary>
        /// <param name="i"></param>
        /// <param name="startNum"></param>
        /// <param name="strConnector"></param>
        /// <returns></returns>
        protected string BuildPageText(int i, int startNum, string connector)
        {
            return i == startNum ? (1 == startNum ? connector + i.ToString() : string.Empty) : connector + i.ToString();
        }

        /// <summary>
        /// 创建页码数字文本
        /// </summary>
        /// <param name="i"></param>
        /// <param name="startNum"></param>
        /// <param name="strTargetUrl"></param>
        /// <param name="strExtension"></param>
        /// <param name="strParameter"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        protected string BuildPageNum(int i, int startNum, string targetUrl, string extension, string parameter, string pageName, string pageIndex)
        {
            string mark = this.BuildQuestionMark(targetUrl, extension);
            string param = string.Empty;
            if (!parameter.Equals(string.Empty))
            {
                param = String.Format("{0}{1}", mark, parameter);
                mark = "&";
            }
            if (pageIndex.Equals(string.Empty))
            {
                string num = i > 0 || !this.HideZeroPage ? String.Format("{0}{1}={2}", mark, pageName, i.ToString()) : string.Empty;
                return String.Format("{0}{1}", param, i == startNum ? (startNum > 0 || !this.HideZeroPage ? num : string.Empty) : num);
            }
            else
            {
                return param + String.Format("{0}{1}={2}", mark, pageName, pageIndex); //启用页面跳转输入框时，当前是第0（索引从0开始）页也可以跳转
            }
        }

        protected string BuildPageText(int i, int startNum, string targetUrl, string extension, string parameter, string pageName)
        {
            string mark = this.BuildQuestionMark(targetUrl, extension);
            string param = string.Empty;
            if (!parameter.Equals(string.Empty))
            {
                param = String.Format("{0}{1}", mark, parameter);
                mark = "&";
            }
            string strNum = i > 0 || !this.HideZeroPage ? String.Format("{0}{1}={2}", mark, pageName, i.ToString()) : string.Empty;

            return String.Format("{0}{1}", param, i == startNum ? (startNum > 0 || !this.HideZeroPage ? strNum : string.Empty) : strNum);
        }

        protected string BuildCurrentPageUrl(PaginationParam param, string strPageIndex)
        {
            if (param.UrlRewrite)
            {
                return String.Format("{0}{1}{2}{3}{4}", param.TargetUrl, param.Parameter,
                    this.BuildPageNum(param.PageIndex, param.PageIndex, param.Connector, strPageIndex), param.Extension, param.LastParameter);
            }
            else
            {
                return String.Format("{0}{1}{2}{3}", param.TargetUrl, param.Extension,
                    this.BuildPageNum(param.PageIndex, param.PageIndex, param.TargetUrl, param.Extension, param.Parameter, param.PageName, strPageIndex),
                    param.LastParameter);
            }
        }

        protected string BuildQuestionMark(string targetUrl, string extension)
        {
            return String.Format("{0}{1}", targetUrl, extension).IndexOf('?') >= 0 ? "&" : "?";
        }
        #endregion

        #region  创建上、下页文本
        protected PaginationTextMark BuildTextMarker(PaginationMarkType type)
        {
            PaginationTextMark textMarker = new PaginationTextMark();
            switch (type)
            {
                case PaginationMarkType.Symbol:
                    textMarker.First = "&lt;&lt;";
                    textMarker.Previous = "&lt;";
                    textMarker.Next = "&gt;";
                    textMarker.Last = "&gt;&gt;";
                    break;
                case PaginationMarkType.Chinese:
                    textMarker.First = "首页";
                    textMarker.Previous = "上一页";
                    textMarker.Next = "下一页";
                    textMarker.Last = "末页";
                    break;
                case PaginationMarkType.Enblish:
                    textMarker.First = "First";
                    textMarker.Previous = "Previous";
                    textMarker.Next = "Next";
                    textMarker.Last = "Last";
                    break;
            }
            return textMarker;
        }
        #endregion

        #region  创建总页数文本
        protected string BuildPageCount(PaginationMarkType type, int pageCount)
        {
            if (type == PaginationMarkType.Enblish)
            {
                return String.Format("<li>Total Pages:<b>{0}</b></li>", pageCount);
            }
            return String.Format("<li>共<b>{0}</b>页</li>", pageCount);
        }
        #endregion

        #region  创建数据统计文本
        public string BuildDataStat(int dataCount, int pageIndex, int pageSize, int pageStart, bool isEnglish)
        {
            int num = (pageIndex - pageStart) * pageSize;
            int end = num + pageSize > dataCount ? dataCount : num + pageSize;
            string strText = isEnglish ? "Showing {0} to {1} Records, Total {2}" : "显示第<b>{0}</b>条到第<b>{1}</b>条记录，共<b>{2}</b>条";
            return String.Format("<span class=\"pagination-data-stat\" style=\"float:right;\">{0}</span>", String.Format(strText, num + 1, end, dataCount));
        }
        protected string BuildDataStat(PaginationParam param)
        {
            return this.BuildDataStat(param.DataCount, param.PageIndex, param.PageSize, param.PageStart, param.MarkType == PaginationMarkType.Enblish);
        }
        #endregion

        #region  创建页面跳转文本框
        protected string BuildPageJump(PaginationParam param)
        {
            int minuend = Math.Abs(param.PageStart - 1);
            string strInput = "<input type=\"text\" class=\"txtPaginationJump\""
                + " style=\"width:30px;margin:0 2px;padding:0 2px;height:19px;line-height:18px;border:solid 1px #ddd;text-align:center;font-size:12px;\""
                + " maxlength=\"8\" value=\"" + (param.PageIndex + minuend) + "\" onkeypress=\"paginationPageJump(event,this.value);\""
                + " onkeyup=\"value=value.replace(/[^\\d]/g,'')\" onblur=\"paginationPageJump(null,this.value);\" />";
            string url = this.BuildCurrentPageUrl(param, "%s");
            string postfix = new Random().Next(1000, 9999).ToString();
            strInput += "<script type=\"text/javascript\">"
                + "var _old_page_index_" + postfix + " = " + (param.PageIndex + minuend) + ";"
                + "function paginationPageJump(e, page){"
                + "var e = null == e ? {keyCode: 13} : e||event;"
                + "if(e.keyCode == 13){"
                + "if('' == page){return false;}"
                + "var _start_num = 1;var _end_num = " + this.PageCount + ";var _cur_num = parseInt(page,10);"
                + "if(_cur_num == _old_page_index_" + postfix + "){return false;}"
                + "if(_cur_num < _start_num || _cur_num > _end_num){return false;}"
                + "var url = '" + url + "'.replace('%s',_cur_num-" + minuend + ");"
                + "location.href = url;"
                + "}"
                + "}"
                + "</script>";
            return String.Format(param.MarkType == PaginationMarkType.Enblish ? "<li><span>Goto:</span>{0}</li>" : "<li><span>跳到第</span>{0}<span>页</span></li>", strInput);
        }
        #endregion

        #region  创建分页标记默认样式
        /// <summary>
        /// 创建分页标记默认样式
        /// </summary>
        /// <returns></returns>
        public string BuildPaginationStyle(PaginationTextStyle style, string strCssName)
        {
            StringBuilder strStyle = new StringBuilder();
            string strBgcolor = "#dbf1ff";
            string strBdcolor = "#7dc9fd";
            string strColor = "#333";

            #region  颜色系
            switch (style)
            {
                case PaginationTextStyle.Red:
                    strBgcolor = "#fff5fa";
                    strBdcolor = "#f00";
                    break;
                case PaginationTextStyle.Blue:
                    strBgcolor = "#dbf1ff";
                    strBdcolor = "#7dc9fd";
                    break;
                case PaginationTextStyle.Gray:
                    strBgcolor = "#eee";
                    strBdcolor = "#ccc";
                    break;
                case PaginationTextStyle.Orange:
                    strBgcolor = "#fff9ed";
                    strBdcolor = "#fc0";
                    break;
                case PaginationTextStyle.Black:
                    strBgcolor = "#000";
                    strBdcolor = "#fff";
                    strColor = "#fff";
                    break;
                case PaginationTextStyle.Green:
                    strBgcolor = "#008000";
                    strBdcolor = "#fff";
                    strColor = "#fff";
                    break;
                case PaginationTextStyle.Fuchsia:
                    strBgcolor = "#f0f";
                    strBdcolor = "#fff";
                    break;
                case PaginationTextStyle.None:
                    strBgcolor = "";
                    strBdcolor = "";
                    break;
            }
            #endregion

            if (!strBgcolor.Equals(string.Empty) && !strBdcolor.Equals(string.Empty))
            {
                strStyle.Append("\r\n<style type=\"text/css\">");
                strStyle.Append(".{0}{{margin:0 auto;padding:0;height:24px;line-height:24px;font-size:12px;font-family:宋体,Arial;overflow:hidden;}}");
                strStyle.Append(".{0} ul{{margin:0;padding:0;overflow:hidden;}}");
                strStyle.Append(".{0} li{{display:inline;margin:0 3px;padding:0;overflow:hidden;}}");
                strStyle.Append(".{0} li a{{text-decoration:none;padding:0 6px;border:solid 1px #dadbdd;line-height:20px;color:#333;display:inline-block;}}");
                strStyle.Append(".{0} li a:hover{{text-decoration:none;background:{1};border:solid 1px {2};color:{3};}}");
                strStyle.Append(".{0} li em{{padding:0 6px;border:solid 1px #dadbdd;line-height:20px;color:#999;display:inline-block;font-style:normal;}}");
                strStyle.Append(".{0} .pgtcls em{{background:{1};border:solid 1px {2};color:{3};}}");

                strStyle.Append(".{0} .pgtcls{{}}");
                strStyle.Append(".{0} .pgtls{{}}");
                strStyle.Append(".{0} .pgts{{}}");
                strStyle.Append(".{0} .pgtxts a{{}}");
                strStyle.Append(".{0} span{{color:#666;}}");
                strStyle.Append(".{0} b{{margin:0 3px;font-weight:normal;}}");
                strStyle.Append(".{0} .txtPaginationJump{{width:30px;margin:0 2px;padding:0 2px;height:19px;line-height:18px;border:solid 1px #ddd;text-align:center;font-size:12px;}}");
                strStyle.Append("</style>\r\n");

                return String.Format(strStyle.ToString(), strCssName, strBgcolor, strBdcolor, strColor);
            }
            return string.Empty;
        }
        #endregion

        #region  公共静态方法
        /// <summary>
        /// 获得总页数
        /// </summary>
        /// <param name="dataCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int GetPageCount(int dataCount, int pageIndex, int pageSize)
        {
            return dataCount % pageSize == 0 ? dataCount / pageSize : (dataCount / pageSize) + 1;
        }

        /// <summary>
        /// 获得数据统计
        /// </summary>
        /// <param name="dataCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageStart"></param>
        /// <returns>0:开始记录条数，1:结束记录条数，2:总记录条数</returns>
        public static int[] GetDataStat(int dataCount, int pageIndex, int pageSize, int pageStart)
        {
            int num = (pageIndex - pageStart) * pageSize;
            int end = num + pageSize > dataCount ? dataCount : num + pageSize;
            return new int[3] { num + 1, end, dataCount };
        }
        #endregion

    }

    #region  分页标记文字样式枚举
    /// <summary>
    /// 分页标记文字样式
    /// </summary>
    public enum PaginationTextStyle
    {
        None,
        Red,
        Blue,
        Gray,
        Orange,
        Black,
        Green,
        Fuchsia,
    }
    #endregion

    #region 分页标记类型枚举
    public enum PaginationMarkType
    {
        Symbol,
        Chinese,
        Enblish,
    }
    #endregion

    #region 分页标记显示类型枚举
    public enum PaginationShowType
    {
        List,
        Text,
    }
    #endregion


    #region  分页参数类
    public class PaginationParam
    {

        #region  属性变量
        private string cssName = "pagination";
        public string CssName
        {
            get { return this.cssName; }
            set { this.cssName = value; }
        }

        private int dataCount = 0;
        public int DataCount
        {
            get { return this.dataCount; }
            set { this.dataCount = value; }
        }

        private int pageSize = 10;
        public int PageSize
        {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }

        private int pageIndex = 0;
        public int PageIndex
        {
            get { return this.pageIndex; }
            set { this.pageIndex = value; }
        }

        private int pageStart = 0;
        public int PageStart
        {
            get { return this.pageStart; }
            set { this.pageStart = value; }
        }

        private int markCount = 10;
        public int MarkCount
        {
            get { return this.markCount; }
            set { this.markCount = value; }
        }

        private string targetUrl = string.Empty;
        /// <summary>
        /// 目标URL
        /// </summary>
        public string TargetUrl
        {
            get { return this.targetUrl; }
            set { this.targetUrl = value; }
        }

        private string parameter = string.Empty;
        /// <summary>
        /// URL参数
        /// </summary>
        public string Parameter
        {
            get { return this.parameter; }
            set { this.parameter = value; }
        }

        private string lastParameter = string.Empty;
        /// <summary>
        /// URL末尾参数
        /// </summary>
        public string LastParameter
        {
            get { return this.lastParameter; }
            set { this.lastParameter = value; }
        }

        private string pageName = "pageIndex";
        /// <summary>
        /// 分页码参数名称
        /// </summary>
        public string PageName
        {
            get { return this.pageName; }
            set { this.pageName = value; }
        }

        private bool isUrlRewrite = false;
        /// <summary>
        /// 是否重写
        /// </summary>
        public bool UrlRewrite
        {
            get { return this.isUrlRewrite; }
            set { this.isUrlRewrite = value; }
        }

        private string connector = "-";
        /// <summary>
        /// 连接符
        /// </summary>
        public string Connector
        {
            get { return this.connector; }
            set { this.connector = value; }
        }

        private string extension = "";
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }

        private PaginationShowType showType = PaginationShowType.List;
        /// <summary>
        /// 分页标记显示方式：List:列表，Text:文本框
        /// </summary>
        public PaginationShowType ShowType
        {
            get { return this.showType; }
            set { this.showType = value; }
        }

        private PaginationMarkType markType = PaginationMarkType.Symbol;
        /// <summary>
        /// 标记类型
        /// </summary>
        public PaginationMarkType MarkType
        {
            get { return this.markType; }
            set { this.markType = value; }
        }

        private PaginationTextMark textMark = null;
        /// <summary>
        /// 文字标记类型
        /// </summary>
        public PaginationTextMark TextMark
        {
            get { return this.textMark; }
            set { this.textMark = value; }
        }

        private PaginationTextStyle textStyle = PaginationTextStyle.Blue;
        /// <summary>
        /// 默认文字样式，样式可在CSS中重写
        /// </summary>
        public PaginationTextStyle TextStyle
        {
            get { return this.textStyle; }
            set { this.textStyle = value; }
        }

        private bool showInvalid = true;
        /// <summary>
        /// 是否显示无效的文字标签
        /// </summary>
        public bool ShowInvalid
        {
            get { return this.showInvalid; }
            set { this.showInvalid = value; }
        }

        private bool showFirstLastTextMark = true;
        /// <summary>
        /// 显示首页、末页 文字标签
        /// </summary>
        public bool ShowFirstLastTextMark
        {
            get { return this.showFirstLastTextMark; }
            set { this.showFirstLastTextMark = value; }
        }

        private bool showEllipsis = false;
        /// <summary>
        /// 显示省略号(快进|快退)按钮
        /// </summary>
        public bool ShowEllipsis
        {
            get { return this.showEllipsis; }
            set { this.showEllipsis = value; }
        }

        private bool showDataCount = false;
        /// <summary>
        /// 是否显示数据条数
        /// </summary>
        public bool ShowDataCount
        {
            get { return this.showDataCount; }
            set { this.showDataCount = value; }
        }

        private bool showPageCount = false;
        /// <summary>
        /// 是否显示总页数
        /// </summary>
        public bool ShowPageCount
        {
            get { return this.showPageCount; }
            set { this.showPageCount = value; }
        }

        private bool showDataStat = false;
        /// <summary>
        /// 是否显示数据统计文字
        /// </summary>
        public bool ShowDataStat
        {
            get { return this.showDataStat; }
            set { this.showDataStat = value; }
        }

        private bool showRefurbish = false;
        /// <summary>
        /// 是否显示刷新按钮
        /// </summary>
        public bool ShowRefurbishButton
        {
            get { return this.showRefurbish; }
            set { this.showRefurbish = value; }
        }

        private bool showPageJump = false;
        /// <summary>
        /// 是否显示分页跳转文本框
        /// </summary>
        public bool ShowPageJump
        {
            get { return this.showPageJump; }
            set { this.showPageJump = value; }
        }

        private bool isHideUrl = false;
        /// <summary>
        /// 是否隐藏URL
        /// </summary>
        public bool HideUrl
        {
            get { return this.isHideUrl; }
            set { this.isHideUrl = value; }
        }

        private bool isHideZeroPage = true;
        /// <summary>
        /// 是否隐藏-0页面
        /// </summary>
        public bool HideZeroPage
        {
            get { return this.isHideZeroPage; }
            set { this.isHideZeroPage = value; }
        }
        #endregion

        public PaginationParam() { }

        public PaginationParam(int dataCount, int pageSize, int pageIndex, int pageStart, string strTargetUrl, string strParameter)
        {
            this.dataCount = dataCount;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.pageStart = pageStart;
            this.targetUrl = strTargetUrl;
            this.parameter = strParameter;
            this.textStyle = PaginationTextStyle.None;
        }

        public PaginationParam(int dataCount, int pageSize, int pageIndex, int pageStart, string strTargetUrl,
            string strParameter, PaginationTextStyle textStyle, string pageName)
        {
            this.dataCount = dataCount;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.pageStart = pageStart;
            this.targetUrl = strTargetUrl;
            this.parameter = strParameter;
            this.textStyle = textStyle;
            this.pageName = pageName;
        }

        public PaginationParam(int dataCount, int pageSize, int pageIndex, int pageStart, string strTargetUrl,
            string strParameter, PaginationTextStyle textStyle, string pageName,
            bool isUrlRewrite, string strConnector, string strExtension, string strLastParameter)
        {
            this.dataCount = dataCount;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.pageStart = pageStart;
            this.targetUrl = strTargetUrl;
            this.parameter = strParameter;
            this.textStyle = textStyle;
            this.pageName = pageName;
            this.isUrlRewrite = isUrlRewrite;
            this.connector = strConnector;
            this.extension = strExtension;
            this.lastParameter = strLastParameter;
        }

        public PaginationParam(int dataCount, int pageSize, int pageIndex, int pageStart, string strTargetUrl,
            string strParameter, PaginationTextStyle textStyle, string pageName,
            bool isUrlRewrite, string strConnector, string strExtension, string strLastParameter,
            PaginationMarkType markType, int markCount,
            bool showInvalid, bool showDataCount, bool showPageCount, bool showDataStat)
        {
            this.dataCount = dataCount;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.pageStart = pageStart;
            this.targetUrl = strTargetUrl;
            this.parameter = strParameter;
            this.textStyle = textStyle;
            this.pageName = pageName;
            this.isUrlRewrite = isUrlRewrite;
            this.connector = strConnector;
            this.extension = strExtension;
            this.lastParameter = strLastParameter;
            this.markType = markType;
            this.markCount = markCount;
            this.showInvalid = showInvalid;
            this.showDataCount = showDataCount;
            this.showPageCount = showPageCount;
            this.showDataStat = showDataStat;
        }

        public PaginationParam(int dataCount, int pageSize, int pageIndex, int pageStart, string strTargetUrl,
            string strParameter, PaginationTextStyle textStyle, string pageName,
            bool isUrlRewrite, string strConnector, string strExtension, string strLastParameter,
            PaginationMarkType markType, int markCount,
            bool showInvalid, bool showDataCount, bool showPageCount, bool showDataStat,
            bool showFirstLastTextMark, bool showEllipsis)
        {
            this.dataCount = dataCount;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.pageStart = pageStart;
            this.targetUrl = strTargetUrl;
            this.parameter = strParameter;
            this.textStyle = textStyle;
            this.pageName = pageName;
            this.isUrlRewrite = isUrlRewrite;
            this.connector = strConnector;
            this.extension = strExtension;
            this.lastParameter = strLastParameter;
            this.markType = markType;
            this.markCount = markCount;
            this.showInvalid = showInvalid;
            this.showDataCount = showDataCount;
            this.showPageCount = showPageCount;
            this.showDataStat = showDataStat;
            this.ShowFirstLastTextMark = showFirstLastTextMark;
            this.ShowEllipsis = showEllipsis;
        }

        public PaginationMarkType ParseMarkType(string strMarkType)
        {
            PaginationMarkType type = PaginationMarkType.Symbol;
            switch (strMarkType)
            {
                case "Symbol":
                    type = PaginationMarkType.Symbol;
                    break;
                case "Chinese":
                    type = PaginationMarkType.Chinese;
                    break;
                case "Enblish":
                    type = PaginationMarkType.Enblish;
                    break;
            }
            return type;
        }

        public PaginationMarkType ParseMarkType(int markType)
        {
            PaginationMarkType type = PaginationMarkType.Symbol;
            switch (markType)
            {
                case 1:
                    type = PaginationMarkType.Symbol;
                    break;
                case 2:
                    type = PaginationMarkType.Chinese;
                    break;
                case 3:
                    type = PaginationMarkType.Enblish;
                    break;
            }
            return type;
        }
    }
    #endregion

    #region  文本标签类
    public class PaginationTextMark
    {
        private string strFirst = "&lt;&lt;";
        public string First
        {
            get { return this.strFirst; }
            set { this.strFirst = value; }
        }

        private string strPrevious = "&lt;";
        public string Previous
        {
            get { return this.strPrevious; }
            set { this.strPrevious = value; }
        }

        private string strNext = "&gt;";
        public string Next
        {
            get { return this.strNext; }
            set { this.strNext = value; }
        }

        private string strLast = "&gt;&gt;";
        public string Last
        {
            get { return this.strLast; }
            set { this.strLast = value; }
        }

        public PaginationTextMark() { }

        public PaginationTextMark(string strFirst, string strPrevious, string strNext, string strLast)
        {
            this.strFirst = strFirst;
            this.strPrevious = strPrevious;
            this.strNext = strNext;
            this.strLast = strLast;
        }
    }
    #endregion

}