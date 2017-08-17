using System;
using System.Collections.Generic;
using System.Text;

namespace OMIS.Common
{
    /// <summary>
    /// 地图纠偏
    /// </summary>
    public class MapCorrect
    {
        protected static int EARTH_RADIUS = 6378245;
        protected static double E_FACTOR = 0.00669342;
        protected static double E_RATE = 0.6667;
        public static double Million = Math.Pow(10, 8);

        protected static double X_PI = 3.14159265358979324 * 3000.0 / 180.0;

        public MapCorrect()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static double yj_sin2(double B)
        {
            double num = 0;
            if (B < 0)
            {
                B = -B;
                num = 1;
            }
            int D = Convert.ToInt32(B / (2 * Math.PI));
            double A = B - D * (2 * Math.PI);
            if (A > Math.PI)
            {
                A = A - Math.PI;
                if (num == 1) { num = 0; }
                else if (num == 0) { num = 1; }
            }
            B = A;
            double _ = B;
            double C = B;
            A = A * A;
            C = C * A;
            _ = _ - C * 0.166666666666667;
            C = C * A;
            _ = _ + C * 0.00833333333333333;
            C = C * A;
            _ = _ - C * 0.000198412698412698;
            C = C * A;
            _ = _ + C * 0.00000275573192239859;
            C = C * A;
            _ = _ - C * 2.50521083854417 * Math.Pow(10, -8);
            if (num == 1) { _ = -_; }
            return _;
        }

        public static double Transform_yj5(double num, double _)
        {
            double A = 300 + 1 * num + 2 * _ + 0.1 * num * num + 0.1 * num * _ + 0.1 * Math.Sqrt(Math.Sqrt(num * num));
            A += (20 * yj_sin2(6 * Math.PI * num) + 20 * yj_sin2(2 * Math.PI * num)) * E_RATE;
            A += (20 * yj_sin2(Math.PI * num) + 40 * yj_sin2(Math.PI * num / 3)) * E_RATE;
            A += (150 * yj_sin2(Math.PI * num / 12) + 300 * yj_sin2(Math.PI * num / 30)) * E_RATE;
            return A;
        }

        public static double Transform_yjy5(double num, double _)
        {
            double A = -100 + 2 * num + 3 * _ + 0.2 * _ * _ + 0.1 * num * _ + 0.2 * Math.Sqrt(Math.Sqrt(num * num));
            A += (20 * yj_sin2(6 * Math.PI * num) + 20 * yj_sin2(2 * Math.PI * num)) * E_RATE;
            A += (20 * yj_sin2(Math.PI * _) + 40 * yj_sin2(Math.PI * _ / 3)) * E_RATE;
            A += (160 * yj_sin2(Math.PI * _ / 12) + 320 * yj_sin2(Math.PI * _ / 30)) * E_RATE;
            return A;
        }

        public static double Transform_jy5(double num, double B)
        {
            double A = yj_sin2(num * Math.PI / 180);
            double _ = Math.Sqrt(1 - E_FACTOR * A * A);
            _ = (B * 180) / (EARTH_RADIUS / _ * Math.Cos(num * Math.PI / 180) * Math.PI);
            return _;
        }

        public static double Transform_jyj5(double A, double B)
        {
            double _ = yj_sin2(A * Math.PI / 180);
            double num = 1 - E_FACTOR * _ * _;
            double C = (EARTH_RADIUS * (1 - E_FACTOR)) / (num * Math.Sqrt(num));
            return (B * 180) / (C * Math.PI);
        }

        /// <summary>
        /// 将正确的经纬度转换成偏移的经纬度
        /// </summary>
        /// <param name="lng">经度值</param>
        /// <param name="lat">纬度值</param>
        /// <returns></returns>
        public static MapLatLng Offset(double lat, double lng)
        {
            MapLatLng F = new MapLatLng();
            double _ = Transform_yj5(lng - 105, lat - 35);
            double C = Transform_yjy5(lng - 105, lat - 35);
            F.Lat = (Math.Round((lat + Transform_jyj5(lat, C)) * Million)) / Million;
            F.Lng = (Math.Round((lng + Transform_jy5(lat, _)) * Million)) / Million;
            return F;
        }

        public static MapLatLng Revert(double lat, double lng)
        {
            MapLatLng E = Offset(lat, lng);
            double D = 0.00001;
            double _ = 1;
            double A = 0;
            MapLatLng F = new MapLatLng();
            F.Lat = Math.Round((lat - (E.Lat - lat)) * Million) / Million;
            F.Lng = Math.Round((lng - (E.Lng - lng)) * Million) / Million;
            while (_ > D)
            {
                A++;
                if (A > 1000) { break; }
                MapLatLng _latLng = Offset(F.Lat, F.Lng);
                double I = F.Lat - (_latLng.Lat - lat);
                double C = F.Lng - (_latLng.Lng - lng);
                MapLatLng H = Offset(I, C);
                _ = Math.Abs(_latLng.Lat - lat) + Math.Abs(_latLng.Lng - lng);
                F.Lat = (Math.Round(I * Million)) / Million;
                F.Lng = (Math.Round(C * Million)) / Million;
            }
            return F;
        }

        public static MapLatLng GpsToBmap(double lat, double lng)
        {
            double x = lng, y = lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * X_PI);
            var theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * X_PI);
            double bd_lng = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;

            return new MapLatLng(bd_lat, bd_lng);
        }

        public static MapLatLng BmapToGps(double bd_lat, double bd_lng)
        {
            double x = bd_lng - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * X_PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * X_PI);
            double lng = z * Math.Cos(theta);
            double lat = z * Math.Sin(theta);

            return new MapLatLng(lat, lng);
        }

    }

    public class MapLatLng
    {
        public double Lat { get; set; }
        public double Lng { get; set; }

        public MapLatLng() { }

        public MapLatLng(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }
    }
}