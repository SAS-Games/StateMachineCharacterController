using UnityEngine;
using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController
{
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
