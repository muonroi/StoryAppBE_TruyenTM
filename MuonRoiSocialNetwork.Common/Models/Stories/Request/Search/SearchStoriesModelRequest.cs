using Newtonsoft.Json;
using MuonRoiSocialNetwork.Common.Enums.Chapters;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Request.Search
{
    public class SearchStoriesModelRequest
    {
        [JsonProperty("searchByTitle")]
        public string SearchByTitle { get; set; } = string.Empty;

        [JsonProperty("searchByCategory")]
        public int SearchByCategory { get; set; } = 0;

        [JsonProperty("searchByTagName")]
        public long[] SearchByTagName { get; set; } = Array.Empty<long>();

        [JsonProperty("searchByNumberChapter")]
        public EnumNumberChapter SearchByNumberChapter { get; set; } = EnumNumberChapter.None;

        [JsonProperty("isNewUpdate")]
        public bool IsNewUpdate { get; set; } = false;

        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; } = 1;

        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 20;
    }
}
