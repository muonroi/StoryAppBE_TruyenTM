using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Category.Base.Request
{
    public class BaseModelCategoryRequest
    {
        [JsonProperty("name_category")]
        public string? NameCategory { get; set; }
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }
    }
}
