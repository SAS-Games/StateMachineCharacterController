using SAS.StateMachineCharacterController;
using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ClimbCommand : IInputCommand
{
    private Action<CallbackContext> _climbStarted;
    private Action<CallbackContext> _climbCanceled;
    private InputAction _inputAction;

    public ClimbCommand(FSMCharacterController controller)
    {
        _climbStarted = _ => controller.OnClimbInitiated();
        _climbCanceled = _ => controller.OnClimbCanceled();
    }

    public void SetActive(InputConfig inputConfig, bool active)
    {
        if (active)
        {
            if (_inputAction != null)
            {
                _inputAction.started -= _climbStarted;
                _inputAction.canceled -= _climbCanceled;
                _inputAction.Disable();
            }

            _inputAction = inputConfig.GetInputAction("Climb");
            if (_inputAction != null)
            {
                _inputAction.started += _climbStarted;
                _inputAction.canceled += _climbCanceled;
                _inputAction.Enable();
            }
        }
        else if (_inputAction != null)
        {
            _inputAction.started -= _climbStarted;
            _inputAction.canceled -= _climbCanceled;
            _inputAction.Disable();
            _inputAction = null;
        }
    }
}
