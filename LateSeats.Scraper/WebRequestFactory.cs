using System.Net;

namespace LateSeats.Scraper
{
    public class WebRequestFactory : IWebRequestFactory
    {
        public IWebRequestWrapper Create(string url, string method = "GET")
        {
            var wrapper = new WebRequestWrapper(WebRequest.Create(url)) { Method = method };
            return wrapper;
        }
    }
}