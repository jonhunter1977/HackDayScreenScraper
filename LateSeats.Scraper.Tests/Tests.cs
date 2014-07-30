using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
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
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(stream);

            Assert.That(flights[0].ArrivalAirport.Name, Is.EqualTo("Ibiza"));
        }

        [Test]
        public void Flight_Departure_Airport_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(stream);

            Assert.That(flights[0].DepartureAirport.Name, Is.EqualTo("Manchester"));
        }

        [Test]
        public void Seats_Remaining_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(stream);

            Assert.That(flights[1].SeatsLeft, Is.EqualTo(6));
            Assert.That(flights[5].SeatsLeft, Is.EqualTo(2));
        }

        [Test]
        public void Departure_Flight_DateTime_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(stream);

            Assert.That(flights[0].ArrivalDate, Is.EqualTo("2014-08-01T21:00:00"));
        }

        [Test]
        public void Return_Flight_DateTime_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(stream);

            Assert.That(flights[0].DepartureDate, Is.EqualTo("2014-08-09T00:40:00"));
        }

        [Test]
        public void Number_Of_Nights_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(stream);

            Assert.That(flights[0].NoOfNights, Is.EqualTo(7));
        }

        [Test]
        public void Departure_Airport_Code_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(stream);

            Assert.That(flights[0].DepartureAirport.Code, Is.EqualTo("MAN"));
        }

        [Test]
        public void Arrival_Airport_Code_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(stream);

            Assert.That(flights[0].ArrivalAirport.Code, Is.EqualTo("IBZ"));
        }

        [Test]
        public void Elastic_Search_Url_Generated_Correctly()
        {
            const string expected = "MIAPMI201407311000";

            var flight = new Flight();
            flight.ArrivalAirport.Code = "PMI";
            flight.DepartureAirport.Code = "MIA";
            flight.DepartureDate = "2014-07-31T10:00:00";

            var result = flight.GenerateFlightId();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Able_To_Query_Thomson_For_Todays_Flights()
        {
            const string expected = "timeSpanStartDay=30&timeSpanStartYearMonth=2014-07&timeSpanEndDay=06&timeSpanEndYearMonth=2014-08";

            var result = new ThomsonQueryBuilder().GetQueries(DateTime.Now, 7);

            Assert.That(result[0], Is.EqualTo(expected));
        }
        [Test]
        public void Smoke_Test()
        {
            WebRequest request = WebRequest.Create("http://localhost:8080/Thomson.htm");
            var response = request.GetResponse();

            var scraper = new LateSeatsScraper();
            var flights = scraper.Scrape(response.GetResponseStream());

            Assert.That(flights[0].ArrivalDate, Is.EqualTo("2014-08-01T21:00:00"));
            Assert.That(flights[0].DepartureAirport.Name, Is.EqualTo("Manchester"));
            Assert.That(flights[0].ArrivalAirport.Name, Is.EqualTo("Ibiza"));
        }
    }
}
