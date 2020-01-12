using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class SocialNetworkInfo
	{
		[JsonConstructor]
		public SocialNetworkInfo(string rssurl, string twitter
		                         , string facebook, string youtube
		                         , string youtubeid)
		{
			RssUrl = rssurl;
			Twitter = twitter;
			Facebook = facebook;
			YouTube = youtube;
			YouTubeId = youtubeid;
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
