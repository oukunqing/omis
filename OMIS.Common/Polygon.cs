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

        #region  点是否在多边形内
        /// <summary>
        /// 判断点是否在多边形内
        /// </summary>
        /// <param name="vertices">多边形各顶点</param>
        /// <param name="point">目标点</param>
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

        #region  点是否在矩形内
        /// <summary>
        /// 判断点是否在矩形内
        /// </summary>
        /// <param name="vertices">矩形的两个顶点</param>
        /// <param name="point">目标点</param>
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

        #region  根据两点间经纬度坐标计算两点间距离
        private const double EARTH_RADIUS = 6378137;

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// 根据两点间经纬度坐标（double值），计算两点间距离，单位：米
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

        #region  点是否在圆内
        /// <summary>
        /// 点是否在圆内
        /// </summary>
        /// <param name="point">目标点</param>
        /// <param name="roundCenterPoint">圆中心点</param>
        /// <param name="r">圆的半径，单位：米</param>
        /// <returns></returns>
        public static bool PointInRound(Point point, Point roundCenterPoint, double r)
        {
            //两点之间的距离
            double distance = GetDistance(point.Lat, point.Lng, roundCenterPoint.Lat, roundCenterPoint.Lng);
            return distance < r;
        }
        #endregion

    }

    #region  点
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