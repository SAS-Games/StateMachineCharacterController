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
        private float _moveInput;
        private FSMCharacterController _fsmCharacterController;
        private float _targetValue = 1;

        private Action<CallbackContext> _jumpPerformed;
        private Action<CallbackContext> _jumpCanceled;

        private Action<CallbackContext> _dashPerformed;

        void Awake()
        {
            _fsmCharacterController = GetComponent<FSMCharacterController>();
            _cameraTransform = Camera.main.transform;
        }

        void OnEnable()
        {
            var moveInputAction = m_InputConfig.GetInputAction("Move");
            moveInputAction.started += OnMove;
            moveInputAction.performed += OnMove;
            moveInputAction.canceled += OnMove;
            moveInputAction.Enable();

            _jumpPerformed = _ => _fsmCharacterController.OnJumpInitiated();
            _jumpCanceled = _ => _fsmCharacterController.OnJumpCanceled();
            var jumpInputAction = m_InputConfig.GetInputAction("Jump");
            jumpInputAction.performed += _jumpPerformed;
            jumpInputAction.canceled += _jumpCanceled;
            jumpInputAction.Enable();


            _dashPerformed = _ => _fsmCharacterController.OnDashInitiated();
            var dashInputAction = m_InputConfig.GetInputAction("Dash");
            dashInputAction.performed += _dashPerformed;
            dashInputAction.Enable();



        }

        private void OnDisable()
        {
            var moveInputAction = m_InputConfig.GetInputAction("Move");
            moveInputAction.started -= OnMove;
            moveInputAction.performed -= OnMove;
            moveInputAction.canceled -= OnMove;

            var jumpInputAction = m_InputConfig.GetInputAction("Jump");
            jumpInputAction.performed -= _jumpPerformed;
            jumpInputAction.canceled -= _jumpCanceled;

            var dashInputAction = m_InputConfig.GetInputAction("Dash");
            dashInputAction.Disable();


            _moveInput = 0f;
            _previousSpeed = 0;
            _fsmCharacterController.movementInput = Vector3.zero;
            _fsmCharacterController.OnMove(0);
        }

        private void Update() => ProcessMovementInput();

        private void ProcessMovementInput()
        {
            Vector3 adjustedMovement = new Vector3(_moveInput, 0f, 0f);
            if (Mathf.Approximately(_moveInput, 0f))
                adjustedMovement = _fsmCharacterController.movementInput * (adjustedMovement.magnitude + .01f);

            float targetSpeed = Mathf.Abs(_moveInput);

            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, m_targetSpeedReachMultiplier * Time.deltaTime);


            _fsmCharacterController.movementInput = adjustedMovement.normalized * targetSpeed;
            _fsmCharacterController.OnMove(targetSpeed);

            _previousSpeed = targetSpeed;
        }


        private void OnMove(InputAction.CallbackContext value)
        {
            _moveInput = value.ReadValue<float>() * _targetValue;
        }
    }
}
