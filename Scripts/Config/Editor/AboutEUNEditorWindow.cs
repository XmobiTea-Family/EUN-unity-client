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
            GUILayout.Label("Copyright XmobiTea");
        }
    }
}

#endif