using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Common
{
    public class GpsLatLng
    {

        #region  度分秒转换为经纬度
        /// <summary>
        /// 度分秒转换为经纬度
        /// </summary>
        /// <param name="degree">度数 绝对值应小于180</param>
        /// <param name="minute">分数 绝对值应小于60</param>
        /// <param name="second">秒数 绝对值应小于60</param>
        /// <param name="decimalDigitsLen">小数位数长度</param>
        /// <returns>返回经纬度值，若返回-1 表示参数错误</returns>
        public static double DMSConvertLatLng(double degree, double minute, double second, int decimalDigitsLen)
        {
            if (Math.Abs(degree) > 180 || Math.Abs(minute) > 60 || Math.Abs(second) > 60)
            {
                return -1;
            }
            double LatLng = degree + minute / 60 + second / 3600;
            return decimalDigitsLen >= 0 ? Math.Round(LatLng, decimalDigitsLen, MidpointRounding.AwayFromZero) : LatLng;
        }

        /// <summary>
        /// 度分秒转换为经纬度
        /// </summary>
        /// <param name="strDMS">度分秒文本值</param>
        /// <param name="decimalDigitsLen">小数位数长度</param>
        /// <returns>返回经纬度值，若返回-1 表示参数错误</returns>
        public static double DMSConvertLatLng(string strDMS, int decimalDigitsLen)
        {
            string[] delimeter = { "°", "′", "″", "'", "\"" };
            string[] arrDms = strDMS.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
            double degree = 0;
            double minute = 0;
            double second = 0;
            int i = 0;
            Regex reg = new Regex(@"^\-?[0-9]+(.[0-9]+)?$");

            foreach (string num in arrDms)
            {
                if (i > 3) break;
                switch (i)
                {
                    case 0:
                        degree = reg.IsMatch(num) ? Convert.ToDouble(num) : 0;
                        break;
                    case 1:
                        minute = reg.IsMatch(num) ? Convert.ToDouble(num) : 0;
                        break;
                    case 2:
                        second = reg.IsMatch(num) ? Convert.ToDouble(num) : 0;
                        break;
                    case 3:
                        if (reg.IsMatch(num))
                        {
                            second = Convert.ToDouble(second + "." + num.PadLeft(3, '0'));
                        }
                        break;
                }
                i++;
            }
            return DMSConvertLatLng(degree, minute, second, decimalDigitsLen);
        }
        #endregion

        #region  经纬度转换为度分秒
        /// <summary>
        /// 经纬度转换为度分秒
        /// </summary>
        /// <param name="latLng">经纬度值</param>
        /// <returns>返回度分秒，若参数错误，则返回度分秒全为-1</returns>
        public static double[] LatLngConvertDMS(double latLng)
        {
            if (Math.Abs(latLng) > 180)
            {
                return new double[3] { -1, -1, -1 };
            }
            double[] dms = new double[3] { 0, 0, 0 };
            //取整为度数
            dms[0] = Math.Floor(latLng);
            //小数部分乘60得到总分数值
            double m = (latLng - dms[0]) * 60;
            //分数值取整
            dms[1] = Math.Floor(m);
            //总分数值小数部分乘60即为秒数值
            dms[2] = (m - dms[1]) * 60;

            return dms;
        }

        /// <summary>
        /// 经纬度转换为度分秒小数
        /// </summary>
        /// <param name="latLng">经度或纬度值</param>
        /// <returns></returns>
        public static double LatLngConvertDMSDecimal(double latLng)
        {
            return ArrayToDecimal(LatLngConvertDMS(latLng));
        }

        public static double LatLngConvertDMSDecimal(string latLng)
        {
            return ArrayToDecimal(LatLngConvertDMS(latLng));
        }

        private static double ArrayToDecimal(double[] dms)
        {
            return (dms[0] + dms[1] / 100 + dms[2] / 100 / 60);
        }

        public static double DMSDecimalConvertLatLng(double dms)
        {
            double g1 = Math.Floor(dms);
            double g2 = ((dms % 100.0d) / 60.0d);
            double latlng = g1 + g2;
            return latlng;
        }

        private string ConvertGps(string gps)
        {
            double g = Convert.ToDouble(gps);
            double g1 = Math.Floor(g / (double)100.0);
            double g2 = ((g % (double)100.0) / (double)60.0);
            double dst = g1 + g2;
            return dst.ToString();
        }


        /// <summary>
        /// 经纬度转换为度分秒
        /// </summary>
        /// <param name="strLatLng">经纬度值文本</param>
        /// <returns>返回度分秒，若参数错误，则返回度分秒全为-1</returns>
        public static double[] LatLngConvertDMS(string strLatLng)
        {
            Regex reg = new Regex(@"^\-?[0-9]+(.[0-9]+)?$");

            if (!strLatLng.Equals(string.Empty) && reg.IsMatch(strLatLng))
            {
                return LatLngConvertDMS(Convert.ToDouble(strLatLng));
            }
            return new double[3] { -1, -1, -1 };
        }

        /// <summary>
        /// 度分秒文本显示
        /// </summary>
        /// <param name="degree">度</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <returns>返回度分秒文本格式</returns>
        public static string DMSToString(double degree, double minute, double second)
        {
            return String.Format("{0}°{1}′{2}″", degree, minute, second);
        }

        /// <summary>
        /// 度分秒文本显示
        /// </summary>
        /// <param name="dms">度分秒</param>
        /// <returns>返回度分秒文本格式</returns>
        public static string DMSToString(double[] dms)
        {
            string[] arrSymbol = new string[] { "°", "′", "″" };
            StringBuilder strDms = new StringBuilder();
            for (int i = 0, c = dms.Length; i < c; i++)
            {
                if (i > 2) break;
                strDms.Append(String.Format("{0}{1}", dms[i], arrSymbol[i]));
            }
            return strDms.ToString();
        }
        #endregion

        #region  经纬度转换为整数
        /// <summary>
        /// 经纬度转换为整数
        /// </summary>
        /// <param name="latlng">经纬度值</param>
        /// <returns>返回经纬度值*360000的整数值</returns>
        public static double LatLngConvertInteger(double latlng)
        {
            return Math.Round(latlng * 3600 * 100, 0, MidpointRounding.AwayFromZero);
        }
        #endregion

        #region  整数转换为经纬度
        /// <summary>
        /// 整数转换为经纬度
        /// </summary>
        /// <param name="number">经纬度整数值</param>
        /// <returns>返回整数值/360000的经纬度值</returns>
        public static double IntegerConvertLatLng(int number)
        {
            return (double)number / 3600 / 100;
        }
        /// <summary>
        /// 整数转换为经纬度
        /// </summary>
        /// <param name="number">经纬度整数值</param>
        /// <param name="decimalDigitsLen">小数位数,若参数为-1表示不进行四舍五入</param>
        /// <returns>返回整数值/360000的经纬度值</returns>
        public static double IntegerConvertLatLng(int number, int decimalDigitsLen)
        {
            double latLng = (double)number / 3600 / 100;
            return decimalDigitsLen >= 0 ? Math.Round(latLng, decimalDigitsLen, MidpointRounding.AwayFromZero) : latLng;
        }
        #endregion

    }
}