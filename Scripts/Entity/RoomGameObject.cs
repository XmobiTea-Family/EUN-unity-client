namespace XmobiTea.EUN.Entity
{
    using System;
    using UnityEngine;
    using XmobiTea.EUN.Common;

    [Serializable]
    public class RoomGameObject
    {
        [SerializeField]
        private int _objectId = -1;
        /// <summary>
        /// The object id room game object
        /// </summary>
        public int objectId
        {
            get { return this._objectId; }
            set
            {
                this._objectId = value;
            }
        }

        [SerializeField]
        private int _ownerId;
        /// <summary>
        /// The owner player id for room game object
        /// </summary>
        public int ownerId
        {
            get { return this._ownerId; }
            set
            {
                this._ownerId = value;
            }
        }

        [SerializeField]
        private string _prefabPath;
        /// <summary>
        /// The prefab path for this room game object
        /// </summary>
        public string prefabPath
        {
            get { return this._prefabPath; }
            set
            {
                this._prefabPath = value;
            }
        }

        [SerializeField]
        private object _synchronizationData;
        /// <summary>
        /// The synchronization data
        /// </summary>
        public object synchronizationData
        {
            get { return this._synchronizationData; }
            set
            {
                this._synchronizationData = value;
            }
        }

        [SerializeField]
        private object _initializeData;
        /// <summary>
        /// The initialize data
        /// </summary>
        public object initializeData
        {
            get { return this._initializeData; }
            set
            {
                this._initializeData = value;
            }
        }

        [SerializeField]
        private EUNHashtable _customProperties;
        /// <summary>
        /// The room game object custom properties
        /// </summary>
        public EUNHashtable customProperties
        {
            get { return this._customProperties; }
            set
            {
                this._customProperties = value;
            }
        }

        public RoomGameObject(EUNArray eunArray)
        {
            this.objectId = eunArray.getInt(0);
            this.ownerId = eunArray.getInt(1);
            this.prefabPath = eunArray.getString(2);
            this.synchronizationData = eunArray.getObject(3);
            this.initializeData = eunArray.getObject(4);
            this.customProperties = eunArray.getEUNHashtable(5);
        }

        /// <summary>
        /// check the room game object is valid
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            return this._objectId > -1;
        }
    }

}
