using Newtonsoft.Json;
using System;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Base.Response
{
    public class BaseReviewStoryModelResponse
    {
        [JsonProperty("displayNameUser")]
        public string DisplayNameUser { get; set; } = string.Empty;

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;

        [JsonProperty("createdDate")]
        public double CreatetedDate { get; set; }

        [JsonProperty("userGuidCreated")]
        public Guid UserGuidCreated { get; set; }
    }
}
