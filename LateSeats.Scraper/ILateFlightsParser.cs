using System.Collections.Generic;
using HtmlAgilityPack;

namespace LateSeats.Scraper
{
    public interface ILateFlightsParser
    {
        IList<Flight> ParseFlights(HtmlNode documentNode);

        List<Airport> ParseDepartureAirports(HtmlNode documentNode);

        string ParseNextUrl(HtmlNode documentNode);
    }
}