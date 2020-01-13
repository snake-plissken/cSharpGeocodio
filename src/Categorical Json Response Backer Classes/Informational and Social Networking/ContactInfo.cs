using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class ContactInfo
    {
        [JsonConstructor]
        public ContactInfo(string url, string address, string phone
                          , string contactform)
        {
            Url = url;
            Address = address;
            Phone = phone;
            ContactForm = contactform;
        }

        public string Url { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [JsonProperty("contact_form")]
        public string ContactForm { get; set; }
    }
}
