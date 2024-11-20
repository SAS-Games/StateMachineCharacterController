using SAS.StateMachineCharacterController;
using SAS.StateMachineGraph;
using System.Linq;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private float m_ForceMultiplier = 1.5f;
    [SerializeField] private float m_MaxForce = 20;
    [SerializeField] private float m_MinUpwardForce = 10;
    [SerializeField] private bool m_UseSurfaceNormal;
    [SerializeField] private string[] m_CollisionTags = { "Player" };

    private void OnTriggerEnter(Collider other)
    {
        if (m_CollisionTags.Contains(other.tag))
        {
            var movementVectorHandler = other.GetComponent<IMovementVectorHandler>();
            if (movementVectorHandler == null)
                return;

            var pos = transform.position;
            Vector3 force;

            if (m_UseSurfaceNormal)
            {
                var collisionPoint = other.ClosestPoint(pos);
                var collisionNormal = pos - (Vector3)collisionPoint;
                force = -collisionNormal * movementVectorHandler.MovementVector.magnitude;
            }
            else
            {
                var incomingSpeedNormal = Vector3.Project(movementVectorHandler.MovementVector, transform.up);
                force = -incomingSpeedNormal;
            }

            force = Vector3.ClampMagnitude(force * m_ForceMultiplier, m_MaxForce);
            force.y = Mathf.Clamp(force.y, m_MinUpwardForce, force.y);
            movementVectorHandler.MovementVector = force;
            EventBus<BounceForeAppliedEvent>.Raise(new BounceForeAppliedEvent { force = force });
            if ((movementVectorHandler as Component).TryGetComponent<Actor>(out Actor actor))
                actor.SetState("Jump Ascending");
        }
    }
}
