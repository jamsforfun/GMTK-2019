using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    public AllPlayerLinker AllPlayerLinker;

    private void OnCollisionEnter(Collision collision)
    {
        /*
        PlayerLinker player = null;
        if (AllPlayerLinker.IsThatColliderIsAPlayer(collision.collider, out player))
        {

        }
        */
    }
}
