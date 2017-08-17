using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OMIS.Common
{
    public class GpsLatLng
    {

        #region  �ȷ���ת��Ϊ��γ��
        /// <summary>
        /// �ȷ���ת��Ϊ��γ��
        /// </summary>
        /// <param name="degree">���� ����ֵӦС��180</param>
        /// <param name="minute">���� ����ֵӦС��60</param>
        /// <param name="second">���� ����ֵӦС��60</param>
        /// <param name="decimalDigitsLen">С��λ������</param>
        /// <returns>���ؾ�γ��ֵ��������-1 ��ʾ��������</returns>
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
        /// �ȷ���ת��Ϊ��γ��
        /// </summary>
        /// <param name="strDMS">�ȷ����ı�ֵ</param>
        /// <param name="decimalDigitsLen">С��λ������</param>
        /// <returns>���ؾ�γ��ֵ��������-1 ��ʾ��������</returns>
        public static double DMSConvertLatLng(string strDMS, int decimalDigitsLen)
        {
            string[] delimeter = { "��", "��", "��", "'", "\"" };
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

        #region  ��γ��ת��Ϊ�ȷ���
        /// <summary>
        /// ��γ��ת��Ϊ�ȷ���
        /// </summary>
        /// <param name="latLng">��γ��ֵ</param>
        /// <returns>���ضȷ��룬�����������򷵻ضȷ���ȫΪ-1</returns>
        public static double[] LatLngConvertDMS(double latLng)
        {
            if (Math.Abs(latLng) > 180)
            {
                return new double[3] { -1, -1, -1 };
            }
            double[] dms = new double[3] { 0, 0, 0 };
            //ȡ��Ϊ����
            dms[0] = Math.Floor(latLng);
            //С�����ֳ�60�õ��ܷ���ֵ
            double m = (latLng - dms[0]) * 60;
            //����ֵȡ��
            dms[1] = Math.Floor(m);
            //�ܷ���ֵС�����ֳ�60��Ϊ����ֵ
            dms[2] = (m - dms[1]) * 60;

            return dms;
        }

        /// <summary>
        /// ��γ��ת��Ϊ�ȷ���С��
        /// </summary>
        /// <param name="latLng">���Ȼ�γ��ֵ</param>
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
        /// ��γ��ת��Ϊ�ȷ���
        /// </summary>
        /// <param name="strLatLng">��γ��ֵ�ı�</param>
        /// <returns>���ضȷ��룬�����������򷵻ضȷ���ȫΪ-1</returns>
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
        /// �ȷ����ı���ʾ
        /// </summary>
        /// <param name="degree">��</param>
        /// <param name="minute">��</param>
        /// <param name="second">��</param>
        /// <returns>���ضȷ����ı���ʽ</returns>
        public static string DMSToString(double degree, double minute, double second)
        {
            return String.Format("{0}��{1}��{2}��", degree, minute, second);
        }

        /// <summary>
        /// �ȷ����ı���ʾ
        /// </summary>
        /// <param name="dms">�ȷ���</param>
        /// <returns>���ضȷ����ı���ʽ</returns>
        public static string DMSToString(double[] dms)
        {
            string[] arrSymbol = new string[] { "��", "��", "��" };
            StringBuilder strDms = new StringBuilder();
            for (int i = 0, c = dms.Length; i < c; i++)
            {
                if (i > 2) break;
                strDms.Append(String.Format("{0}{1}", dms[i], arrSymbol[i]));
            }
            return strDms.ToString();
        }
        #endregion

        #region  ��γ��ת��Ϊ����
        /// <summary>
        /// ��γ��ת��Ϊ����
        /// </summary>
        /// <param name="latlng">��γ��ֵ</param>
        /// <returns>���ؾ�γ��ֵ*360000������ֵ</returns>
        public static double LatLngConvertInteger(double latlng)
        {
            return Math.Round(latlng * 3600 * 100, 0, MidpointRounding.AwayFromZero);
        }
        #endregion

        #region  ����ת��Ϊ��γ��
        /// <summary>
        /// ����ת��Ϊ��γ��
        /// </summary>
        /// <param name="number">��γ������ֵ</param>
        /// <returns>��������ֵ/360000�ľ�γ��ֵ</returns>
        public static double IntegerConvertLatLng(int number)
        {
            return (double)number / 3600 / 100;
        }
        /// <summary>
        /// ����ת��Ϊ��γ��
        /// </summary>
        /// <param name="number">��γ������ֵ</param>
        /// <param name="decimalDigitsLen">С��λ��,������Ϊ-1��ʾ��������������</param>
        /// <returns>��������ֵ/360000�ľ�γ��ֵ</returns>
        public static double IntegerConvertLatLng(int number, int decimalDigitsLen)
        {
            double latLng = (double)number / 3600 / 100;
            return decimalDigitsLen >= 0 ? Math.Round(latLng, decimalDigitsLen, MidpointRounding.AwayFromZero) : latLng;
        }
        #endregion

    }
}