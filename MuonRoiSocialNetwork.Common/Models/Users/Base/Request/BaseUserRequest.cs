using Newtonsoft.Json;
using MuonRoi.Social_Network.User;
using System;

namespace MuonRoiSocialNetwork.Common.Models.Users.Base.Request
{
    public class BaseUserRequest
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("surname")]
        public string? Surname { get; set; }

        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("gender")]
        public EnumGender Gender { get; set; }
    }
}
