using System;
using System.Collections.Generic;
using LateFlights.Scraper;
using LateSeats.Scraper.Exceptions;

namespace LateSeats.Scraper.Runner
{
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

                    var formattedUrl = string.Format(flightUrl, departureAirport, query);
                    
                    Console.WriteLine("Scraping {0}", formattedUrl);

                    while (formattedUrl != null)
                    {
                        try
                        {
                            var scrapeResult = Scrape(scraper, requestFactory, formattedUrl);

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

            Console.WriteLine("Ran in " + (startTime - DateTime.Now).ToString("hh:mm:ss"));
        }

        private static string GetNextUrl(ScrapeResult scrapeResult)
        {
            return scrapeResult.NextUrl == null ? null : "http://flights.thomson.co.uk" + scrapeResult.NextUrl;
        }

        private static ScrapeResult Scrape(ILateFlightScraper scraper, IWebRequestFactory requestFactory, string url)
        {
            var webRequest = requestFactory.Create(url.Replace("&amp;", "&"));
            var response = webRequest.GetResponse();

            var scrapeResult = scraper.Scrape(response.GetResponseStream());

            return scrapeResult;
        }

        private static IEnumerable<string> ScrapeDepartureAirports(ILateFlightScraper scraper, IWebRequestFactory requestFactory)
        {
            return scraper.ScrapeDepartureAirports(requestFactory.Create("http://flights.thomson.co.uk/en/index.html").GetResponseStream());
        }
    }
}