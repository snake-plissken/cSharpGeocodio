using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer classes used to deserialize responses from Geocodio.
    /// </summary>
    public class BatchReverseGeoCodeResponse
	{
		[JsonConstructor]
		public BatchReverseGeoCodeResponse(string query, ReverseGeoCodeResult response)
		{
			Query = query;
			Response = response;
		}

		public string Query { get; set; }
		public ReverseGeoCodeResult Response { get; set; }
	}

}