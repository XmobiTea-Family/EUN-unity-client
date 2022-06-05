﻿namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class JoinLobbyOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.JoinLobby;

        protected override bool reliable => true;

        /// <summary>
        /// JoinLobbyOperationRequest
        /// </summary>
        /// <param name="lobbyId">The lobby id client want to join</param>
        /// <param name="timeout"></param>
        public JoinLobbyOperationRequest(int lobbyId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.LobbyId, lobbyId)
                .Build();
        }
    }
}
