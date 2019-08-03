using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Player Move forward locally")]
public class PlayerMove : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private float speedMove = 5f;
    
    [FoldoutGroup("Object"), SerializeField, Tooltip("ref")]
    private Rigidbody rb = null;

    [FoldoutGroup("Debug"), SerializeField, Tooltip("ref"), ReadOnly]
    private float currentSpeedMove = 0f;

    private void OnEnable()
    {

    }

    /// <summary>
    /// move with input
    /// </summary>
    /// <param name="direction"></param>
    public void MovePhysics(Vector3 direction)
    {
        //UnityMovement.MoveByForcePushing_WithPhysics(rb, direction, currentSpeedMove * entityAction.GetMagnitudeInput(minInput, 1f));
    }

    /// <summary>
    /// move in physics, according to input of player
    /// </summary>
    private void MovePlayer()
    {
        Vector3 dirMove = Vector3.zero;
        //dirMove = entityController.GetFocusedForwardDirPlayer();
        
        MovePhysics(dirMove);
    }

    /// <summary>
    /// handle move physics
    /// </summary>
    private void FixedUpdate()
    {
        /*if (entityController.GetMoveState() == EntityController.MoveState.Move)
        {
            MovePlayer();
        }
        */
    }

    
}
