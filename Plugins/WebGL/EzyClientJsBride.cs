namespace XmobiTea.EUN.Plugin.WebGL
{
    using System.Runtime.InteropServices;

    public class EzyClientJsBride
    {
#if EUN
        [DllImport("__Internal")]
        public static extern void EzyLibrary();

        [DllImport("__Internal")]
        public static extern void EzyInit(string gameObjectName, string zoneName, string appName);

        [DllImport("__Internal")]
        public static extern void EzyConnect(string username, string password, string dataJson, string host);

        [DllImport("__Internal")]
        public static extern void EzyDisconnect();

        [DllImport("__Internal")]
        public static extern bool EzySend(string requestData);
#endif
    }
}