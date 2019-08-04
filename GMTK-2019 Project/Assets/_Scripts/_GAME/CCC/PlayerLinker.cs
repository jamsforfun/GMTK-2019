using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Player Move forward locally")]
public class PlayerLinker : MonoBehaviour
{
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerManager PlayerManager;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerMove PlayerMove;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerInput PlayerInput;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerRender PlayerRender;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerAction PlayerAction;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerCollide PlayerCollide;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerParticle PlayerParticle;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public LandfilOnPlayer LandfilOnPlayer;

    [FoldoutGroup("Object"), Tooltip("ref")]
    public Rigidbody Rigidbody;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public Transform RenderPlayer;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public Transform RenderPlayerTurn;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public Collider ColliderPlayer;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerObjectInteraction PlayerObjectInteraction;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public GroundForwardCheck GroundForwardCheck;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public PlayerSlide PlayerSlide;
    [FoldoutGroup("Object"), Tooltip("ref")]
    public AllPlayerLinker AllPlayerLinker;

    public void Init(AllPlayerLinker allPlayerLinker)
    {
        AllPlayerLinker = allPlayerLinker;
    }
}
