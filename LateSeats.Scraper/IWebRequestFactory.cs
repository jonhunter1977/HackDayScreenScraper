namespace LateSeats.Scraper
{
    public interface IWebRequestFactory
    {
        IWebRequestWrapper Create(string s);
    }
}