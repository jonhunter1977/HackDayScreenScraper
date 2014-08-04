using System.IO;
using LateFlights.Scraper;
using LateSeats.Scraper.Tests.Fakes;
using NUnit.Framework;
using System.Linq;

namespace LateSeats.Scraper.Tests
{
    [TestFixture]
    public class ScraperTests
    {
        public ThomsonScraper ClassUnderTest
        {
            get
            {
                return new ThomsonScraper(new FakeWebRequestFactory(), new ThomsonHtmlParser(), new ElasticSearchWriter());
            }
        }

        [Test]
        public void Flight_Destination_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var result = ClassUnderTest.Scrape(stream);

            Assert.That(result.Flights[0].ArrivalAirport.Name, Is.EqualTo("Ibiza"));
        }

        [Test]
        public void Flight_Departure_Airport_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var result = ClassUnderTest.Scrape(stream);

            Assert.That(result.Flights[0].DepartureAirport.Name, Is.EqualTo("Manchester"));
        }

        [Test]
        public void Seats_Remaining_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var result = ClassUnderTest.Scrape(stream);

            Assert.That(result.Flights[1].SeatsLeft, Is.EqualTo(6));
            Assert.That(result.Flights[5].SeatsLeft, Is.EqualTo(2));
        }

        [Test]
        public void Departure_Flight_DateTime_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var result = ClassUnderTest.Scrape(stream);

            Assert.That(result.Flights[0].ArrivalDate, Is.EqualTo("2014-08-01T21:00:00"));
        }

        [Test]
        public void Return_Flight_DateTime_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson2.htm"));

            var result = ClassUnderTest.Scrape(stream);

            Assert.That(result.Flights[8].DepartureDate, Is.EqualTo("2014-09-09T14:35:00"));
        }

        [Test]
        public void Number_Of_Nights_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var result = ClassUnderTest.Scrape(stream);

            Assert.That(result.Flights[0].NoOfNights, Is.EqualTo(7));
        }

        [Test]
        public void Departure_Airport_Code_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var result = ClassUnderTest.Scrape(stream);

            Assert.That(result.Flights[0].DepartureAirport.Code, Is.EqualTo("MAN"));
        }

        [Test]
        public void Arrival_Airport_Code_Can_Be_Scraped()
        {
            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var results = ClassUnderTest.Scrape(stream);

            Assert.That(results.Flights[0].ArrivalAirport.Code, Is.EqualTo("IBZ"));
        }

        [Test]
        public void Can_Check_For_More_Pages()
        {
            const string expected = "/thomson/en-GB/farefinder/default?pageIndex=5&amp;originAirportCode=MAN&amp;destinationAirportCode=anydest&amp;duration=0&amp;flexDate=0&amp;timeSpanStartDay=29&amp;timeSpanStartYearMonth=2014-07&amp;timeSpanEndDay=05&amp;timeSpanEndYearMonth=2014-08";

            var stream = File.OpenRead(Path.GetFullPath("../../Thomson.htm"));

            var scrapeResult = ClassUnderTest.Scrape(stream);
            var result = scrapeResult.NextUrl;

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Can_Scrape_Departure_Airports()
        {
            const string expected = "ABZ";

            var stream = File.OpenRead(Path.GetFullPath("../../ThomsonDepartureAirports.htm"));

            var departureResult = ClassUnderTest.ScrapeDepartureAirports(stream);

            Assert.True(departureResult.Count > 0);
            Assert.That(departureResult[0], Is.EqualTo(expected));
        }
    }
}
