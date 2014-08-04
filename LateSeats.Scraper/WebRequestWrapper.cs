using System.IO;
using System.Net;

namespace LateSeats.Scraper
{
    public class WebRequestWrapper : IWebRequestWrapper
    {
        private readonly WebRequest _webRequest;
        public string Method
        {
            get
            {
                return _webRequest != null ? _webRequest.Method : "GET";
            }
            set
            {
                if (_webRequest != null)
                    _webRequest.Method = value;
            }
        }
        
        public WebRequestWrapper(WebRequest webRequest)
        {
            _webRequest = webRequest;
        }

        public Stream GetRequestStream()
        {
            return _webRequest.GetRequestStream();
        }

        public WebResponse GetResponse()
        {
            return _webRequest.GetResponse();
        }

        public Stream GetResponseStream()
        {
            return this.GetResponse().GetResponseStream();
        }
    }
}