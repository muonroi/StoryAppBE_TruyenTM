using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.GroupName;

namespace MuonRoiSocialNetwork.Common.Settings.SignalRSettings.SignleName
{
    public class SingleHelperConst
    {
        public SingleHelperConst()
        {

        }
        public static SingleHelperConst Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly SingleHelperConst instance = new();
        }
        public readonly string StreamUserSpecial = "ReceiveNotificationByUser";
    }
}
