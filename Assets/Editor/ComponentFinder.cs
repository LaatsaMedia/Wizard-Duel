using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ComponentFinder : EditorWindow
{
    private MonoScript targetScript;
    private Vector2 scroll;

    private List<Object> results = new();

    [MenuItem("Tools/Find Script References")]
    static void Open()
    {
        GetWindow<ComponentFinder>("Script Finder");
    }

    void OnGUI()
    {
        GUILayout.Label("Find Objects Using Script", EditorStyles.boldLabel);

        targetScript = (MonoScript)EditorGUILayout.ObjectField(
            "Script",
            targetScript,
            typeof(MonoScript),
            false);

        if (GUILayout.Button("Search"))
        {
            Search();
        }

        GUILayout.Space(10);

        GUILayout.Label($"Results ({results.Count})", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var obj in results)
        {
            EditorGUILayout.ObjectField(obj, typeof(Object), true);
        }

        EditorGUILayout.EndScrollView();
    }

    void Search()
    {
        results.Clear();

        if (targetScript == null)
            return;

        System.Type type = targetScript.GetClass();

        if (type == null)
            return;

        // Search loaded scenes
        foreach (Scene scene in EditorSceneManager.GetAllScenes())
        {
            if (!scene.isLoaded)
                continue;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                SearchHierarchy(root, type);
            }
        }

        // Search prefabs
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            SearchHierarchy(prefab, type);
        }

        Debug.Log($"Found {results.Count} object(s).");
    }

    void SearchHierarchy(GameObject root, System.Type type)
    {
        Component[] comps = root.GetComponentsInChildren<Component>(true);

        foreach (Component c in comps)
        {
            if (c == null)
                continue;

            if (c.GetType() == type)
            {
                results.Add(c.gameObject);
            }
        }
    }
}