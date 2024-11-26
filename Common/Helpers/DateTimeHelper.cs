﻿namespace Common.Helpers
{
    public static class DateTimeHelper
    {
        public static string LocalDate(DateTime dateTime)
        {
            var azerbaijaniTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time");
            var azerbaijaniTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, azerbaijaniTimeZone);

            return azerbaijaniTime.ToString("dd.MM.yyyy HH:mm");
        }
    }
}
