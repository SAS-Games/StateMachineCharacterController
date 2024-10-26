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
        private FSMCharacterController _characterController;
        private float _targetValue = 1;

        Action<CallbackContext> _jumpPerformed;
        Action<CallbackContext> _jumpCanceled;

        void Awake()
        {
            _characterController = GetComponent<FSMCharacterController>();
            _cameraTransform = Camera.main.transform;
        }

        void OnEnable()
        {
            var moveInputAction = m_InputConfig.GetInputAction("Move");
            moveInputAction.Enable();
            moveInputAction.started += OnMove;
            moveInputAction.performed += OnMove;
            moveInputAction.canceled += OnMove;

            var jumpInputAction = m_InputConfig.GetInputAction("Jump");
            jumpInputAction.Enable();

            _jumpPerformed = _ => _characterController.OnJumpInitiated();
            _jumpCanceled = _ => _characterController.OnJumpCanceled();

            jumpInputAction.performed += _jumpPerformed;
            jumpInputAction.canceled += _jumpCanceled;



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


            _moveInput = 0f;
            _previousSpeed = 0;
            _characterController.movementInput = Vector3.zero;
            _characterController.OnMove(0);
        }

        private void Update() => ProcessMovementInput();

        private void ProcessMovementInput()
        {
            Vector3 adjustedMovement = new Vector3(_moveInput, 0f, 0f);
            if (Mathf.Approximately(_moveInput, 0f))
                adjustedMovement = _characterController.movementInput * (adjustedMovement.magnitude + .01f);

            float targetSpeed = Mathf.Abs(_moveInput);

            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, m_targetSpeedReachMultiplier * Time.deltaTime);


            _characterController.movementInput = adjustedMovement.normalized * targetSpeed;
            _characterController.OnMove(targetSpeed);

            _previousSpeed = targetSpeed;
        }


        private void OnMove(InputAction.CallbackContext value)
        {
            _moveInput = value.ReadValue<float>() * _targetValue;
        }
    }
}
