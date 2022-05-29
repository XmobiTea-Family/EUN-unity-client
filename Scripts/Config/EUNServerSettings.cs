namespace XmobiTea.EUN.Config
{
    using UnityEngine;

    //[CreateAssetMenu(fileName = EUNServerSettings.ResourcesPath, menuName = "EUN/EUNServerSettings", order = 1)]
    public class EUNServerSettings : ScriptableObject
    {
        /// <summary>
        /// The mode connect to EUNServer
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// The custom host you host in your server
            /// </summary>
            SelfHost = 0,

            /// <summary>
            /// Connect offline, you dont need connection to connect
            /// </summary>
            OfflineMode = 1,

            /// <summary>
            /// Connect to EUN Server
            /// </summary>
            CloudHost = 2,
        }

        /// <summary>
        /// The resource path EUNServerSettings file
        /// </summary>
        public const string ResourcesPath = "EUNServerSettings";

        [SerializeField]
        private string _socketHost = "ws.stackask.com";
        /// <summary>
        /// The server ip address EUN Server
        /// ex: 127.0.0.1, 3.0.1.5,...
        /// </summary>
        public string socketHost => _socketHost;

        [SerializeField]
        private int _socketTCPPort = 23005;
        /// <summary>
        /// The TCP port EUN Server
        /// </summary>
        public int socketTCPPort => _socketTCPPort;

        [SerializeField]
        private int _socketUDPPort = 22611;
        /// <summary>
        /// The UDP port EUN Server
        /// </summary>
        public int socketUDPPort => _socketUDPPort;

        [SerializeField]
        private string _webSocketHost = "ws://ws.stackask.com:22208/ws";
        /// <summary>
        /// The TCP port for websocket, use on WebGL
        /// </summary>
        public string webSocketHost => _webSocketHost;

        [SerializeField]
        private string _zoneName = "EUN Zone";
        /// <summary>
        /// The zone name EUN Server
        /// </summary>
        public string zoneName => _zoneName;

        [SerializeField]
        private string _appName = "EUN App";
        /// <summary>
        /// The app name and plugin name EUN Server
        /// </summary>
        public string appName => _appName;

        [SerializeField]
        private int _sendRate = 20;
        /// <summary>
        /// The send rate for normal OperationRequest
        /// </summary>
        public int sendRate => _sendRate;

        [SerializeField]
        private int _sendRateSynchronizationData = 20;
        /// <summary>
        /// The send rate for sync OperationRequest
        /// </summary>
        public int sendRateSynchronizationData => _sendRateSynchronizationData;

        [SerializeField]
        private bool _useVoiceChat;
        /// <summary>
        /// Use the voice chat in this project
        /// </summary>
        public bool useVoiceChat => _useVoiceChat;

        [SerializeField]
        private int _sendRateVoiceChat = 20;
        /// <summary>
        /// The send rate for voice chat OperationRequest
        /// </summary>
        public int sendRateVoiceChat => _sendRateVoiceChat;

        [SerializeField]
        private Mode _mode = Mode.SelfHost;
        /// <summary>
        /// The current mode EUN Client
        /// </summary>
        public Mode mode => _mode;

        [SerializeField]
        private EUN.Logger.LogType _logType = EUN.Logger.LogType.All;
        /// <summary>
        /// The current log type EUN Client
        /// </summary>
        public EUN.Logger.LogType LogType => _logType;
    }
}
