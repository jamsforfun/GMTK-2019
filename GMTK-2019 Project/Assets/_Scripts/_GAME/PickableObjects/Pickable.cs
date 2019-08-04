using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IKillable
{
    [SerializeField, FoldoutGroup("GamePlay")] private bool _isAvailable = true;
    [SerializeField, FoldoutGroup("GamePlay")] private float _dropInitialForce = 1f;
    [SerializeField, FoldoutGroup("GamePlay")] private float _timeBeforeGetBackTheItem = 0.3f;
    [SerializeField, FoldoutGroup("GamePlay")] private float _timeOfChangingLayerWhenDrop = 0.8f;

    [SerializeField, FoldoutGroup("Object")] private GameState _gameState = default;
    [SerializeField, FoldoutGroup("Object")] private Rigidbody _rigidbody = default;
    [SerializeField, FoldoutGroup("Object")] private Collider _collider = default;
    [SerializeField, FoldoutGroup("Object")] private ItemTransfer _itemTransfer = default;
    [SerializeField, FoldoutGroup("Object"), ReadOnly] private PlayerLinker _playerLinker;

    [SerializeField, FoldoutGroup("Prefabs")] private GameObject _particlePrefabsToCreate;

    [SerializeField] private pickableinput _pickableType;

    [ReadOnly] public Transform AllItems;
    [ReadOnly] public AllPlayerLinker AllPlayerLinker;

    private const float DISTANCE_ON_TOP_OF_PLAYER = 1;
    private const int LAYER_OF_DROPPING_ITEMS = 10;
    private FrequencyCoolDown _timerPickable = new FrequencyCoolDown();

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isAvailable || _gameState.StateOfGame == GameState.StateGame.WIN_GAME)
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
            playerObjectInteraction.SetItem(this, transform.forward, out hasItemSwapped);
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

        playerlinker.PlayerObjectInteraction.SetItem(pickable, playerlinker.Rigidbody.transform.forward, out bool hitemSwapped);
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
        if (_gameState.StateOfGame == GameState.StateGame.WIN_GAME)
        {
            return;
        }
        _collider.enabled = false;
        transform.SetParent(playerTransform);
        transform.localPosition = DISTANCE_ON_TOP_OF_PLAYER * Vector3.up;
        transform.localRotation = Quaternion.identity;
        _rigidbody.isKinematic = true;
    }

    public void DropItem(Vector3 dropDirection)
    {
        gameObject.SetLayerRecursively(LAYER_OF_DROPPING_ITEMS);
        Invoke(nameof(SetLayerBackToDefault), _timeOfChangingLayerWhenDrop);
        DetachFromPlayer(true);
        _collider.enabled = true;
        _rigidbody.AddForce(dropDirection * _dropInitialForce, ForceMode.Impulse);
    }

    public void SetLayerBackToDefault()
    {
        gameObject.SetLayerRecursively(0);
    }

    public void DetachFromPlayer(bool shouldUseGravity = true)
    {
        if (_gameState.StateOfGame == GameState.StateGame.WIN_GAME)
        {
            return;
        }
        transform.SetParent(AllItems);
        _isAvailable = true;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = shouldUseGravity;
        _timerPickable.StartCoolDown(_timeBeforeGetBackTheItem);
    }

    public void Kill()
    {
        if (_gameState.StateOfGame == GameState.StateGame.WIN_GAME)
        {
            return;
        }
        Instantiate(_particlePrefabsToCreate, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

    public pickableinput PickableType
    {
        get { return _pickableType; }
    }
}
