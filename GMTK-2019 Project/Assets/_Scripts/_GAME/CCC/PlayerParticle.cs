using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("ref"), SerializeField]
    private Transform Trails;
    [FoldoutGroup("GamePlay"), Tooltip("ref"), SerializeField]
    private Transform CollideAgainst;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private PlayerLinker _playerLinker;

    public void LaunchCollideAgainst(Vector3 direction)
    {
        //CollideAgainst.gameObject.SetActive(true);
        CollideAgainst.gameObject.SetActiveRecursively(false);
        CollideAgainst.gameObject.SetActiveRecursively(true);
        CollideAgainst.transform.rotation = ExtQuaternion.TurretLookRotation(-direction, CollideAgainst.transform.up);
    }
}
