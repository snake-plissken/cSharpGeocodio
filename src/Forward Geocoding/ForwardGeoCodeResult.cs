using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{

	public class ForwardGeoCodeResult
	{
		public ForwardGeoCodeInput Input { get; set;}
		public GeoCodeInfo[] Results { get; set; }
	}
	
}
