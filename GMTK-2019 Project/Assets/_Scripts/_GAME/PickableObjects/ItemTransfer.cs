using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickable))]
public class ItemTransfer : MonoBehaviour
{
    private enum TransferPhase { None, HighSpeed, Transition, NormalSpeed };

    [SerializeField, FoldoutGroup("Object")] private GameState _gameState = default;
    [SerializeField, FoldoutGroup("Object")] private Rigidbody _rigidbody = default;
    [SerializeField, FoldoutGroup("Object")] private Pickable _pickable = default;
    [SerializeField, FoldoutGroup("Object")] private ManetteButtons _manetteButtons = default;
    [SerializeField, FoldoutGroup("Gameplay")] private float _angularVelocity = 20f;
    [SerializeField, FoldoutGroup("Gameplay")] private float _highSpeedValue = 25f;
    [SerializeField, FoldoutGroup("Gameplay")] private float _normalSpeedValue = 10f;
    [SerializeField, FoldoutGroup("Gameplay")] private float _highSpeedDuration = 0.1f;
    [SerializeField, FoldoutGroup("Gameplay")] private float _lerpDuration = 0.1f;
    [SerializeField, FoldoutGroup("Gameplay")] private AnimationCurve _lerpAspect = default;
    private FrequencyCoolDown _highSpeedCooldown = new FrequencyCoolDown();
    private FrequencyCoolDown _transitionCooldown = new FrequencyCoolDown();
    private Transform _targetPlayerTransform;

    private bool _isSnapping = false;
    [FoldoutGroup("Debug")] public bool IsInTransfer = false;
    [FoldoutGroup("Debug"), SerializeField]
    private TransferPhase _transferPhase = TransferPhase.None;

    public void RegisterGameState(GameState gameState)
    {
        _gameState = gameState;
    }

    public void TransferItemToPlayer(PlayerLinker playerLinker)
    {
        if (_gameState.StateOfGame == GameState.StateGame.WIN_GAME)
        {
            return;
        }
        IsInTransfer = true;
        _isSnapping = true;
        _targetPlayerTransform = playerLinker.Rigidbody.transform;
    }

    private void FixedUpdate()
    {
        if (IsInTransfer)
        {
            if (_transferPhase == TransferPhase.None)
            {
                if (_isSnapping)
                {
                    _pickable.DetachFromPlayer(false);
                    _isSnapping = false;
                    return;
                }
                _rigidbody.drag = 0f;

                Vector3 directionToTarget = (_targetPlayerTransform.position - transform.position);
                RotateSnapToPlayer(directionToTarget);

                _highSpeedCooldown.StartCoolDown(_highSpeedDuration);
                _transferPhase = TransferPhase.HighSpeed;
            }
            
            // High speed phase
            else if (_transferPhase == TransferPhase.HighSpeed)
            {
                RotateToPlayer();
                _rigidbody.velocity = _highSpeedValue * transform.forward;
                float lerpRatio = 1 - _highSpeedCooldown.GetTimer() / _highSpeedDuration;
                
                _rigidbody.position = new Vector3(transform.position.x,
                                                  Mathf.Lerp(transform.position.y, _targetPlayerTransform.position.y, lerpRatio),
                                                  transform.position.z);
                                                
                if (!_highSpeedCooldown.IsRunning())
                {
                    _transferPhase = TransferPhase.Transition;
                    _transitionCooldown.StartCoolDown(_lerpDuration);
                }
            }
            // Transition phase
            else if (_transferPhase == TransferPhase.Transition)
            {
                RotateToPlayer();
                float lerpRatio = 1 - _transitionCooldown.GetTimer() / _lerpDuration;
                _rigidbody.velocity = Mathf.Lerp(_normalSpeedValue, _highSpeedValue, _lerpAspect.Evaluate(lerpRatio)) * transform.forward;
                if (!_transitionCooldown.IsRunning())
                {
                    _transferPhase = TransferPhase.NormalSpeed;
                    _rigidbody.velocity = _normalSpeedValue * transform.forward;
                }
            }
            // Normal speed phase
            else if (_transferPhase == TransferPhase.NormalSpeed)
            {
                RotateToPlayer();
                _rigidbody.velocity = _normalSpeedValue * transform.forward;
            }
        }
    }

    private void RotateSnapToPlayer(Vector3 directionToTarget)
    {
        directionToTarget = (directionToTarget - Vector3.Dot(directionToTarget, Vector3.up) * Vector3.up).FastNormalized();
        _rigidbody.rotation = ExtQuaternion.TurretLookRotation(directionToTarget, Vector3.up);
        /*
        float scalarProduct = Vector3.Dot(transform.forward, directionToTarget);
        float dotProduct = Vector3.Dot(Vector3.Cross(transform.forward, directionToTarget), Vector3.up);
        _rigidbody.rotation = Quaternion.Euler(0,
                                               Mathf.Rad2Deg * Mathf.Acos(scalarProduct) * Mathf.Sign(dotProduct),
                                               0);
                                               */
    }

    private void RotateToPlayer()
    {
        Vector3 directionToTarget = (_targetPlayerTransform.position - transform.position);
        directionToTarget = (directionToTarget - Vector3.Dot(directionToTarget, Vector3.up) * Vector3.up).FastNormalized();
        
        float scalarProduct = Vector3.Dot(transform.forward, directionToTarget);
        if (Mathf.Abs(scalarProduct) < 1)
        {
            float dotProduct = Vector3.Dot(Vector3.Cross(transform.forward, directionToTarget), Vector3.up);
            float angle = Mathf.Rad2Deg * Mathf.Acos(scalarProduct) * Mathf.Sign(dotProduct);
            _rigidbody.rotation = ExtQuaternion.SmoothTurretLookRotation(directionToTarget,
                                                                           Vector3.up,
                                                                           _rigidbody.rotation,
                                                                           _angularVelocity * angle);
        }
    }

    public void StopTransfer()
    {
        IsInTransfer = false;
        _transferPhase = TransferPhase.None;
        _rigidbody.useGravity = true;
        _rigidbody.drag = 2f;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void AddButtonToController(PlayerLinker playerLinker)
    {
        if (_pickable.PickableType == pickableinput.manettesansbouton && _manetteButtons != null)
        {
            _manetteButtons.RegisterPlayerButton(playerLinker);
        }
    }
}
