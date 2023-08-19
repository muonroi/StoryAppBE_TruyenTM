using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.TagInStories.Base.Request
{
    public class BaseTagInStoriesModelRequest
    {
        [JsonProperty("storyId")]
        public int StoryId { get; set; }

        [JsonProperty("tagId")]
        public int TagId { get; set; }
    }
}
