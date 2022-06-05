namespace XmobiTea.EUN.Entity
{
    using System;
    using UnityEngine;

    public enum ObservedType
    {
        Off = 0,
        OnlyEditor = 1,
        RuntimeAndEditor = 2
    }

    [Serializable]
    public struct ObservedComponent
    {
        [SerializeField]
        private ObservedType _type;
        public ObservedType type => _type;

        [SerializeField]
        private EUNBehaviour _eunBehaviour;
        public EUNBehaviour eunBehaviour => _eunBehaviour;
    }
}
