using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace LateSeats.Scraper
{
    public class ThomsonHtmlParser : ILateFlightsParser
    {
        public IList<Flight> ParseFlights(HtmlNode documentNode)
        {
            var element = documentNode.SelectNodes("//table[@class='resultTable dealsResults']/tbody//tr[position()>1]");

            IEnumerable<Flight> flights = new List<Flight>();

            if (element == null)
                return flights.ToList();

            flights = from row in element
                      where row.HasChildNodes
                      let departureAirport = row.ChildNodes[1].InnerText
                      let destination = row.ChildNodes[3].InnerText
                      let departureDate = row.ChildNodes[5].SelectSingleNode("ul/li").InnerText
                      let returnDate = row.ChildNodes[5].SelectSingleNode("ul/li[position()>1]").InnerText
                      let departFlightTime = row.ChildNodes[7].SelectSingleNode("ul/li/ul/li").InnerText
                      let returnFlightTime = row.ChildNodes[7].SelectSingleNode("ul/li[position()>1]/ul/li").InnerText
                      let noOfNights = row.ChildNodes[9].InnerText
                      let departureAirportCode = row.ChildNodes[13].SelectSingleNode("fieldset/input[@id='depAP']").GetAttributeValue("value", "N/a")
                      let arrivalAirportCode = row.ChildNodes[13].SelectSingleNode("fieldset/input[@id='retAP']").GetAttributeValue("value", "N/a")
                      let seats = row.SelectSingleNode("td[@class='seatsLeft']").ChildNodes.Count > 2 ? row.SelectSingleNode("td[@class='seatsLeft']/div").InnerText : "0"
                      select new Flight
                          {
                              DepartureAirport = new Airport { Code = departureAirportCode, Name = departureAirport },
                              ArrivalAirport = new Airport { Code = arrivalAirportCode, Name = destination },
                              ArrivalDate = (departureDate + " " + departFlightTime + ":00").ToFormattedDateString(),
                              SeatsLeft = seats.ToInt32(),
                              DepartureDate = (returnDate + " " + returnFlightTime + ":00").ToFormattedDateString(),
                              NoOfNights = noOfNights.ToInt32()
                          };

            return flights.ToList();
        }

        public List<Airport> ParseDepartureAirports(HtmlNode documentNode)
        {
            var element = documentNode.SelectNodes("//select[@id='depAP']/option[position()>1]");

            var airportCodes = from codeElement in element
                               let code = codeElement.GetAttributeValue("value", "")
                               let name = codeElement.NextSibling.InnerText.Trim()
                               where code != "none"
                               select new Airport() { Name = name, Code = code };

            return airportCodes.ToList();
        }

        public string ParseNextUrl(HtmlNode documentNode)
        {
            var pagers = documentNode.SelectNodes("//a[@class='qPager']");

            if (pagers == null)
                return null;

            var elements = (from pager in pagers
                            where pager.InnerText == "Next"
                            select pager).ToList();

            return !elements.Any() ? null : elements.ElementAt(0).GetAttributeValue("href", null);
        }
    }
}