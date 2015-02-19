using System;

namespace LateSeats.Scraper.Runner
{
    public class Program
    {
        private const string FlightUrl = "http://flights.thomson.co.uk/thomson/en-GB/farefinder/default?pageIndex=1&originAirportCode={0}&destinationAirportCode=anydest&duration=0&flexDate=0&{1}";
        public static void Main(string[] args)
        {
            var runner = new LateSeatsRunner();

            runner.Run(FlightUrl);
        }
    }
}
