using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private Animator _animator;

    public void OpenDoor()
    {
        _animator.SetTrigger("Open");
    }
}
