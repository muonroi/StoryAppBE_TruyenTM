using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Request
{
    public class BookmarkStoryModelRequest
    {
        [JsonProperty("story-id")]
        public int StoryId { get; set; }
    }
}
