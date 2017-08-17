using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Common
{
    public class KilometerPost
    {

        public static string ConvertToKilometerPost(float kilometerPost)
        {
            string[] arr = kilometerPost.ToString().Split('.');
            string z = arr[0];
            string x = arr.Length >= 2 ? arr[1].PadLeft(3, '0') : "000";
            return String.Format("K{0}+{1}", z, x);
        }

        public static string ConvertToKilometerPost(string kilometerPost)
        {
            string strPattern = @"^[-+]?[0-9]+(\.[0-9]+)?$";
            Regex reg = new Regex(strPattern);
            if (!reg.IsMatch(kilometerPost))
            {
                return kilometerPost;
            }
            string[] arr = kilometerPost.ToString().Split('.');
            string z = arr[0];
            string x = arr.Length >= 2 ? arr[1].PadLeft(3, '0') : "000";
            return String.Format("K{0}+{1}", z, x);
        }
                
        public static float ConvertToKilometer(string kilometerPost)
        {
            string strPattern = @"^[-+]?[0-9]+(\.[0-9]+)?$";
            Regex reg = new Regex(strPattern);
            if (reg.IsMatch(kilometerPost))
            {
                return Convert.ToSingle(kilometerPost);
            }
            strPattern = @"^[K|k][0-9]+(\+([0-9]+)?)?$";
            reg = new Regex(strPattern);
            if (reg.IsMatch(kilometerPost))
            {
                return -1;
            }
            return Convert.ToSingle(new Regex("[K|k]").Replace(kilometerPost, "").Replace("+", "."));
        }

        public static bool IsKilometerPost(string kilometerPost)
        {
            string strPattern = @"^[K|k][0-9]+(\+([0-9]+)?)?$";
            Regex reg = new Regex(strPattern);
            if (reg.IsMatch(kilometerPost))
            {
                kilometerPost = ConvertToKilometer(kilometerPost).ToString();
            }
            strPattern = @"^[-+]?[0-9]+(\.[0-9]+)?$";
            reg = new Regex(strPattern);
            return reg.IsMatch(kilometerPost);
        }

    }
}