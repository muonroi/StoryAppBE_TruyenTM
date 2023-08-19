namespace MuonRoiSocialNetwork.Infrastructure.Extentions.Mail
{
    /// <summary>
    /// Body when send mail
    /// </summary>
    public class UserEmailOptions
    {
        /// <summary>
        /// Email to
        /// </summary>
        public List<string>? ToEmails { get; set; }
        /// <summary>
        /// Subjectt mail send
        /// </summary>
        public string? Subject { get; set; }
        /// <summary>
        /// Body mail
        /// </summary>
        public string? Body { get; set; }
        /// <summary>
        /// Placeholders mail
        /// </summary>
        public List<KeyValuePair<string, string>>? PlaceHolders { get; set; }
    }
}
