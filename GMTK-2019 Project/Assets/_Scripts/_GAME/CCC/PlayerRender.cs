using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRender : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerLinker _playerLinker;

    [FoldoutGroup("Object"), Tooltip("ref"), SerializeField]
    private MeshRenderer _meshRenderer;

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }
}
