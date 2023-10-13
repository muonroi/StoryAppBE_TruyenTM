using Newtonsoft.Json;
using MuonRoi.Social_Network.Users;

namespace MuonRoiSocialNetwork.Common.Models.Users.Base.Response
{
    public class BaseUserResponse
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("surname")]
        public string? Surname { get; set; }
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("username")]
        public string? Username { get; set; }
        [JsonProperty("email")]
        public string? Email { get; set; }
        [JsonProperty("address")]
        public string? Address { get; set; }
        [JsonProperty("emailConfirmed")]
        public bool EmailConfirmed { get; set; }
        [JsonProperty("lastLogin")]
        public DateTime LastLogin { get; set; }
        [JsonProperty("avatar")]
        public string? Avatar { get; set; }
        [JsonProperty("status")]
        public EnumAccountStatus Status { get; set; }
        [JsonProperty("accountStatus")]
        public EnumAccountStatus AccountStatus { get; set; }
        [JsonProperty("note")]
        public string? Note { get; set; }
        [JsonProperty("lockReason")]
        public string? LockReason { get; set; }
        [JsonProperty("groupId")]
        public int GroupId { get; set; }
        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }
        [JsonProperty("updateDate")]
        public DateTime UpdateDate { get; set; }
        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }
        [JsonProperty("roleName")]
        public string? RoleName { get; set; }
        [JsonProperty("groupName")]
        public string? GroupName { get; set; }
        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }
    }
}
