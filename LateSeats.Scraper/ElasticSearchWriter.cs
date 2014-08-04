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
                var jsonFlight = SerializeJSon(flight);

                var webRequest = webRequestFactory.Create("http://10.44.35.21:9200/lateseats/flight/" + flight.GenerateFlightId());
                webRequest.Method = "POST";

                using (var requestStream = webRequest.GetRequestStream())
                {
                    var buffer = Encoding.UTF8.GetBytes(jsonFlight);
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    try
                    {
                        using (var response = webRequest.GetResponse())
                        {
                            response.Close();
                        }
                    }
                    catch (WebException e)
                    {
                        throw new ElasticSearchException("Unable to connect to Elastic Search", e);
                    }
                }
            }
        }

        public static string SerializeJSon<T>(T t)
        {
            string jsonString;
            using (var stream = new MemoryStream())
            {
                var ds = new DataContractJsonSerializer(typeof(T));

                ds.WriteObject(stream, t);

                jsonString = Encoding.UTF8.GetString(stream.ToArray());

                stream.Close();
            }
            return jsonString;
        }
    }
}