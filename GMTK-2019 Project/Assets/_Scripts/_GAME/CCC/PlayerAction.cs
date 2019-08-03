﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Player Move forward locally")]
public class PlayerAction : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private PlayerInput _playerInput;
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private PlayerLinker _playerLinker;
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    public AllPlayerLinker AllPlayerLinker;

    /// <summary>
    /// Execute an action on this player
    /// </summary>
    public void AttackThisPlayer(PlayerLinker playerAttack)
    {
        playerAttack.PlayerRender.SetAction();
        if (playerAttack.GetInstanceID() == _playerLinker.GetInstanceID())
        {
            //here we attack on our own
        }
        else
        {
            //here we attack other
        }
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