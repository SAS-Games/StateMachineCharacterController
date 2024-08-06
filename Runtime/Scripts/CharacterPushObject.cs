using SAS.StateMachineCharacterController;
using SAS.Utilities.TagSystem;
using System.Linq;
using UnityEngine;

public class CharacterPushObject : MonoBehaviour
{
    [SerializeField] protected string[] m_CollisionTags = { };
    [SerializeField] protected float m_ForceMagnitude = 10;
    [HideInInspector, FieldRequiresSelf] protected CharacterController _characterController;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (m_CollisionTags.Contains(hit.gameObject.tag) && ShouldPushObject(hit))
        {
            PushObject(hit);
        }
    }

    protected virtual bool ShouldPushObject(ControllerColliderHit hit)
    {
        return true; // Default behavior, can be overridden in derived class
    }

    protected virtual void PushObject(ControllerColliderHit hit)
    {
        var rigidBody = hit.collider.attachedRigidbody;

        if (rigidBody != null)
        {
            // Calculate the force direction based on the character's forward direction
            var forceDirection = transform.forward;
            forceDirection.y = 0; // Ignore the vertical component
            forceDirection.Normalize();

            // Adjust the force magnitude based on the character's velocity
            var adjustedForceMagnitude = m_ForceMagnitude;
            if (_characterController != null)
            {
                var playerVelocity = _characterController.velocity.magnitude;
                adjustedForceMagnitude *= playerVelocity;
            }

            // Apply the force to the rigidbody at the point of contact
            rigidBody.AddForceAtPosition(forceDirection * adjustedForceMagnitude, hit.point, ForceMode.Impulse);
        }
    }

}
