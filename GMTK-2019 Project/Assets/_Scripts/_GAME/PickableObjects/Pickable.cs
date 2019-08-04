using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ItemType
{
    None = 0,
    Copper = 1,
    Plastic = 2,
    Useless = 3,
    ControllerNoButtons = 4,
    ControllerWithButtons = 5,
    Electronics = 6,
    Case = 7,
    Console = 8
}

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IKillable
{
    [SerializeField, FoldoutGroup("GamePlay")] private bool _isAvailable = true;
    [SerializeField, FoldoutGroup("GamePlay")] private float _dropInitialForce = 1f;
    [SerializeField, FoldoutGroup("GamePlay")] private float _timeBeforeGetBackTheItem = 0.3f;

    [SerializeField, FoldoutGroup("Object")] private Rigidbody _rigidbody = default;
    [SerializeField, FoldoutGroup("Object")] private ItemTransfer _itemTransfer = default;
    [SerializeField, FoldoutGroup("Object"), ReadOnly] private PlayerLinker _playerLinker;

    [SerializeField, FoldoutGroup("Prefabs")] private GameObject _particlePrefabsToCreate;


    [SerializeField] private pickableinput _pickableType;

    [ReadOnly] public Transform AllItems;
    [ReadOnly] public AllPlayerLinker AllPlayerLinker;

    private const float DISTANCE_ON_TOP_OF_PLAYER = 1;
    private FrequencyCoolDown _timerPickable = new FrequencyCoolDown();

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
            bool isCollidingWithPreviousPlayer = IsCollidingWithPreviousPlayer(collidingPlayerLinker);
            if (isCollidingWithPreviousPlayer)
            {
                return;
            }

            if (_itemTransfer.IsInTransfer && !IsCollidingWithPreviousPlayer(collidingPlayerLinker))
            {
                _itemTransfer.StopTransfer();
            }

            PlayerObjectInteraction playerObjectInteraction = collidingPlayerLinker.PlayerObjectInteraction;
            bool hasItemSwapped;
            playerObjectInteraction.SetItem(this, out hasItemSwapped);
            _isAvailable = !hasItemSwapped;

            if (hasItemSwapped)
            {
                SetupItemTransform(collidingPlayerLinker.RenderPlayerTurn);
                collidingPlayerLinker.PlayerAction.SetCurrentItem(_itemTransfer);
            }
            _playerLinker = collidingPlayerLinker;

        }
    }

    public static void GivePickableToPlayer(PlayerLinker playerlinker, Pickable pickPrefabs)
    {
        Pickable pickable = Instantiate(pickPrefabs, playerlinker.Rigidbody.transform);
        pickable.AllPlayerLinker = playerlinker.AllPlayerLinker;
        pickable.AllItems = playerlinker.AllPlayerLinker.AllItems;

        playerlinker.PlayerObjectInteraction.SetItem(pickable, out bool hitemSwapped);
        pickable.SetupItemTransform(playerlinker.Rigidbody.transform);
        playerlinker.PlayerAction.SetCurrentItem(pickable._itemTransfer);
    }

    private bool IsCollidingWithPreviousPlayer(PlayerLinker collidingPlayerLinker)
    {
        return _playerLinker != null
            && collidingPlayerLinker.GetInstanceID() == _playerLinker.GetInstanceID()
            && _timerPickable.IsRunning();
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
        DetachFromPlayer(true);
        _rigidbody.AddForce(dropDirection * _dropInitialForce);
        //_rigidbody.velocity = dropDirection * _dropInitialVelocity;
    }

    public void DetachFromPlayer(bool shouldUseGravity = true)
    {
        transform.SetParent(AllItems);
        _isAvailable = true;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = shouldUseGravity;
        _timerPickable.StartCoolDown(_timeBeforeGetBackTheItem);
    }

    public void Kill()
    {
        Instantiate(_particlePrefabsToCreate, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

    public pickableinput PickableType
    {
        get { return _pickableType; }
    }
}
