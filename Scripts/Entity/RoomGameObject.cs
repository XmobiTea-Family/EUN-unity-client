namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;
    using System;
    using UnityEngine;

    [Serializable]
    public class RoomGameObject
    {
        [SerializeField]
        private int objectId = -1;
        /// <summary>
        /// The object id room game object
        /// </summary>
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
        /// <summary>
        /// The owner player id for room game object
        /// </summary>
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
        /// <summary>
        /// The prefab path for this room game object
        /// </summary>
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
        /// <summary>
        /// The synchronization data
        /// </summary>
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
        /// <summary>
        /// The initialize data
        /// </summary>
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
        /// <summary>
        /// The room game object custom properties
        /// </summary>
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

        /// <summary>
        /// check the room game object is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return objectId > -1;
        }
    }
}
