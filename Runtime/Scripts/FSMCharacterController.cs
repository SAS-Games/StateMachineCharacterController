using SAS.StateMachineGraph;
using SAS.StateMachineGraph.Utilities;
using SAS.Core.TagSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct RespawnEvent : IEvent
{
    public Transform transform;
}

namespace SAS.StateMachineCharacterController
{
    public static class FSMCharacterBlackboardKey
    {
        public const string CoyoteTimeCounter = "CoyoteTimeCounter";
        public const string RemainingJumpCount = "RemainingJumpCount";
        public const string MoveSpeed = "MoveSpeed";
        public const string DashSpeed = "DashSpeed";
        public const string IsDashing = "IsDashing";
        public const string Gravity = "Gravity";
        public const string MaxJumpCount = "MaxJumpCount";
        public const string EnergyCost = "EnergyCost";
    }

    [DefaultExecutionOrder(1)]
    [RequireComponent(typeof(Actor)), DisallowMultipleComponent]
    public class FSMCharacterController : MonoBehaviour, IMovementVectorHandler, IMovementVelocityComposer, ICameraLookAt, ICharacter, IActivatable
    {
        [SerializeField] private bool m_FreezeZAxis = true;
        [SerializeField] private RuntimeStateMachineController[] m_StateMachineControllers;
        [SerializeField] private CustomRaycast m_GroundDetection;
        [FieldRequiresSelf] private CharacterController _characterController;

        [field: SerializeField] public LayerMask WallLayer { get; private set; }
        [field: SerializeField] public LayerMask ClimbableLayer { get; private set; }
        [SerializeField] private LayerMask m_GroundLayer;
        [SerializeField] private string m_DeadStateTrigger = "Dead";

        public LayerMask GroundLayer => m_GroundLayer;

        /* [NonSerialized]*/
        internal Vector3 movementVector;

        /* [NonSerialized]*/
        internal Vector3 movementInput;

        internal bool isFacingRight;
        private Transform _transform;
        private Scene _originalScene;
        private EventBinding<GameModeChangedEvent> _gameModeChangedEventBinding;
        private readonly Dictionary<object, MovementVelocityContribution> _movementVelocityContributions = new();

        public float Speed { get; private set; }
        public float NormalizedMoveInput => movementInput.magnitude;
        public Vector3 MoveInput => movementInput;
        public Vector3 MovementVector => movementVector;
        public Vector3 MovementForward
        {
            get
            {
                if (m_FreezeZAxis)
                    return isFacingRight ? Vector3.right : Vector3.left;

                Vector3 forward = _transform != null ? _transform.forward : transform.forward;
                forward.y = 0f;
                return forward.sqrMagnitude > 0.0001f ? forward.normalized : Vector3.forward;
            }
        }


        private int NormalizedMoveInputHash = Animator.StringToHash("MoveInput");
        public Vector3 VerticalVelocity => _characterController.velocity.Multiply(0.0f, 1.0f, 0.0f);
        public ControllerColliderHit LastHit { get; private set; }

        private Actor _actor;

        public Actor Actor
        {
            get
            {
                if (_actor == null)
                    _actor = GetComponent<Actor>();
                return _actor;
            }
        }

        public bool IsGrounded
        {
            get
            {
                if (_characterController.isGrounded)
                    return true;
                if (m_GroundDetection)
                    return m_GroundDetection.Raycast(_transform.position, 0.01f);
                return false;
            }
        }


        Vector3 IMovementVectorHandler.MovementVector
        {
            get => movementVector;
            set => movementVector = value;
        }

        void IMovementVelocityComposer.SetMovementVelocityContribution(object source, Vector3 velocity, MovementVelocityContributionMode mode, int priority)
        {
            if (source == null)
                return;

            _movementVelocityContributions[source] = new MovementVelocityContribution(velocity, mode, priority);
        }

        void IMovementVelocityComposer.ClearMovementVelocityContribution(object source)
        {
            if (source == null)
                return;

            _movementVelocityContributions.Remove(source);
        }

