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
    private float _speedTurnRate = 600f;
    [FoldoutGroup("GamePlay"), Tooltip("speed move forward"), SerializeField]
    private FrequencyEaseAndOut _frequencyEaseAndOut = new FrequencyEaseAndOut();

    [FoldoutGroup("GamePlay"), Tooltip("drag when we move"), SerializeField]
    private float _moveDrag = 0;
    [FoldoutGroup("GamePlay"), Tooltip("drag when we stop"), SerializeField]
    private float _stopDrag = 10f;

    [FoldoutGroup("Object"), SerializeField, Tooltip("ref")]
    private PlayerInput _playerInput;
    [FoldoutGroup("Object"), SerializeField, Tooltip("ref")]
    private PlayerLinker _playerLinker;
    [FoldoutGroup("Object"), SerializeField, Tooltip("ref")]
    private Rigidbody rb = null;

    private Vector3 _lastDirection;
    private Quaternion _wantedDirection;

    private float GetSpeed()
    {
        float speed = _speedMove * _playerInput.GetMagnitudeInput();
        speed *= _frequencyEaseAndOut.EvaluateEveryFrameIn();
        return (speed);
    }

    public void Awake()
    {
        _lastDirection = _playerLinker.transform.forward;
    }

    /// <summary>
    /// move with input
    /// </summary>
    /// <param name="direction"></param>
    public void MovePhysics(Vector2 direction)
    {
        UnityMovement.MoveByForcePushing_WithPhysics(rb, new Vector3(direction.x, 0, direction.y), GetSpeed());
    }

    /// <summary>
    /// get the wanted direction of the Render who turn
    /// </summary>
    /// <returns></returns>
    public Quaternion GetWantedDirection()
    {
        return (_wantedDirection);
    }

    /// <summary>
    /// get the current direction of the Render who turn
    /// </summary>
    /// <returns></returns>
    public Quaternion GetCurrentDirection()
    {
        return (_playerLinker.RenderPlayerTurn.rotation);
    }

    /// <summary>
    /// move in physics, according to input of player
    /// </summary>
    private void MovePlayer()
    {
        _lastDirection = _playerInput.GetMoveDirection();
        MovePhysics(_lastDirection);
    }

    private void TurnRender()
    {
        _playerLinker.RenderPlayerTurn.rotation = ExtQuaternion.DirObject2d(_playerLinker.RenderPlayerTurn.rotation, _lastDirection, _speedTurnRate, out _wantedDirection, ExtQuaternion.TurnType.Y);
    }

    /// <summary>
    /// handle move physics
    /// </summary>
    public void CustomFixedUpdate()
    {
        if (_playerInput.IsMoving(0))
        {
            rb.drag = _moveDrag;
            MovePlayer();
        }
        else
        {
            rb.drag = _stopDrag;
            _frequencyEaseAndOut.EvaluateEveryFrameOut();
        }
        TurnRender();
    }
}
