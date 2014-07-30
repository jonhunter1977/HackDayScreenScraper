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

            var webRequest =
                WebRequest.Create(
                    "http://flights.thomson.co.uk/thomson/en-GB/farefinder/default?pageIndex=4&originAirportCode=MAN&destinationAirportCode=anydest&duration=0&flexDate=0&timeSpanStartDay=29&timeSpanStartYearMonth=2014-07&timeSpanEndDay=05&timeSpanEndYearMonth=2014-08");

            var response = webRequest.GetResponse();
            scraper.ScrapeResponse(response as HttpWebResponse);

            Console.ReadKey();
        }
    }
}
