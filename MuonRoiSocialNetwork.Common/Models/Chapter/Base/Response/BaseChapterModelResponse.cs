
using Newtonsoft.Json;
namespace MuonRoiSocialNetwork.Common.Models.Chapter.Base.Response
{
    public class BaseChapterModelResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("chapterTitle")]
        public string ChapterTitle { get; set; } = string.Empty;

        [JsonProperty("body")]
        public string Body { get; set; } = string.Empty;

        [JsonProperty("numberOfChapter")]
        public long NumberOfChapter { get; set; }

        [JsonProperty("numberOfWord")]
        public int NumberOfWord { get; set; }

        [JsonProperty("storyId")]
        public int StoryId { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonProperty("createdDateTS")]
        public double? CreatedDateTS { get; set; }

        [JsonProperty("updatedDateTS")]
        public double? UpdatedDateTS { get; set; }

        [JsonProperty("createdUserName")]
        public string? CreatedUserName { get; set; }

        [JsonProperty("updatedUserName")]
        public string? UpdatedUserName { get; set; }
    }
}
