using System;
using System.Net;

namespace LateSeats.Scraper.Exceptions
{
    public class ElasticSearchException : Exception
    {
        public ElasticSearchException(string message, WebException e) :base(message, e)
        {
            
        }
    }
}