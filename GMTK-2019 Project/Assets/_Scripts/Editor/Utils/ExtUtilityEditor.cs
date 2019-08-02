using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class ExtUtilityEditor : ScriptableObject
{
    public struct HitSceneView
    {
        public GameObject objHit;
        public Vector3 pointHit;
        public Vector3 normal;
        public Ray Ray;
    }

    public static void DisplayStringIn3D(Vector3 position, string toDisplay)
    {
        GUIStyle textStyle = new GUIStyle();
        textStyle.fontSize = 14;
        textStyle.normal.textColor = Color.black;
        textStyle.alignment = TextAnchor.MiddleCenter;
        Handles.Label(position, toDisplay, textStyle);
    }

    /*
    /// <summary>
    /// From a given script, try to get the editor, with CreateEditor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="scriptType"></param>
    /// <returns></returns>
    public static U GetEditor<T, U>(T script)
    {
        U scriptEditor = (U)U.CreateEditor(script);
        return (scriptEditor);
    }
    */

    /// <summary>
    /// Raycast from mouse Position in scene view for future mouseClick object
    /// </summary>
    public static HitSceneView SetCurrentOverObject(HitSceneView newHit, bool setNullIfNot = true)
    {
        if (Event.current == null)
        {
            return (newHit);
        }
        Vector2 mousePos = Event.current.mousePosition;
        if (mousePos.x < 0 || mousePos.x >= Screen.width || mousePos.y < 0 || mousePos.y >= Screen.height)
        {
            return (newHit);
        }

        RaycastHit _saveRaycastHit;
        Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos);
        newHit.Ray = worldRay;
        //first a test only for point
        //if (Physics.Raycast(worldRay, out saveRaycastHit, Mathf.Infinity, 1 << LayerMask.NameToLayer(gravityAttractorEditor.layerPoint), QueryTriggerInteraction.Ignore))
        if (Physics.Raycast(worldRay, out _saveRaycastHit, Mathf.Infinity))
        {
            if (_saveRaycastHit.collider.gameObject != null)
            {
                newHit.objHit = _saveRaycastHit.collider.gameObject;
                newHit.pointHit = _saveRaycastHit.point;
                newHit.normal = _saveRaycastHit.normal;
            }
        }
        else
        {
            if (setNullIfNot)
            {
                newHit.objHit = null;
                newHit.pointHit = ExtVector3.GetNullVector();
            }
        }
        return (newHit);
    }

    public static T GetScript<T>()
    {
        object obj = UnityEngine.Object.FindObjectOfType(typeof(T));

        if (obj != null)
        {
            return ((T)obj);
            //gameManager = (GameManager)obj;
            //gameManager.indexSaveEditorTmp = gameManager.saveManager.GetMainData().GetLastMapSelectedIndex();
        }

        return (default(T));
    }
    public static T[] GetScripts<T>()
    {
        object[] obj = UnityEngine.Object.FindObjectsOfType(typeof(T));
        T[] list = new T[obj.Length];

        for (int i = 0; i < obj.Length; i++)
        {
            list[i] = (T)obj[i];
        }
        return (list);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public static List<T> GetListSerializedProperty<T>(SerializedProperty property)  where T : class
    {
        List<T> newList = new List<T>();
        int size = property.arraySize;
        for (int i = 0; i < size; i++)
        {
            newList.Add(property.GetArrayElementAtIndex(i).objectReferenceValue as T);
        }
        return (newList);
    }

    /// <summary>
    /// update the Hierarchy window
    /// </summary>
    public static void UpdateHierarchyWindow()
    {
        EditorApplication.RepaintHierarchyWindow();
    }

    public static void UpdateProjectWindow()
    {
        AssetDatabase.Refresh();
    }

    public static void ViewportPanZoomIn(float zoom = 5f)
    {
        //Debug.Log(SceneView.lastActiveSceneView.size);
        if (SceneView.lastActiveSceneView.size > zoom)
            SceneView.lastActiveSceneView.size = zoom;
        SceneView.lastActiveSceneView.Repaint();
    }

    public static void FocusOnSelection(GameObject objToFocus, float zoom = 5f)
    {
        SceneView.lastActiveSceneView.LookAt(objToFocus.transform.position);
        if (zoom != -1)
            ViewportPanZoomIn(zoom);
    }

    public static bool PingAndSelect(GameObject obj)
    {
        if (obj == null)
            return (false);

        EditorGUIUtility.PingObject(obj);
        Selection.activeObject = obj;
        return (true);
    }


    [MenuItem("PERSO/Ext/CreateEmptyParent #e")]
    public static void CreateEmptyParent()
    {
        if (!Selection.activeGameObject)
            return;
        GameObject newParent = new GameObject("Parent of " + Selection.activeGameObject.name);
        int indexFocused = Selection.activeGameObject.transform.GetSiblingIndex();
        newParent.transform.SetParent(Selection.activeGameObject.transform.parent);
        newParent.transform.position = Selection.activeGameObject.transform.position;

        Selection.activeGameObject.transform.SetParent(newParent.transform);
        newParent.transform.SetSiblingIndex(indexFocused);

        Selection.activeGameObject = newParent;
        ExtReflexion.SetExpandedRecursive(newParent, true);
    }

    [MenuItem("PERSO/Ext/DeleteEmptyParent %&e")]
    public static void DeleteEmptyParent()
    {
        if (!Selection.activeGameObject)
            return;

        int sibling = Selection.activeGameObject.transform.GetSiblingIndex();
        Transform parentOfParent = Selection.activeGameObject.transform.parent;
        Transform firstChild = Selection.activeGameObject.transform.GetChild(0);
        while (Selection.activeGameObject.transform.childCount > 0)
        {
            Transform child = Selection.activeGameObject.transform.GetChild(0);
            child.SetParent(parentOfParent);
            child.SetSiblingIndex(sibling);
            sibling++;
        }
        DestroyImmediate(Selection.activeGameObject);

        Selection.activeGameObject = firstChild.gameObject;
    }
}