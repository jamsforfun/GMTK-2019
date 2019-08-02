using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GUILayout;

/// <summary>
/// always display UI in scene (if found: GameManager, RoadCreator, CheckPointsManager)
/// </summary>
public class EditorSceneView : EditorWindow
{
    private static bool _isEnabled = false;
    private static SceneView _currentSceneView; //need to be updated each frame
    private static Event _currentEvent; //need to be updated each frame
    private static ExtUtilityEditor.HitSceneView hitScene;

    /// <summary>
    /// Reload script after each laod scene, compile callBack, or play/unplay event
    /// 
    /// if not called in reload: after compiling, it did'nt show UI
    /// </summary>
    [MenuItem("PERSO/Enable GameManager GUI")]
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void Enable()
    {
        if (_isEnabled)
        {
            Disable();
            return;
        }
        SceneView.onSceneGUIDelegate += OnScene;
        _isEnabled = true;

        SetupAllScripts();
    }

    public static bool IsEnabled()
    {
        return (_isEnabled);
    }

    private static void ResetupIfNull()
    {
        /*
        if (!_raceSceneLinker || !_raceSceneLinker)
        {
            SetupAllScripts();
        }
        */
    }

    /// <summary>
    /// Important ! Setup all script in scene needed
    /// </summary>
    private static void SetupAllScripts()
    {
        //_raceSceneLinker = UtilityEditor.GetScript<RaceSceneLinker>();
    }

    /// <summary>
    /// Raycast from mouse Position in scene view for future mouseClick object
    /// </summary>
    private static void SetCurrentOverObject()
    {
        hitScene = ExtUtilityEditor.SetCurrentOverObject(hitScene);
    }

    private static void RightClickOnObjectInScene()
    {
        //KeySplineComon keySplineComon = hitScene.objHit.GetExtComponentInParents<KeySplineComon>(99, true);
    }

    /// <summary>
    /// all event of the RaceScene UI 
    /// </summary>
    private static void EventInput()
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        bool forced = false;

        switch (_currentEvent.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                break;
        }

        //here click on object with CTRL + ALT + RIGHT CLIC
        if (_currentEvent.button == 1 && _currentEvent.control && _currentEvent.alt && _currentEvent.GetTypeForControl(GUIUtility.GetControlID(FocusType.Passive)) == EventType.MouseUp && hitScene.objHit)
        {
            //DeleteObjectInScene();
        }
        //clic on object with right click only
        else if (_currentEvent.button == 1 && !_currentEvent.control && _currentEvent.GetTypeForControl(GUIUtility.GetControlID(FocusType.Passive)) == EventType.MouseUp && hitScene.objHit)
        {
            RightClickOnObjectInScene();
        }
    }

    
    private static bool CanSetupRaceUI()
    {
        bool canSetup = true;//(_raceSceneLinker && _toolsRaceTrack);
        return (canSetup);
    }
    

    /// <summary>
    /// Draw all GUI
    /// </summary>
    private static void AllGUI()
    {

    }

    private static void OwnGUI()
    {
        _currentSceneView = SceneView.currentDrawingSceneView;
        _currentEvent = Event.current;

        Color oldColor = GUI.backgroundColor;

        AllGUI();
        SetCurrentOverObject();
        EventInput();

        GUI.backgroundColor = oldColor;
    }

    private static void OnScene(SceneView sceneview)
    {
        ResetupIfNull();

        if (CanSetupRaceUI())
        {
            OwnGUI();
        }
    }

    private static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        Debug.Log("Scene GUI : Disabled");
        _isEnabled = false;
    }
}
