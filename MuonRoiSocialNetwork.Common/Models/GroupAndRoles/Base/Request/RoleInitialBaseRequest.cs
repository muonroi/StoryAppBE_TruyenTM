using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Request
{
    public class RoleInitialBaseRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
    }
}
