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

        private float _previousSpeed;
        private Vector2 _moveInput;
        private FSMCharacterController _fsmCharacterController;
        private float _targetValue = 1;

        private Action<CallbackContext> _jumpPerformed;
        private Action<CallbackContext> _jumpCanceled;

        private Action<CallbackContext> _dashPerformed;
        private Action<CallbackContext> _climbInputInitiated;
        private Action<CallbackContext> _climbInputCanceled;

        private InputAction _moveInputAction;

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
        }

        void OnEnable()
        {
            m_InputConfig.GetInputAction("Move").Enable();
            m_InputConfig.GetInputAction("Jump").Enable();
            m_InputConfig.GetInputAction("Dash").Enable();
            m_InputConfig.GetInputAction("Climb").Enable();
        }

        private void OnDisable()
        {
            m_InputConfig.GetInputAction("Move").Disable();
            m_InputConfig.GetInputAction("Jump").Disable();
            m_InputConfig.GetInputAction("Dash").Disable();
            m_InputConfig.GetInputAction("Climb").Disable();

            _moveInput = Vector2Int.zero;
            _previousSpeed = 0;
            _fsmCharacterController.movementInput = Vector3.zero;
            _fsmCharacterController.OnMove(0);
        }

        private void Update() => ProcessMovementInput();

        private void ProcessMovementInput()
        {
            if (_moveInputAction.enabled)
            {
                _moveInput = _moveInputAction.ReadValue<Vector2>() * _targetValue;
                if (Mathf.Abs(_moveInput.x) > 0)
                    _fsmCharacterController.isFacingRight = _moveInput.x > 0 ? true : false;
            }

            Vector3 adjustedMovement = new Vector3(_moveInput.x, _moveInput.y, 0f);
            float targetSpeed = Mathf.Abs(_moveInput.x);
            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, m_targetSpeedReachMultiplier * Time.deltaTime);
            _fsmCharacterController.movementInput = adjustedMovement * targetSpeed;
            _fsmCharacterController.movementInput.y = _moveInput.y;
            _fsmCharacterController.OnMove(targetSpeed);

            _previousSpeed = targetSpeed;
        }
    }
}
