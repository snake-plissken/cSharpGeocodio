using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class SocialNetworkInfo
	{
		public SocialNetworkInfo()
		{
		}

		[JsonProperty("rss_url")]
		public string RssUrl { get; set; }
		public string Twitter { get; set; }
		public string Facebook { get; set; }
		public string YouTube { get; set; }
		[JsonProperty("youtube_id")]
		public string YouTubeId { get; set; }
	}
}
