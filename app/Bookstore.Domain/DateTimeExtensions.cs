namespace Bookstore.Domain
{
    public static class DateTimeExtensions
    {
        public static DateTime OneSecondToMidnight(this DateTime dateTime)
        {
            return dateTime.Date.AddSeconds(86399);
        }

        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day);
        }
    }
}
