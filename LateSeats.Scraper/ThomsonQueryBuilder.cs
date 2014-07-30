using System;
using System.Collections.Generic;

namespace LateSeats.Scraper
{
    public class ThomsonQueryBuilder
    {
        public List<string> GetQueries(DateTime startDate, int maxLookAhead)
        {
            var queries = new List<string>();
            for (int i = 0; i < maxLookAhead; i++)
            {
                var dateTime = startDate;
                queries.Add("timeSpanStartDay=" + dateTime.Day.ToString("d2") + "&timeSpanStartYearMonth=" + dateTime.Year + "-" + dateTime.Month.ToString("d2") + "&timeSpanEndDay=" + dateTime.AddDays(maxLookAhead).Day.ToString("d2") + "&timeSpanEndYearMonth=" + dateTime.AddDays(maxLookAhead).Year + "-" + dateTime.AddDays(maxLookAhead).Month.ToString("d2"));
            }
            return queries;
        } 
    }
}