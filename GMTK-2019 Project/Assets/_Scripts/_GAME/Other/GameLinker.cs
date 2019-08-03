using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLinker : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref")]
    public SceneLoader SceneLoader;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public GameState GameState;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public AllPlayerLinker AllPlayerLinker;
}
