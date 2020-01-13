using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.  Contains items related to the US Census.
    /// </summary>
	public class Census
    {

        [JsonProperty("census_year")]
        public int CensusYear { get; set; }

        [JsonProperty("state_fips")]
        public string StateFIPS { get; set; }

        [JsonProperty("county_fips")]
        public string CountyFIPS { get; set; }

        [JsonProperty("tract_code")]
        public string TractCode { get; set; }

        [JsonProperty("block_code")]
        public string BlockCode { get; set; }

        [JsonProperty("block_group")]
        public string BlockGroup { get; set; }

        [JsonProperty("full_fips")]
        public string Full_FIPS { get; set; }

        [JsonProperty("place")]
        public Place Place { get; set; }

        [JsonProperty("metro_micro_statistical_area")]
        public MetroMicroStatisticalArea MetroMicroStatisticalArea { get; set; }

        [JsonPropertyAttribute("combined_statistical_area")]
        public CombinedStatisticalArea CombinedStatisticalArea { get; set; }

        [JsonProperty("metropolitan_division")]
        public CombinedStatisticalArea MetropolitanDivision { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
