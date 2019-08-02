using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtAnimationCurve
{
    /// <summary>
    /// return a color from a string
    /// </summary>
    public static float GetMaxValue(this AnimationCurve animationCurve, ref int index)
    {
        if (animationCurve.length == 0)
        {
            Debug.LogWarning("no keys");
            index = -1;
            return (0);
        }

        index = 0;
        float maxValue = animationCurve[0].value;
        for (int i = 1; i < animationCurve.length; i++)
        {
            if (animationCurve[i].value > maxValue)
            {
                maxValue = animationCurve[i].value;
                index = i;
            }
        }
        return (maxValue);
    }

    /// <summary>
    /// return a color from a string
    /// </summary>
    public static float GetMinValue(this AnimationCurve animationCurve, ref int index)
    {
        if (animationCurve.length == 0)
        {
            Debug.LogWarning("no keys");
            index = -1;
            return (0);
        }

        index = 0;
        float minValue = animationCurve[0].value;
        for (int i = 1; i < animationCurve.length; i++)
        {
            if (animationCurve[i].value < minValue)
            {
                minValue = animationCurve[i].value;
                index = i;
            }
        }
        return (minValue);
    }
}
