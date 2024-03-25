namespace XmobiTea.EUN.Extensions
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;
    using System.Threading.Tasks;
    using XmobiTea.EUN.Common;

    public static class RoomPlayerExtensions
    {
        /// <summary>
        /// Change new client in room
        /// Only leader client room can change new client
        /// If request success, the EUNManagerBehaviour.OnEUNLeaderClientChange(), EUNBehaviour.OnEUNLeaderClientChange() in other client in this room will callback
        /// </summary>
        /// <param name="newLeaderClient">The new Leader Client</param>
        /// <param name="onResponse"></param>
        public static void changeLeaderClient(this RoomPlayer newLeaderClient, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
            EUNNetwork.changeLeaderClient(newLeaderClient.playerId, onResponse);
        }

        public static async Task<ChangeLeaderClientOperationResponse> changeLeaderClientAsync(this RoomPlayer newLeaderClient)
        {
            ChangeLeaderClientOperationResponse waitingResult = null;

            changeLeaderClient(newLeaderClient, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change the player custom properties of roomPlayer in this room
        /// If request success, the EUNManagerBehaviour.OnEUNCustomPlayerPropertiesChange(), EUNBehaviour.OnEUNCustomPlayerPropertiesChange() in other client in this room callback
        /// </summary>
        /// <param name="roomPlayer">The room player you want to change</param>
        /// <param name="customPlayerProperties">The custom player properties change</param>
        /// <param name="onResponse"></param>
        public static void changeCustomProperties(this RoomPlayer roomPlayer, EUNHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
            EUNNetwork.changePlayerCustomProperties(roomPlayer.playerId, customPlayerProperties, onResponse);
        }

        public static async Task<ChangePlayerCustomPropertiesOperationResponse> changeCustomPropertiesAsync(this RoomPlayer roomPlayer, EUNHashtable customPlayerProperties)
        {
            ChangePlayerCustomPropertiesOperationResponse waitingResult = null;

            changeCustomProperties(roomPlayer, customPlayerProperties, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Transfer new onwer game object room in eunView, to the roomPlayer in this room
        /// If request success, the EUNManagerBehaviour.OnEUNTransferOwnerGameObject() and EUNBehaviour.OnEUNTransferOwnerGameObject() callback
        /// </summary>
        /// <param name="newOwner">The new owner in current room for this eunView</param>
        /// <param name="eunView">The eunView need change new owner</param>
        /// <param name="onResponse"></param>
        public static void transferOwnerGameObjectRoom(this RoomPlayer roomPlayer, EUNView eunViewWillOwned, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.transferOwnerGameObjectRoom(eunViewWillOwned.roomGameObject.objectId, roomPlayer.playerId, onResponse);
        }

        public static async Task<TransferOwnerGameObjectRoomOperationResponse> transferOwnerGameObjectRoomAsync(this RoomPlayer roomPlayer, EUNView eunViewWillOwned)
        {
            TransferOwnerGameObjectRoomOperationResponse waitingResult = null;

            transferOwnerGameObjectRoom(roomPlayer, eunViewWillOwned, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

    }

}
