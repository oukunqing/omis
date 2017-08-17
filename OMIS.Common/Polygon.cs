using System;
using System.Collections.Generic;
using System.Text;

namespace OMIS.Common
{
    public class Polygon
    {
        public Polygon() { }
        /*
        private readonly Point[] _vertices;

        public Polygon(Point[] vertices)
        {
            _vertices = vertices;
        }

        public bool PointInPolygon(Point point)
        {
            return PointInPolygon(_vertices, point);
        }
        */

        #region  ���Ƿ��ڶ������
        /// <summary>
        /// �жϵ��Ƿ��ڶ������
        /// </summary>
        /// <param name="vertices">����θ�����</param>
        /// <param name="point">Ŀ���</param>
        /// <returns></returns>
        public static bool PointInPolygon(Point[] vertices, Point point)
        {
            int c = vertices.Length;
            int j = c - 1;
            bool oddNodes = false;

            for (int i = 0; i < c; i++)
            {
                if (vertices[i].Lat < point.Lat && vertices[j].Lat >= point.Lat || vertices[j].Lat < point.Lat && vertices[i].Lat >= point.Lat)
                {
                    if (vertices[i].Lng + (point.Lat - vertices[i].Lat) / (vertices[j].Lat - vertices[i].Lat) * (vertices[j].Lng - vertices[i].Lng) < point.Lng)
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }

            return oddNodes;
        }
        #endregion

        #region  ���Ƿ��ھ�����
        /// <summary>
        /// �жϵ��Ƿ��ھ�����
        /// </summary>
        /// <param name="vertices">���ε���������</param>
        /// <param name="point">Ŀ���</param>
        /// <returns></returns>
        public static bool PointInRectangle(Point[] vertices, Point point)
        {
            if (vertices[0].Lat < vertices[1].Lat && vertices[0].Lng > vertices[1].Lng)
            {
                return point.Lat >= vertices[0].Lat && point.Lat <= vertices[1].Lat && point.Lng >= vertices[1].Lng && point.Lng <= vertices[0].Lng;
            }
            else if (vertices[0].Lat < vertices[1].Lat && vertices[0].Lng < vertices[1].Lng)
            {
                return point.Lat >= vertices[0].Lat && point.Lat <= vertices[1].Lat && point.Lng >= vertices[0].Lng && point.Lng <= vertices[1].Lng;
            }
            else if (vertices[0].Lat > vertices[1].Lat && vertices[0].Lng > vertices[1].Lng)
            {
                return point.Lat >= vertices[1].Lat && point.Lat <= vertices[0].Lat && point.Lng >= vertices[1].Lng && point.Lng <= vertices[0].Lng;
            }
            else
            {
                return point.Lat >= vertices[1].Lat && point.Lat <= vertices[0].Lat && point.Lng >= vertices[0].Lng && point.Lng <= vertices[1].Lng;
            }
        }
        #endregion

        #region  ��������侭γ�����������������
        private const double EARTH_RADIUS = 6378137;

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// ��������侭γ�����꣨doubleֵ���������������룬��λ����
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 100) / 100;

            return s;
        }

        public static double GetDistance(Point point1, Point point2)
        {
            return GetDistance(point1.Lat, point1.Lng, point2.Lat, point2.Lng);
        }
        #endregion

        #region  ���Ƿ���Բ��
        /// <summary>
        /// ���Ƿ���Բ��
        /// </summary>
        /// <param name="point">Ŀ���</param>
        /// <param name="roundCenterPoint">Բ���ĵ�</param>
        /// <param name="r">Բ�İ뾶����λ����</param>
        /// <returns></returns>
        public static bool PointInRound(Point point, Point roundCenterPoint, double r)
        {
            //����֮��ľ���
            double distance = GetDistance(point.Lat, point.Lng, roundCenterPoint.Lat, roundCenterPoint.Lng);
            return distance < r;
        }
        #endregion

    }

    #region  ��
    public class Point
    {
        public double Lat = 0;
        public double Lng = 0;

        public Point(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }

        public Point(string strLat, string strLng)
        {
            this.Lat = Convert.ToDouble(strLat);
            this.Lng = Convert.ToDouble(strLng);
        }
    }
    #endregion

}