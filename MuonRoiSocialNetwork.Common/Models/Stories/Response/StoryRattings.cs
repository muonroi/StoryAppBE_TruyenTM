using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Response
{
    public class StoryRattings
    {
        [JsonProperty("data")]
        public List<Rattings> Data { get; set; } = new List<Rattings>();
    }
    public class Rattings
    {
        [JsonProperty("value")]
        public double RattingValues { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; } = string.Empty;
    }
}
