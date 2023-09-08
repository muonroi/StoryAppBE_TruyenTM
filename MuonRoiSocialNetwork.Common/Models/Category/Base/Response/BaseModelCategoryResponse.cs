using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoiSocialNetwork.Common.Models.Category.Base.Response
{
    public class BaseModelCategoryResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name_category")]
        public string NameCategory { get; set; } = string.Empty;
        [JsonProperty("icon")]
        public string IconName { get; set; } = string.Empty;
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }
    }
}
