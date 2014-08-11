using System.Collections.Generic;
using System.IO;

namespace LateSeats.Scraper
{
    public interface ILateFlightsScraper
    {
        ScrapeResult Scrape(Stream stream);
        IList<string> ScrapeDepartureAirports(Stream stream);
    }
}