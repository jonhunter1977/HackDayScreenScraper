using System.Net;

namespace LateSeats.Scraper
{
    public class WebRequestFactory : IWebRequestFactory
    {
        public IWebRequestWrapper Create(string s)
        {
            return new WebRequestWrapper(WebRequest.Create(s));
        }
    }
}