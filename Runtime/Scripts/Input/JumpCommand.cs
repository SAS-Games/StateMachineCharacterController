using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController
{
    public class JumpCommand : ChainedInputCommand
    {
        protected override string InputActionName { get; } = "Jump";

        public JumpCommand(FSMCharacterController controller)
        {
            AddHandler(InputActionPhase.Started, new ConditionalInputHandler(
                () => true,
                _ => controller.OnJumpInitiated()
            ));

            AddHandler(InputActionPhase.Canceled, new ConditionalInputHandler(
                () => true,
                _ => controller.OnJumpCanceled()
            ));
        }
    }
}