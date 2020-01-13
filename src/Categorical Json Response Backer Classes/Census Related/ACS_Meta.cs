using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.  Used when dealing with Census info.
    /// </summary>
    public class ACS_Meta
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("survey_years")]
        public string SurveyYears { get; set; }

        [JsonProperty("survey_duration_years")]
        public string SurveyDurationYears { get; set; }
    }
}
