using System;
using System.Collections.Generic;
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

            foreach (var query in new ThomsonQueryBuilder().GetQueries(DateTime.Now, 7))
            {
                var webRequest =
                WebRequest.Create(
                    "http://flights.thomson.co.uk/thomson/en-GB/farefinder/default?pageIndex=4&originAirportCode=MAN&destinationAirportCode=anydest&duration=0&flexDate=0&" + query.ToString());

                var response = webRequest.GetResponse();
                var flights = scraper.Scrape(response.GetResponseStream());

                foreach (var flight in flights)
                {
                    Console.WriteLine(flight.DepartureAirport.Name + ", " + flight.ArrivalAirport.Name + ", " + flight.ArrivalDate + ", " + flight.DepartureDate + ", " + flight.SeatsLeft + ", " + flight.DepartureAirport.Code + ", " + flight.ArrivalAirport.Code);
                }
            }
            

            Console.ReadKey();
        }
    }
}
