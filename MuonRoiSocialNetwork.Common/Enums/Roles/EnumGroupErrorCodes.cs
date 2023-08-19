namespace MuonRoi.Social_Network.Roles
{
    /// <summary>
    /// Role Error Code
    /// </summary>
    public enum EnumRoleErrorCodes
    {
        /// <summary>
        /// Danh sách quyền không được null
        /// </summary>
        ROL01C,

        /// <summary>
        /// Quyền không tồn tại
        /// </summary>
        ROL02C,

        /// <summary>
        /// Tên quyền không đúng
        /// </summary>
        ROL03C,

        /// <summary>
        /// Tên quyền bắt buộc nhập
        /// </summary>
        ROL04C,

        /// <summary>
        /// Tên quyền tối đa 100 kí tự
        /// </summary>
        ROL05C,

        /// <summary>
        /// Mô tả quyền không quá 1000 kí tự
        /// </summary>
        ROL06C,

        /// <summary>
        /// Tên quyền đã tồn tại
        /// </summary>
        ROL07C,
    }

    /// <summary>
    /// Group Error Codes
    /// </summary>
    public enum EnumGroupErrorCodes
    {
        /// <summary>
        /// Tên nhóm không được bỏ trống
        /// </summary>
        GRP01C,

        /// <summary>
        /// Mô tả nhóm không được quá 500 kí tự
        /// </summary>
        GRP02C,

        /// <summary>
        /// UserId không được bỏ trống
        /// </summary>
        GRP03C,

        /// <summary>
        /// Nhóm không tồn tại
        /// </summary>
        GRP04C,

        /// <summary>
        /// Không thể xóa khi còn user trong nhóm
        /// </summary>
        GRP05C,

        /// <summary>
        /// Tên nhóm không quá 100 kí tự
        /// </summary>
        GRP06C,

        /// <summary>
        /// Nhóm đang là cha nhóm khác Không thể xóa. (Xóa tất cả nhóm con liên quan trước khi xóa)
        /// </summary>
        GRP07C,

        /// <summary>
        /// Nhóm cha không được là cha của nhóm hiện tại
        /// </summary>
        GRP08C,

        /// <summary>
        /// Mã nhóm không được trống
        /// </summary>
        GRP09C,

        /// <summary>
        /// Mã nhóm không quá 100 kí tự
        /// </summary>
        GRP10C,

        /// <summary>
        /// Loại nhóm không được trống
        /// </summary>
        GRP11C,

        /// <summary>
        /// Loại nhóm không quá 100 kí tự
        /// </summary>
        GRP12C,

        /// <summary>
        /// Nhóm đã tồn tại
        /// </summary>
        GRP13C,

        /// <summary>
        /// Nhóm cha không tồn tại
        /// </summary>
        GRP14C,
    }

    /// <summary>
    /// Defines the EnumPermissionErrorCodes.
    /// </summary>
    public enum EnumPermissionErrorCodes
    {
        /// <summary>
        /// Tên nhóm quyền không được trống
        /// </summary>
        PER01C,

        /// <summary>
        /// Tên nhóm quyền không quá 100 kí tự
        /// </summary>
        PER02C,

        /// <summary>
        /// Mã nhóm quyền không được quá 100 kí tự
        /// </summary>
        PER03C,

        /// <summary>
        /// Mô tả nhóm quyền không được quá 500 kí tự
        /// </summary>
        PER04C,

        /// <summary>
        /// Chức năng nhóm quyền không được trống
        /// </summary>
        PER05C,

        /// <summary>
        /// Chức năng nhóm quyền không được quá 100 kí tự
        /// </summary>
        PER06C,

        /// <summary>
        /// Nhóm quyền đã tồn tại
        /// </summary>
        PER07C,

        /// <summary>
        /// Pagezie không quá 100 dòng
        /// </summary>
        PER08C,

        /// <summary>
        /// Nhóm quyền không tồn tại
        /// </summary>
        PER09C,

        /// <summary>
        /// Nhóm quyền cha không được là cha của nhóm quyền hiện tại
        /// </summary>
        PER10C,

        /// <summary>
        /// Nhóm quyền cha không tồn tại
        /// </summary>
        PER11C,

        /// <summary>
        /// Nhóm quyền đang là cha nhóm quyền khác không thể xóa. (Xóa các nhóm quyền con liên quan trước khi xóa)
        /// </summary>
        PER12C,

        /// <summary>
        /// Mã nhóm quyền không được trống
        /// </summary>
        PER13C,

        /// <summary>
        /// Danh sách nhóm quyền không được trống
        /// </summary>
        PER14C,

        /// <summary>
        /// Danh sách nhóm quyền không tồn tại
        /// </summary>
        PER15C,
    }
}
