using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SAS.StateMachineCharacterController
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputConfig m_InputConfig;
        [SerializeField] private float m_targetSpeedReachMultiplier = 10;
        private Transform _cameraTransform;

        private FSMCharacterController _fsmCharacterController;

        private Action<CallbackContext> _jumpPerformed;
        private Action<CallbackContext> _jumpCanceled;

        private Action<CallbackContext> _dashPerformed;
        private Action<CallbackContext> _climbInputInitiated;
        private Action<CallbackContext> _climbInputCanceled;

        private InputAction _moveInputAction;
        private IMovementInputProcessor _movementProcessor;
        private EventBinding<GameModeChagedEvent> _gameModeChagedEventBinding;


        void Awake()
        {
            _fsmCharacterController = GetComponent<FSMCharacterController>();
            _cameraTransform = Camera.main.transform;

            _jumpPerformed = _ => _fsmCharacterController.OnJumpInitiated();
            _jumpCanceled = _ => _fsmCharacterController.OnJumpCanceled();

            _dashPerformed = _ => _fsmCharacterController.OnDashInitiated();

            _climbInputInitiated = _ => _fsmCharacterController.OnClimbInitiated();
            _climbInputCanceled = _ => _fsmCharacterController.OnClimbCanceled();

            _moveInputAction = m_InputConfig.GetInputAction("Move");

            var jumpInputAction = m_InputConfig.GetInputAction("Jump");
            jumpInputAction.performed += _jumpPerformed;
            jumpInputAction.canceled += _jumpCanceled;

            var dashInputAction = m_InputConfig.GetInputAction("Dash");
            dashInputAction.performed += _dashPerformed;

            var climbInputAction = m_InputConfig.GetInputAction("Climb");
            climbInputAction.started += _climbInputInitiated;
            climbInputAction.canceled += _climbInputCanceled;

            _gameModeChagedEventBinding = new EventBinding<GameModeChagedEvent>(evt => OnGameModeChanged(evt));
        }

        void OnEnable()
        {
            m_InputConfig.GetInputAction("Move").Enable();
            m_InputConfig.GetInputAction("Jump").Enable();
            m_InputConfig.GetInputAction("Dash").Enable();
            m_InputConfig.GetInputAction("Climb").Enable();
            EventBus<GameModeChagedEvent>.Register(_gameModeChagedEventBinding);
        }

        private void OnDisable()
        {
            m_InputConfig.GetInputAction("Move").Disable();
            m_InputConfig.GetInputAction("Jump").Disable();
            m_InputConfig.GetInputAction("Dash").Disable();
            m_InputConfig.GetInputAction("Climb").Disable();

            _fsmCharacterController.movementInput = Vector3.zero;
            _fsmCharacterController.OnMove(0);
            EventBus<GameModeChagedEvent>.Deregister(_gameModeChagedEventBinding);

        }

        private void Update() => _movementProcessor?.ProcessMovement(_moveInputAction, _fsmCharacterController, _cameraTransform);

        void OnGameModeChanged(GameMode gameMode)
        {
            SetMovementProcessor(gameMode);
        }

        private void SetMovementProcessor(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.SideScroller3D:
                    _movementProcessor = new SideScrollerMovementProcessor(m_targetSpeedReachMultiplier);
                    break;
                case GameMode.OpenWorld3d:
                    _movementProcessor = new OpenWorldMovementProcessor(m_targetSpeedReachMultiplier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
            }
        }

    }

    public interface IMovementInputProcessor
    {
        void ProcessMovement(InputAction moveInputAction, FSMCharacterController controller, Transform cameraTransform);
    }

    public class SideScrollerMovementProcessor : IMovementInputProcessor
    {
        private readonly float _targetSpeedReachMultiplier;
        private float _previousSpeed;

        public SideScrollerMovementProcessor(float targetSpeedReachMultiplier)
        {
            _targetSpeedReachMultiplier = targetSpeedReachMultiplier;
        }

        public void ProcessMovement(InputAction moveInputAction, FSMCharacterController controller, Transform cameraTransform)
        {
            Vector2 moveInput = moveInputAction.ReadValue<Vector2>();
            if (Mathf.Abs(moveInput.x) > 0)
                controller.isFacingRight = moveInput.x > 0;

            Vector3 adjustedMovement = new Vector3(moveInput.x, moveInput.y, 0f);
            float targetSpeed = Mathf.Abs(moveInput.x);
            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, _targetSpeedReachMultiplier * Time.deltaTime);

            controller.movementInput = adjustedMovement * targetSpeed;
            controller.OnMove(targetSpeed);

            _previousSpeed = targetSpeed;
        }
    }

    public class OpenWorldMovementProcessor : IMovementInputProcessor
    {
        private readonly float _targetSpeedReachMultiplier;
        private float _previousSpeed;

        public OpenWorldMovementProcessor(float targetSpeedReachMultiplier)
        {
            _targetSpeedReachMultiplier = targetSpeedReachMultiplier;
        }

        public void ProcessMovement(InputAction moveInputAction, FSMCharacterController controller, Transform cameraTransform)
        {
            Vector2 moveInput = moveInputAction.ReadValue<Vector2>();
            Vector3 adjustedMovement;

            if (cameraTransform != null)
            {
                Vector3 cameraForward = cameraTransform.forward;
                cameraForward.y = 0f;
                Vector3 cameraRight = cameraTransform.right;
                cameraRight.y = 0f;

                adjustedMovement = cameraRight.normalized * moveInput.x + cameraForward.normalized * moveInput.y;
            }
            else
            {
                Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
                adjustedMovement = new Vector3(moveInput.x, 0f, moveInput.y);
            }

            if (moveInput.sqrMagnitude == 0.0f)
                adjustedMovement = controller.transform.forward * (adjustedMovement.magnitude + .01f);

            var targetSpeed = Mathf.Clamp01(moveInput.magnitude);
            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * _targetSpeedReachMultiplier);

            controller.movementInput = adjustedMovement.normalized * targetSpeed;
            controller.OnMove(targetSpeed);

            _previousSpeed = targetSpeed;
        }
    }
}
