using Newtonsoft.Json;
using System;

namespace MuonRoiSocialNetwork.Common.Models.TagInStories.Base.Response
{
    public class BaseTagInStoriesModelResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("storyid")]
        public int StoryId { get; set; }

        [JsonProperty("tagid")]
        public int TagId { get; set; }
    }
}
