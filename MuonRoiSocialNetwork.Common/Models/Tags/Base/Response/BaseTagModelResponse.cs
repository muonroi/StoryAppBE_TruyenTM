using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Tags.Base.Response
{
    public class BaseTagModelResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; } = string.Empty;

        [JsonProperty("details")]
        public string Details { get; set; } = string.Empty;
    }
}
