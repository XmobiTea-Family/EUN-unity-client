namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;
    using System;
    using UnityEngine;

    [Serializable]
    public class RoomGameObject
    {
        [SerializeField]
        private int _objectId;
        /// <summary>
        /// The object id room game object
        /// </summary>
        public int objectId
        {
            get { return _objectId; }
            set
            {
                _objectId = value;
            }
        }

        [SerializeField]
        private int _ownerId;
        /// <summary>
        /// The owner player id for room game object
        /// </summary>
        public int ownerId
        {
            get { return _ownerId; }
            set
            {
                _ownerId = value;
            }
        }

        [SerializeField]
        private string _prefabPath;
        /// <summary>
        /// The prefab path for this room game object
        /// </summary>
        public string prefabPath
        {
            get { return _prefabPath; }
            set
            {
                _prefabPath = value;
            }
        }

        [SerializeField]
        private object _synchronizationData;
        /// <summary>
        /// The synchronization data
        /// </summary>
        public object synchronizationData
        {
            get { return _synchronizationData; }
            set
            {
                _synchronizationData = value;
            }
        }

        [SerializeField]
        private object _initializeData;
        /// <summary>
        /// The initialize data
        /// </summary>
        public object initializeData
        {
            get { return _initializeData; }
            set
            {
                _initializeData = value;
            }
        }

        [SerializeField]
        private EUNHashtable _customProperties;
        /// <summary>
        /// The room game object custom properties
        /// </summary>
        public EUNHashtable customProperties
        {
            get { return _customProperties; }
            set
            {
                _customProperties = value;
            }
        }

        public RoomGameObject(EUNArray eunArray)
        {
            objectId = eunArray.GetInt(0);
            ownerId = eunArray.GetInt(1);
            prefabPath = eunArray.GetString(2);
            synchronizationData = eunArray.GetObject(3);
            initializeData = eunArray.GetObject(4);
            customProperties = eunArray.GetEUNHashtable(5);
        }

        /// <summary>
        /// check the room game object is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return _objectId != 0;
        }
    }
}
