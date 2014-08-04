using System.Collections.Generic;

namespace LateSeats.Scraper
{
    public class ScrapeResult
    {
        public string NextUrl { get; set; }

        public IList<Flight> Flights { get; set; }
    }
}