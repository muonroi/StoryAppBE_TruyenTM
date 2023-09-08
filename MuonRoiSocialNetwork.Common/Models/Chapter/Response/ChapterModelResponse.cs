using MuonRoiSocialNetwork.Common.Models.Chapter.Base.Response;
using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Chapter.Response
{
    public class ChapterModelResponse : BaseChapterModelResponse
    {
        [JsonProperty("body_chunk")]
        public List<string> BodyChunk { get; set; }
        [JsonProperty("chunk_size")]
        public int ChunkSize { get; set; }
    }
}
