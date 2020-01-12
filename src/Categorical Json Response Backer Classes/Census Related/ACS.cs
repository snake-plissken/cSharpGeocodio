using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.  Used when dealing with Census info.
    /// </summary>
    public class ACS
    {
        [JsonProperty("meta")]
        public ACS_Meta Meta { get; set; }

        [JsonProperty("demographics")]
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Demographics { get; set; }

        [JsonProperty("economics")]
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Economics { get; set; }

        [JsonProperty("families")]
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Families { get; set; }

        [JsonProperty("housing")]
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Housing { get; set; }

        [JsonProperty("social")]
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Social { get; set; }
    }
}
