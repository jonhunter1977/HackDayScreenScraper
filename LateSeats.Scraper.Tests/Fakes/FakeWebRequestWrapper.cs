using System.IO;
using System.Net;

namespace LateSeats.Scraper.Tests.Fakes
{
    public class FakeWebRequestWrapper : IWebRequestWrapper
    {
        public string Method { get; set; }

        public Stream GetRequestStream()
        {
            return new MemoryStream();
        }

        public WebResponse GetResponse()
        {
            return new FakeHttpWebResponse();
        }

        public Stream GetResponseStream()
        {
            return new FakeHttpWebResponse().GetResponseStream();
        }
    }
}