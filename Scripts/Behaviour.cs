namespace XmobiTea.EUN
{
    using UnityEngine;

    [RequireComponent(typeof(EUNView))]
    public class Behaviour : MonoBehaviour
    {
        /// <summary>
        /// The eunView for this EUN Behaviour
        /// </summary>
        public EUNView eunView { get; private set; }

        void Awake()
        {
            OnCustomAwake();
        }

        void Start()
        {
            OnCustomStart();
        }

        void OnEnable()
        {
            OnCustomEnable();
        }

        void OnDisable()
        {
            OnCustomDisable();
        }

        void OnDestroy()
        {
            OnCustomDestroy();
        }

        /// <summary>
        /// This is a MonoBehaviour.Awake()
        /// </summary>
        protected virtual void OnCustomAwake()
        {
            if (eunView == null) eunView = GetComponent<EUNView>();
        }

        /// <summary>
        /// This is a MonoBehaviour.Start()
        /// </summary>
        protected virtual void OnCustomStart()
        {

        }

        /// <summary>
        /// This is a MonoBehaviour.OnEnable()
        /// </summary>
        protected virtual void OnCustomEnable()
        {

        }

        /// <summary>
        /// This is a MonoBehaviour.OnDisable()
        /// </summary>
        protected virtual void OnCustomDisable()
        {

        }

        /// <summary>
        /// This is a MonoBehaviour.OnDestroy()
        /// </summary>
        protected virtual void OnCustomDestroy()
        {

        }
    }
}
