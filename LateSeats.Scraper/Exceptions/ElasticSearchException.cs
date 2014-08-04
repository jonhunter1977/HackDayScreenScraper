using System;
using System.Net;

namespace LateSeats.Scraper.Exceptions
{
    internal class ElasticSearchException : Exception
    {
        public ElasticSearchException(string message, WebException e) :base(message, e)
        {
            
        }
    }
}