using System;
using Newtonsoft.Json;

namespace cSharpGeocodio.ForwardGeocodingObjects
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class ForwardGeoCodeResult
    {
        private ForwardGeoCodeResult() { }

        [JsonConstructor]
        public ForwardGeoCodeResult(ForwardGeoCodeInput input, GeoCodeInfo[] results)
        {
            Input = input;
            Results = results;
        }

        public ForwardGeoCodeInput Input { get; set; }
        public GeoCodeInfo[] Results { get; set; }
        public string[] _warnings { get; set; }
    }

}
