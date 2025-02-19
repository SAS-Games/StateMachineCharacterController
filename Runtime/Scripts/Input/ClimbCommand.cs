using SAS.StateMachineCharacterController;
using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ClimbCommand : IInputCommand
{
    private readonly FSMCharacterController _controller;
    private Action<CallbackContext> _climbStarted;
    private Action<CallbackContext> _climbCanceled;
    private InputAction _inputAction;

    public ClimbCommand(FSMCharacterController controller)
    {
        _controller = controller;
        _climbStarted = _ => _controller.OnClimbInitiated();
        _climbCanceled = _ => _controller.OnClimbCanceled();
    }

    public void Enable(InputConfig inputConfig)
    {
        _inputAction = inputConfig.GetInputAction("Climb");
        _inputAction.started += _climbStarted;
        _inputAction.canceled += _climbCanceled;
        _inputAction.Enable();
    }

    public void Disable(InputConfig inputConfig)
    {
        if (_inputAction != null)
        {
            _inputAction.started -= _climbStarted;
            _inputAction.canceled -= _climbCanceled;
            _inputAction.Disable();
        }
    }
}
