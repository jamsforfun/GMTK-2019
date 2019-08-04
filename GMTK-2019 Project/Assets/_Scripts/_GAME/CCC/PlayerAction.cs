using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Player Move forward locally")]
public class PlayerAction : MonoBehaviour
{
    [FoldoutGroup("Object"), SerializeField] private GameState _gameState = default;
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private PlayerInput _playerInput;
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private PlayerLinker _playerLinker;
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField, ReadOnly]
    public AllPlayerLinker AllPlayerLinker;

    private ItemTransfer _itemtranfer = default;

    private void Start()
    {
        if (_gameState == null)
        {
            _gameState = ExtUtilityFunction.GetScript<GameState>();
        }
    }

    /// <summary>
    /// Execute an action on this player
    /// </summary>
    public void AttackThisPlayer(PlayerLinker playerAttack)
    {
        if (_gameState.StateOfGame == GameState.StateGame.WIN_GAME)
        {
            return;
        }
        playerAttack.PlayerRender.SetAction();
        if (playerAttack.GetInstanceID() == _playerLinker.GetInstanceID())
        {
            //here we attack on our own
        }
        else
        {
            //here we attack other
            if (_itemtranfer != null)
            {
                _itemtranfer.TransferItemToPlayer(playerAttack);
                RemoveItem();
            }
        }
    }

    private void RemoveItem()
    {

        _playerLinker.PlayerObjectInteraction.RemoveItem();
        _itemtranfer = null;
    }

    public void SetCurrentItem(ItemTransfer currentItem)
    {
        _itemtranfer = currentItem;
    }

    /// <summary>
    /// handle move physics
    /// </summary>
    public void CustomUpdate()
    {
        PlayerLinker playerAttack = null;
        if (_playerInput.FireA)
        {
            playerAttack = AllPlayerLinker.ActionButton(_playerLinker, 0);
        }
        else if (_playerInput.FireB)
        {
            playerAttack = AllPlayerLinker.ActionButton(_playerLinker, 1);
        }
        else if (_playerInput.FireY)
        {
            playerAttack = AllPlayerLinker.ActionButton(_playerLinker, 2);
        }
        else if (_playerInput.FireX)
        {
            playerAttack = AllPlayerLinker.ActionButton(_playerLinker, 3);
        }
        if (playerAttack != null)
        {
            AttackThisPlayer(playerAttack);
        }
    }
}
