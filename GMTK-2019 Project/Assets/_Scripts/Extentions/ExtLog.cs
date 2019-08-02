using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtLog
{
    /// <summary>
    /// is this list contain a string (nice for enum test)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tag"></param>
    /// <param name="listEnum"></param>
    /// <returns></returns>
    public static void LogList<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
        }
    }
    
    public static void LogArray<T>(this T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(array[i]);
        }
    }
}
