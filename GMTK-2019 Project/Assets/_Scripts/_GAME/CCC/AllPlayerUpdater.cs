﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AllPlayerUpdater : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private AllPlayerLinker _allPlayerLinker;

    private void SetInEditorIdPlayer()
    {
        for (int i = 0; i < _allPlayerLinker.PlayerLinker.Length; i++)
        {
            _allPlayerLinker.PlayerLinker[i].PlayerManager.Id = i;
            _allPlayerLinker.PlayerLinker[i].transform.name = _allPlayerLinker.PlayerLinker[i].PlayerManager.PlayerSettings.NamePlayer + " " + i;
            _allPlayerLinker.PlayerLinker[i].PlayerRender.SetMaterial(_allPlayerLinker.PlayerLinker[i].PlayerManager.PlayerSettings.Material);
            _allPlayerLinker.PlayerLinker[i].PlayerRender.SetText(_allPlayerLinker.ReferenceButtonPlayer[i].TextButton, _allPlayerLinker.PlayerLinker[i].PlayerManager.PlayerSettings.Material.color);
        }
    }

    /// <summary>
    /// update all the players
    /// </summary>
    private void UpdateAllPlayer()
    {
        for (int i = 0; i < _allPlayerLinker.PlayerLinker.Length; i++)
        {
            _allPlayerLinker.PlayerLinker[i].PlayerInput.CustomUpdate();
        }
    }

    /// <summary>
    /// fixedUpdate all the player
    /// </summary>
    private void FixedUpdateAllPlayer()
    {
        for (int i = 0; i < _allPlayerLinker.PlayerLinker.Length; i++)
        {
            _allPlayerLinker.PlayerLinker[i].PlayerMove.CustomFixedUpdate();
        }
    }

    public void Update()
    {
#if UNITY_EDITOR
        SetInEditorIdPlayer();
#endif
        if (!Application.isPlaying)
        {
            return;
        }

        UpdateAllPlayer();
    }

    public void FixedUpdate()
    {
        FixedUpdateAllPlayer();
    }
}
