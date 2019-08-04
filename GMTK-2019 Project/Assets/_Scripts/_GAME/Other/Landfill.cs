﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Landfill : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("ref"), SerializeField]
    private float Range = 3f;

    [FoldoutGroup("GamePlay"), Tooltip(""), SerializeField]
    public string[] PlayerLayer = new string[] { "Player"};

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    public AllPlayerLinker AllPlayerLinker;


    private Collider[] _results = new Collider[30];
    private LayerMask _layerMask;

    private void Awake()
    {
        _layerMask = LayerMask.GetMask(PlayerLayer);
    }

    private void Update()
    {
        ExtDrawGuizmos.DebugWireSphere(transform.position, Color.red, Range);

        if (!Application.isPlaying)
        {
            return;
        }

        int numberFound = Physics.OverlapSphereNonAlloc(transform.position, Range, _results, _layerMask);
        for (int i = 0; i < numberFound; i++)
        {
            PlayerLinker linker = _results[i].gameObject.GetExtComponentInParents<PlayerLinker>(99, true);
            if (linker == null)
            {
                continue;
            }
            LandfilOnPlayer landfil = linker.LandfilOnPlayer;
            landfil.SetInside(this);
        }

        for (int i = 0; i < AllPlayerLinker.PlayerLinker.Length; i++)
        {
            AllPlayerLinker.PlayerLinker[i].LandfilOnPlayer.TryToLeave();
        }
    }
}
