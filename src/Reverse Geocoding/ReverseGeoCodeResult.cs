using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class ReverseGeoCodeResult
	{
		[JsonConstructor]
		public ReverseGeoCodeResult(GeoCodeInfo[] results)
		{
			Results = results;
		}

		public GeoCodeInfo[] Results { get; set; }
	}

}
