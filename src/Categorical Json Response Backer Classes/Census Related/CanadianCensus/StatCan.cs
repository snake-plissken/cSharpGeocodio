﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpGeocodio.CanadanianCensus
{
    /// <summary>
    /// Census geographic units of Cana.  See: https://en.wikipedia.org/wiki/Census_geographic_units_of_Canada
    /// </summary>
    public class StatCan
    {
        public Division Division { get; set; }
        [JsonProperty("consolidated_subdivision")]
        public Division ConsolidatedSubdivision { get; set; }
        public Division Subdivision { get; set; }
        [JsonProperty("economic_region")]
        public string EnconomicRegion { get; set;}
        [JsonProperty("statistical_area")]
        public StatisticalArea StatisticalArea { get; set; }
        [JsonProperty("cma_ca")]
        public Division CMA_CA { get; set; }
        public string Tract { get; set; }
        [JsonProperty("census_year")]
        public int CensusYear { get; set; }
    }
}
