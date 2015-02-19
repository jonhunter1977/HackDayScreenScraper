using System;
using System.Collections.Generic;
using LateSeats.Scraper.Exceptions;
using System.Linq;

namespace LateSeats.Scraper.Runner
{
    public class LateSeatsRunner
    {
        private readonly IWebRequestFactory _requestFactory;
        private readonly ILateFlightsScraper _scraper;

        public LateSeatsRunner()
        {
            _requestFactory = new WebRequestFactory();
            _scraper = new ThomsonScraper(_requestFactory, new ThomsonHtmlParser(), new ElasticSearchWriter());
        }

        public void Run(string flightUrl)
        {
            var startTime = DateTime.Now;
            
            var pagesScraped = 0;
            var elasticSearchErrorEncountered = false;

            var departureAirports = ScrapeDepartureAirports();
            
            foreach (var departureAirport in departureAirports.Select(x => x.Code))
            {
                Console.WriteLine("Finding flights from {0}", departureAirport);
                foreach (var query in new ThomsonQueryBuilder().GetFlightQueries(DateTime.Now, 7))
                {
                    if (elasticSearchErrorEncountered)
                        break;

                    var formattedUrl = string.Format(flightUrl, departureAirport, query);
                    
                    Console.WriteLine("Scraping {0}", formattedUrl);

                    while (formattedUrl != null)
                    {
                        try
                        {
                            var scrapeResult = Scrape(formattedUrl);

                            formattedUrl = GetNextUrl(scrapeResult);

                            pagesScraped++;
                        }
                        catch (ElasticSearchException e)
                        {
                            Console.WriteLine(e.Message);
                            elasticSearchErrorEncountered = true;
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    pagesScraped = 0;
                }
            }

            Console.WriteLine(pagesScraped + " pages scraped");
            Console.WriteLine("Ran in " + (DateTime.Now - startTime));
        }

        private static string GetNextUrl(ScrapeResult scrapeResult)
        {
            return scrapeResult.NextUrl == null ? null : "http://flights.thomson.co.uk" + scrapeResult.NextUrl;
        }

        private ScrapeResult Scrape(string url)
        {
            var webRequest = _requestFactory.Create(url.Replace("&amp;", "&"));
            
            var response = webRequest.GetResponse();

            var scrapeResult = _scraper.Scrape(response.GetResponseStream());

            return scrapeResult;
        }

        private IEnumerable<Airport> ScrapeDepartureAirports()
        {
            return _scraper.ScrapeDepartureAirports(_requestFactory.Create("http://flights.thomson.co.uk/en/index.html").GetResponseStream());
        }
    }
}