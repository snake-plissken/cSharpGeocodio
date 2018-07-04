using System;
using Newtonsoft.Json;

namespace cSharpGeocodio.ForwardGeocodingObjects
{
	//
	//Needed for response when geocoding single location
	//
	public class ForwardGeoCodeInput
	{
		private ForwardGeoCodeInput() { }

		[JsonConstructor]
		public ForwardGeoCodeInput(AddressComponent input
								   , string formattedAddress)
		{
			Input = input;
			FormattedAddress = formattedAddress;
		}

		public AddressComponent Input { get; set; }
		[JsonProperty("formatted_address")]
		public string FormattedAddress { get; set; }
	}
}
