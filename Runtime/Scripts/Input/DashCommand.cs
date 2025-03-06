using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SAS.StateMachineCharacterController
{
    public class DashCommand : IInputCommand
    {
        private Action<CallbackContext> _dashPerformed;
        private InputAction _inputAction;

        public DashCommand(FSMCharacterController controller)
        {
            _dashPerformed = _ => controller.OnDashInitiated();
        }

        public void SetActive(InputConfig inputConfig, bool active)
        {
            if (active)
            {
                if (_inputAction != null)
                {
                    _inputAction.performed -= _dashPerformed;
                    _inputAction.Disable();
                }

                _inputAction = inputConfig.GetInputAction("Dash");
                if (_inputAction != null)
                {
                    _inputAction.performed += _dashPerformed;
                    _inputAction.Enable();
                }
            }
            else if (_inputAction != null)
            {
                _inputAction.performed -= _dashPerformed;
                _inputAction.Disable();
                _inputAction = null;
            }

        }
    }
}
