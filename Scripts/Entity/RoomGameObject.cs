namespace EUN.Entity
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
    using EUN.Common;
    using System;
    using UnityEngine;

    [Serializable]
    public class RoomGameObject
    {
        [SerializeField]
        private int objectId;
        public int ObjectId
        {
            get { return objectId; }
            set
            {
                objectId = value;
            }
        }

        [SerializeField]
        private int ownerId;
        public int OwnerId
        {
            get { return ownerId; }
            set
            {
                ownerId = value;
            }
        }

        [SerializeField]
        private string prefabPath;
        public string PrefabPath
        {
            get { return prefabPath; }
            set
            {
                prefabPath = value;
            }
        }

        [SerializeField]
        private object synchronizationData;
        public object SynchronizationData
        {
            get { return synchronizationData; }
            set
            {
                synchronizationData = value;
            }
        }

        [SerializeField]
        private object initializeData;
        public object InitializeData
        {
            get { return initializeData; }
            set
            {
                initializeData = value;
            }
        }

        [SerializeField]
        private CustomHashtable customProperties;
        public CustomHashtable CustomProperties
        {
            get { return customProperties; }
            set
            {
                customProperties = value;
            }
        }

#if EUN
        public RoomGameObject(EzyArray ezyArray)
        {
            ObjectId = ezyArray.get<int>(0);
            OwnerId = ezyArray.get<int>(1);
            PrefabPath = ezyArray.get<string>(2);
            SynchronizationData = ezyArray.get<object>(3);
            InitializeData = ezyArray.get<object>(4);
            CustomProperties = new CustomHashtable(ezyArray.get<EzyObject>(5));
        }
#endif
    }
}