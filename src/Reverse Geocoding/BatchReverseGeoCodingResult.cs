using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class BatchReverseGeoCodingResult
	{
		[JsonConstructor]
		public BatchReverseGeoCodingResult(BatchReverseGeoCodeResponse[] results)
		{
			Results = results;
		}

		public BatchReverseGeoCodeResponse[] Results { get; set; }
	}

}
