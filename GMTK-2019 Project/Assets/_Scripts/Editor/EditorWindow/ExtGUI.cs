using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

/// <summary>
/// all of this function must be called in OnSceneGUI
/// </summary>
public static class ExtGUI
{
    /// <summary>
    /// draw a float field with a slider to move
    /// must be called in OnSceneGUI
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float FloatField(float value, string label = "", float min = -9999, float max = 9999, params GUILayoutOption[] options)
    {
        using (HorizontalScope horizontalScope = new HorizontalScope(GUILayout.MaxWidth(50)))
        {
            if (!label.IsNullOrEmpty())
            {
                GUILayout.Label(label);
            }
            value = EditorGUILayout.FloatField(value, options);
            value = EditorGUILayout.Slider(value, min, max, GUILayout.Width(2));
        }
        return (value);
    }
}
