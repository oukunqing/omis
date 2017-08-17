using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace OMIS.Common
{
    public class HexBin
    {

        #region  提取HEX文件内容
        public static string DistillHexContent(string hex)
        {
            StringBuilder result = new StringBuilder();
            string[] delimiter = {"\r\n"};
            string[] arrHex = hex.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            Regex reg = new Regex(@"^:\w{7}0\w+$");
            Regex regEnd = new Regex(@"^:\w{7}1$");

            foreach (string str in arrHex)
            {
                if (regEnd.IsMatch(str))
                {
                    break;
                }
                if (!str.Equals(string.Empty))
                {
                    if (reg.IsMatch(str))
                    {
                        result.Append(str.Substring(9, str.Length - 11));
                    }
                }
            }
            return result.ToString();
        }
        #endregion

        #region  Hex转换成Bin  return byte[]
        public static byte[] HexConvertBin(string hex)
        {
            try
            {
                Regex reg = new Regex(@"^:\w+$");
                if (reg.Matches(hex).Count > 0)
                {
                    hex = DistillHexContent(hex);
                }
                Int32 i;
                Int32 j = 0;
                Int32 Length = hex.Length;
                byte[] bin = new byte[Length / 2];
                for (i = 0; i < Length; i += 2) //两字符合并成一个16进制字节
                {
                    bin[j++] = (byte)Int16.Parse(hex.Substring(i, 2), NumberStyles.HexNumber);
                }
                return bin;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}