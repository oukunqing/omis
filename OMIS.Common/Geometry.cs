using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Common
{
    /// <summary>
    /// Geometry 的摘要说明
    /// </summary>
    public class Geometry
    {
        public Geometry()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region  弧度转换角度
        public static double RadianConvertAngle(double radian)
        {
            return radian * 180 / Math.PI;
        }
        #endregion

        #region  角度转换弧度
        public static double AngleConvertRadian(double angle)
        {
            return angle * Math.PI / 180;
        }
        #endregion

    }
}