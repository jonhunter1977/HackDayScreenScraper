using System.Net;

namespace LateSeats.Scraper
{
    public class WebRequestFactory : IWebRequestFactory
    {
        public IWebRequestWrapper Create(string s)
        {
            var wrapper = new WebRequestWrapper(WebRequest.Create(s));
            return wrapper;
        }

        public IWebRequestWrapper Create(string url, string method)
        {
            var wrapper = Create(url);
            wrapper.Method = method;
            return wrapper;
        }
    }
}