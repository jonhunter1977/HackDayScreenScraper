using System.Collections.Generic;
using System.IO;

namespace LateSeats.Scraper
{
    public interface ILateFlightsScraper
    {
        ScrapeResult Scrape(Stream stream);
        IList<Airport> ScrapeDepartureAirports(Stream stream);
    }
}