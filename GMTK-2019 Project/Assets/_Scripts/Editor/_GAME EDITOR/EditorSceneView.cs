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

    private static bool _needToSetupUI = true;
    public const string MAIN_WINDOW = "MAIN_WINDOW";
    private static TinyEditorWindowSceneView _mainWindow;

    private static GameLinker _gameLinker;

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
        _needToSetupUI = true;

        SetupAllScripts();
    }

    private static void SetupEditorWindowNavigator()
    {
        _mainWindow = new TinyEditorWindowSceneView();
        float xFromRight = 140; //from the right, substract this value to go left a bit
        float width = 120;
        float x = EditorGUIUtility.currentViewWidth - width - xFromRight;
        float y = 20;
        float height = 150;
        Rect rectNav = new Rect(x, y, width, height);
        _mainWindow.Init(MAIN_WINDOW, "EditorWindow", 0, rectNav, _currentSceneView, _currentEvent, true, true, true, true);
    }

    private static void SetupAllNavigator()
    {
        _needToSetupUI = false;

        SetupEditorWindowNavigator();
    }

    public static bool IsEnabled()
    {
        return (_isEnabled);
    }

    private static void ResetupIfNull()
    {
        if (!_gameLinker)
        {
            SetupAllScripts();
        }
    }

    /// <summary>
    /// Important ! Setup all script in scene needed
    /// </summary>
    private static void SetupAllScripts()
    {
        _gameLinker = ExtUtilityEditor.GetScript<GameLinker>();
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
        bool canSetup = _gameLinker;
        return (canSetup);
    }
    
    public static void DisplayMain()
    {
        using (HorizontalScope horizontalScope = new HorizontalScope())
        {
            if (GUILayout.Button("Menu"))
            {
                //EditorSceneManager.MarkAllScenesDirty();
                EditorSceneManager.SaveOpenScenes();
                _gameLinker.SceneLoader.LoadSceneByTrackIndex(0);
            }
            if (GUILayout.Button("Game"))
            {
                EditorSceneManager.SaveOpenScenes();
                _gameLinker.SceneLoader.LoadSceneByTrackIndex(1);
            }
        }
    }

    /// <summary>
    /// Draw all GUI
    /// </summary>
    private static void AllGUI()
    {
        _mainWindow.ShowEditorWindow(DisplayMain, _currentSceneView, _currentEvent);
    }

    private static void OwnGUI()
    {
        _currentSceneView = SceneView.currentDrawingSceneView;
        _currentEvent = Event.current;

        Color oldColor = GUI.backgroundColor;

        if (_needToSetupUI)
        {
            SetupAllNavigator();
        }

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
