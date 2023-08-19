using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Chapter.Base.Request
{
    public class BaseChapterModelRequest
    {
        [JsonProperty("chapter_title")]
        public string ChapterTitle { get; set; } = string.Empty;
        [JsonProperty("body")]
        public string Body { get; set; } = string.Empty;
        [JsonProperty("number_of_chapter")]
        public long NumberOfChapter { get; set; }
        [JsonProperty("story_id")]
        public int StoryId { get; set; }
    }
}
