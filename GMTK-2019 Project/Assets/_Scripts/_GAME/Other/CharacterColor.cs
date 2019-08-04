using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour
{
    [SerializeField] private Color[] _idColors = { };
    [SerializeField] private Renderer[] _meshToColor = { };


    private void Awake()
    {
        PlayerManager player = GetComponent<PlayerManager>();

        Color targetColor = _idColors[Mathf.Clamp(player.Id,0,_idColors.Length)];
        for (int i = 0; i < _meshToColor.Length; i++)
        {
            _meshToColor[i].material.SetColor("_Color", targetColor);
        }
    }
}
