using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Base.Request
{
    public class BaseStoryModelResquest
    {
        [JsonProperty("story_Title")]
        public string StoryTitle { get; set; } = string.Empty;

        [JsonProperty("story_Synopsis")]
        public string StorySynopsis { get; set; } = string.Empty;

        [JsonProperty("authorName")]
        public string AuthorName { get; set; } = string.Empty;

        [JsonProperty("isShow")]
        public bool IsShow { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
    }
}
