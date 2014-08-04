using System;

namespace LateSeats.Scraper
{
    public static class Extensions
    {
        public static int ToInt32(this string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string ToFormattedDateString(this string date)
        {
            try
            {
                return DateTime.Parse(date).ToString("yyyy-MM-ddTHH:mm:ss"); 
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}