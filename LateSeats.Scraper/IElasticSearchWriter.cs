using System.Collections.Generic;

namespace LateSeats.Scraper
{
    public interface IElasticSearchWriter
    {
        void Post(IEnumerable<Flight> flights, IWebRequestFactory webRequestFactory);
        void Post(IEnumerable<Airport> airports, IWebRequestFactory webRequestFactory);
    }
}