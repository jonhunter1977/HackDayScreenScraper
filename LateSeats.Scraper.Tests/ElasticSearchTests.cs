using NUnit.Framework;

namespace LateSeats.Scraper.Tests
{
    [TestFixture]
    public class ElasticSearchTests
    {
        [Test]
        public void Elastic_Search_Url_Generated_Correctly()
        {
            const string expected = "MIAPMI201407311000";

            var flight = new Flight
                {
                    ArrivalAirport = { Code = "PMI" },
                    DepartureAirport = { Code = "MIA" },
                    DepartureDate = "2014-07-31T10:00:00"
                };

            var result = flight.GenerateFlightId();

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}