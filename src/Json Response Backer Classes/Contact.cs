using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class ContactInfo
	{
		public ContactInfo()
		{
		}

		public string Url { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		[JsonProperty("contact_form")]
		public string ContactForm { get; set; }
	}
}
