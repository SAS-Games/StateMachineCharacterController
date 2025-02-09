using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public interface IMovementVectorHandler
    {
        Vector3 MovementVector { get; set; }
    }

    public interface ICameraLookAt
    {
        Transform Target { get; }
    }
}