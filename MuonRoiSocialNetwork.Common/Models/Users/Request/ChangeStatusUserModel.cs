using Newtonsoft.Json;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Request;

namespace MuonRoiSocialNetwork.Common.Models.Users.Request
{
    public class ChangeStatusUserModel : BaseUserRequest
    {
        [JsonProperty("accountStatus")]
        public EnumAccountStatus AccountStatus { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }
    }
}
