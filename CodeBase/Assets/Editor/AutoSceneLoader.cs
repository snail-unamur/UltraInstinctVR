using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.TestTools.CodeCoverage;
using UnityEngine.TestTools;

[InitializeOnLoad]
public class AutoPlayOnStart
{
    static AutoPlayOnStart()
    {
        // 🔥 important: delayCall marche mieux que update
        EditorApplication.delayCall += StartPlayMode;
    }

    static void StartPlayMode()
    {
        EditorApplication.delayCall -= StartPlayMode;

        EnableCoverage();

        string scenePath = "Assets/Scenes/SampleScene.unity";

        if (!EditorSceneManager.GetActiveScene().path.Equals(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
        }

        if (!EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = true;
        }
    }

    static void EnableCoverage()
    {
        // ✅ API correcte
        Coverage.enabled = true;



        Debug.Log("Code Coverage enabled.");
    }
}