using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class Bio
    {
        [JsonConstructor]
        public Bio(string lastname, string firstname, string birthday
                  , string gender, string party)
        {
            LastName = lastname;
            FirstName = firstname;
            Birthday = birthday;
            Gender = gender;
            Party = party;
        }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string Party { get; set; }

    }
}
