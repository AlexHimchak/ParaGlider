using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherStationsEvents
{
    public class Coords
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class Location
    {
        public Coords coords { get; set; }
        public string areaDescription { get; set; }
    }

    public class WindDirection
    {
        public string value { get; set; }
        public string units { get; set; }
    }

    public class WindSpeed
    {
        public string value { get; set; }
        public string units { get; set; }
    }

    public class Gust
    {
        public string value { get; set; }
        public string units { get; set; }
    }

    public class Temperature
    {
        public string value { get; set; }
        public string units { get; set; }
    }

    public class TemperatureDewPoint
    {
        public string value { get; set; }
        public string units { get; set; }
    }

    public class BarometricPressure
    {
        public string value { get; set; }
        public string units { get; set; }
    }

    public class Event
    {
        public Location location { get; set; }
        public DateTime timestamp { get; set; }
        public WindDirection windDirection { get; set; }
        public WindSpeed windSpeed { get; set; }
        public Gust gust { get; set; }
        public Temperature temperature { get; set; }
        public TemperatureDewPoint temperatureDewPoint { get; set; }
        public BarometricPressure barometricPressure { get; set; }
        public string relativeHumidity { get; set; }
    }
}
