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

    public Vector3 GetLastDirection()
    {
        return (_lastDirection);
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
    public void MovePhysics3d(Vector3 direction)
    {
        Debug.DrawRay(rb.transform.position, direction * 50, Color.red);
        UnityMovement.MoveByForcePushing_WithPhysics(rb, direction, GetSpeed());
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

    public void ApplyImpulse(Vector3 normalizeDirection, float intensity)
    {
        _playerLinker.PlayerParticle.LaunchCollideAgainst(normalizeDirection);
        _playerLinker.Rigidbody.AddForce(normalizeDirection * intensity, ForceMode.Impulse);
    }

    /// <summary>
    /// move in physics, according to input of player
    /// </summary>
    private void MovePlayer()
    {
        _lastDirection = _playerInput.GetMoveDirection();

        if (_playerLinker.GroundForwardCheck.IsStraffNeeded())
        {
            Vector3 straffDir = _playerLinker.PlayerSlide.GetStraffDirection();
            MovePhysics3d(straffDir);
        }
        else
        {
            MovePhysics(_lastDirection);
        }
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
