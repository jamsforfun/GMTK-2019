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

    [SerializeField, FoldoutGroup("Debug")] private bool _isInTransfer = false;

    public void TransferItemToPlayer(PlayerLinker playerLinker)
    {
        StartCoroutine(TransferCoroutine(playerLinker));
    }

    private IEnumerator TransferCoroutine(PlayerLinker playerLinker)
    {
        _pickable.DetachFromPlayer();
        _rigidbody.drag = 0f;
        _rigidbody.useGravity = false;
        float timeSinceStart = 0f;
        _isInTransfer = true;
        Transform playerTransform = playerLinker.Rigidbody.transform;
        Vector3 directionToTarget = (playerTransform.position - transform.position).normalized;
        RotateSnapToPlayer(directionToTarget);
        while (timeSinceStart < _highSpeedDuration)
        {
            RotateToPlayer(playerTransform);
            _rigidbody.velocity = _highSpeedValue * transform.forward;
            yield return null;
            timeSinceStart += Time.deltaTime;
        }
        while (timeSinceStart - _highSpeedDuration < _lerpDuration)
        {
            float lerpRatio = (timeSinceStart - _highSpeedDuration) / _lerpDuration;
            Vector3 previousVelocityDirection = _rigidbody.velocity.normalized;
            _rigidbody.velocity = Mathf.Lerp(_normalSpeedValue, _highSpeedValue, _lerpAspect.Evaluate(lerpRatio)) * previousVelocityDirection;
            RotateToPlayer(playerTransform);
            yield return null;
            timeSinceStart += Time.deltaTime;
        }
        _rigidbody.velocity = _normalSpeedValue * transform.forward;
        while (_isInTransfer)
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
        _isInTransfer = false;
        _rigidbody.useGravity = true;
        _rigidbody.drag = 2f;
        _rigidbody.isKinematic = false;
    }
}
