namespace XmobiTea.EUN.Constant
{
    /// <summary>
    /// The targets will receive EUNRPC
    /// </summary>
    public enum EUNTargets : byte
    {
        /// <summary>
        /// All player in this room
        /// </summary>
        All = 4,

        /// <summary>
        /// All player in this room, but via server
        /// </summary>
        AllViaServer = 0,

        /// <summary>
        /// Others player but this client can not receive
        /// </summary>
        Others = 1,

        /// <summary>
        /// Only leaderclient can receive
        /// </summary>
        LeaderClient = 2,

        /// <summary>
        /// Only me can receive
        /// </summary>
        OnlyMe = 3,

    }

}
