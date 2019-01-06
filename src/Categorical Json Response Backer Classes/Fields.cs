using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class Fields
	{

		[JsonProperty("congressional_districts")]
		public CongressionalDistrictV2[] CongressionalDistricts { get; set; }

		[JsonProperty("state_legislative_districts")]
		public StateLegislature StateLegislature { get; set; }

		[JsonProperty("school_districts")]
		public SchoolDistricts SchoolDistricts { get; set; }

		[JsonProperty("timezone")]
		public TimeZone TimeZone { get; set; }

		[JsonProperty("census")]
		public Census Census { get; set; }

	}

}
