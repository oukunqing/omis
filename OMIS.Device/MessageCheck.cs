using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Device
{
    public class MessageCheck
    {

        public static Regex regMsg = new Regex(@"^##[\d]{4}(.)+CP=&&(.)*?&&[\w]{4}$");

        #region  检测报文消息格式
        public static List<string> CheckMessageFormat(string msg, ref string stack)
        {
            try
            {
                List<string> list = new List<string>();

                if (msg.Length > 0)
                {
                    stack += msg;

                    bool isComplete = stack.EndsWith("\r\n");

                    string[] arr = stack.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //若报文不完整，最后一段内容先不处理
                    int c = arr.Length - (isComplete ? 0 : 1);

                    //若报文不完整，将最后一段内容缓存到消息栈中
                    stack = isComplete ? "" : arr[c];
                    
                    for (int i = 0; i < c; i++)
                    {
                        Match m = regMsg.Match(arr[i].Trim());
                        if (m != null && m.Value.Length > 0)
                        {
                            if (CheckMessageContent(m.Value))
                            {
                                list.Add(m.Value);
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  检测报文消息内容
        public static bool CheckMessageContent(string msg)
        {
            try
            {
                int c = msg.Length;
                int len = DataConvert.ConvertValue(msg.Substring(2, 4), 0);
                string con = msg.Substring(6, c - 6 - 4);
                string crc = msg.Substring(c - 4);

                return len == con.Length && crc.Equals(CRC.ToCRC16(con));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}