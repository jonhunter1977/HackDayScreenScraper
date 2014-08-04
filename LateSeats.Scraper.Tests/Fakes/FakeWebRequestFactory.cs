namespace LateSeats.Scraper.Tests.Fakes
{
    public class FakeWebRequestFactory : IWebRequestFactory
    {
        public IWebRequestWrapper Create(string s)
        {
            return new FakeWebRequestWrapper();
        }
    }
}