#if UNITY_EDITOR
namespace EUN.Config.Editor
{
    using UnityEditor;

    using UnityEngine;

    public class AboutEUNEditorWindow : EditorWindow
    {
        private void OnEnable()
        {

        }

        void OnGUI()
        {
            GUILayout.Space(2);
            GUILayout.Label("EUN Version: " + EzyNetwork.Version);

            GUILayout.Space(5);
            GUILayout.Label("Copyright 2022 XmobiTea Family");
            GUILayout.Label("EUN (Ezy Unity Networking) use the core of Ezyfox.");
        }
    }
}

#endif