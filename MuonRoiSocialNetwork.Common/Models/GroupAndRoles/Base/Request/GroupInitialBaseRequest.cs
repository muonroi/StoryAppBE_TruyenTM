using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Request
{
    public class GroupInitialBaseRequest
    {
        [JsonProperty("group_name")]
        public string GroupName { get; set; } = string.Empty;
    }
}
