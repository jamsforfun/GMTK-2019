﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtArray
{
    /// <summary>
    /// when we are looping in an array, determine if we are at the end loop of it
    /// </summary>
    /// <typeparam name="T">type of the array</typeparam>
    /// <param name="collection">current array</param>
    /// <param name="i">index we currently are</param>
    /// <returns>true if we are at the end loop</returns>
    public static bool AreWeAtLastLoop<T>(this T[] collection, int i)
    {
        return (i + 1 >= collection.Length);
    }

    /// <summary>
    /// append an array into another
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    public static T[] Append<T>(T[] collection, T[] otherToApend)
    {
        T[] newArray = new T[collection.Length + otherToApend.Length];
        for (int i = 0; i < collection.Length; i++)
        {
            newArray[i] = collection[i];
        }
        for (int j = 0; j < otherToApend.Length; j++)
        {
            newArray[collection.Length + j] = otherToApend[j];
        }
        return (newArray);
    }

    /// <summary>
    /// add element in an array (incredibly slow, only for editor !)
    /// </summary>
    /// <typeparam name="T">type inside the array</typeparam>
    /// <param name="array">array to add</param>
    /// <param name="itemToAdd">item to add</param>
    /// <param name="index">index where to add the element</param>
    /// <param name="succes">have we succed or not ?</param>
    /// <returns>return the new array</returns>
    public static T[] AddAtIndex<T>(T[] array, T itemToAdd, int index, out bool succes)
    {
        if (index < 0 || index > array.Length)
        {
            succes = false;
            return (array);
        }
        T[] newArray = new T[array.Length + 1];
        for (int i = 0; i < index; i++)
        {
            newArray[i] = array[i];
        }
        newArray[index] = itemToAdd;
        for (int i = index; i < array.Length; i++)
        {
            newArray[i + 1] = array[i];
        }
        succes = true;
        return (newArray);
    }

    /// <summary>
    /// do not use, it's slow ! only in editor
    /// </summary>
    /// <typeparam name="T">type inside the array</typeparam>
    /// <param name="array">array to add</param>
    /// <param name="itemToAdd">item to add</param>
    /// <returns>return the new array</returns>
    public static T[] Add<T>(T[] array, T itemToAdd)
    {
        T[] newArray = new T[array.Length + 1];
        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }
        newArray[array.Length] = itemToAdd;
        return (newArray);
    }

    public static void ClearAndDestroy<T>(this T[] collection, bool immediate = false) where T : Component
    {
        for (int i = 0; i < collection.Length; i++)
        {
            if (immediate)
            {
                if (Application.isPlaying)
                {
                    GameObject.Destroy(collection[i].gameObject);
                }
                else
                {
                    GameObject.DestroyImmediate(collection[i].gameObject);
                }
            }
            else
            {
                GameObject.Destroy(collection[i].gameObject);
            }
        }
        collection.Clear();
    }

    public static float ClosestTo(this Array collection, float target, ref int indexFound)
    {
        // NB Method will return int.MaxValue for a sequence containing no elements.
        // Apply any defensive coding here as necessary.
        float closest = float.MaxValue;
        float minDifference = float.MaxValue;
        int index = 0;
        foreach (float element in collection)
        {
            float difference = Math.Abs((float)element - target);
            if (minDifference > difference)
            {
                minDifference = (float)difference;
                closest = element;
                indexFound = index;
            }
            index++;
        }

        return closest;
    }

    /// <summary>
    /// active all gameObject in an array of components
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="active"></param>
    public static void ActiveAll<T>(this T[] collection, bool active) where T : Component
    {
        for (int i = 0; i < collection.Length; i++)
        {
            collection[i].gameObject.SetActive(active);
        }
    }

    /// <summary>
    /// Fill with null value
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="target"></param>
    /// <param name="indexFound"></param>
    /// <returns></returns>
    public static void FillWith<T>(this T[] arrayFloat, T numberToFill)
    {
        for (int i = 0; i < arrayFloat.Length; i++)
        {
            arrayFloat[i] = numberToFill;
        }
    }

    /// <summary>
    /// true return if in the list, there is a word that is substring of the fileName
    /// </summary>
    /// <param name="toTransform"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static int ContainSubStringInArray(string[] toTransform, string fileName)
    {
        for (int i = 0; i < toTransform.Length; i++)
        {
            if (fileName.Contains(toTransform[i]))
                return (i);
        }
        return (-1);
    }


    #region General Methods

    public static bool IsEmpty(this IEnumerable lst)
    {
        if (lst is IList)
        {
            return (lst as IList).Count == 0;
        }
        else
        {
            return !lst.GetEnumerator().MoveNext();
        }
    }

    /// <summary>
    /// Get how deep into the enumerable the first instance of the object is.
    /// </summary>
    /// <param name="lst"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int Depth(this IEnumerable lst, object obj)
    {
        int i = 0;
        foreach (var o in lst)
        {
            if (object.Equals(o, obj)) return i;
            i++;
        }
        return -1;
    }

    /// <summary>
    /// Get how deep into the enumerable the first instance of the value is.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lst"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Depth<T>(this IEnumerable<T> lst, T value)
    {
        int i = 0;
        foreach (var v in lst)
        {
            if (object.Equals(v, value)) return i;
            i++;
        }
        return -1;
    }

    public static IEnumerable<T> Like<T>(this IEnumerable lst)
    {
        foreach (var obj in lst)
        {
            if (obj is T) yield return (T)obj;
        }
    }

    public static bool Compare<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        var e1 = first.GetEnumerator();
        var e2 = second.GetEnumerator();

        while (true)
        {
            var b1 = e1.MoveNext();
            var b2 = e2.MoveNext();
            if (!b1 && !b2) break; //reached end of list

            if (b1 && b2)
            {
                if (!object.Equals(e1.Current, e2.Current)) return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public static bool Contains<T>(this T[,] arr, T value)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                if (EqualityComparer<T>.Default.Equals(arr[i, j], value)) return true;
            }
        }

        return false;
    }

    public static T GetValueAfterOrDefault<T>(this IEnumerable<T> lst, T element, bool loop = false)
    {
        if (lst is IList<T>)
        {
            var arr = lst as IList<T>;
            if (arr.Count == 0) return default(T);

            int i = arr.IndexOf(element) + 1;
            if (loop) i = i % arr.Count;
            else if (i >= arr.Count) return default(T);
            return arr[i];
        }
        else
        {
            var e = lst.GetEnumerator();
            if (!e.MoveNext()) return default(T);
            var first = e.Current;
            if (object.Equals(e.Current, element))
            {
                if (e.MoveNext())
                {
                    return e.Current;
                }
                else if (loop)
                {
                    return first;
                }
                else
                {
                    return default(T);
                }
            }

            while (e.MoveNext())
            {
                if (object.Equals(e.Current, element))
                {
                    if (e.MoveNext())
                    {
                        return e.Current;
                    }
                    else if (loop)
                    {
                        return first;
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            return default(T);
        }
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> lst, T element)
    {
        if (lst == null) throw new System.ArgumentNullException("lst");
        foreach (var e in lst)
        {
            if (!object.Equals(e, element)) yield return e;
        }
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> lst, T element, IEqualityComparer<T> comparer)
    {
        if (lst == null) throw new System.ArgumentNullException("lst");
        if (comparer == null) throw new System.ArgumentNullException("comparer");
        foreach (var e in lst)
        {
            if (!comparer.Equals(e, element)) yield return e;
        }
    }

    #endregion
    
    #region Array Methods

    public static T[] Empty<T>()
    {
        return TempArray<T>.Empty;
    }

    public static T[] Temp<T>(T value)
    {
        return TempArray<T>.Temp(value);
    }

    public static T[] Temp<T>(T value1, T value2)
    {
        return TempArray<T>.Temp(value1, value2);
    }

    public static T[] Temp<T>(T value1, T value2, T value3)
    {
        return TempArray<T>.Temp(value1, value2, value3);
    }

    public static T[] Temp<T>(T value1, T value2, T value3, T value4)
    {
        return TempArray<T>.Temp(value1, value2, value3, value4);
    }

    public static void ReleaseTemp<T>(T[] arr)
    {
        TempArray<T>.Release(arr);
    }

    /// <summary>
    /// swap 2 element in an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    public static void Swap<T>(ref T[] array, int index1, int index2)
    {
        T tmp = array[index1];
        array[index1] = array[index2];
        array[index2] = tmp;
    }




    public static int IndexOf(this System.Array lst, object obj)
    {
        return System.Array.IndexOf(lst, obj);
    }

    public static int IndexOf<T>(this T[] lst, T obj)
    {
        return System.Array.IndexOf(lst, obj);
    }

    public static bool InBounds(this System.Array arr, int index)
    {
        return index >= 0 && index <= arr.Length - 1;
    }

    public static void Clear(this System.Array arr)
    {
        if (arr == null) return;
        System.Array.Clear(arr, 0, arr.Length);
    }

    public static void Copy<T>(IEnumerable<T> source, System.Array destination, int index)
    {
        if (source is System.Collections.ICollection)
            (source as System.Collections.ICollection).CopyTo(destination, index);
        else
        {
            int i = 0;
            foreach (var el in source)
            {
                destination.SetValue(el, i + index);
                i++;
            }
        }
    }


    #endregion

    #region HashSet Methods

    public static T Pop<T>(this HashSet<T> set)
    {
        if (set == null) throw new System.ArgumentNullException("set");

        var e = set.GetEnumerator();
        if (e.MoveNext())
        {
            set.Remove(e.Current);
            return e.Current;
        }

        throw new System.ArgumentException("HashSet must not be empty.");
    }

    #endregion

    #region Special Types

    private class TempArray<T>
    {

        private static object _lock = new object();
        private static volatile T[] _empty;
        private static volatile T[] _oneArray;
        private static volatile T[] _twoArray;
        private static volatile T[] _threeArray;
        private static volatile T[] _fourArray;

        public static T[] Empty
        {
            get
            {
                if (_empty == null) _empty = new T[0];
                return _empty;
            }
        }

        public static T[] Temp(T value)
        {
            T[] arr;

            lock (_lock)
            {
                if (_oneArray != null)
                {
                    arr = _oneArray;
                    _oneArray = null;
                }
                else
                {
                    arr = new T[1];
                }
            }

            arr[0] = value;
            return arr;
        }

        public static T[] Temp(T value1, T value2)
        {
            T[] arr;

            lock (_lock)
            {
                if (_oneArray != null)
                {
                    arr = _twoArray;
                    _twoArray = null;
                }
                else
                {
                    arr = new T[2];
                }
            }

            arr[0] = value1;
            arr[1] = value2;
            return arr;
        }

        public static T[] Temp(T value1, T value2, T value3)
        {
            T[] arr;

            lock (_lock)
            {
                if (_oneArray != null)
                {
                    arr = _threeArray;
                    _threeArray = null;
                }
                else
                {
                    arr = new T[3];
                }
            }

            arr[0] = value1;
            arr[1] = value2;
            arr[2] = value3;
            return arr;
        }

        public static T[] Temp(T value1, T value2, T value3, T value4)
        {
            T[] arr;

            lock (_lock)
            {
                if (_oneArray != null)
                {
                    arr = _fourArray;
                    _fourArray = null;
                }
                else
                {
                    arr = new T[4];
                }
            }

            arr[0] = value1;
            arr[1] = value2;
            arr[2] = value3;
            arr[3] = value4;
            return arr;
        }


        public static void Release(T[] arr)
        {
            if (arr == null) return;
            System.Array.Clear(arr, 0, arr.Length);

            lock (_lock)
            {
                switch (arr.Length)
                {
                    case 1:
                        _oneArray = arr;
                        break;
                    case 2:
                        _twoArray = arr;
                        break;
                    case 3:
                        _threeArray = arr;
                        break;
                    case 4:
                        _fourArray = arr;
                        break;
                }
            }
        }
    }

    #endregion
}
