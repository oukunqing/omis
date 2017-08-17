using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Device
{

    public class MessageInfo
    {
        
        public string QN { get; set; }

        public int ST { get; set; }

        public int CN { get; set; }

        public string PW { get; set; }

        public string MN { get; set; }

        public int CmdFlag { get; set; }

        public string CP { get; set; }

        public List<ContentInfo> ConList { get; set; }
        
        public MessageInfo()
        {
            this.ConList = new List<ContentInfo>();
        }

        public MessageInfo(string QN, int CN, string MN, string CP)
        {
            this.QN = QN.PadLeft(17, '0');
            this.ST = 32;
            this.CN = CN;
            this.PW = "123456";
            this.MN = MN;
            this.CP = CP;
            this.CmdFlag = 1;

            this.ConList = new List<ContentInfo>();
        }

    }

    public class ContentInfo
    {
        public string Code { get; set; }

        public string Val { get; set; }

        public ContentInfo() { }

        public ContentInfo(string code, string val)
        {
            this.Code = code;
            this.Val = val;
        }

    }

}