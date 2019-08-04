using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickable))]
public class ItemTransfer : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Object")] private Rigidbody _rigidbody = default;
    [SerializeField, FoldoutGroup("Object")] private Pickable _pickable = default;
    [SerializeField, FoldoutGroup("Gameplay")] private float _angularVelocity = 20f;
    [SerializeField, FoldoutGroup("Gameplay")] private float _highSpeedValue = 25f;
    [SerializeField, FoldoutGroup("Gameplay")] private float _normalSpeedValue = 10f;
    [SerializeField, FoldoutGroup("Gameplay")] private float _highSpeedDuration = 0.1f;
    [SerializeField, FoldoutGroup("Gameplay")] private float _lerpDuration = 0.1f;
    [SerializeField, FoldoutGroup("Gameplay")] private AnimationCurve _lerpAspect = default;
    private Coroutine _transferCoroutine;

    [FoldoutGroup("Debug")] public bool IsInTransfer = false;

    public void TransferItemToPlayer(PlayerLinker playerLinker)
    {
        _transferCoroutine = StartCoroutine(TransferCoroutine(playerLinker));
    }

    private IEnumerator TransferCoroutine(PlayerLinker playerLinker)
    {
        _pickable.DetachFromPlayer();
        _rigidbody.drag = 0f;
        _rigidbody.useGravity = false;
        float timeSinceStart = 0f;
        IsInTransfer = true;
        Transform playerTransform = playerLinker.Rigidbody.transform;
        Vector3 directionToTarget = (playerTransform.position - transform.position).normalized;
        RotateSnapToPlayer(directionToTarget);
        while (timeSinceStart < _highSpeedDuration)
        {
            RotateToPlayer(playerTransform);
            _rigidbody.velocity = _highSpeedValue * transform.forward;
            transform.position = new Vector3(transform.position.x,
                                             Mathf.LerpUnclamped(transform.position.y, playerTransform.position.y, timeSinceStart / _highSpeedDuration),
                                             transform.position.z);
            yield return null;
            timeSinceStart += Time.deltaTime;
        }
        while (timeSinceStart - _highSpeedDuration < _lerpDuration)
        {
            float lerpRatio = (timeSinceStart - _highSpeedDuration) / _lerpDuration;
            _rigidbody.velocity = Mathf.Lerp(_normalSpeedValue, _highSpeedValue, _lerpAspect.Evaluate(lerpRatio)) * transform.forward;
            RotateToPlayer(playerTransform);
            yield return null;
            timeSinceStart += Time.deltaTime;
        }
        _rigidbody.velocity = _normalSpeedValue * transform.forward;
        while (IsInTransfer)
        {
            RotateToPlayer(playerTransform);
            _rigidbody.velocity = _normalSpeedValue * transform.forward;
            yield return null;
        }

    }

    private void RotateSnapToPlayer(Vector3 directionToTarget)
    {
        directionToTarget = (directionToTarget - Vector3.Dot(directionToTarget, Vector3.up) * Vector3.up).normalized;
        float scalarProduct = Vector3.Dot(transform.forward, directionToTarget);
        float dotProduct = Vector3.Dot(Vector3.Cross(transform.forward, directionToTarget), Vector3.up);
        transform.Rotate(Vector3.up, Mathf.Rad2Deg * Mathf.Acos(scalarProduct) * Mathf.Sign(dotProduct));
    }

    private void RotateToPlayer(Transform playerTransform)
    {
        Vector3 directionToTarget = (playerTransform.position - transform.position);
        directionToTarget = (directionToTarget - Vector3.Dot(directionToTarget, Vector3.up) * Vector3.up).normalized;
        float scalarProduct = Vector3.Dot(transform.forward, directionToTarget);
        if (Mathf.Abs(scalarProduct) < 1)
        {
            float dotProduct = Vector3.Dot(Vector3.Cross(transform.forward, directionToTarget), Vector3.up);
            float angle = Mathf.Rad2Deg * Mathf.Acos(scalarProduct) * Mathf.Sign(dotProduct);
            _rigidbody.angularVelocity = _angularVelocity * angle * transform.up;
        }
    }

    public void StopTransfer()
    {
        if (_transferCoroutine != null)
        {
            StopCoroutine(_transferCoroutine);
        }
        IsInTransfer = false;
        _rigidbody.useGravity = true;
        _rigidbody.drag = 2f;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
