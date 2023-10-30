using MuonRoiSocialNetwork.Common.Models.Stories.Base.Response;
using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Response
{
    public class StoryModelResponse : BaseStoryModelResponse
    {
        [JsonProperty("rank_number")]
        public int RankNumber { get; set; }
        [JsonProperty("total_chapter")]
        public int TotalChapter { get; set; }
        [JsonProperty("total_vote")]
        public int TotalVote { get; set; }
        [JsonProperty("first_chapter_id")]
        public long? FirstChapterId { get; set; }
        [JsonProperty("last_chapter_id")]
        public long? LastChapterId { get; set; }
        [JsonProperty("slug_author")]
        public string? SlugAuthor { get; set; }
        [JsonProperty("is_bookmark")]
        public bool? IsBookmark { get; set; }
        [JsonProperty("total_pageindex")]
        public int TotalPageIndex { get; set; }
    }
}
