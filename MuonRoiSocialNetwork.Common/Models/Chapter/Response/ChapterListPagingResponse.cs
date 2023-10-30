
namespace MuonRoiSocialNetwork.Common.Models.Chapter.Response
{
    public class ChapterListPagingResponse
    {
        public int From { get; set; }
        public int To { get; set; }
        public long FromId { get; set; }
        public long ToId { get; set; }
        public int Index { get; set; }
        public long Total { get; set; }
    }
}
