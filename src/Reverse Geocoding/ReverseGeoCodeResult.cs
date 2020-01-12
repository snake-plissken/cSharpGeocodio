using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
	public class ReverseGeoCodeResult
	{
		[JsonConstructor]
		public ReverseGeoCodeResult(GeoCodeInfo[] results)
		{
			Results = results;
		}

		public GeoCodeInfo[] Results { get; set; }
        public string[] _warnings { get; set; }
	}

}
