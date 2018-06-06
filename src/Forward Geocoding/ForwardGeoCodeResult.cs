using System;
using Newtonsoft.Json;

namespace cSharpGeocodio.ForwardGeocodingObjects
{
	public class ForwardGeoCodeResult
	{
		public ForwardGeoCodeInput Input { get; set;}
		public GeoCodeInfo[] Results { get; set; }
	}
	
}
