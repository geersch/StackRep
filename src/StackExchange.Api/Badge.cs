using Newtonsoft.Json;

namespace StackExchange.Api
{
    [WrapperObject("items")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Badge
    {
        [JsonProperty(PropertyName = "badge_id")]
        public int BadgeId { get; set; }

        [JsonProperty(PropertyName = "rank")]
        public string Rank { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "award_count")]
        public int AwardCount { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}