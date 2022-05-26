namespace XmobiTea.EUN.Plugin.WebGL
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The bride to connect via WebSocket for WebGL
    /// </summary>
    public class EzyClientJsBride
    {
#if EUN
        /// <summary>
        /// Init the ezyfox js client library
        /// </summary>
        [DllImport("__Internal")]
        public static extern void EzyLibrary();

        /// <summary>
        /// Init the ezyfox with zone name app name for websocket
        /// </summary>
        /// <param name="gameObjectName">the game object to receive callback</param>
        /// <param name="zoneName">The zone name on settings EUN Server</param>
        /// <param name="appName">The plugin name and app name on settings EUN Server</param>
        [DllImport("__Internal")]
        public static extern void EzyInit(string gameObjectName, string zoneName, string appName);

        /// <summary>
        /// Connect by username and password, and data json and url host
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="dataJson"></param>
        /// <param name="host"></param>
        [DllImport("__Internal")]
        public static extern void EzyConnect(string username, string password, string dataJson, string host);

        /// <summary>
        /// Disconnect
        /// </summary>
        [DllImport("__Internal")]
        public static extern void EzyDisconnect();

        /// <summary>
        /// Send the request to EUNServer
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [DllImport("__Internal")]
        public static extern bool EzySend(string requestData);
#endif
    }
}