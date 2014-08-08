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
            return DepartureAirport.Code + ArrivalAirport.Code + DateTime.Parse(ArrivalDate).ToString("yyyyMMddHHmm");
        }
    }
}

