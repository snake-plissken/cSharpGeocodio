using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class Location
	{
		[JsonConstructor]
		public Location(float latitude, float longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		[JsonProperty("lat")]
		public float Latitude { get; set; }
		[JsonProperty("lng")]
		public float Longitude { get; set; }
	}
}
