using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
    /// <summary>
    /// JSON backer class used when deserializing responses from Geocodio.
    /// </summary>
    public class References
    {
        [JsonConstructor]
        public References(string bioguideid, string thomasid
                          , string opensecretsid, string lisid
                          , string cspanid, string govtrackid, string votesmartid
                          , string ballotpediaid, string washingtonpostid
                          , string icpsrid, string wikipediaid)
        {
            BioguideId = bioguideid;
            ThomasId = thomasid;
            OpenSecretsId = opensecretsid;
            LisId = lisid;
            CspanId = cspanid;
            GovtrackId = govtrackid;
            VotesmartId = VotesmartId;
            BallotpediaId = ballotpediaid;
            WashingtonPostId = washingtonpostid;
            IcpsrId = icpsrid;
            WikipediaId = wikipediaid;
        }

        [JsonProperty("bioguide_id")]
        public string BioguideId { get; set; }
        [JsonProperty("thomas_id")]
        public string ThomasId { get; set; }
        [JsonProperty("opensecrets_id")]
        public string OpenSecretsId { get; set; }
        [JsonProperty("lis_id")]
        public string LisId { get; set; }
        [JsonProperty("cspan_id")]
        public string CspanId { get; set; }
        [JsonProperty("govtrack_id")]
        public string GovtrackId { get; set; }
        [JsonProperty("votesmart_id")]
        public string VotesmartId { get; set; }
        [JsonProperty("ballotpedia_id")]
        public string BallotpediaId { get; set; }
        [JsonProperty("washington_post_id")]
        public string WashingtonPostId { get; set; }
        [JsonProperty("icpsr_id")]
        public string IcpsrId { get; set; }
        [JsonProperty("wikipedia_id")]
        public string WikipediaId { get; set; }

    }
}
