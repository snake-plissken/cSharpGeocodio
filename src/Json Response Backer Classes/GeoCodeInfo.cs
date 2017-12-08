using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class GeoCodeInfo
	{
		[JsonProperty("address_components")]
		public AddressComponent AddressComponents { get; set; }
		[JsonProperty("formatted_address")]
		public string FormattedAddress { get; set; }
		public Location Location { get; set; }
		public double Accuracy { get; set; }
		[JsonProperty("accuracy_type")]
		public string AccuracyType { get; set; }
		public string Source { get; set; }
		public Fields Fields { get; set; }
	}
}
