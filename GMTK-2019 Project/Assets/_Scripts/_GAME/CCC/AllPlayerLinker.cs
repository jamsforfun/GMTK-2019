using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayerLinker : MonoBehaviour
{
    [Serializable]
    public struct ReferenceButton
    {
        public string TextButton;
        public PlayerLinker PlayerLinker;
    }

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    public PlayerLinker[] PlayerLinker;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    public ReferenceButton[] ReferenceButtonPlayer;
}
