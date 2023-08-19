using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Base.Response
{
    public class BaseStoryModelResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        [JsonProperty("story_title")]
        public string StoryTitle { get; set; } = string.Empty;

        [JsonProperty("story_synopsis")]
        public string StorySynopsis { get; set; } = string.Empty;

        [JsonProperty("img_url")]
        public string ImgUrl { get; set; } = string.Empty;

        [JsonProperty("is_show")]
        public bool IsShow { get; set; }

        [JsonProperty("total_view")]
        public int TotalView { get; set; }

        [JsonProperty("total_favorite")]
        public int TotalFavorite { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonProperty("category_name")]
        public string NameCategory { get; set; } = string.Empty;

        [JsonProperty("author_name")]
        public string AuthorName { get; set; } = string.Empty;

        [JsonProperty("nameTag")]
        public List<string> NameTag { get; set; } = new List<string>();

        [JsonProperty("updated_date_ts")]
        public double? UpdatedDateTs { get; set; }

        [JsonProperty("updated_date")]
        public string UpdatedDateString { get; set; } = string.Empty;
    }
}
