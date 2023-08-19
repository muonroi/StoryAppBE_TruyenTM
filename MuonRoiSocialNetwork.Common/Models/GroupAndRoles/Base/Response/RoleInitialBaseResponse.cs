using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response
{
    public class RoleInitialBaseResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
