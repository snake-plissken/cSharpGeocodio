using System;
using Newtonsoft.Json;

namespace cSharpGeocodio.ForwardGeocodingObjects
{
	public class BatchForwardGeoCodeResult
	{
		private BatchForwardGeoCodeResult() { }

		[JsonConstructor]
		public BatchForwardGeoCodeResult(BatchForwardGeoCodeRecord[] results)
		{
			Results = results;
		}

		public BatchForwardGeoCodeRecord[] Results { get; set; }
	}
}
