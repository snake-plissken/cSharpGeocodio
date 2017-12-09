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

	public class ForwardGeoCodeResult
	{
		public ForwardGeoCodeInput Input { get; set;}
		public GeoCodeInfo[] Results { get; set; }
	}

	public class BatchForwardGeoCodeResult
	{
		public BatchForwardGeoCodeRecord[] Results { get; set; }
	}

	public class BatchForwardGeoCodeRecord
	{
		public string Query { get; set; }
		public ForwardGeoCodeResult Response { get; set; }
	}
}
