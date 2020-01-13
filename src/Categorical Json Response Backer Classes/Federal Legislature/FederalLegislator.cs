using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class FederalLegislator
    {
        [JsonConstructor]
        public FederalLegislator(string type, Bio bio, ContactInfo contactinfo
                                , SocialNetworkInfo socialnetworkinfo
                                , References references
                                , string source)
        {
            Type = type;
            Bio = bio;
            ContactInfo = contactinfo;
            SocialNetworkInfo = socialnetworkinfo;
            References = references;
            Source = source;
        }

        public string Type { get; set; }
        public Bio Bio { get; set; }
        [JsonProperty("contact")]
        public ContactInfo ContactInfo { get; set; }
        [JsonProperty("social")]
        public SocialNetworkInfo SocialNetworkInfo { get; set; }
        public References References { get; set; }
        public string Source { get; set; }

    }
}
