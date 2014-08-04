using System;
using System.Collections.Generic;
using LateFlights.Scraper;
using LateSeats.Scraper.Exceptions;

namespace LateSeats.Scraper.Runner
{
    public class Program
    {
        private const string FlightUrl = "http://flights.thomson.co.uk/thomson/en-GB/farefinder/default?pageIndex=1&originAirportCode={0}&destinationAirportCode=anydest&duration=0&flexDate=0&{1}";

        public static void Main(string[] args)
        {
            var runner = new LateSeatsRunner();

            runner.Run(FlightUrl);
            
            Console.ReadKey();
        }

        
    }

    public class LateSeatsRunner
    {
        public void Run(string flightUrl)
        {
            var startTime = DateTime.Now;
            var requestFactory = new WebRequestFactory();
            var scraper = new ThomsonScraper(requestFactory, new ThomsonHtmlParser(), new ElasticSearchWriter());
            var pagesScraped = 0;
            var elasticSearchErrorEncountered = false;

            foreach (var departureAirport in ScrapeDepartureAirports(scraper, requestFactory))
            {
                Console.WriteLine("Finding flights from {0}", departureAirport);
                foreach (var query in new ThomsonQueryBuilder().GetFlightQueries(DateTime.Now, 7))
                {
                    if (elasticSearchErrorEncountered)
                        break;

                    var url = string.Format(flightUrl, departureAirport, query);
                    Console.WriteLine("Scraping {0}", url);

                    while (url != null)
                    {
                        try
                        {
                            var scrapeResult = Scrape(scraper, requestFactory, url);

                            url = GetNextUrl(scrapeResult);

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

            Console.WriteLine("Ran in " + (startTime - DateTime.Now).ToString("hh:mm:ss"));
        }

        private static string GetNextUrl(ScrapeResult scrapeResult)
        {
            return scrapeResult.NextUrl == null ? null : "http://flights.thomson.co.uk" + scrapeResult.NextUrl;
        }

        private static ScrapeResult Scrape(ThomsonScraper scraper, IWebRequestFactory requestFactory, string url)
        {
            var webRequest = requestFactory.Create(url.Replace("&amp;", "&"));
            var response = webRequest.GetResponse();

            var scrapeResult = scraper.Scrape(response.GetResponseStream());

            return scrapeResult;
        }

        private static IEnumerable<string> ScrapeDepartureAirports(ThomsonScraper scraper, IWebRequestFactory requestFactory)
        {
            return scraper.ScrapeDepartureAirports(requestFactory.Create("http://flights.thomson.co.uk/en/index.html").GetResponseStream());
        }
    }
}
