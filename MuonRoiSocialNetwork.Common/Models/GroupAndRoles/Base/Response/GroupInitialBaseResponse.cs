using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response
{
    public class GroupInitialBaseResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("group_name")]
        public string GroupName { get; set; } = string.Empty;
    }
}
