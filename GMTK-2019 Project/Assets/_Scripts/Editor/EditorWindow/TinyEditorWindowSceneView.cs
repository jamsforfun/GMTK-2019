using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TinyEditorWindowSceneView
{
    public struct SaveEditorPref
    {
        public Rect Position;
        public bool Minimised;
    }

    private SaveEditorPref _saveEditorPreference = new SaveEditorPref();

    public string GetJsonOfSave()
    {
        _saveEditorPreference.Position = _windowMenu;
        _saveEditorPreference.Minimised = _isMinimised;
        string json = JsonUtility.ToJson(_saveEditorPreference);
        return (json);
    }

    public SaveEditorPref GetFromJsonDatasEditorPref(string json)
    {
        _saveEditorPreference = JsonUtility.FromJson<SaveEditorPref>(json);
        return (_saveEditorPreference);
    }

    private Rect _windowMenu;

    public delegate void ContentToDrawDelegate();
    private ContentToDrawDelegate _methodToCall;
    private SceneView _sceneView;
    private Event _currentEvent;
    private float _saveHeight = 0;
    private bool _isMinimised = false;

    private string _keyEditorPref = "";
    private string _nameEditorWindow = "";
    private int _id;

    private bool _draggable = false;
    private bool _isMinimisable = true;
    private Vector2 _lastPositionRight;
    private bool _snapRight;
    private bool _snapUp;

    private Rect oldRect;

    /// <summary>
    /// need to be called in OnGUI
    /// </summary>
    public void Init(string keyEditorPref,
                    string nameEditorWindow,
                    int id,

                    Rect windowMenu,
                    SceneView sceneView,
                    Event currentEvent,

                    bool draggable,
                    bool isMinimisable,
                    bool snapRight,
                    bool snapUp,
                    
                    bool applySetup = true)
    {
        _keyEditorPref = keyEditorPref;
        _nameEditorWindow = nameEditorWindow;
        _id = id;
        _windowMenu = windowMenu;
        _sceneView = sceneView;
        _currentEvent = currentEvent;

        _draggable = draggable;
        _isMinimisable = isMinimisable;
        _snapRight = snapRight;
        _snapUp = snapUp;

        _isMinimised = false;
        _saveHeight = _windowMenu.height;

        if (applySetup)
        {
            ApplySetup();
        }
    }

    private void ApplySetup()
    {
        if (!_keyEditorPref.IsNullOrEmpty() && EditorPrefs.HasKey(_keyEditorPref))
        {
            ApplySaveSettings();
        }
        SavePositionFromRight(_snapRight, _snapUp);
        //_windowMenu = ReworkPosition(_windowMenu, _snapRight, _snapUp);
    }

    /// <summary>
    /// apply the settings of the saved EditorPref
    /// </summary>
    private void ApplySaveSettings()
    {
        string keyRect = EditorPrefs.GetString(_keyEditorPref);
        _saveEditorPreference = GetFromJsonDatasEditorPref(keyRect);
        _windowMenu = _saveEditorPreference.Position;
        _isMinimised = _saveEditorPreference.Minimised;
        if (!_isMinimisable)
        {
            _isMinimisable = false;
        }
    }

    private bool HasChanged()
    {
        if (_saveEditorPreference.Position != _windowMenu
            || _saveEditorPreference.Minimised != _isMinimised)
        {
            return (true);
        }
        return (false);
    }

    private void SavePositionFromRight(bool snapRight, bool snapUp)
    {
        _lastPositionRight.x = (snapRight) ? EditorGUIUtility.currentViewWidth - _windowMenu.x : _windowMenu.x;
        _lastPositionRight.y = (snapUp) ? _windowMenu.y : _sceneView.camera.pixelRect.size.y - _windowMenu.y;
    }

    private Vector2 GetNewRightPositiionFromOld(bool snapXRight, bool snapYUp)
    {
        Vector2 pos;
        pos.x = (snapXRight) ? EditorGUIUtility.currentViewWidth - _lastPositionRight.x : _lastPositionRight.x;
        pos.y = (snapYUp) ? _lastPositionRight.y : _sceneView.camera.pixelRect.size.y - _lastPositionRight.y;
        return (pos);
    }

    /// <summary>
    /// called every frame, 
    /// </summary>
    /// <param name="action"></param>
    public Rect ShowEditorWindow(ContentToDrawDelegate action,
                                SceneView sceneView,
                                Event current)
    {
        _methodToCall = action;
        _sceneView = sceneView;
        _currentEvent = current;

        _windowMenu = ReworkPosition(_windowMenu, _snapRight, _snapUp);

        if (!_isMinimisable)
        {
            _isMinimised = false;
        }

        if (_isMinimised)
        {
            _windowMenu.height = 15f;
            _windowMenu = GUI.Window(_id, _windowMenu, NavigatorWindow, _nameEditorWindow);
            SavePositionFromRight(_snapRight, _snapUp);
        }
        else
        {
            //here resize auto
            _windowMenu = GUILayout.Window(_id, _windowMenu, NavigatorWindow, _nameEditorWindow, GUILayout.ExpandHeight(true));
            SavePositionFromRight(_snapRight, _snapUp);
        }

        if (!_keyEditorPref.IsNullOrEmpty() && HasChanged())
        {
            EditorPrefs.SetString(_keyEditorPref, GetJsonOfSave());
        }
        return (_windowMenu);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="minimise"></param>
    /// <param name="hard"></param>
    public void SetMinimise(bool minimise, bool hard)
    {
        if (!_isMinimisable)
        {
            if (!hard)
            {
                return;
            }
            _isMinimised = minimise;
        }
        _isMinimised = minimise;
    }

    /// <summary>
    /// called to display the minimsie button
    /// </summary>
    private void DisplayMinimise()
    {
        if (_isMinimised)
        {
            GUILayout.BeginArea(new Rect(0, 0, _windowMenu.width, 15));
            GUILayout.Label("   " + _nameEditorWindow);
            GUILayout.EndArea();
        }
        

        float x = _windowMenu.width - 20;
        float y = 0;
        float width = 20f;
        float height = 15f;

        GUILayout.BeginArea(new Rect(x, y, width, height));
        bool minimise = GUILayout.Button(" - ");
        GUILayout.EndArea();

        if (minimise)
        {
            _isMinimised = (_isMinimised) ? false : true;
        }
    }

    private void DisplayDraggable()
    {
        GUILayout.BeginArea(new Rect(0, 0, 15, 15));
        GUILayout.Label("#");
        GUILayout.EndArea();
    }

    /// <summary>
    /// give restriction to position of the panel
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    private Rect ReworkPosition(Rect currentPosition, bool snapRight, bool snapUp)
    {
        Vector2 snapToRight = GetNewRightPositiionFromOld(snapRight, snapUp);
        currentPosition.x = snapToRight.x;
        currentPosition.y = snapToRight.y;

        float minX = 10;
        float minY = 20;
        float maxX = EditorGUIUtility.currentViewWidth - currentPosition.width - minX;
        float maxY = _sceneView.camera.pixelRect.size.y - currentPosition.height + 5;

        if (currentPosition.x < minX)
        {
            currentPosition.x = minX;
        }
        if (currentPosition.y < minY)
        {
            currentPosition.y = minY;
        }
        if (currentPosition.x > maxX)
        {
            currentPosition.x = maxX;
        }

        //here the _sceneView.camera.pixelRect.size.y is incoherent when applying this event
        if (_currentEvent.type != EventType.DragPerform
            && _currentEvent.type != EventType.Used
            && _currentEvent.type != EventType.ExecuteCommand
            && _currentEvent.type != EventType.Ignore
            && _currentEvent.type != EventType.MouseDown
            && _currentEvent.type != EventType.MouseUp)
        {
            //Debug.Log(_currentEvent.type);
            if (currentPosition.y > maxY)
            {
                currentPosition.y = maxY;
            }
        }
        return (currentPosition);
    }


    /// <summary>
    /// draw the editor window with everything in it
    /// </summary>
    /// <param name="id"></param>
    private void NavigatorWindow(int id)
    {
        if (_isMinimisable)
        {
            DisplayMinimise();
        }

        if (_draggable)
        {
            DisplayDraggable();
            GUI.DragWindow(new Rect(0, 0, _windowMenu.width - 20, 20));  //allow to drag de window only at the top
        }

        //don't draw anything if it's minimised
        if (_isMinimised)
        {
            return;
        }
        _methodToCall();
    }
}
