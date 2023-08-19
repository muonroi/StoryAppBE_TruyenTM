using Newtonsoft.Json;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;

namespace MuonRoiSocialNetwork.Common.Models.Users.Response
{
    public class UserModelResponse : BaseUserResponse
    {
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("jwtToken")]
        public string? JwtToken { get; set; }

        [JsonProperty("refreshToken")]
        public string? RefreshToken { get; set; }

        [JsonProperty("locationUserLogin")]
        public LocationUserLogin? LocationUserLogin { get; set; }
    }
}
