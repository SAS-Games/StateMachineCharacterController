using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SAS.StateMachineCharacterController
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputConfig m_InputConfig;
        [SerializeField] private float m_targetSpeedReachMultplier = 10;
        [SerializeField] private float m_MoveInputScale = 0.6f;
        private Transform _cameraTransform;

        private float _previousSpeed;
        private Vector2 _inputVector;
        private FSMCharacterController _characterController;
        private float _targetValue;

        Action<CallbackContext> _jumpPerformed;
        Action<CallbackContext> _jumpCanceled;

        Action<CallbackContext> _runStarted;
        Action<CallbackContext> _runCanceled;

        void Awake()
        {
            _targetValue = m_MoveInputScale;
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

            var runInputAction = m_InputConfig.GetInputAction("Run");
            runInputAction.Enable();

            _runStarted = _ =>
            {
                _targetValue = 1;
                _inputVector = moveInputAction.ReadValue<Vector2>() * _targetValue;

            };

            _runCanceled = _ =>
            {
                _targetValue = m_MoveInputScale;
                _inputVector = moveInputAction.ReadValue<Vector2>() * _targetValue;
            };

            runInputAction.started += _runStarted;
            runInputAction.canceled += _runCanceled;

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

            var runInputAction = m_InputConfig.GetInputAction("Run");
            runInputAction.started -= _runStarted;
            runInputAction.canceled -= _runCanceled;
        }

        private void Update() => ProcessMovementInput();

        private void ProcessMovementInput()
        {
            Vector3 adjustedMovement;
            if (_cameraTransform != null)
            {
                //Get the two axes from the camera and flatten them on the XZ plane
                Vector3 cameraForward = _cameraTransform.forward;
                cameraForward.y = 0f;
                Vector3 cameraRight = _cameraTransform.right;
                cameraRight.y = 0f;

                //Use the two axes, modulated by the corresponding inputs, and construct the final vector
                adjustedMovement = cameraRight.normalized * _inputVector.x + cameraForward.normalized * _inputVector.y;
            }
            else
            {
                //No CameraManager exists in the scene, so the input is just used absolute in world-space
                Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
                adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);
            }

            //Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
            if (_inputVector.sqrMagnitude == 0.0f)
                adjustedMovement = _characterController.transform.forward * (adjustedMovement.magnitude + .01f);

            //Accelerate/decelerate
            var targetSpeed = Mathf.Clamp01(_inputVector.magnitude);

            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * m_targetSpeedReachMultplier);
            _characterController.movementInput = adjustedMovement.normalized * targetSpeed;
            _characterController.OnMove(targetSpeed);
            _previousSpeed = targetSpeed;
        }

        private void OnMove(InputAction.CallbackContext value)
        {
            _inputVector = value.ReadValue<Vector2>() * _targetValue;
        }
    }
}
