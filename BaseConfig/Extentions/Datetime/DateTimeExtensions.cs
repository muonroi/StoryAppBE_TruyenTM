namespace BaseConfig.Extentions.Datetime
{
    public static class DateTimeExtensions
    {
        public static bool IsTheSameDate(this DateTime srcDate, DateTime desDate)
        {
            return srcDate.Year == desDate.Year && srcDate.Month == desDate.Month && srcDate.Day == desDate.Day;
        }

        public static DateTime TimeStampToDate(double timeStamp)
        {
            timeStamp = timeStamp < 0.0 ? 0.0 : timeStamp;
            return TimeStampToDateTime(timeStamp).Date;
        }

        public static DateTime TimeStampToDateTime(double timeStamp)
        {
            timeStamp = timeStamp < 0.0 ? 0.0 : timeStamp;
            return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timeStamp)).DateTime;
        }

        public static double GetTimeStamp(this DateTime dateTime, bool includedTimeValue = false)
        {
            return Math.Round((includedTimeValue ? dateTime : dateTime.Date).Subtract(new DateTime(1970, 1, 1)).TotalSeconds, 0);
        }

        public static double GetFirstDayOfMonthTimeStamp(this DateTime dateTime, bool includeTimeValue = false)
        {
            DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, 1);
            return dateTime2.GetTimeStamp(includeTimeValue);
        }

        public static DateTimeOffset GetTimeZoneExpiryDate(DateTimeOffset dateTimeOffset, int zoneHour)
        {
            return new DateTimeOffset(dateTimeOffset.ToOffset(new TimeSpan(zoneHour, 0, 0)).Date, new TimeSpan(zoneHour, 0, 0));
        }

        public static bool GreaterThanWithoutDay(this DateTime dtFrom, DateTime dtTo)
        {
            if (dtFrom.Year > dtTo.Year)
            {
                return true;
            }

            if (dtFrom.Year == dtTo.Year)
            {
                return dtFrom.Month > dtTo.Month;
            }

            return false;
        }

        public static List<DateRangeInfo> GetDateRangeInfoByMonth(DateTime fromDateTime, DateTime toDateTime)
        {
            List<DateRangeInfo> list = new List<DateRangeInfo>();
            if (fromDateTime.Date == toDateTime.Date)
            {
                DateRangeInfo item = new DateRangeInfo
                {
                    FromDate = fromDateTime,
                    ToDate = toDateTime
                };
                list.Add(item);
                return list;
            }

            while (toDateTime.AddMonths(1).GreaterThanWithoutDay(fromDateTime))
            {
                DateRangeInfo item2 = new DateRangeInfo
                {
                    FromDate = !list.Any() ? fromDateTime : new DateTime(fromDateTime.Year, fromDateTime.Month, 1),
                    ToDate = toDateTime.GreaterThanWithoutDay(fromDateTime) ? new DateTime(fromDateTime.Year, fromDateTime.Month, DateTime.DaysInMonth(fromDateTime.Year, fromDateTime.Month)) : toDateTime
                };
                list.Add(item2);
                fromDateTime = fromDateTime.AddMonths(1);
            }

            return list;
        }

        public static int ConverTimestampToYearMonth(double timeStamp)
        {
            DateTime dateTime = TimeStampToDate(timeStamp);
            string text = dateTime.Month < 10 ? "0" + dateTime.Month : dateTime.Month.ToString() ?? "";
            string s = dateTime.Year + text;
            return int.Parse(s);
        }

        public static int ConverTimestampToYearMonthDay(double timeStamp)
        {
            DateTime dateTime = TimeStampToDate(timeStamp);
            string text = dateTime.Month < 10 ? "0" + dateTime.Month : dateTime.Month.ToString() ?? "";
            string text2 = dateTime.Day < 10 ? "0" + dateTime.Day : dateTime.Day.ToString() ?? "";
            string s = dateTime.Year + text + text2;
            return int.Parse(s);
        }
    }
}
