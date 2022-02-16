#if UNITY_EDITOR
namespace EUN.Config.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEditor;
    using UnityEditor.Callbacks;

    using UnityEngine;

    [CustomEditor(typeof(EzyServerSettings))]
    public class EzyServerSettingsEditor : Editor
    {
        SerializedProperty damageProp;
        SerializedProperty armorProp;
        SerializedProperty gunProp;

        void OnEnable()
        {
            // Setup the SerializedProperties.
            damageProp = serializedObject.FindProperty("damage");
            armorProp = serializedObject.FindProperty("armor");
            gunProp = serializedObject.FindProperty("gun");
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();

            //// Show the custom GUI controls.
            //EditorGUILayout.IntSlider(damageProp, 0, 100, new GUIContent("Damage"));

            //// Only show the damage progress bar if all the objects have the same damage value:
            //if (!damageProp.hasMultipleDifferentValues)
            //    ProgressBar(damageProp.intValue / 100.0f, "Damage");

            //EditorGUILayout.IntSlider(armorProp, 0, 100, new GUIContent("Armor"));

            //// Only show the armor progress bar if all the objects have the same armor value:
            //if (!armorProp.hasMultipleDifferentValues)
            //    ProgressBar(armorProp.intValue / 100.0f, "Armor");

            //EditorGUILayout.PropertyField(gunProp, new GUIContent("Gun Object"));

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("EUN/Ezy Server Settings")]
        private static void OpenEzyServerSettings()
        {
            var ezyServerSettings = Resources.Load(EzyServerSettings.ResourcesPath) as EzyServerSettings;
            if (ezyServerSettings == null)
            {
                ezyServerSettings = ScriptableObject.CreateInstance<EzyServerSettings>();
                var path = "Assets/Ezy Unity Networking/Resources/" + EzyServerSettings.ResourcesPath + ".asset";
                AssetDatabase.CreateAsset(ezyServerSettings, path);

                Debug.Log(ezyServerSettings + " ezyServerSettings create success at path " + path);
            }

            //Selection.activeObject = ezyServerSettings;
            //Selection.objects = new Object[] { ezyServerSettings };
            Selection.SetActiveObjectWithContext(ezyServerSettings, ezyServerSettings);
        }

        [MenuItem("EUN/Docs")]
        private static void OpenDocs()
        {
            Application.OpenURL("https://youngmonkeys.org/ezyfox-sever/");
        }

        [MenuItem("EUN/About")]
        private static void OpenAbout()
        {
            EditorWindow.GetWindow(typeof(AboutEUNEditorWindow));
        }

        [DidReloadScripts]
        public static void OnCompileScripts()
        {
            UpdateEzyRPCCommand();
        }

        private static void UpdateEzyRPCCommand()
        {
            var assemblie = typeof(EzyServerSettings).Assembly;

            var types = assemblie.GetTypes();

            var methodNameLst = new List<string>();

            var dic = new Dictionary<int, string>();

            foreach (var type in types)
            {
                var methodInfos = type.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(x => x.GetCustomAttributes(typeof(EzyRPC), false).Length > 0);

                foreach (var methodInfo in methodInfos)
                {
                    if (!methodNameLst.Contains(methodInfo.Name)) methodNameLst.Add(methodInfo.Name);
                }
            }

            var needModifier = false;

            var ezyRPCCommandValues = Enum.GetValues(typeof(EzyRPCCommand));

            foreach (EzyRPCCommand ezyRPCCommandValue in ezyRPCCommandValues)
            {
                if (methodNameLst.Contains(ezyRPCCommandValue.ToString())) dic.Add((int)ezyRPCCommandValue, ezyRPCCommandValue.ToString());
                else if (!needModifier) needModifier = true;
            }

            foreach (var methodName in methodNameLst)
            {
                var dicValues = dic.Values.ToList();

                if (!dicValues.Contains(methodName))
                {
                    var dicKeys = dic.Keys.ToList();

                    var key = 0;
                    while (dicKeys.Contains(key))
                    {
                        key++;
                    }

                    dic.Add(key, methodName);

                    if (!needModifier) needModifier = true;
                }
            }

            if (needModifier)
            {
                var stringBuilder = new System.Text.StringBuilder();

                stringBuilder.AppendLine("// dont modifier this, this file will auto generate");
                stringBuilder.AppendLine("public enum EzyRPCCommand");
                stringBuilder.AppendLine("{");
                foreach (var c in dic)
                {
                    stringBuilder.AppendLine("    " + c.Value + " = " + c.Key + ",");
                    Debug.Log("EzyRPCCommand add " + c.Value + " = " + c.Key);
                }
                stringBuilder.AppendLine("}");

                // generate this file
                var path = Application.dataPath + "/Ezy Unity Networking/Scripts/Constant/EzyRPCCommand.cs";
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

                System.IO.File.WriteAllText(path, stringBuilder.ToString());

                UnityEditor.AssetDatabase.Refresh();
            }
        }
    }
}
#endif