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
    }
}
