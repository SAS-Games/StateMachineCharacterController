using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputConfig m_InputConfig;
        [SerializeField] private float m_targetSpeedReachMultiplier = 10;
        private Transform _cameraTransform;

        private FSMCharacterController _fsmCharacterController;
        private InputAction _moveInputAction;
        private IMovementInputProcessor _movementProcessor;
        private EventBinding<GameModeChagedEvent> _gameModeChagedEventBinding;
        private Dictionary<string, IInputCommand> _commands = new();


        void Awake()
        {
            _fsmCharacterController = GetComponent<FSMCharacterController>();
            _cameraTransform = Camera.main.transform;

            _moveInputAction = m_InputConfig.GetInputAction("Move");

            _commands["Jump"] = new JumpCommand(_fsmCharacterController);
            _commands["Jump"].SetActive(m_InputConfig, true);
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
}
