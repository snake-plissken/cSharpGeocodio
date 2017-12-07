using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	//
	//Needed for response when geocoding single location
	//
	public class ForwardGeoCodeInput
	{
		public AddressComponent Input { get; set; }
		[JsonProperty("formatted_address")]
		public string FormattedAddress { get; set; }
	}

	public class ForwardGeoCodeRecord
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

	public class ForwardGeoCodeResult
	{
		public ForwardGeoCodeInput Input { get; set;}
		public ForwardGeoCodeRecord[] Results { get; set; }
	}

	public class BatchForwardGeoCodeResult
	{
		public BatchForwardGeoCodeRecord[] Results { get; set; }
	}

	public class BatchForwardGeoCodeRecord
	{
		public string Query { get; set; }
		public BatchForwardGeoCodeResponse Response { get; set; }
	}

	public class BatchForwardGeoCodeResponse
	{
		public ForwardGeoCodeInput Input { get; set; }
		public ForwardGeoCodeRecord[] Results { get; set; }
	}
}
