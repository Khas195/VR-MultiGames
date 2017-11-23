using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using script.BoidBehavior;
using UnityEngine;

public class FleeBehavior : BoidBehavior
{
    private Vector3 _desiredVelocity = Vector3.zero;
		
    [Header("Gizmos")]
    [SerializeField]
    private Color _fleeColor = Color.black;
    
    public override void PerformBehavior()
    {
        if (!IsEnable || BoidController == null || BoidController.Target == null)
        {
            return;
        }

        _desiredVelocity = (transform.position - BoidController.Target.transform.position).normalized *
                           BoidController.Movement.MaxSpeed;

        SteeringForce = _desiredVelocity - BoidController.Velocity;
    }

    private void OnDrawGizmos()
    {
        if (IsEnable && IsDrawGizmos)
        {
            Gizmos.color = _fleeColor;
            Gizmos.DrawLine(transform.position, transform.position + _desiredVelocity);
        }
    }
}
