#if UNITY_EDITOR
namespace XmobiTea.EUN.Config.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using UnityEditor;
    using UnityEditor.Callbacks;

    using UnityEngine;

    using Debug = UnityEngine.Debug;

    [CustomEditor(typeof(EzyServerSettings))]
    public class EzyServerSettingsEditor : Editor
    {
        private const string EzyRPC_Parent_Path = "/EUN-unity-client-custom/Scripts/Constant";
        private const string EzyRPC_Path = EzyRPC_Parent_Path + "/EzyRPCCommand.cs";
        private const string EzyRPC_Disable_Path = "/EUN-unity-client/Scripts/Constant/EzyRPCCommand.cs";
        private const string EzyServerSettings_Path = "Assets/EUN-unity-client-custom/Resources";
        private const string EzyfoxServerCsharpClientLink = "https://github.com/XmobiTea-Family/ezyfox-server-csharp-client.git";

        Vector2 scrollPos = new Vector2(0, 0);

        SerializedProperty socketHost;
        SerializedProperty socketTCPPort;
        SerializedProperty socketUDPPort;
        SerializedProperty webSocketHost;
        SerializedProperty zoneName;
        SerializedProperty appName;
        SerializedProperty sendRate;
        SerializedProperty sendRateSynchronizationData;
        SerializedProperty sendRateVoiceChat;
        SerializedProperty mode;
        SerializedProperty useVoiceChat;

        bool useVoiceChatLastValue;

        private static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.iOS,
            BuildTargetGroup.Android,
            BuildTargetGroup.WebGL,
            BuildTargetGroup.WSA,
        };

        private void OnEnable()
        {
            socketHost = serializedObject.FindProperty("_socketHost");
            socketTCPPort = serializedObject.FindProperty("_socketTCPPort");
            socketUDPPort = serializedObject.FindProperty("_socketUDPPort");
            webSocketHost = serializedObject.FindProperty("_webSocketHost");
            zoneName = serializedObject.FindProperty("_zoneName");
            appName = serializedObject.FindProperty("_appName");
            sendRate = serializedObject.FindProperty("_sendRate");
            sendRateSynchronizationData = serializedObject.FindProperty("_sendRateSynchronizationData");
            sendRateVoiceChat = serializedObject.FindProperty("_sendRateVoiceChat");
            mode = serializedObject.FindProperty("_mode");
            useVoiceChat = serializedObject.FindProperty("_useVoiceChat");

            useVoiceChatLastValue = useVoiceChat.boolValue;
        }

        public override void OnInspectorGUI()
        {
            var boldStyle = new GUIStyle();
            boldStyle.fontStyle = FontStyle.Bold;

            var italicStyle = new GUIStyle();
            italicStyle.fontStyle = FontStyle.Italic;

            if (!IsEUNSetup())
            {
                if (GUILayout.Button("Setup EUN"))
                {
                    SetupEUN();
                }
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(mode);

            var ezyMode = (EzyServerSettings.Mode)mode.intValue;
            if (ezyMode == EzyServerSettings.Mode.SelfHost)
            {
                EditorGUILayout.LabelField("This is online mode, you will connect to your host with info:", italicStyle);
                EditorGUILayout.Space(2);

                EditorGUILayout.PropertyField(socketHost);
                EditorGUILayout.PropertyField(socketTCPPort);
                EditorGUILayout.PropertyField(socketUDPPort);
                EditorGUILayout.PropertyField(webSocketHost);
                EditorGUILayout.PropertyField(zoneName);
                EditorGUILayout.PropertyField(appName);
                EditorGUILayout.PropertyField(sendRate);
                EditorGUILayout.PropertyField(sendRateSynchronizationData);

                EditorGUILayout.Space(2);
                EditorGUILayout.PropertyField(useVoiceChat);
                if (useVoiceChat.boolValue)
                {
                    if (useVoiceChatLastValue != useVoiceChat.boolValue)
                    {
                        useVoiceChatLastValue = useVoiceChat.boolValue;
                        SetScriptingDefineSymbols("EUN_VOICE_CHAT");
                    }

                    EditorGUILayout.PropertyField(sendRateVoiceChat);
                }
                else
                {
                    if (useVoiceChatLastValue != useVoiceChat.boolValue)
                    {
                        useVoiceChatLastValue = useVoiceChat.boolValue;
                        RemoveScriptingDefineSymbols("EUN_VOICE_CHAT");
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("This is offline mode, you can not play with other player.", italicStyle);
            }

            serializedObject.ApplyModifiedProperties();

            var ezyRPCCommandValues = Enum.GetValues(typeof(EzyRPCCommand));

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("All EzyRPC Command (" + ezyRPCCommandValues.Length + ")", boldStyle);
            EditorGUILayout.Space(5);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Height(150));
            EditorGUILayout.BeginVertical();

            foreach (EzyRPCCommand ezyRPCCommandValue in ezyRPCCommandValues)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(ezyRPCCommandValue.ToString(), GUILayout.Width(200));
                EditorGUILayout.LabelField(((int)ezyRPCCommandValue).ToString(), GUILayout.Width(150));

                GUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        [MenuItem("EUN/EUN Settings")]
        private static void OpenEzyServerSettings()
        {
            var ezyServerSettings = Resources.Load(EzyServerSettings.ResourcesPath) as EzyServerSettings;
            if (ezyServerSettings == null)
            {
                if (!System.IO.Directory.Exists(EzyServerSettings_Path))
                    System.IO.Directory.CreateDirectory(EzyServerSettings_Path);

                ezyServerSettings = ScriptableObject.CreateInstance<EzyServerSettings>();
                var path = EzyServerSettings_Path + "/" + EzyServerSettings.ResourcesPath + ".asset";
                AssetDatabase.CreateAsset(ezyServerSettings, path);

                Debug.Log(ezyServerSettings + " ezyServerSettings create success at path " + path);
            }

            Selection.SetActiveObjectWithContext(ezyServerSettings, ezyServerSettings);
        }

        [MenuItem("EUN/About Ezyfox")]
        private static void AboutEzyfox()
        {
            Application.OpenURL("https://youngmonkeys.org/ezyfox-sever/");
        }

        [MenuItem("EUN/Go to EUN-server")]
        private static void GotoEUNserver()
        {
            Application.OpenURL("https://github.com/XmobiTea-Family/EUN-server");
        }

        [MenuItem("EUN/About")]
        private static void OpenAbout()
        {
            EditorWindow.GetWindow(typeof(AboutEUNEditorWindow));
        }

        [MenuItem("EUN/Setup EUN")]
        private static void SetupEUN()
        {
            if (EditorUtility.DisplayDialog("Confirm", "Setup EUN", "Yes", "No"))
            {
                if (IsEUNSetup())
                {
                    EditorUtility.DisplayDialog("Notice", "EUN has setup!", "Ok");

                    return;
                }

                if (!CloneEzyFoxServerCsharpClient())
                {
                    EditorUtility.DisplayDialog("Error", "Please use terminal to execute on the Assets path: git clone https://github.com/youngmonkeys/ezyfox-server-csharp-client.git", "Ok");

                    return;
                }

                SetScriptingDefineSymbols("EUN");

                EditorUtility.DisplayDialog("Notice", "EUN setup successfully!", "Ok");
            }
        }

        private static bool CloneEzyFoxServerCsharpClient()
        {
            if (!IsEzyfoxServerCsharpClone())
            {
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    CommandOutput("git clone " + EzyfoxServerCsharpClientLink, Application.dataPath);

                    AssetDatabase.Refresh();

                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsEUNSetup()
        {
            foreach (var buildTargetGroup in buildTargetGroups)
            {
                if (!HasScriptingDefineSymbols(buildTargetGroup, "EUN")) return false;
            }

            if (!IsEzyfoxServerCsharpClone()) return false;

            return true;
        }

        private static bool HasScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string symbol)
        {
            var scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            return scriptingDefineSymbols.Equals(symbol) || scriptingDefineSymbols.Contains(symbol + ";") || scriptingDefineSymbols.Contains(";" + symbol);
        }

        private static void SetScriptingDefineSymbols(string symbol)
        {
            foreach (var buildTargetGroup in buildTargetGroups)
            {
                SetScriptingDefineSymbols(buildTargetGroup, symbol);
            }
        }

        private static void RemoveScriptingDefineSymbols(string symbol)
        {
            foreach (var buildTargetGroup in buildTargetGroups)
            {
                RemoveScriptingDefineSymbols(buildTargetGroup, symbol);
            }
        }

        private static void SetScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string symbol)
        {
            if (HasScriptingDefineSymbols(buildTargetGroup, symbol)) return;

            var scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            scriptingDefineSymbols += ";" + symbol + ";";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, scriptingDefineSymbols);
        }

        private static void RemoveScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string symbol)
        {
            if (!HasScriptingDefineSymbols(buildTargetGroup, symbol)) return;

            var scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (scriptingDefineSymbols.Equals(symbol))
            {
                scriptingDefineSymbols = string.Empty;
            }
            else if (scriptingDefineSymbols.Contains(symbol + ";"))
            {
                scriptingDefineSymbols = scriptingDefineSymbols.Replace(symbol + ";", string.Empty);
            }
            else if (scriptingDefineSymbols.Contains(";" + symbol))
            {
                scriptingDefineSymbols = scriptingDefineSymbols.Replace(";" + symbol, string.Empty);
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, scriptingDefineSymbols);
        }

        private static bool IsEzyfoxServerCsharpClone()
        {
            var ezyfoxServerCsharpClient = Application.dataPath + "/ezyfox-server-csharp-client";
            return System.IO.Directory.Exists(ezyfoxServerCsharpClient);
        }

        private static string CommandOutput(string command, string workingDirectory = null)
        {
            try
            {
                var procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

                procStartInfo.RedirectStandardError = procStartInfo.RedirectStandardInput = procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                if (null != workingDirectory)
                {
                    procStartInfo.WorkingDirectory = workingDirectory;
                }

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                var sb = new StringBuilder();
                proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
                {
                    sb.AppendLine(e.Data);
                };
                proc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
                {
                    sb.AppendLine(e.Data);
                };

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
                return sb.ToString();
            }
            catch (Exception objException)
            {
                return $"Error in command: {command}, {objException.Message}";
            }
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
                var methodInfos = type.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(x => x.GetCustomAttributes(typeof(EzyRPCAttribute), false).Length > 0);

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
                    
                    var key = dicKeys.Max() + 1;

                    dic.Add(key, methodName);

                    if (!needModifier) needModifier = true;
                }
            }

            if (needModifier)
            {
                var enableStringBuilder = new StringBuilder();
                var disableStringBuilder = new StringBuilder();

                enableStringBuilder.AppendLine("#if EUN");
                disableStringBuilder.AppendLine("#if !EUN");

                enableStringBuilder.AppendLine("// dont modifier this, this file will auto generate by EUN");
                disableStringBuilder.AppendLine("// dont modifier this, this file will auto generate by EUN");
                
                enableStringBuilder.AppendLine("public enum EzyRPCCommand");
                disableStringBuilder.AppendLine("public enum EzyRPCCommand");

                enableStringBuilder.AppendLine("{");
                disableStringBuilder.AppendLine("{");

                foreach (var c in dic)
                {
                    enableStringBuilder.AppendLine("    " + c.Value + " = " + c.Key + ",");
                    disableStringBuilder.AppendLine("    " + c.Value + " = " + c.Key + ",");

                    Debug.Log("EzyRPCCommand add " + c.Value + " = " + c.Key);
                }

                enableStringBuilder.AppendLine("}");
                disableStringBuilder.AppendLine("}");

                enableStringBuilder.AppendLine("#endif");
                disableStringBuilder.AppendLine("#endif");

                var dirPath = Application.dataPath + EzyRPC_Parent_Path;
                if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);

                // generate this file
                var enableFilePath = Application.dataPath + EzyRPC_Path;
                var disableFilePath = Application.dataPath + EzyRPC_Disable_Path;

                if (System.IO.File.Exists(enableFilePath)) System.IO.File.Delete(enableFilePath);

                System.IO.File.WriteAllText(enableFilePath, enableStringBuilder.ToString());
                System.IO.File.WriteAllText(disableFilePath, disableStringBuilder.ToString());

                UnityEditor.AssetDatabase.Refresh();
            }
        }
    }
}
#endif