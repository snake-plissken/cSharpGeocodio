using System;
using Newtonsoft.Json;

namespace cSharpGeocodio.ForwardGeocodingObjects
{
	public class BatchForwardGeoCodeRecord
	{
		public string Query { get; set; }
		public ForwardGeoCodeResult Response { get; set; }
	}
}
