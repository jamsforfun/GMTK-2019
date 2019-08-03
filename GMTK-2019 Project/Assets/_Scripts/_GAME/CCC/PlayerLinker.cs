using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Player Move forward locally")]
public class PlayerLinker : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerController PlayerController;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerMove PlayerMove;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerInput PlayerInput;


}
