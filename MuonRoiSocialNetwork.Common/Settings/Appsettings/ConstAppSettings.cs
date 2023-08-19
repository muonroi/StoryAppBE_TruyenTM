namespace MuonRoiSocialNetwork.Common.Settings.Appsettings
{
    public sealed class ConstAppSettings
    {
        public ConstAppSettings()
        {

        }
        public static ConstAppSettings Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly ConstAppSettings instance = new();
        }
        public readonly string CONNECTIONSTRING_DB = "ConnectionStrings:MuonRoi";
        public readonly string CONNECTIONSTRING_HANGFIRE = "ConnectionStrings:HangfireConnection";
        public readonly string CONNECTIONSTRING_REDIS = "ConnectionStrings:Redis";
        public readonly string APPLICATIONSERECT = "Application:SERECT";
        public readonly string ENV_ACCESSKEY = "Application:ENV_ACCESSKEY";
        public readonly string APPLICATIONAPPDOMAIN = "Application:AppDomain";
        public readonly string APPLICATIONEMAILCONFIRMED = "Application:EmailConfirmation";
        public readonly string ENV_SERECT = "Application:ENV_SERECT";
        public readonly string LIFE_TIME = "Application:LIFE_TIME";
        public readonly string WHATSAPP_ACESSTOKEN = "Whatsapp:AcessToken";
    }
}
