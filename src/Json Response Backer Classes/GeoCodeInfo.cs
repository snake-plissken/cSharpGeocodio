using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class GeoCodeInfo
	{
		[JsonConstructor]
		public GeoCodeInfo(AddressComponent addresscomponents, string formattedaddress
		                   , Location location, double accuracy
		                   , string accuracytype, string source
		                   , Fields fields)
		{
			AddressComponents = addresscomponents;
			FormattedAddress = formattedaddress;
			Location = location;
			Accuracy = accuracy;
			AccuracyType = accuracytype;
			Source = source;
			Fields = fields;
		}

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
