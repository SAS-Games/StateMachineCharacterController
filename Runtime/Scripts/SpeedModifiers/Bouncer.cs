using SAS.StateMachineCharacterController;
using System.Linq;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private float m_ForceMultiplier = 1.5f;
    [SerializeField] private float m_MaxForce = 20;
    [SerializeField] private bool m_UseSurfaceNormal;
    [SerializeField] private string[] m_CollisionTags = { "Player" };

    private void OnTriggerEnter(Collider other)
    {
        if (m_CollisionTags.Contains(other.tag))
        {
            var fsmCharacterController = other.GetComponent<FSMCharacterController>();

            var pos = transform.position;
            Vector3 force;

            if (m_UseSurfaceNormal)
            {
                var collisionPoint = other.ClosestPoint(pos);
                var collisionNormal = pos - (Vector3)collisionPoint;
                force = -collisionNormal * fsmCharacterController.movementVector.magnitude;
                Debug.Log(force);
            }
            else
            {
                var incomingSpeedNormal = Vector3.Project(fsmCharacterController.movementVector, transform.up);
                force = -incomingSpeedNormal;
            }

            force = Vector3.ClampMagnitude(force * m_ForceMultiplier, m_MaxForce);
            fsmCharacterController.movementVector = force;
            EventBus<BounceForeAppliedEvent>.Raise(new BounceForeAppliedEvent { force = force });
        }
    }
}
