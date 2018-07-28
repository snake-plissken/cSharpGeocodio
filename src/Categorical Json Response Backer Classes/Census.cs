using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class Census
	{
		[JsonProperty("state_fips")]
		public string StateFIPS { get; set; }
		[JsonProperty("county_fips")]
		public string CountyFIPS { get; set; }
		[JsonProperty("place_fips")]
		public string PlaceFIPS { get; set; }
		[JsonProperty("tract_code")]
		public string TractCode { get; set; }
		[JsonProperty("block_group")]
		public string BlockGroup { get; set; }
		[JsonProperty("block_code")]
		public string BlockCode { get; set; }
		[JsonProperty("census_year")]
		public int CensusYear { get; set; }
	}
}
