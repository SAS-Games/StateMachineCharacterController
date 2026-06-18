using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController
{
    public interface IInputHandler
    {
        PlayerInput PlayerInput { get; set; }
        InputConfig InputConfig { get; }
        IInputCommand RegisterInputCommand(string command, IInputCommand inputCommand, bool activate = false);
        bool TryGetInputCommand(string command, out IInputCommand inputCommand);
    }

    [RequireComponent(typeof(PlayerInput))]
    public class InputHandler : MonoBehaviour, IInputHandler
    {
        [SerializeField] private InputConfig m_InputConfig;
        [SerializeField] private float m_targetSpeedReachMultiplier = 10;
        private Transform _cameraTransform;

        private FSMCharacterController _fsmCharacterController;
        public InputAction MoveInputAction { get; private set; }
        private IMovementInputProcessor _movementProcessor;
        private IMovementInputMultiplier _movementInputMultiplier;
        private EventBinding<GameModeChangedEvent> _gameModeChagedEventBinding;
        private Dictionary<string, IInputCommand> _commands = new();
        private PlayerInput _playerInput;

        public PlayerInput PlayerInput
        {
            get => _playerInput;
            set => _playerInput = value;
        }

        InputConfig IInputHandler.InputConfig => m_InputConfig;

        private void Awake()
        {
            _fsmCharacterController = GetComponent<FSMCharacterController>();
            _cameraTransform = Camera.main.transform;
            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>(); // fallback

            SetupInputHandler();
            _gameModeChagedEventBinding = new EventBinding<GameModeChangedEvent>(evt => OnGameModeChanged(evt));
            _movementInputMultiplier = GetComponent<IMovementInputMultiplier>();
            if (_movementInputMultiplier is null)
                _movementInputMultiplier = new NullMovementInputMultiplier();
        }

        private void SetupInputHandler()
        {
            if (_playerInput == null)
            {
                Debug.LogError("No player input handler assigned.");
                return;
            }

            m_InputConfig = Instantiate(m_InputConfig);

            m_InputConfig.Initialize(_playerInput);
            MoveInputAction = m_InputConfig.GetInputAction("Move");
            _playerInput.actions.Enable();
        }

        public IInputCommand RegisterInputCommand(string command, IInputCommand inputCommand, bool activate = false)
        {
            if (!_commands.TryAdd(command, inputCommand))
            {
                Debug.LogWarning($"Input commands already contains the Key: {command}");
                return _commands[command];
            }

            inputCommand.SetActive(m_InputConfig, activate);
            return inputCommand;
        }
        
        public bool TryGetInputCommand(string command, out IInputCommand inputCommand)
        {
            return _commands.TryGetValue(command, out inputCommand);
        }

        public IInputCommand GetCommand(string command)
        {
            if (!_commands.TryGetValue(command, out IInputCommand inputCommand))
                Debug.LogWarning($"Input commands already contains the Key: {command}");
            return inputCommand;
        }

        void OnEnable()
        {
            if (_playerInput != null)
                _playerInput.actions.Enable();

            EventBus<GameModeChangedEvent>.Register(_gameModeChagedEventBinding);
            _movementProcessor = new OpenWorldMovementProcessor(m_targetSpeedReachMultiplier);
        }

        private void OnDisable()
        {
            if (_playerInput != null)
                _playerInput.actions.Disable();

            _fsmCharacterController.movementInput = Vector3.zero;
            _fsmCharacterController.OnMove(0);
            EventBus<GameModeChangedEvent>.Deregister(_gameModeChagedEventBinding);
        }

        private void Update()
        {
            _movementProcessor?.ProcessMovement(MoveInputAction, _fsmCharacterController, _cameraTransform, _movementInputMultiplier.Multiplier);
        }

        private void OnDestroy()
        {
            foreach (var command in _commands)
            {
                command.Value.SetActive(m_InputConfig, false);
            }
        }

        void OnGameModeChanged(GameMode gameMode)
        {
            SetMovementProcessor(gameMode);
        }

        private void SetMovementProcessor(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.SideScroller:
                    _movementProcessor = new SideScrollerMovementProcessor(m_targetSpeedReachMultiplier);
                    break;
                case GameMode.FreeRoam:
                    _movementProcessor = new OpenWorldMovementProcessor(m_targetSpeedReachMultiplier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
            }
        }

        public void EnableAbility(string abilityName, bool enable)
        {
            if (_commands.TryGetValue(abilityName, out var command))
                command.SetActive(m_InputConfig, enable);
        }
    }

    public interface IMovementInputProcessor
    {
        void ProcessMovement(InputAction moveInputAction, FSMCharacterController controller, Transform cameraTransform, float multiplier = 1.0f);
    }

    public interface IMovementInputMultiplier
    {
        float Multiplier { get; set; }
    }

    public class NullMovementInputMultiplier : IMovementInputMultiplier
    {
        public float Multiplier { get; set; } = 1;
    }

    public interface IConditionalInputHandler
    {
        bool CanExecute();
        void Execute(InputAction.CallbackContext context);
        void AddAction(Action<InputAction.CallbackContext> action);
    }

    public interface IInputCommand
    {
        void SetActive(InputConfig inputConfig, bool active);
        void AddHandler(InputActionPhase phase, IConditionalInputHandler handler, int priority = 0);
        void RemoveHandler(InputActionPhase phase, IConditionalInputHandler handler);
    }

    public class HandlerEntry
    {
        public IConditionalInputHandler Handler;
        public int Priority;

        public HandlerEntry(IConditionalInputHandler handler, int priority)
        {
            Handler = handler;
            Priority = priority;
        }
    }
}