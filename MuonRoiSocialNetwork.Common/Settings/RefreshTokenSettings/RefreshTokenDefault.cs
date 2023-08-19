using MuonRoiSocialNetwork.Common.Settings.Appsettings;

namespace MuonRoiSocialNetwork.Common.Settings.RefreshTokenSettings
{
    public sealed class RefreshTokenDefault
    {
        public RefreshTokenDefault()
        {

        }
        public static RefreshTokenDefault Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly RefreshTokenDefault instance = new();
        }
        /// <summary>
        /// Key user info when login
        /// </summary>
        public readonly string keyUserModelResponseLogin = "ModelResponseLogin";
        /// <summary>
        /// Key user info when register
        /// </summary>
        public readonly string keyUserModelResponseRegister = "ModelResponseRegister";
        /// <summary>
        /// Life time expiration
        /// </summary>
        public readonly TimeSpan expirationTimeLogin = TimeSpan.FromMinutes(60);
        /// <summary>
        /// Life time slidingExpiration
        /// </summary>
        public readonly TimeSpan slidingExpirationLogin = TimeSpan.FromMinutes(65);
    }
}
