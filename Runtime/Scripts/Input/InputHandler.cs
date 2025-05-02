using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputConfig m_InputConfig;
        [SerializeField] private float m_targetSpeedReachMultiplier = 10;
        private Transform _cameraTransform;

        private FSMCharacterController _fsmCharacterController;
        private InputAction _moveInputAction;
        private IMovementInputProcessor _movementProcessor;
        private EventBinding<GameModeChangedEvent> _gameModeChagedEventBinding;
        private Dictionary<string, IInputCommand> _commands = new();
        private PlayerInput _playerInput;


        void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            m_InputConfig = Instantiate(m_InputConfig);
            m_InputConfig.Initialize(_playerInput);

            _fsmCharacterController = GetComponent<FSMCharacterController>();
            _cameraTransform = Camera.main.transform;

            _moveInputAction = m_InputConfig.GetInputAction("Move");
            CreateInputCommand("Jump", new JumpCommand(_fsmCharacterController), true);
            CreateInputCommand("Dash", new DashCommand(_fsmCharacterController), true);
            CreateInputCommand("Climb", new ClimbCommand(_fsmCharacterController));

            _gameModeChagedEventBinding = new EventBinding<GameModeChangedEvent>(evt => OnGameModeChanged(evt));
        }

        public void CreateInputCommand(string command, IInputCommand inputCommand, bool activate = false)
        {
            if (!_commands.ContainsKey(command))
                _commands.Add(command, inputCommand);
            else
                Debug.LogWarning($"Input commands already contains the Key: {command}");
            _commands[command].SetActive(m_InputConfig, activate);
        }

        public IInputCommand GetCommand(string command)
        {
            if (!_commands.TryGetValue(command, out IInputCommand inputCommand))
                Debug.LogWarning($"Input commands already contains the Key: {command}");
            return inputCommand;
        }

        void OnEnable()
        {
            _playerInput.actions.Enable();

            EventBus<GameModeChangedEvent>.Register(_gameModeChagedEventBinding);
            _movementProcessor = new OpenWorldMovementProcessor(m_targetSpeedReachMultiplier);
        }

        private void OnDisable()
        {
            _playerInput.actions.Disable();

            _fsmCharacterController.movementInput = Vector3.zero;
            _fsmCharacterController.OnMove(0);
            EventBus<GameModeChangedEvent>.Deregister(_gameModeChagedEventBinding);
        }

        private void Update() =>
            _movementProcessor?.ProcessMovement(_moveInputAction, _fsmCharacterController, _cameraTransform);

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
        void ProcessMovement(InputAction moveInputAction, FSMCharacterController controller, Transform cameraTransform);
    }

    public interface IInputCommand
    {
        void SetActive(InputConfig inputConfig, bool active);
    }

    public interface IInputCallbackRegistry
    {
        void RegisterCallback(Action callback);
        void UnregisterCallback(Action callback);
    }
}