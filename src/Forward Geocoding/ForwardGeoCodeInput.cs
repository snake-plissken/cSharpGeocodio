using System;
using Newtonsoft.Json;

namespace cSharpGeocodio.ForwardGeocodingObjects
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
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
