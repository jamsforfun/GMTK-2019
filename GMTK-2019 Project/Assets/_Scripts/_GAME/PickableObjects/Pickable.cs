using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour
{
    [SerializeField, FoldoutGroup("GamePlay")] private bool _isAvailable = true;
    [SerializeField, FoldoutGroup("GamePlay")] private float _dropInitialVelocity = 1f;

    [SerializeField, FoldoutGroup("Object")] private Rigidbody _rigidbody = default;

    [ReadOnly] public Transform AllItems;
    [ReadOnly] public AllPlayerLinker AllPlayerLinker;

    private const float DISTANCE_ON_TOP_OF_PLAYER = 1;

    //Comment to commit

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isAvailable)
        {
            return;
        }
        PlayerLinker collidingPlayerLinker;
        bool isColliderAPlayer = IsColliderAPlayer(collision, out collidingPlayerLinker);
        if (isColliderAPlayer)
        {
            PlayerObjectInteraction playerObjectInteraction = collidingPlayerLinker.PlayerObjectInteraction;
            if (!playerObjectInteraction.HasItem)
            {
                _isAvailable = false;
                SetupItemTransform(collidingPlayerLinker.RenderPlayerTurn);
                playerObjectInteraction.SetItem(this);
            }
        }
    }

    private bool IsColliderAPlayer(Collision collision, out PlayerLinker collisionPlayerLinker)
    {
        foreach (PlayerLinker playerLinker in AllPlayerLinker.PlayerLinker)
        {
            if (playerLinker.ColliderPlayer == collision.collider)
            {
                collisionPlayerLinker = playerLinker;
                return true;
            }
        }
        collisionPlayerLinker = null;
        return false;
    }

    private void SetupItemTransform(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
        transform.localPosition = DISTANCE_ON_TOP_OF_PLAYER * Vector3.up;
        transform.localRotation = Quaternion.identity;
        _rigidbody.isKinematic = true;
    }

    public void DropItem()
    {
        Vector3 dropDirection = transform.parent.forward;
        transform.SetParent(AllItems);
        _isAvailable = true;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = dropDirection * _dropInitialVelocity;
    }
}
