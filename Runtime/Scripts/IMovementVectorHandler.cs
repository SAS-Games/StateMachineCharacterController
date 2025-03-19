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

    public interface ICharacter
    {
        Vector3 Position { get; }
        Vector3 Forward { get; }
    }
}