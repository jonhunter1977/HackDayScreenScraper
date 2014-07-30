using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LateSeats.Scraper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var scraper = new LateSeatsScraper();
            var pagesScraped = 0;
            foreach (var query in new ThomsonQueryBuilder().GetQueries(DateTime.Now, 30))
            {
                var url = "http://flights.thomson.co.uk/thomson/en-GB/farefinder/default?pageIndex=1&originAirportCode=MAN&destinationAirportCode=anydest&duration=0&flexDate=0&" + query;

                while (url != null)
                {
                    var webRequest = WebRequest.Create(url.Replace("&amp;", "&"));

                    var response = webRequest.GetResponse();

                    var scrapeResult = scraper.Scrape(response.GetResponseStream());
                    pagesScraped++;

                    if (scrapeResult.NextUrl == null)
                        url = null;
                    else
                        url = "http://flights.thomson.co.uk" + scrapeResult.NextUrl;

                    foreach (var flight in scrapeResult.Flights)
                    {
                        Console.WriteLine(flight.DepartureAirport.Name + ", " + flight.ArrivalAirport.Name + ", " +
                                          flight.ArrivalDate + ", " + flight.DepartureDate + ", " + flight.SeatsLeft +
                                          ", " + flight.DepartureAirport.Code + ", " + flight.ArrivalAirport.Code);
                    }

                    File.AppendAllText("C://urls.txt", url);
                }
            }

            Console.WriteLine(pagesScraped + " pages scraped");
            Console.ReadKey();
        }
    }
}
