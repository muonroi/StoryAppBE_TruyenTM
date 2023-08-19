using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using MuonRoiSocialNetwork.Common.Models.Stories.Base.Request;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Request
{
    public class StoryModelRequest : BaseStoryModelResquest
    {
        /// <summary>
        /// Avatar raw
        /// </summary>
        [JsonProperty("avatarTemp")]
        public IFormFile? AvatarTemp { get; set; }
    }
}
