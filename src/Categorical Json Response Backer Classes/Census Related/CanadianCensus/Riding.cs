using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpGeocodio.src.Categorical_Json_Response_Backer_Classes.Census_Related.CanadianCensus
{
    class Riding
    {
        public string Code { get; set; }
        [JsonProperty("name_french")]
        public string NameFrench { get; set; }
        [JsonProperty("name_english")]
        public string NameEnglish { get; set; }
    }
}