        [SerializeField] private Transform m_LookAtTarget;
        Transform ICameraLookAt.Target => m_LookAtTarget;

        public Vector3 Position => _transform.position;
        public Vector3 Forward => _transform.forward;
        public Transform Transform => _transform;

        private void Awake()
        {
            OnGameModeChanged(GameModeInitializer.CurrentGameMode);
            this.Initialize();
            _transform = transform;
            SetFacingDirection();
            _originalScene = gameObject.scene;
            _gameModeChangedEventBinding = new EventBinding<GameModeChangedEvent>(evt => OnGameModeChanged(evt));
            EventBus<GameModeChangedEvent>.Register(_gameModeChangedEventBinding);
        }

        private void OnEnable()
        {
            _transform = transform;
        }

        public void OnMove(float normalizedMoveInput)
        {
            Speed = (float)Math.Round(normalizedMoveInput, 2);
            Actor.SetFloat(NormalizedMoveInputHash, Speed);
        }

        public void OnJumpInitiated()
        {
            Actor.SetTrigger("Jump");
            Actor.SetBool("JumpHold", true);
        }

        public void OnJumpCanceled()
        {
            Actor.SetBool("JumpHold", false);
        }

        public void OnDashInitiated()
        {
            Actor.SetTrigger("Dash");
        }

        public void OnClimbInitiated()
        {
            Actor.SetBool("Climb", true);
        }

        public void OnClimbCanceled()
        {
            Actor.SetBool("Climb", false);
        }

        public void OnFire()
        {
            Actor.SetBool("Attack", true);
            Debug.Log("OnFire");
        }

        public void OnFireCanceled()
        {
            Actor.SetBool("Attack", false);
        }

        public void SetFacingDirection()
        {
            float yRotation = _transform.localEulerAngles.y;
            if (Mathf.Abs(yRotation) < 0.0001f)
                yRotation = 0;

            const float facingThreshold = 5f; // Small threshold for smoother rotations
            isFacingRight = (yRotation >= 0 && yRotation <= facingThreshold) ||
                            (yRotation >= 360 - facingThreshold && yRotation <= 360);
        }

        public bool IsTouchingLayerSide(LayerMask layerMask, out RaycastHit hitInfo, float maxSlopeAngle = 0.1f)
        {
            return IsTouchingLayerSide(_transform.forward, layerMask, out hitInfo, maxSlopeAngle);
        }

        public bool IsTouchingLayerSide(Vector3 facingDirection, LayerMask layerMask, out RaycastHit hitInfo,
            float maxSlopeAngle = 0.1f)
        {
            // Calculate check radius using CharacterController radius and skinWidth
            float checkRadius = _characterController.radius + _characterController.skinWidth + 0.01f;
            Vector3 characterPosition = _transform.position + Vector3.up * (_characterController.height / 2);

            // SphereCast to the side of the character to detect walls
            bool hit = Physics.SphereCast(characterPosition, _characterController.skinWidth, facingDirection,
                out hitInfo, checkRadius, layerMask);

            // Check if the hit normal meets the slope tolerance criteria
            if (hit && Vector3.Dot(hitInfo.normal, Vector3.up) < maxSlopeAngle)
                return true;

            return false;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            LastHit = hit;
        }

        private void Update()
        {
            Vector3 baseMovement = movementVector;
            bool hasMovementContributions = _movementVelocityContributions.Count > 0;
            Vector3 composedMovement = ComposeMovementVelocity(baseMovement);

            if (m_FreezeZAxis)
                composedMovement.z = 0; // Ensure no Z-axis movement

            // Move the character
            _characterController.Move(composedMovement * Time.deltaTime);

            // Constrain the Z position
            if (m_FreezeZAxis)
                _transform.SetZLocalPosition(0);

            // Contributions are temporary overlays. Writing the composed velocity back into
            // movementVector would make the last contribution become permanent after it is cleared.
            movementVector = hasMovementContributions ? baseMovement : _characterController.velocity;
        }

