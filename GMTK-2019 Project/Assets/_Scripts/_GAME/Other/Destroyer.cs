using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private Transform ParticleFire;

    private void OnTriggerEnter(Collider other)
    {
        IKillable kill = other.gameObject.GetComponent<IKillable>();
        if (kill != null)
        {
            kill.Kill();
        }
    }
}
