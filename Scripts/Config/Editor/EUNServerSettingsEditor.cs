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

    [CustomEditor(typeof(EUNServerSettings))]
    public class EUNServerSettingsEditor : Editor
    {
        private const string EUNRPC_Parent_Path = "/EUN-unity-client-custom/Scripts/Constant";
        private const string EUNRPC_Path = EUNRPC_Parent_Path + "/EUNRPCCommand.cs";
        private const string EUNRPC_Disable_Path = "/EUN-unity-client/Scripts/Constant/EUNRPCCommand.cs";
        private const string EUNServerSettings_Path = "Assets/EUN-unity-client-custom/Resources";
        private const string ezyfoxserverCsharpClientLink = "https://github.com/XmobiTea-Family/ezyfox-server-csharp-client.git";

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
        SerializedProperty logType;
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
            logType = serializedObject.FindProperty("_logType");
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

            EditorGUILayout.PropertyField(logType);
            EditorGUILayout.PropertyField(mode);

            var eunMode = (EUNServerSettings.Mode)mode.intValue;
            if (eunMode == EUNServerSettings.Mode.SelfHost)
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

            var eunRPCCommandValues = Enum.GetValues(typeof(EUNRPCCommand));

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("All EUNRPC Command (" + eunRPCCommandValues.Length + ")", boldStyle);
            EditorGUILayout.Space(5);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Height(150));
            EditorGUILayout.BeginVertical();

            foreach (EUNRPCCommand eunRPCCommandValue in eunRPCCommandValues)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(eunRPCCommandValue.ToString(), GUILayout.Width(200));
                EditorGUILayout.LabelField(((int)eunRPCCommandValue).ToString(), GUILayout.Width(150));

                GUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        [MenuItem("EUN/EUN Settings")]
        private static void OpenEUNServerSettings()
        {
            var eunServerSettings = Resources.Load(EUNServerSettings.ResourcesPath) as EUNServerSettings;
            if (eunServerSettings == null)
            {
                if (!System.IO.Directory.Exists(EUNServerSettings_Path))
                    System.IO.Directory.CreateDirectory(EUNServerSettings_Path);

                eunServerSettings = ScriptableObject.CreateInstance<EUNServerSettings>();
                var path = EUNServerSettings_Path + "/" + EUNServerSettings.ResourcesPath + ".asset";
                AssetDatabase.CreateAsset(eunServerSettings, path);

                Debug.Log(eunServerSettings + " ezyServerSettings create success at path " + path);
            }

            Selection.SetActiveObjectWithContext(eunServerSettings, eunServerSettings);
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

                if (!CloneEzyfoxserverCsharpClient())
                {
                    EditorUtility.DisplayDialog("Error", "Please use terminal to execute on the Assets path: git clone " + ezyfoxserverCsharpClientLink, "Ok");

                    return;
                }

                SetScriptingDefineSymbols("EUN");

                EditorUtility.DisplayDialog("Notice", "EUN setup successfully!", "Ok");
            }
        }

        private static bool CloneEzyfoxserverCsharpClient()
        {
            if (!IsezyfoxserverCsharpClone())
            {
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    CommandOutput("git clone " + ezyfoxserverCsharpClientLink, Application.dataPath);

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

            if (!IsezyfoxserverCsharpClone()) return false;

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

        private static bool IsezyfoxserverCsharpClone()
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
            UpdateEUNRPCCommand();
        }

        private static void UpdateEUNRPCCommand()
        {
            var assemblie = typeof(EUNServerSettings).Assembly;

            var types = assemblie.GetTypes();

            var methodNameLst = new List<string>();

            var dic = new Dictionary<int, string>();

            foreach (var type in types)
            {
                var methodInfos = type.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(x => x.GetCustomAttributes(typeof(EUNRPCAttribute), false).Length > 0);

                foreach (var methodInfo in methodInfos)
                {
                    if (!methodNameLst.Contains(methodInfo.Name)) methodNameLst.Add(methodInfo.Name);
                }
            }

            var needModifier = false;

            var eunRPCCommandValues = Enum.GetValues(typeof(EUNRPCCommand));

            foreach (EUNRPCCommand eunRPCCommandValue in eunRPCCommandValues)
            {
                if (methodNameLst.Contains(eunRPCCommandValue.ToString())) dic.Add((int)eunRPCCommandValue, eunRPCCommandValue.ToString());
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

            needModifier = true;

            if (needModifier)
            {
                var enableStringBuilder = new StringBuilder();
                var disableStringBuilder = new StringBuilder();

                enableStringBuilder.AppendLine("#if EUN");
                disableStringBuilder.AppendLine("#if !EUN");

                enableStringBuilder.AppendLine("// dont modifier this, this file will auto generate by EUN");
                disableStringBuilder.AppendLine("// dont modifier this, this file will auto generate by EUN");
                
                enableStringBuilder.AppendLine("public enum EUNRPCCommand");
                disableStringBuilder.AppendLine("public enum EUNRPCCommand");

                enableStringBuilder.AppendLine("{");
                disableStringBuilder.AppendLine("{");

                foreach (var c in dic)
                {
                    enableStringBuilder.AppendLine("    " + c.Value + " = " + c.Key + ",");
                    disableStringBuilder.AppendLine("    " + c.Value + " = " + c.Key + ",");

                    Debug.Log("EUNRPCCommand add " + c.Value + " = " + c.Key);
                }

                enableStringBuilder.AppendLine("}");
                disableStringBuilder.AppendLine("}");

                enableStringBuilder.AppendLine("#endif");
                disableStringBuilder.AppendLine("#endif");

                var dirPath = Application.dataPath + EUNRPC_Parent_Path;
                if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);

                // generate this file
                var enableFilePath = Application.dataPath + EUNRPC_Path;
                var disableFilePath = Application.dataPath + EUNRPC_Disable_Path;

                if (System.IO.File.Exists(enableFilePath)) System.IO.File.Delete(enableFilePath);
                if (System.IO.File.Exists(disableFilePath)) System.IO.File.Delete(disableFilePath);

                System.IO.File.WriteAllText(enableFilePath, enableStringBuilder.ToString());
                System.IO.File.WriteAllText(disableFilePath, disableStringBuilder.ToString());

                UnityEditor.AssetDatabase.Refresh();
            }
        }
    }
}
#endif
