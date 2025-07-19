using SAS.StateMachineCharacterController;
using UnityEngine.InputSystem;

public class ClimbCommand : ChainedInputCommand
{
    protected override string InputActionName { get; } = "Climb";

    public ClimbCommand(FSMCharacterController controller)
    {
          AddHandler(InputActionPhase.Started, new ConditionalInputHandler(
            () => true,
            _ => controller.OnClimbInitiated()
        ));

        AddHandler(InputActionPhase.Canceled, new ConditionalInputHandler(
            () => true,
            _ => controller.OnClimbCanceled()
        ));
    }
}
