using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TypeInfoBox("Player Move forward locally")]
public class PlayerMove : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private float _speedMove = 5f;
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private AnimationCurve _easeAcceleration = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    [FoldoutGroup("GamePlay"), Tooltip("drag when we move"), SerializeField]
    private float _moveDrag = 0;
    [FoldoutGroup("GamePlay"), Tooltip("drag when we stop"), SerializeField]
    private float _stopDrag = 10f;

    [FoldoutGroup("Object"), SerializeField, Tooltip("ref")]
    private PlayerInput _playerInput;
    [FoldoutGroup("Object"), SerializeField, Tooltip("ref")]
    private Rigidbody rb = null;

    /// <summary>
    /// move with input
    /// </summary>
    /// <param name="direction"></param>
    public void MovePhysics(Vector2 direction)
    {
        UnityMovement.MoveByForcePushing_WithPhysics(rb, new Vector3(direction.x, 0, direction.y), _speedMove * _playerInput.GetMagnitudeInput());
    }

    /// <summary>
    /// move in physics, according to input of player
    /// </summary>
    private void MovePlayer()
    {
        Vector3 dirMove = _playerInput.GetMoveDirection();
        MovePhysics(dirMove);
    }

    /// <summary>
    /// handle move physics
    /// </summary>
    public void CustomFixedUpdate()
    {
        if (_playerInput.IsMoving())
        {
            rb.drag = _moveDrag;
            MovePlayer();
        }
        else
        {
            rb.drag = _stopDrag;
        }
    }
}
