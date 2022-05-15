namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;
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
        private EUNHashtable customProperties;
        public EUNHashtable CustomProperties
        {
            get { return customProperties; }
            set
            {
                customProperties = value;
            }
        }

        public RoomGameObject(EUNArray eunArray)
        {
            ObjectId = eunArray.GetInt(0);
            OwnerId = eunArray.GetInt(1);
            PrefabPath = eunArray.GetString(2);
            SynchronizationData = eunArray.GetObject(3);
            InitializeData = eunArray.GetObject(4);
            CustomProperties = eunArray.GetEUNHashtable(5);
        }
    }
}
