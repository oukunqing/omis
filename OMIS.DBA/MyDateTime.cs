using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.DBA
{
    public abstract class MyDateTime
    {
        public enum DT_TYPE_ENUM
        {
            Current = 0,
            Last = -1,
            Next = 1,
            Multiple = 2,
        }

        #region  DataTime ConvertToTimeStamp
        public static int ConvertToTimeStamp(DateTime dateTime)
        {
            try
            {
                //DateTime dtStart = new DateTime(1970, 1, 1, 8, 0, 0);
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return Convert.ToInt32((dateTime - dtStart).TotalSeconds);
            }
            catch (Exception ex) { throw (ex); }
        }
        public static int ConvertToTimeStamp(string dateTime)
        {
            try
            {
                return ConvertToTimeStamp(DateTime.Parse(dateTime));
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  TimeStamp ConvertToDateTime
        public static DateTime ConvertToDateTime(int timeStamp)
        {
            try
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long time = long.Parse(timeStamp + "0000000");
                return dtStart.Add(new TimeSpan(time));
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string DateTimeToString(DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format);
        }
        #endregion
        

        #region  检测日期时间格式
        private static bool CheckDateTimeFormat(string dateTime)
        {
            try
            {
                DateTime.Parse(dateTime);

                return true;
            }
            catch (Exception ex) { return false; }
        }
        #endregion

        #region  获得以周为单位周期的日期
        /// <summary>
        /// 获得以周为单位周期的日期
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="weeks">周的数量</param>
        /// <param name="weeksOffset">周偏移：0-本周，-1表示从上周开始，依此类推</param>
        /// <param name="weekDayOffset">周日期偏移：0-从周一开始</param>
        /// <param name="byDate">所在日期：根据此日期找到所在的周，若为空，则以当前时间来找</param>
        /// <param name="minDaysOffset">最小周日期：向后偏移</param>
        /// <param name="maxDaysOffset">最大周日期：向后偏移</param>
        /// <returns></returns>
        public static List<string> GetWeekDates(DT_TYPE_ENUM type, int weeks, int weeksOffset, int weekDayOffset, string byDate, int minDaysOffset, int maxDaysOffset)
        {
            try
            {
                List<string> dates = new List<string>();

                DateTime dtNow = CheckDateTimeFormat(byDate) ? DateTime.Parse(byDate) : DateTime.Now;

                //确定几天是星期几
                int wd = dtNow.DayOfWeek.GetHashCode();
                if (0 == wd)
                {
                    wd = 7;
                }
                int _weeks = 0;

                switch (type)
                {
                    case DT_TYPE_ENUM.Current:
                        weeksOffset = 0;
                        break;
                    case DT_TYPE_ENUM.Last:
                        weeksOffset = -1;
                        break;
                    case DT_TYPE_ENUM.Next:
                        weeksOffset = 1;
                        break;
                    default:
                        _weeks = weeks;
                        break;
                }
                //算出周一
                dtNow = dtNow.AddDays(1 - wd + (7 * weeksOffset));
                //从星期几开始
                if (weekDayOffset > 0)
                {
                    weekDayOffset -= 1;
                }

                dates.Add(dtNow.AddDays(weekDayOffset).AddDays(minDaysOffset).ToString("yyyy-MM-dd"));
                dates.Add(dtNow.AddDays(7 + (7 * _weeks) - 1 + weekDayOffset).AddDays(maxDaysOffset).ToString("yyyy-MM-dd"));

                return dates;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得以月为单位周期的日期
        public static List<string> GetMonthDates(DT_TYPE_ENUM type, int months, int monthsOffset, int monthDayOffset, string byDate)
        {
            try
            {
                List<string> dates = new List<string>();

                DateTime dtNow = CheckDateTimeFormat(byDate) ? DateTime.Parse(byDate) : DateTime.Now;

                int monthDays = 0;
                int _months = 0;

                switch (type)
                {
                    case DT_TYPE_ENUM.Current:
                        monthsOffset = 0;
                        break;
                    case DT_TYPE_ENUM.Last:
                        monthsOffset = -1;
                        break;
                    case DT_TYPE_ENUM.Next:
                        monthsOffset = 1;
                        break;
                    default:
                        _months = months;
                        break;
                }
                if (monthsOffset != 0)
                {
                    dtNow = dtNow.AddMonths(monthsOffset);
                }

                if (monthDayOffset > 0)
                {
                    dtNow = DateTime.Parse(dtNow.ToString("yyyy-MM-01"));

                    //获取起始月份天数
                    monthDays = GetMonthDays(dtNow.Year, dtNow.Month);
                    if (monthDayOffset > monthDays)
                    {
                        monthDayOffset = monthDays;
                    }
                    dtNow = dtNow.AddDays(monthDayOffset);
                    dates.Add(dtNow.ToString("yyyy-MM-dd"));

                    dtNow = dtNow.AddDays(-1).AddMonths(_months + 1);
                    dates.Add(dtNow.ToString("yyyy-MM-dd"));
                }
                else
                {
                    dates.Add(dtNow.ToString("yyyy-MM-01"));

                    dtNow = dtNow.AddMonths(_months);
                    //获取月份天数
                    monthDays = GetMonthDays(dtNow.Year, dtNow.Month);

                    dates.Add(dtNow.ToString(String.Format("yyyy-MM-{0:D2}", monthDays)));
                }
                return dates;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得以年为单位周期的日期
        public static List<string> GetYearDates(DT_TYPE_ENUM type, int years, int yearsOffset, int yearDayOffset, string byDate)
        {
            try
            {
                List<string> dates = new List<string>();

                DateTime dtNow = CheckDateTimeFormat(byDate) ? DateTime.Parse(byDate) : DateTime.Now;

                int _years = 0;

                switch (type)
                {
                    case DT_TYPE_ENUM.Current:
                        yearsOffset = 0;
                        break;
                    case DT_TYPE_ENUM.Last:
                        yearsOffset = -1;
                        break;
                    case DT_TYPE_ENUM.Next:
                        yearsOffset = 1;
                        break;
                    default:
                        _years = years;
                        break;
                }
                if (yearsOffset != 0)
                {
                    dtNow = dtNow.AddYears(yearsOffset);
                }

                if (yearDayOffset > 0)
                {
                    dtNow = DateTime.Parse(dtNow.ToString("yyyy-01-01"));

                    dtNow = dtNow.AddDays(yearDayOffset);
                    dates.Add(dtNow.ToString("yyyy-MM-dd"));

                    dtNow = dtNow.AddDays(-1).AddYears(_years + 1);
                    dates.Add(dtNow.ToString("yyyy-MM-dd"));
                }
                else
                {
                    dates.Add(dtNow.ToString("yyyy-01-01"));
                    dates.Add(dtNow.AddYears(_years).ToString("yyyy-12-31"));
                }

                return dates;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  获得月份天数
        public static int GetMonthDays(DateTime dt)
        {
            return GetMonthDays(dt.Year, dt.Month);
        }

        public static int GetMonthDays(int year, int month)
        {
            int days = 31;

            if (4 == month || 6 == month || 9 == month || 11 == month)
            {
                days = 30;
            }
            else if (2 == month)
            {
                bool isLeap = (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
                days = isLeap ? 29 : 28;
            }

            return days;
        }
        #endregion


        #region  获得一年中所有周的日期
        public static List<List<string>> GetWeekDatesOfYear(int year)
        {
            List<List<string>> lstWeekDate = new List<List<string>>();

            bool isLeap = (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
            //获得一年的总天数
            int totalDays = isLeap ? 366 : 365;
            //第一天
            DateTime dtFirst = DateTime.Parse(String.Format("{0}-01-01", year));
            //确定第一天为星期几
            int weekDay = dtFirst.DayOfWeek.GetHashCode();
            //一年总共几周，53
            int weeks = totalDays / 7 + 1;
            int rd = 0;
            int md = 0;
            int weekidx = 0;

            List<string> dates = new List<string>();

            if (weekDay != 1)
            {
                rd = 7 - weekDay;
                md = 1;
                weekidx++;

                dates.Add(dtFirst.ToString("yyyy-MM-dd"));
                dates.Add(dtFirst.AddDays(rd).ToString("yyyy-MM-dd"));

                lstWeekDate.Add(dates);
                rd += 1;
            }

            for (int i = weekidx; i < weeks; i++)
            {
                dates = new List<string>();

                int days = (i - md) * 7 + rd;
                dates.Add(dtFirst.AddDays(days).ToString("yyyy-MM-dd"));

                days = (i - (md - 1)) * 7 + rd - 1;
                if (days >= totalDays)
                {
                    days = totalDays - 1;
                }
                dates.Add(dtFirst.AddDays(days).ToString("yyyy-MM-dd"));

                lstWeekDate.Add(dates);
            }

            return lstWeekDate;
        }
        #endregion


        #region  获得以季度为单位周期的日期(待续)
        public List<string> GetQuarterDate(string type, int year, int quarters, int startQuarters, int startQuarterDay, string startDate)
        {
            try
            {
                DateTime dtNow = DateTime.Now;

                List<string> dates = new List<string>();

                return dates;
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

    }
}