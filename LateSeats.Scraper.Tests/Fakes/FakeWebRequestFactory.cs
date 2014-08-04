namespace LateSeats.Scraper.Tests.Fakes
{
    public class FakeWebRequestFactory : IWebRequestFactory
    {
        public IWebRequestWrapper Create(string s)
        {
            return new FakeWebRequestWrapper();
        }

        public IWebRequestWrapper Create(string url, string method)
        {
            return new FakeWebRequestWrapper()
                {
                    Method = method
                };
        }
    }
}