using System;
using NUnit.Framework;

namespace LateSeats.Scraper.Tests
{
    [TestFixture]
    public class ThomsonQueryBuilderTests
    {
        [Test]
        public void Able_To_Query_Thomson_For_Todays_Flights()
        {
            var startDate = DateTime.Now;

            var expected = string.Format("timeSpanStartDay={0}&timeSpanStartYearMonth={1}-{2}&timeSpanEndDay={3}&timeSpanEndYearMonth={4}-{5}",
                                         startDate.Day.ToString("d2"),
                                         startDate.Year,
                                         startDate.Month.ToString("d2"),
                                         startDate.AddDays(7).Day.ToString("d2"),
                                         startDate.AddDays(7).Year,
                                         startDate.AddDays(7).Month.ToString("d2"));

            var result = new ThomsonQueryBuilder().GetFlightQueries(startDate, 7);

            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}