using System.Globalization;

namespace BaseConfig.Extentions.Datetime
{
    public static class CheckDateTime
    {
        public static bool IsValidDateTime(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return false;
            string someDateToCheck = "12/12/9999";
            if (DateTime.TryParseExact(someDateToCheck, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return true;
            }
            return false;
        }
    }
}
