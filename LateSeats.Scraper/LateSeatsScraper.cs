using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace LateSeats.Scraper
{
    public class LateSeatsScraper
    {
        public IList<Flight> Scrape(string url)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();

            doc.Load(url);
            
            var flights = ParseHtml(doc);

            return flights.ToList();
        }

        private static IEnumerable<Flight> ParseHtml(HtmlDocument doc)
        {
            var element = doc.DocumentNode.SelectNodes("//table[@class='resultTable dealsResults']/tbody//tr[position()>1]");

            var flights = from row in element
                          where row.HasChildNodes
                          let departure = row.ChildNodes[1].InnerText
                          let destination = row.ChildNodes[3].InnerText
                          let departs = row.ChildNodes[5].SelectSingleNode("ul/li").InnerText
                          let seats =
                              row.SelectSingleNode("td[@class='seatsLeft']").ChildNodes.Count > 2
                                  ? row.SelectSingleNode("td[@class='seatsLeft']/div").InnerText
                                  : "0"
                          select new Flight
                              {
                                  DepartureAirport = departure,
                                  Destination = destination,
                                  DepartsOn = DateTime.Parse(departs),
                                  SeatsLeft = IntHelper.ToInt32(seats)
                              };
            return flights;
        }

        public IList<Flight> ScrapeResponse(HttpWebResponse url)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();

            doc.Load(url.GetResponseStream());

            var flights = ParseHtml(doc);

            return flights.ToList();
        }
    }

    public class IntHelper
    {
        public static int ToInt32(string seats)
        {
            try
            {
                return Convert.ToInt32(seats);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}