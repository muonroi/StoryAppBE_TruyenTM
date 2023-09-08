using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Chapter.Response
{
    public class ChapterChunkResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("chapterTitle")]
        public string ChapterTitle { get; set; } = string.Empty;

        [JsonProperty("body")]
        public List<string> BodyChunk { get; set; }

        [JsonProperty("numberOfChapter")]
        public long NumberOfChapter { get; set; }

        [JsonProperty("numberOfWord")]
        public int NumberOfWord { get; set; }

        [JsonProperty("storyId")]
        public int StoryId { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
    }
}
