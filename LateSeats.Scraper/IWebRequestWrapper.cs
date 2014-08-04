using System.IO;
using System.Net;

namespace LateSeats.Scraper
{
    public interface IWebRequestWrapper
    {
        string Method { get; set; }

        Stream GetRequestStream();

        WebResponse GetResponse();

        Stream GetResponseStream();
    }
}