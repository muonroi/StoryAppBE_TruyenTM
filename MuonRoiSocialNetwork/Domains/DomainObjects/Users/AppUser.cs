using BaseConfig.EntityObject.Entity;
using BaseConfig.EntityObject.EntityObject;
using MuonRoi.Social_Network.Roles;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.User;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;
using MuonRoiSocialNetwork.Domains.DomainObjects.Users;
using MuonRoiSocialNetwork.Domains.DomainObjects.Storys;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.IdentityModel.Tokens;

namespace MuonRoi.Social_Network.Users
{
    /// <summary>
    /// Table User Members
    /// </summary>
    [Table("appuser")]
    public class AppUser : IdentityUser<Guid>
    {
        /// <summary>
        /// FirstName''s User
        /// </summary>
        [Required(ErrorMessage = nameof(EnumUserErrorCodes.USR03C))]
        [MaxLength(100, ErrorMessage = nameof(EnumUserErrorCodes.USR08C))]
        [Column("name")]
        public string? Name { get; set; }
        /// <summary>
        /// LastName''s User
        /// </summary>
        [Required(ErrorMessage = nameof(EnumUserErrorCodes.USR04C))]
        [MaxLength(100, ErrorMessage = nameof(EnumUserErrorCodes.USR09C))]
        [Column("surname")]
        public string? Surname { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = nameof(EnumUserErrorCodes.USR05C))]
        [MaxLength(100, ErrorMessage = nameof(EnumUserErrorCodes.USR10C))]
        [MinLength(5, ErrorMessage = nameof(EnumUserErrorCodes.USR15C))]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_\.]{3,99}[a-z0-9](\@([a-zA-Z0-9][a-zA-Z0-9\.]+[a-zA-Z0-9]{2,}){1,5})?$", ErrorMessage = nameof(EnumUserErrorCodes.USR14C))]
        [Column("username")]
        public override string UserName { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = nameof(EnumUserErrorCodes.USR06C))]
        [MaxLength(1000, ErrorMessage = nameof(EnumUserErrorCodes.USR11C))]
        [MinLength(8, ErrorMessage = nameof(EnumUserErrorCodes.USR26C))]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = nameof(EnumUserErrorCodes.USR17C))]
        [Column("password_hash")]
        public override string? PasswordHash { get; set; }
        /// <summary>
        /// Email address
        /// </summary>
        /// <value></value>
        [MaxLength(1000, ErrorMessage = nameof(EnumUserErrorCodes.USR20C))]
        [RegularExpression(@"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$", ErrorMessage = nameof(EnumUserErrorCodes.USR19C))]
        [Column("email")]
        public override string? Email { get; set; }
        /// <summary>
        /// Address''s User
        /// </summary>
        [MaxLength(1000, ErrorMessage = nameof(EnumUserErrorCodes.USR18C))]
        [Column("address")]
        public string? Address { get; set; }
        /// <summary>
        /// BirthDate''s User
        /// </summary>
        [Required(ErrorMessage = nameof(EnumUserErrorCodes.USRC38C))]
        [Column("birthdate")]
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// Key hash password
        /// </summary>
        [MaxLength(1000, ErrorMessage = nameof(EnumUserErrorCodes.USR12C))]
        [Column("salf")]
        public string? Salt { get; set; }
        /// <summary>
        /// Gender''s User
        /// </summary>
        [Column("gender")]
        public EnumGender Gender { get; set; }
        /// <summary>
        /// Last login date
        /// </summary>
        /// <value></value>
        [Column("last_login")]
        public DateTime? LastLogin { get; set; }
        /// <summary>
        /// Location user last login
        /// </summary>
        [MaxLength(1000)]
        [Column("last_login_location")]
        public string? LastLoginLocation { get; set; } = string.Empty;
        /// <summary>
        /// Avatar Link
        /// </summary>
        /// <value></value>
        [MaxLength(1000, ErrorMessage = nameof(EnumNotificationStoryErrorCodes.NT01))]
        [Column("avatar")]
        public string? Avatar { get; set; }

        /// <summary>
        /// Status of account
        /// </summary>
        /// <value></value>
        [Required(ErrorMessage = nameof(EnumUserErrorCodes.USR22C))]
        [Column("status")]
        public EnumAccountStatus Status { get; set; }

        /// <summary>
        /// Ghi chú cho tài khoản
        /// </summary>
        /// <value></value>
        [Column("note")]
        public string? Note { get; set; }

        /// <summary>
        /// Lý do khóa tài khoản
        /// </summary>
        /// <value></value>
        [Column("lock_reason")]
        public string? LockReason { get; set; }

        /// <summary>
        /// GroupId of account
        /// </summary>
        [Column("group_id")]
        public long? GroupId { get; set; }
        /// <summary>
        /// Account status isOn | isOf
        /// </summary>
        [Column("account_status")]
        public EnumAccountStatus AccountStatus { get; set; }
        /// <summary>
        /// Number request send mail
        /// </summary>
        [Column("count_request_sendmail")]
        public int CountRequestSendMail { get; set; }
        /// <summary>
        /// CreatedDateTS
        /// </summary>
        [Column("created_date")]
        public double? CreatedDateTS { get; set; }
        /// <summary>
        /// UpdatedDateTS
        /// </summary>
        [Column("updated_date")]
        public double? UpdatedDateTS { get; set; }
        /// <summary>
        /// DeletedDateTS
        /// </summary>
        [Column("deleted_date")]
        public double? DeletedDateTS { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public GroupUserMember? GroupUserMember { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<BookmarkStory>? BookMarkStory { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<StoryNotifications>? StoryNotifications { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<StoryPublish>? StoryPublish { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<StoryReview>? StoryReview { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<FollowingAuthor>? FollowingAuthor { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public UserLogin? UserLoggin { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<StoryFavorite>? StoryFavorite { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        protected List<ErrorResult> _errorMessages = new();

        /// <summary>
        /// ErrorMessages
        /// </summary>
        [JsonIgnore]
        public IReadOnlyCollection<ErrorResult> ErrorMessages => _errorMessages;
        /// <summary>
        /// IsValid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            ValidationContext validationContext = new(this, null, null);
            List<ValidationResult> list = new();
            if (!Validator.TryValidateObject(this, validationContext, list, validateAllProperties: true))
            {
                foreach (ValidationResult item in list)
                {
                    ErrorResult errorResult = new()
                    {
                        ErrorCode = item.ErrorMessage,
                        ErrorMessage = Helpers.GetErrorMessage(item.ErrorMessage ?? string.Empty)
                    };
                    foreach (string memberName in item.MemberNames)
                    {
                        PropertyInfo? property = validationContext.ObjectType.GetProperty(memberName);
                        object? value = property?.GetValue(validationContext.ObjectInstance, null);
                        errorResult.ErrorValues.Add(Helpers.GenerateErrorResult(memberName, value));
                    }

                    _errorMessages.Add(errorResult);
                }
            }

            return _errorMessages.Count == 0;
        }
    }
}