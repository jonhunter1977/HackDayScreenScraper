using System;

namespace LateSeats.Scraper
{
    public class Flight
    {
        public string Destination { get; set; }

        public string DepartureAirport { get; set; }

        public int SeatsLeft { get; set; }

        public DateTime DepartsOn { get; set; }

        public DateTime ReturnsOn { get; set; }

        public int NoOfNights { get; set; }
    }
}