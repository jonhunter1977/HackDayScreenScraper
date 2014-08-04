namespace LateSeats.Scraper
{
    public interface IWebRequestFactory
    {
        IWebRequestWrapper Create(string url, string method = "GET");
    }
}