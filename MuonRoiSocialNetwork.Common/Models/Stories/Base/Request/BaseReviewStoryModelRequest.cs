using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Base.Request
{
    public class BaseReviewStoryModelRequest
    {
        [JsonProperty("storyId")]
        public int StoryId { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;
    }
}
