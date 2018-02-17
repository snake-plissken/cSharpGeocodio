using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class CongressionalDistrictV2
	{
		public CongressionalDistrictV2()
		{
		}

		public string Name { get; set; }
		[JsonProperty("district_number")]
		public int DistrictNumber { get; set; }
		[JsonProperty("congress_number")]
		public string CongressNumber { get; set; }
		[JsonProperty("congress_years")]
		public string CongressYears { get; set; }
		public int Proportion { get; set; }
		[JsonProperty("current_legislators")]
		public FederalLegislator[] CurrentLegislators { get; set; }
	}
}
