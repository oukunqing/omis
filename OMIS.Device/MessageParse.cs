using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace OMIS.Device
{
    public class MessageParse: DataConvert
    {


        #region  属性

        public static JavaScriptSerializer js = new JavaScriptSerializer();

        /// <summary>
        /// 报头标识
        /// </summary>
        public static string MESSAGE_HEAD = "##";

        /// <summary>
        /// 内容长度标识字节长度
        /// </summary>
        public static int CONTENT_LENGTH = 4;

        /// <summary>
        /// CP开头
        /// </summary>
        public static string CP_HEAD = "CP=&&";
        /// <summary>
        /// CP开头字节长度
        /// </summary>
        public static int CP_HEAD_LENGTH = CP_HEAD.Length;
        /// <summary>
        /// CP结尾
        /// </summary>
        public static string CP_FOOT = "&&";
        /// <summary>
        /// CP结尾字节长度
        /// </summary>
        public static int CP_FOOT_LENGTH = CP_FOOT.Length;
        /// <summary>
        /// 提取CP内容的正则表达式
        /// </summary>
        public static Regex regCP = new Regex(String.Format("{0}(.)*?{1}", CP_HEAD, CP_FOOT));
        /// <summary>
        /// 上传数据命令码
        /// </summary>
        public static int UPLOAD_DATA_CN = 2011;

        #endregion

        #region  解析消息内容
        public static MessageInfo ParseMessage(string msg)
        {
            try
            {
                var len = msg.Length;
                int pos = msg.LastIndexOf(MESSAGE_HEAD);

                //判断是否存在报头标识，并验证消息内容长度
                if (pos >= 0 && len >= pos + CONTENT_LENGTH)
                {
                    int idx = pos + MESSAGE_HEAD.Length;

                    //获取内容长度
                    int conLen = ConvertValue(msg.Substring(idx, CONTENT_LENGTH), 0);

                    idx += CONTENT_LENGTH;

                    //判断内容长度，并判断消息内容长度
                    if (conLen > 0 && len >= idx + conLen + 4)
                    {
                        //获取消息正文内容
                        string con = msg.Substring(idx, conLen);
                        //计算CRC
                        string crc = CRC.ToCRC16(con);
                        //获取消息内容CRC
                        string _crc = msg.Substring(idx + conLen, 4);

                        //验证CRC校验码是否正确
                        if (crc.Equals(_crc))
                        {
                            //提取CP内容是否成功
                            bool isSuccess = false;
                            //提取到的CP内容
                            string CP = DistillContent(ref con, ref isSuccess);

                            if (isSuccess)
                            {
                                MessageInfo o = ConvertInfo(con);
                                if (o != null)
                                {
                                    o.CP = CP;

                                    return o;
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  提取CP内容
        public static string DistillContent(ref string msg, ref bool isSuccess)
        {
            Match m = regCP.Match(msg);
            if (m != null)
            {
                string val = m.Value;
                int len = val.Length;

                if (len >= (CP_HEAD_LENGTH + CP_FOOT_LENGTH))
                {
                    //提取到CP内容之后，将CP内容从源内容中移除
                    msg = msg.Replace(val, "");
                    
                    isSuccess = true;

                    return val.Substring(CP_HEAD_LENGTH, len - CP_HEAD_LENGTH - CP_FOOT_LENGTH);
                }
            }
            return msg;
        }
        #endregion

        #region  转换消息
        private static MessageInfo ConvertInfo(string con)
        {
            try
            {
                if (con.EndsWith(";"))
                {
                    con = con.Substring(0, con.Length - 1);
                }
                //将字符串转换成JSON字符串
                string json = String.Format("{{\"{0}\"}}", con.Replace("=", "\":\"").Replace(";", "\",\""));

                Dictionary<string, object> par = js.Deserialize<Dictionary<string, object>>(json);

                MessageInfo o = new MessageInfo();
                o.QN = ConvertValue(par, "QN", "");
                o.CN = ConvertValue(par, "CN", 0);
                o.MN = ConvertValue(par, "MN", "");
                o.PW = ConvertValue(par, "PW", "");
                o.ST = ConvertValue(par, "ST", 0);
                o.CmdFlag = ConvertValue(par, "CmdFlag", 1);

                return o;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion


        #region  解析消息正文内容(仅解析指令中的内容，不包括上传的传感器数据)
        public static List<ContentInfo> ParseContent(string cp)
        {
            return ParseContent(cp, ";");
        }
        public static List<ContentInfo> ParseContent(string cp, string separator)
        {
            List<ContentInfo> list = new List<ContentInfo>();
            if (cp.Equals(string.Empty))
            {
                list.Add(new ContentInfo("Con", ""));
            }
            else
            {
                string[] arr = cp.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in arr)
                {
                    if (s.IndexOf('=') >= 0)
                    {
                        string[] tmp = s.Split('=');
                        list.Add(new ContentInfo(tmp[0], tmp[1]));
                    }
                    else
                    {
                        list.Add(new ContentInfo("Con", s));
                    }
                }
            }
            return list;
        }
        #endregion

        #region  提取指定的内容
        public static string DistillContentValue(List<ContentInfo> list, string code)
        {
            foreach (ContentInfo co in list)
            {
                if (co.Code.Equals(code))
                {
                    return co.Val;
                }
            }
            return null;
        }
        #endregion



        #region  组装报文消息
        public static string BuildMessage(string con)
        {
            return String.Format("##{0:D4}{1}{2}\r\n", con.Length, con, CRC.ToCRC16(con));
        }
        #endregion

        #region  组装注册回复消息报文
        public static string BuildRegistResponseMsg(string MN, string QN)
        {
            return BuildMessage(String.Format("ST=91;CN=9022;PW=12345;MN={0};CmdFlag=0;CP=&&QN={1};Logon=1&&", MN, QN));
        }

        public static string BuildHeartbeatResponseMsg(string MN, string QN)
        {
            return BuildMessage(String.Format("ST=91;CN=9022;PW=12345;MN={0};CmdFlag=0;CP=&&QN={1};Logon=1&&", MN, QN));
        }
        #endregion

        #region  组装校时消息报文
        public static string BuildTimeCalibratMsg(string MN)
        {
            string QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string CP = DateTime.Now.ToString("yyyyMMddHHmmss");

            return BuildMessage(String.Format("QN={0};ST=32;CN=1012;PW=123456;MN={1};CmdFlag=1;CP=&&{2}&&", QN, MN, CP));
        }
        #endregion

    }
}