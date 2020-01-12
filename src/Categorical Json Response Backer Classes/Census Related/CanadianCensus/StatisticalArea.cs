using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpGeocodio.CanadanianCensus
{
    /// <summary>
    /// Contains details related to StatCan or Riding fields.
    /// </summary>
    public class StatisticalArea
    {
        public string Code { get; set; }
        [JsonProperty("code_description")]
        public string CodeDescription { get; set; }
        public string Type { get; set; }
        [JsonProperty("type_description")]
        public string TypeDescription { get; set; }

    }
}
