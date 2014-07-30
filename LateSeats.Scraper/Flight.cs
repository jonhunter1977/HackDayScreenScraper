using System;
using System.Runtime.Serialization;

namespace LateSeats.Scraper
{
    [DataContract]
    public class Flight
    {
        [DataMember(Name = "destination_airport")]
        public Airport ArrivalAirport { get; set; }

        [DataMember(Name = "departure_airport")]
        public Airport DepartureAirport { get; set; }

        [DataMember(Name = "seats_remaining")]
        public int SeatsLeft { get; set; }

        [DataMember(Name = "arrival_date")]
        public string ArrivalDate { get; set; }

        [DataMember(Name = "departure_date")]
        public string DepartureDate { get; set; }

        [DataMember(Name = "nights")]
        public int NoOfNights { get; set; }
        
        public Flight()
        {
            ArrivalAirport = new Airport();
            DepartureAirport = new Airport();
        }

        public string GenerateFlightId()
        {
            var depDate = DateTime.Parse(DepartureDate);
            var id = DepartureAirport.Code + ArrivalAirport.Code + depDate.ToString("yyyyMMddHHmm");
            return id;
        }
    }

    [DataContract]
    public class Airport
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}

