namespace MuonRoi.Social_Network.Storys
{
    public enum EnumStoryErrorCode
    {
        /// <summary>
        /// Tiêu đề truyện không được để trống
        /// </summary>
        ST00,
        /// <summary>
        /// Tiêu đề truyện vượt quá kí tự tối đa (255)
        /// </summary>
        ST01,
        /// <summary>
        /// Tiêu đề truyện tối thiểu 3 kí tự
        /// </summary>
        ST02,
        /// <summary>
        /// Mô tả về truyện không được để trống
        /// </summary>
        ST03,
        /// <summary>
        /// Mô tả truyện vượt quá kí tự quy định (5000)
        /// </summary>
        ST04,
        /// <summary>
        /// Mô tả truyện tối thiểu phải từ 100 kí tự
        /// </summary>
        ST05,
        /// <summary>
        /// Đường dẫn ảnh không được để trống
        /// </summary>
        ST06,
        /// <summary>
        /// Vui lòng chọn trạng thái truyện (Công khai | Riêng tư)
        /// </summary>
        ST08,
        /// <summary>
        /// Tên truyện đã tồn tại
        /// </summary>
        ST09,

        /// <summary>
        /// Truyện không tồn tại
        /// </summary>
        ST10,

        /// <summary>
        /// Có lỗi xảy ra khi thêm truyện. Vui lòng thử lại
        /// </summary>
        ST11,

        /// <summary>
        /// Số lượng dữ liệu lấy ra phải nhỏ hơn hoặc bằng 200
        /// </summary>
        ST12,

        /// <summary>
        /// Cập nhật lượt xem không thành công
        /// </summary>
        ST13,

        /// <summary>
        /// Truyện đã được thích
        /// </summary>
        ST14,
        /// <summary>
        /// Nội dung cập nhật trống
        /// </summary>
        ST15,
    }
    public enum EnumNotificationStoryErrorCodes
    {
        /// <summary>
        /// Đường dẫn thông báo không được trống
        /// </summary>
        NT00,
        /// <summary>
        /// Độ dài tối đa của đường dẫn là 1000 kí tự
        /// </summary>
        NT01,
        /// <summary>
        /// Tiêu đề không được để trống
        /// </summary>
        NT02,
        /// <summary>
        /// Tiêu đề vượt quá kí tự quy định 200 kí tự
        /// </summary>
        NT03,
        /// <summary>
        /// Nội dung không được để trống
        /// </summary>
        NT04,
        /// <summary>
        /// Nội dung vượt quá kí tự quy định 350 kí tự
        /// </summary>
        NT05,
        /// <summary>
        /// Không có thông báo
        /// </summary>
        NT06,
    }
}
