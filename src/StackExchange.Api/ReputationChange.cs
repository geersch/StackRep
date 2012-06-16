using System;
using Newtonsoft.Json;

namespace StackExchange.Api
{
    [WrapperObject("items")]
    [JsonObject(MemberSerialization.OptIn)]
    public class ReputationChange
    {
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "post_id")]
        public int PostId { get; set; }

        [JsonProperty(PropertyName = "post_type")]
        public string PostType { get; set; }

        [JsonProperty(PropertyName = "vote_type")]
        public string VoteType { get; set; }

        [JsonProperty(PropertyName = "reputation_change")]
        public int Change { get; set; }

        [JsonProperty(PropertyName = "on_date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime OnDate { get; set; }

        [JsonProperty(PropertyName = "title")]
        [JsonConverter(typeof(HtmlEncodedConverter))]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}