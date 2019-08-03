using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("ref"), SerializeField]
    private float _impulseForce = 50f;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private PlayerLinker _playerLinker;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField, ReadOnly]
    public AllPlayerLinker AllPlayerLinker;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.collider == null || AllPlayerLinker == null)
        {
            return;
        }

        if (AllPlayerLinker.IsThatColliderIsAPlayer(collision.collider, out PlayerLinker player))
        {
            Debug.Log("collide with " + player);
            player.PlayerMove.ApplyImpulse((player.Rigidbody.transform.position - _playerLinker.Rigidbody.transform.position).FastNormalized(), _impulseForce);
        }
    }
}
