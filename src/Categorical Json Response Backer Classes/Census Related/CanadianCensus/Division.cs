using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpGeocodio.CanadanianCensus
{
    public class Division
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        [JsonProperty("type_description")]
        public string TypeDescription { get; set; }
    }
}
