using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpGeocodio.CanadanianCensus
{
    /// <summary>
    /// Canadian census field for Canadian electoral district.  See: https://en.wikipedia.org/wiki/List_of_Canadian_federal_electoral_districts
    /// </summary>
    public class Riding
    {
        public string Code { get; set; }
        [JsonProperty("name_french")]
        public string NameFrench { get; set; }
        [JsonProperty("name_english")]
        public string NameEnglish { get; set; }
    }
}
