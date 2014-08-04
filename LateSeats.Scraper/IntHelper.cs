using System;

namespace LateSeats.Scraper
{
    public class IntHelper
    {
        public static int ToInt32(string seats)
        {
            try
            {
                return Convert.ToInt32(seats);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}