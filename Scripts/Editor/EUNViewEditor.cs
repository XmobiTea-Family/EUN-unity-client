namespace XmobiTea.EUN.Editor
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(EUNView))]
    public class EUNViewEditor : Editor
    {
        EUNView eunView;

        SerializedProperty objectIdProperty;
        SerializedProperty observedComponentsProperty;

        private int observedComponentsLength;

        private void OnEnable()
        {
            var roomGameObjectProperty = serializedObject.FindProperty("_roomGameObject");

            objectIdProperty = roomGameObjectProperty.FindPropertyRelative("_objectId");

            observedComponentsProperty = serializedObject.FindProperty("_observedComponents");

            observedComponentsLength = observedComponentsProperty.arraySize;

            eunView = (EUNView)target;

            if (Application.isEditor && !Application.isPlaying)
            {
                //var roomGameObjectProperty = serializedObject.FindProperty("roomGameObject");
                //var objectIdProperty = roomGameObjectProperty.FindPropertyRelative("objectId");

                var eunViews = GameObject.FindObjectsOfType<EUNView>();

                var id = -1;

                while (eunViews.Count(x => x != eunView && x.roomGameObject.objectId == id) != 0)
                {
                    id -= 1;
                }

                objectIdProperty.intValue = id;

                if (serializedObject.hasModifiedProperties) serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var isPrefab = IsPrefab(eunView);

            if (isPrefab)
            {
                EditorGUILayout.LabelField("The Object Id will enable if game object in scene!");
            }
            else
            {
                if (Application.isPlaying)
                {
                    EditorGUILayout.LabelField(new GUIContent("Object Id"), new GUIContent(objectIdProperty.intValue.ToString()));
                }
                else
                {
                    EditorGUILayout.PropertyField(objectIdProperty, new GUIContent("Object Id"));
                }
            }

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("The sync eun behaviour you want to send sync and get sync data");
            EditorGUILayout.Space(10);

            observedComponentsLength = EditorGUILayout.IntField(new GUIContent("Observed Compenents Length"), observedComponentsLength);
            observedComponentsProperty.arraySize = observedComponentsLength;

            for (var i = 0; i < observedComponentsProperty.arraySize; i++)
            {
                EditorGUILayout.PropertyField(observedComponentsProperty.GetArrayElementAtIndex(i));
            }

            if (serializedObject.hasModifiedProperties) serializedObject.ApplyModifiedProperties();
        }

        public static bool IsPrefab(EUNView eunView)
        {
            if (eunView == null) return false;
            var go = eunView.gameObject;

#if UNITY_2021_2_OR_NEWER
            return UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(go) != null || EditorUtility.IsPersistent(go);
#elif UNITY_2018_3_OR_NEWER
            return UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(go) != null || EditorUtility.IsPersistent(go);
#else
            return EditorUtility.IsPersistent(go);
#endif
        }
    }
}
