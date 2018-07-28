using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class Fields
	{
		private CongressionalDistrict congressionalDistrict = null;
		private StateLegislature stateLegislature = null;
		private SchoolDistrict schoolDistrict = null;
		private TimeZone timeZone = null;
		private Census census = null;

		[JsonProperty("congressional_districts")]
		public CongressionalDistrictV2[] CongressionalDistricts { get; set; }

		[JsonProperty("state_legislative_districts")]
		public StateLegislature StateLegislature { get; set; }

		[JsonProperty("school_districts")]
		public SchoolDistricts SchoolDistricts { get; set; }

		public TimeZone TimeZone { get; set; }

		public Census Census { get; set; }
	}

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

	public class CongressionalDistrict
	{
		public string Name { get; set; }
		[JsonProperty("district_number")]
		public int DistrictNumber { get; set; }
		[JsonProperty("congress_number")]
		public string CongressNumber { get; set; }
		[JsonProperty("congress_years")]
		public string CongressYears { get; set; }
	}


}
