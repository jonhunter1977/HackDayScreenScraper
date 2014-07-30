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
                          let departureAirport = row.ChildNodes[1].InnerText
                          let destination = row.ChildNodes[3].InnerText
                          let departureDate = row.ChildNodes[5].SelectSingleNode("ul/li").InnerText
                          let returnDate = row.ChildNodes[5].SelectSingleNode("ul/li[position()>1]").InnerText
                          let departFlightTime = row.ChildNodes[7].SelectSingleNode("ul/li/ul/li").InnerText
                          let returnFlightTime = row.ChildNodes[7].SelectSingleNode("ul/li/ul/li[position()>1]").InnerText
                          let noOfNights = row.ChildNodes[9].InnerText
                          let seats =
                              row.SelectSingleNode("td[@class='seatsLeft']").ChildNodes.Count > 2
                                  ? row.SelectSingleNode("td[@class='seatsLeft']/div").InnerText
                                  : "0"
                          select new Flight
                              {
                                  DepartureAirport = departureAirport,
                                  Destination = destination,
                                  DepartsOn = DateTime.Parse(departureDate + " " + departFlightTime + ":00"),
                                  SeatsLeft = IntHelper.ToInt32(seats),
                                  ReturnsOn = DateTime.Parse(returnDate + " " + returnFlightTime + ":00"),
                                  NoOfNights = IntHelper.ToInt32(noOfNights)
                              };
            return flights;
        }

        public IList<Flight> ScrapeResponse(HttpWebResponse url)
        {
            var doc = new HtmlDocument();

            doc.Load(url.GetResponseStream());

            var flights = ParseHtml(doc);

            foreach (var flight in flights)
            {
                Console.WriteLine(flight.DepartureAirport + ", " + flight.Destination + ", " + flight.DepartsOn + ", " + flight.ReturnsOn + ", " + flight.SeatsLeft);
            }
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