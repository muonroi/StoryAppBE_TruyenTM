namespace MuonRoiSocialNetwork.Common.Models.Chapter.Response
{
    public class ChapterPreviewResponse
    {
        public long ChapterId { get; set; }
        public long NumberOfChapter { get; set; }
        public string ChapterName { get; set; } = string.Empty;
        public int Index { get; set; }
    }
}
