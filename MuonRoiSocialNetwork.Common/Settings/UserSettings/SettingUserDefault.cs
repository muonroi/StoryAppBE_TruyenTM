namespace MuonRoiSocialNetwork.Common.Settings.UserSettings
{
    public sealed class SettingUserDefault
    {
        public SettingUserDefault()
        {

        }
        public static SettingUserDefault Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly SettingUserDefault instance = new();
        }
        /// <summary>
        /// The numbers maximum of fail login attemps for current user
        /// </summary>
        public readonly int loginAttempDefault = 5;
        /// <summary>
        /// The numbers maximum character when fotgot password 
        /// </summary>
        public readonly int genarePasswordDefaultCharacter = 8;
        /// <summary>
        /// The numbers maximum character of refreshToken
        /// </summary>
        public readonly int genareRefreshToken = 32;
        /// <summary>
        /// ExpiryDay of refresh token
        /// </summary>
        public readonly int refreshTokenExpiryDay = 7;
        /// <summary>
        /// The number hour when convert datetime utc to asia 
        /// </summary>
        public readonly int hourAsia = 7;
        /// <summary>
        /// Expiry time life of access token
        /// </summary>
        public readonly int minuteExpitryLogin = 15;
        /// <summary>
        /// Expiry time life of token email
        /// </summary>
        public readonly int minuteExpitryConfirmEmail = 5;
        /// <summary>
        /// The character use random password
        /// </summary>
        public readonly string alphabet = "ab$%cdefoGHvwBCiDEghi!$%^*jklkmnpuFx789y@zAK@#LMNO^^STUVPQqr^&XYRWZ1IJ456st0";
        /// <summary>
        /// Group default
        /// </summary>
        public readonly int groupDefault = 4;
        /// <summary>
        /// Role default
        /// </summary>
        public readonly Guid roleDefault = new("5EF7D163-8249-445C-8895-4EB97329AF7E");
        /// <summary>
        /// Number max when request send mail veritification
        /// </summary>
        public readonly int maxNumberRequestSendMail = 3;
        /// <summary>
        /// Number max when login fail
        /// </summary>
        public readonly int maxNumberLogin = 5;
        /// <summary>
        /// Expiry time life time of otp code
        /// </summary>
        public readonly TimeSpan minuteExpitryOtpCode = TimeSpan.FromMinutes(3);
    }
}
