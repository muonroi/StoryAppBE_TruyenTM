using Newtonsoft.Json;
using System;

namespace MuonRoiSocialNetwork.Common.Models.TagInStories.Base.Response
{
    public class BaseTagInStoriesModelResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("storyGuid")]
        public Guid StoryGuid { get; set; }

        [JsonProperty("tagId")]
        public int TagId { get; set; }
    }
}
