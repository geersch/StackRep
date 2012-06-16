using Newtonsoft.Json;

namespace StackExchange.Api
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Error
    {
        [JsonProperty(PropertyName = "error_id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "error_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "error_message")]
        public string Message { get; set; }
    }
}
