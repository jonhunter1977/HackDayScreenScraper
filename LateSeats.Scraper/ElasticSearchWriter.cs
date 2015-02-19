using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using LateSeats.Scraper.Exceptions;

namespace LateSeats.Scraper
{
    public class ElasticSearchWriter : IElasticSearchWriter
    {
        public void Post(IEnumerable<Flight> flights, IWebRequestFactory webRequestFactory)
        {
            foreach (var flight in flights)
            {
                var webRequest = webRequestFactory.Create("http://10.44.35.21:9200/lateseats/flight/" + flight.GenerateFlightId(), "POST");

                using (var requestStream = webRequest.GetRequestStream())
                {
                    WriteToStream(flight, requestStream);
                    try
                    {
                        using (var response = webRequest.GetResponse())
                        {
                            response.Close();
                        }
                    }
                    catch (WebException e)
                    {
                        throw new ElasticSearchException("Unable to connect to Elastic Search, call Steve!", e);
                    }
                }
            }
        }

        public void Post(IEnumerable<Airport> airports, IWebRequestFactory webRequestFactory)
        {
            foreach (var airport in airports)
            {
                var webRequest = webRequestFactory.Create("http://10.44.35.21:9200/lateseats/airport/" + airport.GenerateAirportId(), "POST");

                using (var requestStream = webRequest.GetRequestStream())
                {
                    WriteToStream(airport, requestStream);
                    try
                    {
                        using (var response = webRequest.GetResponse())
                        {
                            response.Close();
                        }
                    }
                    catch (WebException e)
                    {
                        throw new ElasticSearchException("Unable to connect to Elastic Search, call Steve!", e);
                    }
                }
            }
        }
        private static void WriteToStream<T>(T objectToWrite, Stream requestStream)
        {
            var buffer = GetJsonBytes(objectToWrite);
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();
        }

        private static byte[] GetJsonBytes<T>(T obj)
        {
            var jsonFlight = SerializeJSon(obj);
            var buffer = Encoding.UTF8.GetBytes(jsonFlight);
            return buffer;
        }

        public static string SerializeJSon<T>(T obj)
        {
            string jsonString;
            using (var stream = new MemoryStream())
            {
                var ds = new DataContractJsonSerializer(typeof(T));

                ds.WriteObject(stream, obj);

                jsonString = Encoding.UTF8.GetString(stream.ToArray());

                stream.Close();
            }
            return jsonString;
        }
    }
}