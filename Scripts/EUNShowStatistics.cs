namespace XmobiTea.EUN
{
    using UnityEngine;

    public class EUNShowStatistics : MonoBehaviour
    {
        private void OnGUI()
        {
            var peerStatistics = EUNNetwork.GetPeerStatistics();

            GUILayout.Label("Ping: " + peerStatistics.GetPing());
            GUILayout.Space(15);

            GUILayout.Label("Send Bytes: " + peerStatistics.GetSendBytes());
            GUILayout.Label("Send Packets: " + peerStatistics.GetSendPackets());
            GUILayout.Space(10);

            GUILayout.Label("Recv Bytes: " + peerStatistics.GetRecvBytes());
            GUILayout.Label("Recv Packets: " + peerStatistics.GetRecvPackets());
            GUILayout.Space(15);

            if (GUILayout.Button("Clear bytes and packets"))
            {
                peerStatistics.ClearBytes();
                peerStatistics.ClearPackets();
            }
        }
    }
}
