using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampRbSpeed : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("rigidbody"), SerializeField]
    private float maxSpeed = 10f;

    [FoldoutGroup("Object"), Tooltip("rigidbody"), SerializeField]
    private Rigidbody rb = null;

    private float actualVelocity = 0f;
    public float GetActualVelocity() => actualVelocity;

    public void ReduceDecendingSpeedToAMin(float minSpeedDecent)
    {
        //TODO 
        if (GetActualVelocity() > minSpeedDecent)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, minSpeedDecent);
        }
    }

    public void DoClamp(float speed)
    {
        if (GetActualVelocity() > speed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        }
    }

    private void ClampSpeed()
    {
        if (GetActualVelocity() > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void FixedUpdate()
    {
        actualVelocity = rb.velocity.magnitude;
        ClampSpeed();
    }
}
