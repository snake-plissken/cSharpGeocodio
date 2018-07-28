using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class TimeZone
	{
		private TimeZone() { }

		[JsonConstructor]
		public TimeZone(string name, decimal utc_offset, bool observes_dst,
						 string abbreviation, string source)
		{
			this.Name = name;
			this.UTC_Offset = utc_offset;
			this.Observes_DST = observes_dst;
			this.Abbreviation = abbreviation;
			this.Source = source;
		}

		public string Name { get; set; }
		[JsonProperty("utc_offset")]
		public decimal UTC_Offset { get; set; }
		[JsonProperty("observes_dst")]
		public bool Observes_DST { get; set; }
		public string Abbreviation { get; set; }
		public string Source { get; set; }

	}
}
