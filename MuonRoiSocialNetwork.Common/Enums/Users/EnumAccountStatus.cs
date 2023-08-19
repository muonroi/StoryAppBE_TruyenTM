namespace MuonRoi.Social_Network.Users
{
    public enum EnumAccountStatus
    {
        /// <summary>
        /// No change
        /// </summary>
        None = 0,
        /// <summary>
        /// Active
        /// </summary>
        Active = 1,

        /// <summary>
        /// Deactive
        /// </summary>
        InActive = 2,

        /// <summary>
        /// Lock account
        /// </summary>
        Locked = 3,
        /// <summary>
        /// Confirmed email
        /// </summary>
        Confirmed = 4,
        /// <summary>
        /// Non confirm email
        /// </summary>
        UnConfirm = 5,
        /// <summary>
        /// Account is online
        /// </summary>
        IsOnl = 6,
        /// <summary>
        /// Account is offline
        /// </summary>
        IsOf = 7,
        /// <summary>
        /// Is renewPassword
        /// </summary>
        IsRenew = 8,
    }
}
