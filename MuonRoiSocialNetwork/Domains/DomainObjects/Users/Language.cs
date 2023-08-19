using MuonRoiSocialNetwork.Common.Enums.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoiSocialNetwork.Domains.DomainObjects.Users
{
    public class Language
    {
        [Column("lang")]
        public LanguageEnum LanguageName { get; set; }
    }
}
