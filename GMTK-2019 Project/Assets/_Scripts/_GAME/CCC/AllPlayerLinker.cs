using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AllPlayerLinker : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref")]
    private List<PlayerLinker> _playerLinker;

#if UNITY_EDITOR
    public void Update()
    {
        /*
        for (int i = 0; i < _playerController.Count; i++)
        {
            _playerController.
        }
        */
    }
#endif
}
