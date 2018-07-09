using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class AddressComponent
	{
		[JsonConstructor]
		public AddressComponent(string number, string predirectional
								 , string street, string suffix
								 , string formattedstreet, string city
								 , string county, string state
								 , string zip, string country)
		{
			Number = number;
			Predirectional = predirectional;
			Street = street;
			Suffix = suffix;
			FormattedStreet = formattedstreet;
			City = city;
			County = county;
			State = state;
			Zip = zip;
			Country = country;
		}

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
