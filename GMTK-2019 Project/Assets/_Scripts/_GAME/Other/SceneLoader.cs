using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[TypeInfoBox("Manage Loading/unloading of RaceTracks")]
public class SceneLoader : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), InlineEditor()]
    public SceneNames SceneNames;

    private bool closing = false;

    /// <summary>
    /// load a scene by name
    /// </summary>
    /// <param name="nameSceneToLoad">scene to load (only the name)</param>
    [FoldoutGroup("Debug"), Button]
    public void LoadSceneByName(string nameSceneToLoad)
    {
        for (int i = 0; i < SceneNames.ScenesToLoad.Count; i++)
        {
            if (string.Equals(SceneNames.ScenesToLoad[i].SceneName, nameSceneToLoad))
            {
                LoadSceneByTrackIndex(i);
            }
        }
        Debug.Log("didn't find any scene");
    }

    /// <summary>
    /// load scene by index in the list
    /// </summary>
    /// <param name="index"></param>
    [FoldoutGroup("Debug"), Button]
    public bool LoadSceneByTrackIndex(int index)
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadSceneAsync(SceneNames.ScenesToLoad[index].SceneName, LoadSceneMode.Single);
        }
#if UNITY_EDITOR
        else
        {
            EditorSceneManager.OpenScene(SceneNames.ScenesToLoad[index].ScenePath + SceneNames.ScenesToLoad[index].SceneName + ".unity", OpenSceneMode.Single);
        }
#endif
        return (true);
    }

    /// <summary>
    /// Unload a given scene by index in the list
    /// </summary>
    /// <param name="index"></param>
    [FoldoutGroup("Debug"), Button]
    public bool UnloadScene(int index)
    {
        if (Application.isPlaying)
        {
            SceneManager.UnloadSceneAsync(SceneNames.ScenesToLoad[index].ScenePath);
        }
#if UNITY_EDITOR
        else
        {
            EditorSceneManager.CloseScene(SceneManager.GetSceneByName(SceneNames.ScenesToLoad[index].SceneName), true);
        }
#endif
        return (true);
    }

    [Button]
    public void Reload()
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            EditorSceneManager.OpenScene(EditorSceneManager.GetActiveScene().path, OpenSceneMode.Single);
        }
    }

    /// <summary>
    /// Called if we manualy close
    /// </summary>
    private void OnApplicationQuit()
    {
        if (closing)
            return;
        Quit();
    }

    /// <summary>
    /// Exit game (in play mode or in runtime)
    /// </summary>
    [ContextMenu("Quit")]
    public void Quit()
    {
        closing = true;
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        Application.Quit();
#endif
    }
}
