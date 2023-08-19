namespace MuonRoi.Social_Network.Chapters
{
    public enum EnumChapterErrorCode
    {
        /// <summary>
        /// Tiêu đề chương không được để trống
        /// </summary>
        CT00,
        /// <summary>
        /// Tiêu đề chương vượt quá kí tự quy định (255) 
        /// </summary>
        CT01,
        /// <summary>
        /// Tiêu đề chương phải từ 3 kí tự trở lên
        /// </summary>
        CT02,
        /// <summary>
        /// Nội dung truyện không được trống
        /// </summary>
        CT04,
        /// <summary>
        /// Nội dung truyện vượt quá kí tự quy định (100000)
        /// </summary>
        CT05,
        /// <summary>
        /// Nội dung truyện phải từ 750 kí tự trở lên
        /// </summary>
        CT06,
        /// <summary>
        /// Số chương không được để trống
        /// </summary>
        CT07,
        /// <summary>
        /// Số chương vượt quá kí tự quy định 200
        /// </summary>
        CT08,
        /// <summary>
        /// Số chữ trong truyện không được trống
        /// </summary>
        CT09,
        /// <summary>
        /// Đường dẫn chương truyện
        /// </summary>
        CT10,
        /// <summary>
        /// Chương không tồn tại
        /// </summary>
        CT11,
    }
}
