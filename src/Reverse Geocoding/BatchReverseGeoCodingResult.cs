using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class BatchReverseGeoCodingResult
    {
        [JsonConstructor]
        public BatchReverseGeoCodingResult(BatchReverseGeoCodeResponse[] results)
        {
            Results = results;
        }

        public BatchReverseGeoCodeResponse[] Results { get; set; }
    }

}
