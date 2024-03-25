namespace XmobiTea.EUN.Unity
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.constant;
#if !UNITY_EDITOR && UNITY_WEBGL
    using XmobiTea.EUN.Plugin.WebGL;
#endif
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Networking;
    using XmobiTea.EUN.Bride;
    using XmobiTea.EUN.Common;
    using System.Collections.Generic;
    using XmobiTea.EUN.Helper;
    using UnityEngine.SceneManagement;

    internal class ServiceUpdate : UnityEngine.MonoBehaviour
    {
        internal NetworkingPeer peer;

        public void Start()
        {
            SceneManager.sceneLoaded -= this.onSceneLoaded;
            SceneManager.sceneLoaded += this.onSceneLoaded;
        
        }

        private void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            StartCoroutine(this.ieOnSceneLoaded(scene, loadSceneMode));
        }

        System.Collections.IEnumerator ieOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            yield return new UnityEngine.WaitForSeconds(0.1f);

            peer?.onSceneLoaded();
        }

        void Update()
        {
            this.peer?.service();
        }


        private void handleOnConnectionSuccess()
        {
            EUNSocketObject.onConnectionSuccess?.Invoke();
        }

        private void handleOnDisconnection(int reason)
        {
            EUNSocketObject.onDisconnection?.Invoke((EzyDisconnectReason)reason);
        }

        private void handleOnConnectionFailure(int reason)
        {
            EUNSocketObject.onConnectionFailure.Invoke((EzyConnectionFailedReason)reason);
        }

        private void handleOnLoginError(string jsonArr)
        {
            EUNSocketObject.onLoginError?.Invoke(new EUNArray.Builder().addAll(getObjectLstFromJsonArr(jsonArr) as System.Collections.IList).build());
        }

        private void handleOnAppAccess(string jsonArr)
        {
            EUNSocketObject.onAppAccess?.Invoke(new EUNArray.Builder().addAll(getObjectLstFromJsonArr(jsonArr) as System.Collections.IList).build());
        }

        private void handleOnResponse(string jsonArr)
        {
            EUNSocketObject.onResponse?.Invoke(new EUNArray.Builder().addAll(getObjectLstFromJsonArr(jsonArr) as System.Collections.IList).build());
        }

        private void handleOnEvent(string jsonArr)
        {
            EUNSocketObject.onEvent?.Invoke(new EUNArray.Builder().addAll(getObjectLstFromJsonArr(jsonArr) as System.Collections.IList).build());
        }

        private IList<object> getObjectLstFromJsonArr(string jsonArr)
        {
            return (IList<object>)Parser.Parse(jsonArr);
        }

    }

}
