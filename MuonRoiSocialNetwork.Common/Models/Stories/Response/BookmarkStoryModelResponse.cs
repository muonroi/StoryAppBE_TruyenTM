using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Response
{
    public class BookmarkStoryModelResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("story_guid")]
        public Guid StoryGuid { get; set; }
        [JsonProperty("user_guid")]
        public Guid UserGuid { get; set; }
    }
}
