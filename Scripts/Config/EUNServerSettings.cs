namespace XmobiTea.EUN.Config
{
    using UnityEngine;

    //[CreateAssetMenu(fileName = EUNServerSettings.ResourcesPath, menuName = "EUN/EUNServerSettings", order = 1)]
    public class EUNServerSettings : ScriptableObject
    {
        public enum Mode
        {
            SelfHost = 0,
            OfflineMode = 1,
        }

        public const string ResourcesPath = "EUNServerSettings";

        [SerializeField]
        private string _socketHost = "ws.stackask.com";
        public string socketHost => _socketHost;

        [SerializeField]
        private int _socketTCPPort = 23005;
        public int socketTCPPort => _socketTCPPort;

        [SerializeField]
        private int _socketUDPPort = 22611;
        public int socketUDPPort => _socketUDPPort;

        [SerializeField]
        private string _webSocketHost = "ws://ws.stackask.com:22208/ws";
        public string webSocketHost => _webSocketHost;

        [SerializeField]
        private string _zoneName = "EUN";
        public string zoneName => _zoneName;

        [SerializeField]
        private string _appName = "EUN";
        public string appName => _appName;

        [SerializeField]
        private int _sendRate = 20;
        public int sendRate => _sendRate;

        [SerializeField]
        private int _sendRateSynchronizationData = 20;
        public int sendRateSynchronizationData => _sendRateSynchronizationData;

        [SerializeField]
        private bool _useVoiceChat;
        public bool useVoiceChat => _useVoiceChat;

        [SerializeField]
        private int _sendRateVoiceChat = 20;
        public int sendRateVoiceChat => _sendRateVoiceChat;

        [SerializeField]
        private Mode _mode = Mode.SelfHost;
        public Mode mode => _mode;
    }
}
