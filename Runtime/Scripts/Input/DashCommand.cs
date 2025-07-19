using SAS.StateMachineGraph;
using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController
{
    public class DashCommand : ChainedInputCommand
    {
        protected override string InputActionName { get; } = "Dash";

        public DashCommand(FSMCharacterController controller)
        {
            AddHandler(InputActionPhase.Performed,
                new ConditionalInputHandler(() => true, _ => { controller.OnDashInitiated(); }));
        }
    }
}