using System.Runtime.Serialization;

namespace LateSeats.Scraper
{
    [DataContract]
    public class Airport
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        public string GenerateAirportId()
        {
            return Code;
        }
    }
}