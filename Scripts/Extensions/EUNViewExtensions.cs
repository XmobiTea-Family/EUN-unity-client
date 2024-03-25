namespace XmobiTea.EUN.Extensions
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;
    using System.Threading.Tasks;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Common;

    public static class EUNViewExtensions
    {
        /// <summary>
        /// Transfer new onwer game object room in eunView, to the roomPlayer in this room
        /// If request success, the EUNManagerBehaviour.OnEUNTransferOwnerGameObject() and EUNBehaviour.OnEUNTransferOwnerGameObject() callback
        /// </summary>
        /// <param name="newOwner">The new owner in current room for this eunView</param>
        /// <param name="eunView">The eunView need change new owner</param>
        /// <param name="onResponse"></param>
        public static void transferOwnerGameObjectRoom(this EUNView eunView, RoomPlayer newOwner, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.transferOwnerGameObjectRoom(eunView.roomGameObject.objectId, newOwner.playerId, onResponse);
        }

        public static async Task<TransferOwnerGameObjectRoomOperationResponse> transferOwnerGameObjectRoomAsync(this EUNView eunView, RoomPlayer newOwner)
        {
            TransferOwnerGameObjectRoomOperationResponse waitingResult = null;

            transferOwnerGameObjectRoom(eunView, newOwner, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Send a RPC command by eunView
        /// If request success, the internal EUNBehaviour.EUNRPC() will call
        /// </summary>
        /// <param name="eunView">The eunView need send RPC</param>
        /// <param name="targets">The targets need send to</param>
        /// <param name="command">The command need send</param>
        /// <param name="rpcData">The rpc data attach with command</param>
        public static void rpc(this EUNView eunView, EUNTargets targets, EUNRPCCommand command, params object[] rpcData)
        {
            EUNNetwork.rpcGameObjectRoom(targets, eunView.roomGameObject.objectId, (int)command, rpcData);
        }

        /// <summary>
        /// Send destroy the game object room in eunView
        /// If request success, the EUNManagerBehaviour.OnEUNDestroyGameObjectRoom() and EUNBehaviour.OnEUNDestroyGameObjectRoom() callback
        /// </summary>
        /// <param name="eunView">The eunView need destroy</param>
        /// <param name="onResponse"></param>
        public static void destroyGameObjectRoom(this EUNView eunView, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.destroyGameObjectRoom(eunView.roomGameObject.objectId, onResponse);
        }

        public static async Task<DestroyGameObjectRoomRoomOperationResponse> destroyGameObjectRoomAsync(this EUNView eunView)
        {
            DestroyGameObjectRoomRoomOperationResponse waitingResult = null;

            destroyGameObjectRoom(eunView, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change the custom properties of roomGameObject
        /// If request success, the EUNManagerBehaviour.OnEUNCustomGameObjectPropertiesChange() and EUNBehaviour.OnEUNCustomGameObjectPropertiesChange() callback
        /// </summary>
        /// <param name="eunView">The room game object in eunView need change custom properties</param>
        /// <param name="customProperties">The custom properties you want change</param>
        /// <param name="onResponse"></param>
        public static void changeGameObjectCustomProperties(this EUNView eunView, EUNHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.changeGameObjectCustomProperties(eunView.roomGameObject.objectId, customProperties, onResponse);
        }

        public static async Task<ChangeGameObjectRoomOperationResponse> changeGameObjectCustomPropertiesAsync(this EUNView eunView, EUNHashtable customProperties)
        {
            ChangeGameObjectRoomOperationResponse waitingResult = null;

            changeGameObjectCustomProperties(eunView, customProperties, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

    }

}
