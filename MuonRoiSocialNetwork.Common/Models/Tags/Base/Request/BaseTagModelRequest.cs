using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Tags.Base.Request
{
    public class BaseTagModelRequest
    {
        [JsonProperty("tagName")]
        public string TagName { get; set; } = string.Empty;

        [JsonProperty("details")]
        public string Details { get; set; } = string.Empty;
    }
}
