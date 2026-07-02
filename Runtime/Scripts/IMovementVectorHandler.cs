using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public interface IMovementVectorHandler
    {
        Vector3 MovementVector { get; set; }
    }

    public enum MovementVelocityContributionMode
    {
        /// <summary>Replace horizontal velocity while preserving vertical velocity such as gravity or jumping.</summary>
        OverrideHorizontal,

        /// <summary>Add this velocity on top of the currently composed movement.</summary>
        Additive,

        /// <summary>Replace the full movement velocity before additive contributions are applied.</summary>
        OverrideFull
    }

    public interface IMovementVelocityComposer
    {
        void SetMovementVelocityContribution(object source, Vector3 velocity, MovementVelocityContributionMode mode, int priority = 0);
        void ClearMovementVelocityContribution(object source);
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
