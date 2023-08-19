using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Storys;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Chapters
{
    /// <summary>
    /// Table Chapter
    /// </summary>
    public class Chapter : Entity
    {
        /// <summary>
        /// Title
        /// </summary>
        [Required(ErrorMessage = nameof(EnumChapterErrorCode.CT00))]
        [MaxLength(255, ErrorMessage = nameof(EnumChapterErrorCode.CT01))]
        [MinLength(3, ErrorMessage = nameof(EnumChapterErrorCode.CT02))]
        [Column("chapter_title")]
        public string ChapterTitle { get; set; } = string.Empty;
        /// <summary>
        /// Content of story
        /// </summary>
        [Required(ErrorMessage = nameof(EnumChapterErrorCode.CT04))]
        [MaxLength(100000, ErrorMessage = nameof(EnumChapterErrorCode.CT05))]
        [MinLength(750, ErrorMessage = nameof(EnumChapterErrorCode.CT06))]
        [Column("body")]
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Number of each the chapter
        /// </summary>
        [Required(ErrorMessage = nameof(EnumChapterErrorCode.CT07))]
        [Column("number_of_chapter")]
        public long NumberOfChapter { get; set; }

        /// <summary>
        /// Sum character in chapter
        /// </summary>

        [Required(ErrorMessage = nameof(EnumChapterErrorCode.CT09))]
        [Column("number_of_word")]
        public int NumberOfWord { get; set; }

        [Required(ErrorMessage = nameof(EnumChapterErrorCode.CT09))]
        [Column("story_id")]
        public long StoryId { get; set; }
        /// <summary>
        /// Slug of chapter
        /// </summary>
        [Required(ErrorMessage = nameof(EnumChapterErrorCode.CT10))]
        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        public Story Story { get; set; }
    }
}
