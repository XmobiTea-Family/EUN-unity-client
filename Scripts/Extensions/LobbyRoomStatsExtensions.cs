namespace XmobiTea.EUN.Extensions
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;
    using System.Threading.Tasks;

    public static class LobbyRoomStatsExtensions
    {
        /// <summary>
        /// Join this room if you are not inroom
        /// If the password right, and the room valid, you can join room
        /// If request success, the EUNManagerBehaviour.OnEUNJoinRoom() callback
        /// </summary>
        /// <param name="lobbyRoomStats">The lobby room stats you want join room</param>
        /// <param name="password">The password for lobby room</param>
        /// <param name="onResponse"></param>
        public static void joinRoom(this LobbyRoomStats lobbyRoomStats, string password = null, Action<JoinRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.joinRoom(lobbyRoomStats.roomId, password, onResponse);
        }

        public static async Task<JoinRoomOperationResponse> joinRoomAsync(this LobbyRoomStats lobbyRoomStats, string password = null)
        {
            JoinRoomOperationResponse waitingResult = null;

            joinRoom(lobbyRoomStats, password, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

    }

}
