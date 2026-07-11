using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSwitcherWindow : EditorWindow
{
    private const string PrefKey = "SceneSwitcher.StartScene";

    private SceneAsset startingScene;
    private Vector2 scroll;

    [MenuItem("Tools/Scene Switcher")]
    public static void Open()
    {
        GetWindow<SceneSwitcherWindow>("Scene Switcher");
    }

    private void OnEnable()
    {
        LoadStartingScene();
    }

    private void LoadStartingScene()
    {
        string path = EditorPrefs.GetString(PrefKey, "");

        if (!string.IsNullOrEmpty(path))
        {
            startingScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            EditorSceneManager.playModeStartScene = startingScene;
        }
        else
        {
            startingScene = null;
            EditorSceneManager.playModeStartScene = null;
        }
    }

    private void SetStartingScene(SceneAsset scene)
    {
        startingScene = scene;

        if (scene == null)
        {
            EditorPrefs.DeleteKey(PrefKey);
            EditorSceneManager.playModeStartScene = null;
            return;
        }

        string path = AssetDatabase.GetAssetPath(scene);

        EditorPrefs.SetString(PrefKey, path);
        EditorSceneManager.playModeStartScene = scene;
    }

    private void OnGUI()
    {
        GUILayout.Space(5);

        GUILayout.Label("Scene Switcher", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Press Open to edit a scene.\n" +
            "Select one scene as the Play Mode Starting Scene.",
            MessageType.Info);

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Starting Scene:", GUILayout.Width(100));

        EditorGUILayout.LabelField(
            startingScene == null ? "None" : startingScene.name,
            EditorStyles.boldLabel);

        GUILayout.FlexibleSpace();

        GUI.enabled = startingScene != null;
        if (GUILayout.Button("Clear", GUILayout.Width(70)))
        {
            SetStartingScene(null);
        }
        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var buildScene in EditorBuildSettings.scenes)
        {
            if (!buildScene.enabled)
                continue;

            SceneAsset scene =
                AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path);

            if (scene == null)
                continue;

            bool isStartScene = scene == startingScene;

            Color previousColor = GUI.backgroundColor;

            if (isStartScene)
                GUI.backgroundColor = Color.green;

            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Open", GUILayout.Width(60)))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(buildScene.path);
                }
            }

            GUILayout.Label(scene.name);

            GUILayout.FlexibleSpace();

            bool selected = GUILayout.Toggle(
                isStartScene,
                "Start",
                "Button",
                GUILayout.Width(60));

            if (selected && !isStartScene)
            {
                SetStartingScene(scene);
            }

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = previousColor;
        }

        EditorGUILayout.EndScrollView();
    }
}