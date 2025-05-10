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

    public interface IEntity
    {
        Vector3 Position { get; }
        Vector3 Forward { get; }
        Transform Transform { get; }
    }

    public interface ICharacter : IEntity
    {
    }
    public interface ITarget : IEntity
    {
        bool IsActive { get; }
    }
}
