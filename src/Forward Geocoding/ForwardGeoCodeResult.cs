using System;
using Newtonsoft.Json;

namespace cSharpGeocodio.ForwardGeocodingObjects
{
	public class ForwardGeoCodeResult
	{
		private ForwardGeoCodeResult() { }

		[JsonConstructor]
		public ForwardGeoCodeResult(ForwardGeoCodeInput input, GeoCodeInfo[] results)
		{
			Input = input;
			Results = results;
		}

		public ForwardGeoCodeInput Input { get; set;}
		public GeoCodeInfo[] Results { get; set; }
	}
	
}