        private Vector3 ComposeMovementVelocity(Vector3 baseVelocity)
        {
            if (_movementVelocityContributions.Count == 0)
                return baseVelocity;

            Vector3 composedVelocity = baseVelocity;
            Vector3 additiveVelocity = Vector3.zero;

            bool hasHorizontalOverride = false;
            Vector3 horizontalOverrideVelocity = Vector3.zero;
            int horizontalOverridePriority = int.MinValue;

            bool hasFullOverride = false;
            Vector3 fullOverrideVelocity = Vector3.zero;
            int fullOverridePriority = int.MinValue;

            foreach (var contribution in _movementVelocityContributions.Values)
            {
                switch (contribution.Mode)
                {
                    case MovementVelocityContributionMode.Additive:
                        additiveVelocity += contribution.Velocity;
                        break;

                    case MovementVelocityContributionMode.OverrideHorizontal:
                        if (!hasHorizontalOverride || contribution.Priority >= horizontalOverridePriority)
                        {
                            hasHorizontalOverride = true;
                            horizontalOverridePriority = contribution.Priority;
                            horizontalOverrideVelocity = contribution.Velocity;
                        }
                        break;

                    case MovementVelocityContributionMode.OverrideFull:
                        if (!hasFullOverride || contribution.Priority >= fullOverridePriority)
                        {
                            hasFullOverride = true;
                            fullOverridePriority = contribution.Priority;
                            fullOverrideVelocity = contribution.Velocity;
                        }
                        break;
                }
            }

            if (hasFullOverride && (!hasHorizontalOverride || fullOverridePriority >= horizontalOverridePriority))
            {
                composedVelocity = fullOverrideVelocity;
            }
            else if (hasHorizontalOverride)
            {
                composedVelocity.x = horizontalOverrideVelocity.x;
                composedVelocity.z = horizontalOverrideVelocity.z;
            }

            return composedVelocity + additiveVelocity;
        }

        private readonly struct MovementVelocityContribution
        {
            public readonly Vector3 Velocity;
            public readonly MovementVelocityContributionMode Mode;
            public readonly int Priority;

            public MovementVelocityContribution(Vector3 velocity, MovementVelocityContributionMode mode, int priority)
            {
                Velocity = velocity;
                Mode = mode;
                Priority = priority;
            }
        }

        void SetSceneToOriginal()
        {
            SceneManager.MoveGameObjectToScene(gameObject, _originalScene);
        }

        void Respawn()
        {
            _originalScene = gameObject.scene;
            SetFacingDirection();
            EventBus<RespawnEvent>.Raise(new RespawnEvent { transform = _transform });
        }

        void OnGameModeChanged(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.SideScroller:
                    m_FreezeZAxis = true;
                    break;
                case GameMode.FreeRoam:
                    m_FreezeZAxis = false;
                    break;
            }
        }

        public void OnDeath()
        {
            Actor.SetTrigger(m_DeadStateTrigger);
        }

        private void OnDisable()
        {
            _transform = null;
        }

        void OnDestroy()
        {
            EventBus<GameModeChangedEvent>.Deregister(_gameModeChangedEventBinding);
        }

        /// <summary>
        /// Teleports the player to a specific position by temporarily disabling the CharacterController.
        /// This prevents any internal interference from the CharacterController system,
        /// which might otherwise override the position on the next frame.
        /// </summary>
        /// <param name="position">The world-space position to teleport the player to.</param>

        public void SetPosition(Vector3 position)
        {
            _characterController.enabled = false;
            _transform.position = position;
            _characterController.enabled = true;
        }

        public void SetLocalPosition(Vector3 position)
        {
            _characterController.enabled = false;
            _transform.localPosition = position;
            _characterController.enabled = true;
        }

        void IActivatable.Activate()
        {
            enabled = true;
        }

        void IActivatable.Deactivate()
        {
            enabled = false;
        }
    }
}
