#if UNITY_EDITOR
namespace XmobiTea.EUN.Config.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

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
        private const string EzyfoxServerCsharpClientRepoUrl = "https://github.com/XmobiTea-Family/ezyfox-server-csharp-client.git";

        Vector2 scrollPos = new Vector2(0, 0);

        SerializedProperty socketHost;
        SerializedProperty socketTCPPort;
        SerializedProperty socketUDPPort;
        SerializedProperty webSocketHost;
        SerializedProperty zoneName;
        SerializedProperty appName;
        SerializedProperty secretKey;
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
            this.socketHost = this.serializedObject.FindProperty("_socketHost");
            this.socketTCPPort = this.serializedObject.FindProperty("_socketTCPPort");
            this.socketUDPPort = this.serializedObject.FindProperty("_socketUDPPort");
            this.webSocketHost = this.serializedObject.FindProperty("_webSocketHost");
            this.zoneName = this.serializedObject.FindProperty("_zoneName");
            this.appName = this.serializedObject.FindProperty("_appName");
            this.secretKey = this.serializedObject.FindProperty("_secretKey");
            this.sendRate = this.serializedObject.FindProperty("_sendRate");
            this.sendRateSynchronizationData = this.serializedObject.FindProperty("_sendRateSynchronizationData");
            this.sendRateVoiceChat = this.serializedObject.FindProperty("_sendRateVoiceChat");
            this.mode = this.serializedObject.FindProperty("_mode");
            this.logType = this.serializedObject.FindProperty("_logType");
            this.useVoiceChat = this.serializedObject.FindProperty("_useVoiceChat");

            this.useVoiceChatLastValue = this.useVoiceChat.boolValue;
        }

        public override void OnInspectorGUI()
        {
            var boldStyle = new GUIStyle();
            boldStyle.fontStyle = FontStyle.Bold;

            var italicStyle = new GUIStyle();
            italicStyle.fontStyle = FontStyle.Italic;

            if (!isEUNSetup())
            {
                if (GUILayout.Button("Setup EUN"))
                {
                    setupEUN();
                }
            }

            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.logType);
            EditorGUILayout.PropertyField(this.mode);

            var eunMode = (EUNServerSettings.Mode)this.mode.intValue;
            if (eunMode == EUNServerSettings.Mode.SelfHost)
            {
                EditorGUILayout.LabelField("This is online mode, you will connect to your host with info:", italicStyle);
                EditorGUILayout.Space(2);

                EditorGUILayout.PropertyField(this.socketHost);
                EditorGUILayout.PropertyField(this.socketTCPPort);
                EditorGUILayout.PropertyField(this.socketUDPPort);
                EditorGUILayout.PropertyField(this.webSocketHost);
                EditorGUILayout.PropertyField(this.zoneName);
                EditorGUILayout.PropertyField(this.appName);
                EditorGUILayout.PropertyField(this.secretKey);

                EditorGUILayout.Space(2);
                EditorGUILayout.PropertyField(this.sendRate);
                EditorGUILayout.PropertyField(this.sendRateSynchronizationData);

                EditorGUILayout.Space(2);
                EditorGUILayout.PropertyField(this.useVoiceChat);
                if (this.useVoiceChat.boolValue)
                {
                    if (this.useVoiceChatLastValue != this.useVoiceChat.boolValue)
                    {
                        this.useVoiceChatLastValue = this.useVoiceChat.boolValue;
                        setScriptingDefineSymbols("EUN_USING_VOICE_CHAT");
                    }

                    EditorGUILayout.PropertyField(sendRateVoiceChat);
                }
                else
                {
                    if (this.useVoiceChatLastValue != this.useVoiceChat.boolValue)
                    {
                        this.useVoiceChatLastValue = this.useVoiceChat.boolValue;
                        removeScriptingDefineSymbols("EUN_USING_VOICE_CHAT");
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("This is offline mode, you can not play with other player.", italicStyle);
            }

            this.serializedObject.ApplyModifiedProperties();

            var eunRPCCommandValues = Enum.GetValues(typeof(EUNRPCCommand));

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("All EUNRPC Command (" + eunRPCCommandValues.Length + ")", boldStyle);
            EditorGUILayout.Space(5);
            this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, true, GUILayout.Height(150));
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

        [MenuItem("XmobiTea EUN/EUN Settings")]
        private static void openEUNServerSettings()
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

        [MenuItem("XmobiTea EUN/About Ezyfox")]
        private static void aboutEzyfox()
        {
            Application.OpenURL("https://youngmonkeys.org/ezyfox-sever/");
        }

        [MenuItem("XmobiTea EUN/Go to EUN-server")]
        private static void gotoEUNserver()
        {
            Application.OpenURL("https://github.com/XmobiTea-Family/EUN-server");
        }

        [MenuItem("XmobiTea EUN/About")]
        private static void openAbout()
        {
            EditorWindow.GetWindow(typeof(AboutEUNEditorWindow));
        }

        [MenuItem("XmobiTea EUN/Setup EUN")]
        private static void setupEUN()
        {
            if (EditorUtility.DisplayDialog("Confirm", "Setup EUN", "Yes", "No"))
            {
                if (isEUNSetup())
                {
                    EditorUtility.DisplayDialog("Notice", "EUN has setup!", "Ok");

                    return;
                }

                if (!cloneEzyfoxServerCsharpClient())
                {
                    EditorUtility.DisplayDialog("Error", "Please use terminal to execute on the Assets path: git clone " + EzyfoxServerCsharpClientRepoUrl, "Ok");

                    return;
                }

                setScriptingDefineSymbols("EUN_USING_ONLINE");

                EditorUtility.DisplayDialog("Notice", "EUN setup successfully!", "Ok");
            }
        }

        private static bool cloneEzyfoxServerCsharpClient()
        {
            if (!isEzyfoxServerCsharpClone())
            {
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    commandOutput("git clone " + EzyfoxServerCsharpClientRepoUrl, Application.dataPath);

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

        private static bool isEUNSetup()
        {
            foreach (var buildTargetGroup in buildTargetGroups)
            {
                if (!hasScriptingDefineSymbols(buildTargetGroup, "EUN")) return false;
            }

            if (!isEzyfoxServerCsharpClone()) return false;

            return true;
        }

        private static bool hasScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string symbol)
        {
            var scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            return scriptingDefineSymbols.Equals(symbol) || scriptingDefineSymbols.Contains(symbol + ";") || scriptingDefineSymbols.Contains(";" + symbol);
        }

        private static void setScriptingDefineSymbols(string symbol)
        {
            foreach (var buildTargetGroup in buildTargetGroups)
            {
                setScriptingDefineSymbols(buildTargetGroup, symbol);
            }
        }

        private static void removeScriptingDefineSymbols(string symbol)
        {
            foreach (var buildTargetGroup in buildTargetGroups)
            {
                removeScriptingDefineSymbols(buildTargetGroup, symbol);
            }
        }

        private static void setScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string symbol)
        {
            if (hasScriptingDefineSymbols(buildTargetGroup, symbol)) return;

            var scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            scriptingDefineSymbols += ";" + symbol + ";";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, scriptingDefineSymbols);
        }

        private static void removeScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string symbol)
        {
            if (!hasScriptingDefineSymbols(buildTargetGroup, symbol)) return;

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

        private static bool isEzyfoxServerCsharpClone()
        {
            var ezyfoxServerCsharpClient = Application.dataPath + "/ezyfox-server-csharp-client";
            return System.IO.Directory.Exists(ezyfoxServerCsharpClient);
        }

        private static string commandOutput(string command, string workingDirectory = null)
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

                var sb = new System.Text.StringBuilder();
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
        public static void onCompileScripts()
        {
            updateEUNRPCCommand();
        }

        private static void updateEUNRPCCommand()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = new List<Type>();

            foreach (var assemblie in assemblies)
            {
                types.AddRange(assemblie.GetTypes());
            }

            var methodNameLst = new List<string>();

            var dict = new Dictionary<int, string>();

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
                if (methodNameLst.Contains(eunRPCCommandValue.ToString())) dict.Add((int)eunRPCCommandValue, eunRPCCommandValue.ToString());
                else if (!needModifier) needModifier = true;
            }

            foreach (var methodName in methodNameLst)
            {
                var dictValues = dict.Values.ToList();

                if (!dictValues.Contains(methodName))
                {
                    var dictKeys = dict.Keys.ToList();
                    
                    var key = dictKeys.Max() + 1;

                    dict.Add(key, methodName);

                    if (!needModifier) needModifier = true;
                }
            }

            needModifier = true;

            if (needModifier)
            {
                var enableStringBuilder = new System.Text.StringBuilder();
                var disableStringBuilder = new System.Text.StringBuilder();

                enableStringBuilder.AppendLine("#if EUN_USING_ONLINE");
                disableStringBuilder.AppendLine("#if !EUN_USING_ONLINE");

                enableStringBuilder.AppendLine("// dont modifier this, this file will auto generate by XmobiTea EUN");
                disableStringBuilder.AppendLine("// dont modifier this, this file will auto generate by XmobiTea EUN");
                
                enableStringBuilder.AppendLine("public enum EUNRPCCommand");
                disableStringBuilder.AppendLine("public enum EUNRPCCommand");

                enableStringBuilder.AppendLine("{");
                disableStringBuilder.AppendLine("{");

                foreach (var c in dict)
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
