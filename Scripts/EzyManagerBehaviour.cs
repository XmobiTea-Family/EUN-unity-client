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

    public class EzyManagerBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            EzyNetwork.SubscriberEzyBehaviour(this);
        }

        protected virtual void OnEnable() { }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnDisable() { }

        protected virtual void OnDestroy()
        {
            EzyNetwork.UnSubscriberEzyBehaviour(this);
        }

        public virtual void OnEzyCustomPlayerPropertiesChange(RoomPlayer player, CustomHashtable customPropertiesChange) { }

        public virtual void OnEzyCustomRoomPropertiesChange(CustomHashtable customPropertiesChange) { }

        public virtual void OnEzyCustomGameObjectPropertiesChange(RoomGameObject roomGameObject, CustomHashtable customPropertiesChange) { }

        public virtual void OnEzyZoneConnected() { }

        public virtual void OnEzyConnected() { }

        public virtual void OnEzyLoginError() { }

        public virtual void OnEzyConnectionFailure(EzyConnectionFailedReason reason) { }

        public virtual void OnEzyDisconnected(EzyDisconnectReason reason) { }

        public virtual void OnEzyJoinLobby() { }

        public virtual void OnEzyLeftLobby() { }

        public virtual void OnEzyJoinRoom() { }

        public virtual void OnEzyLeaderClientChange(RoomPlayer newLeaderClientPlayer) { }

        public virtual void OnEzyLeftRoom() { }

        public virtual void OnEzyOtherPlayerJoinRoom(RoomPlayer player) { }

        public virtual void OnEzyOtherPlayerLeftRoom(RoomPlayer player) { }

        public virtual void OnEzyReceiveChatAll(ChatMessage message) { }

        public virtual void OnEzyReceiveChatLobby(ChatMessage message) { }

        public virtual void OnEzyReceiveChatRoom(ChatMessage message, RoomPlayer sender) { }

        public virtual void OnEzyRoomInfoChange(CustomHashtable customPropertiesChange) { }

        public virtual EzyView OnEzyViewNeedCreate(RoomGameObject roomGameObject) { return null; }

        public virtual void OnEzyTransferOwnerGameObject(RoomGameObject roomGameObject, RoomPlayer newOwner) { }
    }
}
