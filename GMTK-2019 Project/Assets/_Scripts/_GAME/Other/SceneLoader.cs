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
    [Serializable]
    public struct SceneToLoad
    {
        public string SceneName;
        public string ScenePath;       
    }

    [FoldoutGroup("GamePlay")]
    public List<SceneToLoad> ScenesToLoad = new List<SceneToLoad>();

    private bool closing = false;

    /// <summary>
    /// load a scene by name
    /// </summary>
    /// <param name="nameSceneToLoad">scene to load (only the name)</param>
    [FoldoutGroup("Debug"), Button]
    public void LoadSceneByName(string nameSceneToLoad)
    {
        for (int i = 0; i < ScenesToLoad.Count; i++)
        {
            if (string.Equals(ScenesToLoad[i].SceneName, nameSceneToLoad))
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
            SceneManager.LoadSceneAsync(ScenesToLoad[index].SceneName, LoadSceneMode.Single);
        }
#if UNITY_EDITOR
        else
        {
            EditorSceneManager.OpenScene(ScenesToLoad[index].ScenePath + ScenesToLoad[index].SceneName + ".unity", OpenSceneMode.Single);
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
            SceneManager.UnloadSceneAsync(ScenesToLoad[index].ScenePath);
        }
#if UNITY_EDITOR
        else
        {
            EditorSceneManager.CloseScene(SceneManager.GetSceneByName(ScenesToLoad[index].SceneName), true);
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
