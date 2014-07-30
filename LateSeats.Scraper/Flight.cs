using System;
using System.Runtime.Serialization;

namespace LateSeats.Scraper
{
    [DataContract]
    public class Flight
    {
        [DataMember(Name = "destination_airport")]
        public Airport Destination { get; set; }

        [DataMember(Name = "departure_airport")]
        public Airport DepartureAirport { get; set; }

        [DataMember(Name = "seats_remaining")]
        public int SeatsLeft { get; set; }

        [DataMember(Name = "arrival_date")]
        public string ArrivalDate { get; set; }

        [DataMember(Name = "departure_date")]
        public string DepartureDate { get; set; }

        [DataMember(Name="nights")]
        public int NoOfNights { get; set; }

        [DataMember(Name = "code")]
        public string AirportCode { get; set; }

        public Flight()
        {
            Destination = new Airport();
            DepartureAirport = new Airport();
        }

        public class Airport
        {
            [DataMember(Name = "code")]
            public string Code { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }
        }
    }
}

