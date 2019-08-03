using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneNames", menuName = "")]
public class SceneNames : ScriptableObject
{
    [Serializable]
    public struct SceneToLoad
    {
        public string SceneName;
        public string ScenePath;
    }

    public List<SceneToLoad> ScenesToLoad = new List<SceneToLoad>();
}
