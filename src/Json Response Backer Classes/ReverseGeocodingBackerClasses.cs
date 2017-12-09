using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class ReverseGeoCodeResult
	{
		public GeoCodeInfo[] Results { get; set; }
	}

	public class BatchReverseGeoCodingResult
	{
		public BatchReverseGeoCodeResponse[] Results { get; set; }
	}

	public class BatchReverseGeoCodeResponse
	{
		public string Query { get; set; }
		public ReverseGeoCodeResult Response { get; set; }
	}

}
