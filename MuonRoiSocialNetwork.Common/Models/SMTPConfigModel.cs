namespace MuonRoiSocialNetwork.Common.Models
{
    public class SMTPConfigModel
    {
        /// <summary>
        /// User send
        /// </summary>
        public string? SenderAddress { get; set; }
        /// <summary>
        /// Display name user send
        /// </summary>
        public string? SenderDisplayName { get; set; }
        /// <summary>
        /// User name user send
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Host send mail default is: smtp.gmail.com
        /// </summary>
        public string? Host { get; set; }
        /// <summary>
        /// Port send mail default is: 587
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Is use SSL? 
        /// </summary>
        public bool EnableSSL { get; set; }
        /// <summary>
        /// Is use credentials?
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
        /// <summary>
        /// Body display mail use html design
        /// </summary>
        public bool IsBodyHTML { get; set; }
    }
}
