using System;
using System.Collections.Generic;
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
        private Dictionary<string, IInputCommand> _commands = new();


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

            _commands["Jump"] = new JumpCommand(_fsmCharacterController);
            _commands["Jump"].Enable(m_InputConfig);
            _commands["Dash"] = new DashCommand(_fsmCharacterController);
            _commands["Climb"] = new ClimbCommand(_fsmCharacterController);


            _gameModeChagedEventBinding = new EventBinding<GameModeChagedEvent>(evt => OnGameModeChanged(evt));
        }

        void OnEnable()
        {
            m_InputConfig.InputActionAsset.Enable();
            EventBus<GameModeChagedEvent>.Register(_gameModeChagedEventBinding);
        }

        private void OnDisable()
        {
            m_InputConfig.InputActionAsset.Disable();
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

        public void EnableAbility(string abilityName)
        {
            if (_commands.TryGetValue(abilityName, out var command))
            {
                command.Enable(m_InputConfig);
            }
        }

        public void DisableAbility(string abilityName)
        {
            if (_commands.TryGetValue(abilityName, out var command))
            {
                command.Disable(m_InputConfig);
            }
        }

    }

    public interface IMovementInputProcessor
    {
        void ProcessMovement(InputAction moveInputAction, FSMCharacterController controller, Transform cameraTransform);
    }

    public interface IInputCommand
    {
        void Enable(InputConfig inputConfig);
        void Disable(InputConfig inputConfig);
    }
}
