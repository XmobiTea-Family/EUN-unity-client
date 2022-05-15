namespace XmobiTea.EUN
{
#if EUN
    using com.tvd12.ezyfoxserver.client.constant;
#else
    using XmobiTea.EUN.Entity.Support;
#endif
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;

    using UnityEngine;

    public class EUNManagerBehaviour : MonoBehaviour, IEUNManagerBehaviour
    {
        protected virtual void Awake()
        {
            EUNNetwork.SubscriberEUNBehaviour(this);
        }

        protected virtual void OnEnable() { }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnDisable() { }

        protected virtual void OnDestroy()
        {
            EUNNetwork.UnSubscriberEUNBehaviour(this);
        }

        public virtual void OnEUNCustomPlayerPropertiesChange(RoomPlayer player, EUNHashtable customPropertiesChange) { }

        public virtual void OnEUNCustomRoomPropertiesChange(EUNHashtable customPropertiesChange) { }

        public virtual void OnEUNCustomGameObjectPropertiesChange(RoomGameObject roomGameObject, EUNHashtable customPropertiesChange) { }

        public virtual void OnEUNZoneConnected() { }

        public virtual void OnEUNConnected() { }

        public virtual void OnEUNLoginError() { }

        public virtual void OnEUNConnectionFailure(EzyConnectionFailedReason reason) { }

        public virtual void OnEUNDisconnected(EzyDisconnectReason reason) { }

        public virtual void OnEUNJoinLobby() { }

        public virtual void OnEUNLeftLobby() { }

        public virtual void OnEUNJoinRoom() { }

        public virtual void OnEUNLeaderClientChange(RoomPlayer newLeaderClientPlayer) { }

        public virtual void OnEUNLeftRoom() { }

        public virtual void OnEUNOtherPlayerJoinRoom(RoomPlayer player) { }

        public virtual void OnEUNOtherPlayerLeftRoom(RoomPlayer player) { }

        public virtual void OnEUNReceiveChatAll(ChatMessage message) { }

        public virtual void OnEUNReceiveChatLobby(ChatMessage message) { }

        public virtual void OnEUNReceiveChatRoom(ChatMessage message, RoomPlayer sender) { }

        public virtual void OnEUNRoomInfoChange(EUNHashtable customPropertiesChange) { }

        public virtual EUNView OnEUNViewNeedCreate(RoomGameObject roomGameObject) { return null; }

        public virtual void OnEUNTransferOwnerGameObject(RoomGameObject roomGameObject, RoomPlayer newOwner) { }
    }
}
