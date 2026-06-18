using UnityEngine;
using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController
{
    public class SideScrollerMovementProcessor : IMovementInputProcessor
    {
        private readonly float _targetSpeedReachMultiplier;
        private float _previousSpeed;

        public SideScrollerMovementProcessor(float targetSpeedReachMultiplier)
        {
            _targetSpeedReachMultiplier = targetSpeedReachMultiplier;
        }

        public void ProcessMovement(InputAction moveInputAction, FSMCharacterController controller, Transform cameraTransform, float multiplier = 1)
        {
            Vector2 moveInput = moveInputAction.ReadValue<Vector2>();
            if (Mathf.Abs(moveInput.x) > 0)
                controller.isFacingRight = moveInput.x > 0;

            Vector3 adjustedMovement = new Vector3(moveInput.x, moveInput.y, 0f);
            float targetSpeed = Mathf.Abs(moveInput.x);
            targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, _targetSpeedReachMultiplier * Time.deltaTime);

            controller.movementInput = adjustedMovement * targetSpeed;
            controller.movementInput.y = adjustedMovement.y;
            controller.OnMove(targetSpeed);

            _previousSpeed = targetSpeed;
        }
    }
}