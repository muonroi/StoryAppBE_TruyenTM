using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Categories;
using MuonRoi.Social_Network.Chapters;
using MuonRoi.Social_Network.Tags;
using MuonRoiSocialNetwork.Domains.DomainObjects.Storys;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Storys
{
    /// <summary>
    /// Table Story
    /// </summary>
    public class Story : Entity
    {
        /// <summary>
        /// Guid in story 
        /// </summary>
        [Column("guid")]
        public override Guid Guid { get; set; }
        /// <summary>
        /// Title of story
        /// </summary>
        [Required(ErrorMessage = nameof(EnumStoryErrorCode.ST00))]
        [MaxLength(255, ErrorMessage = nameof(EnumStoryErrorCode.ST01))]
        [MinLength(3, ErrorMessage = nameof(EnumStoryErrorCode.ST02))]
        [Column("story_title")]
        public string StoryTitle { get; set; } = string.Empty;
        /// <summary> 
        /// Description of story
        /// </summary>
        [Required(ErrorMessage = nameof(EnumStoryErrorCode.ST03))]
        [MaxLength(5000, ErrorMessage = nameof(EnumStoryErrorCode.ST04))]
        [MinLength(100, ErrorMessage = nameof(EnumStoryErrorCode.ST05))]
        [Column("story_synopsis")]
        public string StorySynopsis { get; set; } = string.Empty;
        /// <summary>
        /// Url img of story
        /// </summary>
        [Required(ErrorMessage = nameof(EnumStoryErrorCode.ST06))]
        [MaxLength(1000, ErrorMessage = nameof(EnumNotificationStoryErrorCodes.NT01))]
        [Column("img_url")]
        public string ImgUrl { get; set; } = string.Empty;
        /// <summary>
        /// Is show ? true : false
        /// </summary>
        [Required(ErrorMessage = nameof(EnumStoryErrorCode.ST08))]
        [Column("is_show")]
        public bool IsShow { get; set; }
        /// <summary>
        /// Total view of story
        /// </summary>
        [Column("total_view")]
        public int TotalView { get; set; }
        /// <summary>
        /// Total like of story
        /// </summary>
        [Column("total_favorite")]
        public int TotalFavorite { get; set; }
        /// <summary>
        /// Rating of story
        /// </summary>
        [Column("rating")]
        public double Rating { get; set; }
        /// <summary>
        /// Rating list of story
        /// </summary>
        [Column("list_rattings")]
        public string ListRattings { get; set; } = string.Empty;
        /// <summary>
        /// Slug of story
        /// </summary>
        [Required(ErrorMessage = nameof(EnumStoryErrorCode.ST00))]
        [MaxLength(255, ErrorMessage = nameof(EnumStoryErrorCode.ST01))]
        [MinLength(3, ErrorMessage = nameof(EnumStoryErrorCode.ST02))]
        [Column("slug")]
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// Foreign key category
        /// </summary>
        [Column("category_id")]
        public long CategoryId { get; set; }
        /// <summary>
        /// Authorname of story
        /// </summary>
        [Column("author_name")]
        public string AuthorName { get; set; } = string.Empty;
        /// <summary>
        /// Total chapter of story
        /// </summary>
        [Column("total_chapter")]
        public int TotalChapter { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<Chapter>? Chapters { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public Category? Category { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<StoryNotifications>? StoryNotifications { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<TagInStory>? TagInStory { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public List<StoryFavorite>? StoryFavorite { get; set; }
    }
}
