using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;

namespace LateSeats.Scraper
{
    public class ThomsonScraper : ILateFlightsScraper
    {
        private readonly IWebRequestFactory _webRequestFactory;
        private readonly ILateFlightsParser _lateFlightsParser;
        private readonly IElasticSearchWriter _elasticSearchWriter;

        public ThomsonScraper(IWebRequestFactory webRequestFactory, ILateFlightsParser lateFlightsParser, IElasticSearchWriter elasticSearchWriter)
        {
            _webRequestFactory = webRequestFactory;
            _lateFlightsParser = lateFlightsParser;
            _elasticSearchWriter = elasticSearchWriter;
        }

        public ScrapeResult Scrape(Stream stream)
        {
            var scrapeResult = new ScrapeResult();
            var doc = new HtmlDocument();

            doc.Load(stream);

            scrapeResult.Flights = _lateFlightsParser.ParseFlights(doc.DocumentNode);
            scrapeResult.NextUrl = _lateFlightsParser.ParseNextUrl(doc.DocumentNode);

            _elasticSearchWriter.Post(scrapeResult.Flights, _webRequestFactory);

            return scrapeResult;
        }

        public IList<Airport> ScrapeDepartureAirports(Stream stream)
        {
            var doc = new HtmlDocument();

            doc.Load(stream);

            var departureAirports = _lateFlightsParser.ParseDepartureAirports(doc.DocumentNode);

            _elasticSearchWriter.Post(departureAirports, _webRequestFactory);

            return departureAirports;
        }
    }
}