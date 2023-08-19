using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Stories.Request.Search
{
    public class GetStoriesByCategoryRequest
    {
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
    }
}
