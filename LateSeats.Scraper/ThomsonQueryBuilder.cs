using System;
using System.Collections.Generic;

namespace LateSeats.Scraper
{
    public class ThomsonQueryBuilder
    {
        public List<string> GetFlightQueries(DateTime startDate, int maxLookAhead)
        {
            var queries = new List<string>();
            for (var i = 0; i < maxLookAhead; i++)
            {
                var query = string.Format("timeSpanStartDay={0}&timeSpanStartYearMonth={1}-{2}&timeSpanEndDay={3}&timeSpanEndYearMonth={4}-{5}",
                                         startDate.Day.ToString("d2"),
                                         startDate.Year,
                                         startDate.Month.ToString("d2"),
                                         startDate.AddDays(7).Day.ToString("d2"),
                                         startDate.AddDays(7).Year,
                                         startDate.AddDays(7).Month.ToString("d2"));

                queries.Add(query);
            }
            return queries;
        } 
    }
}