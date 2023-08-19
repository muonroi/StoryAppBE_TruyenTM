namespace MuonRoi.Social_Network.Tags
{
    public enum EnumTagsErrorCode
    {
        /// <summary>
        /// Tên thẻ không được trống
        /// </summary>
        TT00,
        /// <summary>
        /// Tên thẻ quá dài (nhỏ hơn hoặc bằng 50 kí tự)
        /// </summary>
        TT01,
        /// <summary>
        /// Tên thẻ phải từ 3 kí tự trở lên
        /// </summary>
        TT02,
        /// <summary>
        /// Mô tả thẻ không được trống
        /// </summary>
        TT03,
        /// <summary>
        /// Mô tả thẻ quá dài ( nhỏ hơn hoặc bằng 500 kí tự)
        /// </summary>
        TT04,
        /// <summary>
        /// Vui lòng chọn thẻ
        /// </summary>
        TT05,
        /// <summary>
        /// Vui lòng chọn truyện
        /// </summary>
        TT06,
        /// <summary>
        /// Vui lòng chọn chương truyện
        /// </summary>
        TT07,
        /// <summary>
        /// Thẻ không tồn tại
        /// </summary>
        TT08,
        /// <summary>
        /// Thêm thẻ thất bại.Vui lòng thử lại
        /// </summary>
        TT09,
        /// <summary>
        /// Số lượng thẻ phải nhỏ hơn 30
        /// </summary>
        TT10,
        /// <summary>
        /// Thẻ đã tồn tại
        /// </summary>
        TT11,
    }
}
