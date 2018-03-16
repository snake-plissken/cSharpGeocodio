using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class BatchReverseGeoCodeResponse
	{
		public string Query { get; set; }
		public ReverseGeoCodeResult Response { get; set; }
	}

}