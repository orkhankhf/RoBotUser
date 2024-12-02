namespace Common.Helpers
{
    public static class DateTimeHelper
    {
        public static string LocalDate(DateTime dateTime)
        {
            var azerbaijaniTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time");
            var azerbaijaniTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, azerbaijaniTimeZone);

            return azerbaijaniTime.ToString("dd.MM.yyyy HH:mm");
        }

        public static bool IsRestrictedTimeToSendMessage()
        {
            // Convert UTC time to Azerbaijan time
            var azerbaijanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time");
            var azerbaijanTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, azerbaijanTimeZone);

            // Define restricted time range
            var restrictedStart = new TimeSpan(21, 0, 0); // 21:00
            var restrictedEnd = new TimeSpan(8, 30, 0);  // 08:30

            // Check if current time falls within the restricted range
            return azerbaijanTime.TimeOfDay >= restrictedStart || azerbaijanTime.TimeOfDay <= restrictedEnd;
        }
    }
}
