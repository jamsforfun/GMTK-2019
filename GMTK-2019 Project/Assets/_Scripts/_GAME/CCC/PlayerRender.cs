using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRender : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerLinker _playerLinker;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private MeshRenderer _meshRenderer;
    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private TextMeshProUGUI _text; 

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    public void SetText(string text, Color color)
    {
        _text.text = text;
        _text.color = color;
    }
}
