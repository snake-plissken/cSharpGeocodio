using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class Bio
	{
		public Bio()
		{
		}

		[JsonProperty("last_name")]
		public string LastName { get; set; }
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		public string Birthday { get; set; }
		public string Gender { get; set; }
		public string Party { get; set; }

	}
}
