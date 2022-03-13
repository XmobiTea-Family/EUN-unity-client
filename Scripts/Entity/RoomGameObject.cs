namespace EUN.Entity
{
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

        public RoomGameObject(CustomArray customArray)
        {
            ObjectId = customArray.GetInt(0);
            OwnerId = customArray.GetInt(1);
            PrefabPath = customArray.GetString(2);
            SynchronizationData = customArray.GetObject(3);
            InitializeData = customArray.GetObject(4);
            CustomProperties = customArray.GetCustomHashtable(5);
        }
    }
}