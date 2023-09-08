using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoiSocialNetwork.Common.Models.Category.Base.Request
{
    public class BaseModelCategoryRequest
    {
        [JsonProperty("name_category")]
        public string? NameCategory { get; set; }
        [JsonProperty("icon")]
        public string IconName { get; set; } = string.Empty;
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }
    }
}
