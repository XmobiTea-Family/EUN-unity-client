namespace EUN.Config
{
    using UnityEngine;

    //[CreateAssetMenu(fileName = EzyServerSettings.ResourcesPath, menuName = "EUN/EzyServerSettings", order = 1)]
    public class EzyServerSettings : ScriptableObject
    {
        public enum Mode
        {
            SelfHost = 0,
            OfflineMode = 1,
        }

        public const string ResourcesPath = "EzyServerSettings";

        [SerializeField]
        private string _socketHost = "127.0.0.1";
        public string socketHost => _socketHost;

        [SerializeField]
        private int _socketTCPPort = 3005;
        public int socketTCPPort => _socketTCPPort;

        [SerializeField]
        private int _socketUDPPort = 2611;
        public int socketUDPPort => _socketUDPPort;

        [SerializeField]
        private string _webSocketHost = "ws://127.0.0.1:2208/ws";
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