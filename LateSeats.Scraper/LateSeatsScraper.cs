using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using HtmlAgilityPack;

namespace LateSeats.Scraper
{
    public class LateSeatsScraper
    {
        private static IList<Flight> ParseFlights(HtmlDocument doc)
        {
            var element = doc.DocumentNode.SelectNodes("//table[@class='resultTable dealsResults']/tbody//tr[position()>1]");

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
                      let seats = row.SelectSingleNode("td[@class='seatsLeft']").ChildNodes.Count > 2
                              ? row.SelectSingleNode("td[@class='seatsLeft']/div").InnerText
                              : "0"
                      select new Flight
                          {
                              DepartureAirport = new Airport() { Code = departureAirportCode, Name = departureAirport },
                              ArrivalAirport = new Airport() { Code = arrivalAirportCode, Name = destination },
                              ArrivalDate = DateTime.Parse(departureDate + " " + departFlightTime + ":00").ToString("yyyy-MM-ddTHH:mm:ss"),
                              SeatsLeft = IntHelper.ToInt32(seats),
                              DepartureDate = DateTime.Parse(returnDate + " " + returnFlightTime + ":00").ToString("yyyy-MM-ddTHH:mm:ss"),
                              NoOfNights = IntHelper.ToInt32(noOfNights)
                          };

            return flights.ToList();
        }

        public ScrapeResult Scrape(Stream stream)
        {
            var scrapeResult = new ScrapeResult();
            var doc = new HtmlDocument();

            doc.Load(stream);

            scrapeResult.Flights = ParseFlights(doc);
            scrapeResult.NextUrl = ParseNextUrl(doc);

            PostToElasticSearch(scrapeResult.Flights);

            return scrapeResult;
        }

        private string ParseNextUrl(HtmlDocument doc)
        {
            var pagers = doc.DocumentNode.SelectNodes("//a[@class='qPager']");

            var elements = from pager in pagers
                           where pager.InnerText == "Next"
                           select pager;

            if (!elements.Any())
                return null;

            return elements.ElementAt(0).GetAttributeValue("href", null);
        }

        private static void PostToElasticSearch(IEnumerable<Flight> flights)
        {
            foreach (var flight in flights)
            {
                var jsonFlight = SerializeJSon(flight);

                var webRequest = WebRequest.Create("http://10.44.35.21:9200/lateseats/flight/" + flight.GenerateFlightId());
                webRequest.Method = "POST";

                using (var requestStream = webRequest.GetRequestStream())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(jsonFlight);
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    try
                    {
                        using (var response = webRequest.GetResponse())
                        {
                            response.Close();
                        }
                    }
                    catch (WebException e)
                    {

                    }
                }
            }
        }

        public static string SerializeJSon<T>(T t)
        {
            var stream = new MemoryStream();
            var ds = new DataContractJsonSerializer(typeof(T));
            var s = new DataContractJsonSerializerSettings();
            ds.WriteObject(stream, t);
            var jsonString = Encoding.UTF8.GetString(stream.ToArray());
            stream.Close();
            return jsonString;
        }
    }

    public class ScrapeResult
    {
        public string NextUrl { get; set; }

        public IList<Flight> Flights { get; set; }
    }

    public class WebRequestWrapper : IWebRequest
    {
        public string Method { get; set; }

        public string Url { get; set; }
        private WebRequest _webRequest;
        public Stream GetRequestStream()
        {
            if (_webRequest == null)
                _webRequest = WebRequest.Create(Url);
            return _webRequest.GetRequestStream();
        }

        public WebResponse GetResponse()
        {
            if (_webRequest == null)
                _webRequest = WebRequest.Create(Url);
            return _webRequest.GetResponse();
        }
    }

    public interface IWebRequest
    {
        string Method { get; set; }
        string Url { get; set; }
        Stream GetRequestStream();
        WebResponse GetResponse();
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