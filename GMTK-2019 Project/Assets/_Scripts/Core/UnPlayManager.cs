using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

/// <summary>
/// Exexute a function when we leave / enter the play mode
/// </summary>
[ExecuteInEditMode, TypeInfoBox("Execute action when leaving / entering play mode")]
public class UnPlayManager : MonoBehaviour
{
    void Awake()
    {
        if (!Application.isPlaying)
        {
            Debug.Log("Awake in edit mode");
            if (PlayerPrefs.GetInt("should spawn") == 1)
            {
                PlayerPrefs.SetInt("should spawn", 0);
                ExecuteOutOfPlayMode(true);
            }
            else
            {
                ExecuteOutOfPlayMode(false);
            }
        }
        else
        {
            ExecuteInPlayMode();
        }
    }

    /// <summary>
    /// called at awake of edit mode
    /// </summary>
    private void ExecuteOutOfPlayMode(bool fromPlay)
    {
        Debug.Log("first awake in edit mode");
        //_sceneStartHere.InitCalledFromEditor(fromPlay);
    }

    /// <summary>
    /// called at awake of play mode
    /// </summary>
    private void ExecuteInPlayMode()
    {
        Debug.Log("first awake in play mode");
        //_sceneStartHere.InitFromPlayMode(false);
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            PlayerPrefs.SetInt("should spawn", 1);
        }
    }
}
