using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class FederalLegislator
	{
		public FederalLegislator()
		{
			
		}

		public string Type { get; set; }
		public Bio Bio { get; set; }
		[JsonProperty("contact")]
		public ContactInfo ContactInfo { get; set; }
		[JsonProperty("social")]
		public SocialNetworkInfo SocialNetworkInfo { get; set; }
		public References References { get; set; }
		public string Source { get; set; }

	}
}
