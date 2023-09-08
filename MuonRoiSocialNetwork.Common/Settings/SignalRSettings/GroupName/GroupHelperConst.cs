namespace MuonRoiSocialNetwork.Common.Settings.SignalRSettings.GroupName
{
    public sealed class GroupHelperConst
    {
        public GroupHelperConst()
        {

        }
        public static GroupHelperConst Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly GroupHelperConst instance = new();
        }
        public readonly string StreamGlobal = "ReceiveGlobalNotification";
        public readonly string StreamNameFavorite = "ReceiveNotificationFromFavorite";
        public readonly string StreamNameSingle = "ReceiveSingleNotification";
        public readonly string GroupNameFavorite = "StoryFavoritePublished_{0}";
        public readonly string GroupNameVoteHear = "VoteHeartToAuthor_{0}";
    }
}
