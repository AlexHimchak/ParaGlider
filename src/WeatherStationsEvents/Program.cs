using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace WeatherStationsEvents
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Weather Stations Event Streamer" + Environment.NewLine);
            AmazonKinesisClient kinesisClient = null;
            try
            {
                //load config file
                var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
                var kinesisStreamName = configuration["kinesis-stream"];
                int timeDelay = Convert.ToInt16(configuration["time-delay"]);
                kinesisClient = new AmazonKinesisClient(region: Amazon.RegionEndpoint.USEast1);

                // Load mock json data from local file then convert to json
                var weatherEvents = JsonConvert.DeserializeObject<IList<Event>>(File.ReadAllText("weather-data.json"));
                foreach (var weatherEvent in weatherEvents)
                {
                    // Send each weather item as an event to kinesis
                    string dataAsJson = JsonConvert.SerializeObject(weatherEvent);
                    using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(dataAsJson)))
                    {
                        var requestRecord = new PutRecordRequest
                        {
                            StreamName = kinesisStreamName,
                            PartitionKey = "weather",
                            Data = memoryStream
                        };
                        var response = kinesisClient.PutRecordAsync(requestRecord).Result;
                        if ((int)response.HttpStatusCode == 200)
                            Console.WriteLine($"Successfully sent record to Kinesis. Sequence number: {response.SequenceNumber}. Next event in {timeDelay}(s)");
                        else
                            Console.Write($"Failed on: {dataAsJson}");
                    }
                    Thread.Sleep(timeDelay * 1000);
                }
            }
            catch (AmazonClientException amazonEx)
            {
                Console.WriteLine("Opps! Looks like you don't have an Amazon cli setup or incorrect Kinesis stream name.  Update appsettings.json in this build directory. Check the exception below for more details.");
                Console.Write(amazonEx.GetBaseException().ToString());
                Console.ReadLine();
                throw;
            }
            catch (Exception ex)
            {
                Console.Write(ex.GetBaseException().ToString());
                Console.ReadLine();
                throw;
            }
            finally {
                kinesisClient?.Dispose();
            }
        }
    }
}