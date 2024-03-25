namespace XmobiTea.EUN.Extensions
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;
    using System.Threading.Tasks;

    public static class LobbyStatsExtensions
    {
        /// <summary>
        /// Join this lobby
        /// If join lobby success, the EUNManagerBehaviour.OnEUNJoinLobby() will callback
        /// </summary>
        /// <param name="lobbyStats">This lobby stats</param>
        /// <param name="subscriberChat">You want join subscriber chat in this lobby</param>
        /// <param name="onResponse"></param>
        public static void joinLobby(this LobbyStats lobbyStats, bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            EUNNetwork.joinLobby(lobbyStats.lobbyId, subscriberChat, onResponse);
        }

        public static async Task<JoinLobbyOperationResponse> joinLobbyAsync(this LobbyStats lobbyStats, bool subscriberChat)
        {
            JoinLobbyOperationResponse waitingResult = null;

            joinLobby(lobbyStats, subscriberChat, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

    }

}
