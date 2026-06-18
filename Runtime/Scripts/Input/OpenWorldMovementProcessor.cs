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

        public void ProcessMovement(InputAction moveInputAction, FSMCharacterController controller, Transform cameraTransform, float multiplier = 1)
        {
           // cameraTransform = null;
            Vector2 moveInput = moveInputAction.ReadValue<Vector2>();
            Vector3 adjustedMovement;
            Transform targetTransform = cameraTransform == null ? controller.transform : cameraTransform;
            Vector3 forward = targetTransform.forward;
            Vector3 right = targetTransform.right;
            forward.y = 0f;
            right.y = 0f;
            
            adjustedMovement = right.normalized * moveInput.x + forward.normalized * moveInput.y;
            
            if (moveInput.sqrMagnitude == 0.0f)
                adjustedMovement = controller.transform.forward * (adjustedMovement.magnitude + .01f);

            var targetSpeed = Mathf.Clamp01(moveInput.magnitude);
            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * _targetSpeedReachMultiplier);

            controller.movementInput = adjustedMovement.normalized * (targetSpeed * multiplier);
            controller.OnMove(targetSpeed);

            _previousSpeed = targetSpeed;
        }
    }
}