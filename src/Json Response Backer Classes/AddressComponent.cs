using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class AddressComponent
	{
		public string Number { get; set; }
		public string Predirectional { get; set; }
		public string Street { get; set; }
		public string Suffix { get; set; }
		[JsonProperty("formatted_street")]
		public string FormattedStreet { get; set; }
		public string City { get; set; }
		public string County { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
	}
}
