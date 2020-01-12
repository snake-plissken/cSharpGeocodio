using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using cSharpGeocodio.CanadanianCensus;

namespace cSharpGeocodio
{
	public class Fields
	{

        /// <summary>
        /// US Federal legislature information.
        /// </summary>
		[JsonProperty("congressional_districts")]
		public CongressionalDistrictV2[] CongressionalDistricts { get; set; }

        /// <summary>
        /// US State legislature information.
        /// </summary>
		[JsonProperty("state_legislative_districts")]
		public StateLegislature StateLegislature { get; set; }

        /// <summary>
        /// US school district information.
        /// </summary>
		[JsonProperty("school_districts")]
		public SchoolDistricts SchoolDistricts { get; set; }

        /// <summary>
        /// Timezone information.  The standardized name follows the tzdb format. E.g. America/New_York.
        /// </summary>
		[JsonProperty("timezone")]
		public TimeZone TimeZone { get; set; }

        /// <summary>
        /// US Census information.
        /// </summary>
		[JsonProperty("census")]
		public Dictionary<string, Census> Census { get; set; }

        /// <summary>
        /// American Commnity Survey, a more frequent version of the decennial census.  See: https://en.wikipedia.org/wiki/American_Community_Survey
        /// </summary>
        [JsonProperty("acs")]
        public ACS ACS_Results { get; set; }

        /// <summary>
        /// Canadian census field for Canadian electoral district.  See: https://en.wikipedia.org/wiki/List_of_Canadian_federal_electoral_districts
        /// </summary>
        public CanadanianCensus.Riding Riding { get; set; }

        /// <summary>
        /// Census geographic units of Cana.  See: https://en.wikipedia.org/wiki/Census_geographic_units_of_Canada
        /// </summary>
        public CanadanianCensus.StatCan StatCan { get; set; }

    }

}
