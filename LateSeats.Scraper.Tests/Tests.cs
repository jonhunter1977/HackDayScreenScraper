using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LateSeats.Scraper.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Flight_Destination_Can_Be_Scraped()
        {
            //var url = "http://flights.thomson.co.uk/thomson/en-GB/farefinder/default?pageIndex=4&originAirportCode=MAN&destinationAirportCode=anydest&duration=0&flexDate=0&timeSpanStartDay=29&timeSpanStartYearMonth=2014-07&timeSpanEndDay=05&timeSpanEndYearMonth=2014-08";
            var url = Path.GetFullPath("../../Thomson.htm");

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(url);

            Assert.That(flights[0].Destination, Is.EqualTo("Ibiza"));
        }

        [Test]
        public void Flight_Departure_Airport_Can_Be_Scraped()
        {
            var url = Path.GetFullPath("../../Thomson.htm");

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(url);

            Assert.That(flights[0].DepartureAirport, Is.EqualTo("Manchester"));
        }

        [Test]
        public void Seats_Remaining_Can_Be_Scraped()
        {
            var url = Path.GetFullPath("../../Thomson.htm");

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(url);

            Assert.That(flights[1].SeatsLeft, Is.EqualTo(6));
            Assert.That(flights[5].SeatsLeft, Is.EqualTo(2));
        }

        [Test]
        public void Departure_Date_Can_Be_Scraped()
        {
            var url = Path.GetFullPath("../../Thomson.htm");

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(url);

            Assert.That(flights[0].DepartsOn, Is.EqualTo(new DateTime(2014,08,01,0,0,0,0)));
        }

        [Test]
        public void Smoke_Test()
        {
            WebRequest request = WebRequest.Create("http://localhost:8080/Thomson.htm");
            var response = request.GetResponse();

            var scraper = new LateSeatsScraper();
            var flights = scraper.ScrapeResponse(response as HttpWebResponse);

            Assert.That(flights[0].DepartsOn, Is.EqualTo(new DateTime(2014, 08, 01, 0, 0, 0, 0)));
            Assert.That(flights[0].DepartureAirport, Is.EqualTo("Manchester"));
            Assert.That(flights[0].Destination, Is.EqualTo("Ibiza"));
        }
    }
}
