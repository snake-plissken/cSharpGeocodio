using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class Location
	{
		[JsonProperty("lat")]
		public float Latitude { get; set; }
		[JsonProperty("lng")]
		public float Longitude { get; set; }
	}
}
