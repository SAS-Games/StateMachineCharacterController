using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SAS.StateMachineCharacterController
{
    public class JumpCommand : IInputCommand
    {
        private readonly FSMCharacterController _controller;
        private Action<CallbackContext> _jumpPerformed;
        private Action<CallbackContext> _jumpCanceled;
        private InputAction _inputAction;

        public JumpCommand(FSMCharacterController controller)
        {
            _controller = controller;
            _jumpPerformed = _ => _controller.OnJumpInitiated();
            _jumpCanceled = _ => _controller.OnJumpCanceled();
        }

        public void SetActive(InputConfig inputConfig, bool active)
        {
            if (active)
            {
                if (_inputAction != null)
                {
                    _inputAction.performed -= _jumpPerformed;
                    _inputAction.canceled -= _jumpCanceled;
                    _inputAction.Disable();
                }

                _inputAction = inputConfig.GetInputAction("Jump");
                if (_inputAction != null)
                {
                    _inputAction.performed += _jumpPerformed;
                    _inputAction.canceled += _jumpCanceled;
                    _inputAction.Enable();
                }
            }
            else if (_inputAction != null)
            {
                _inputAction.performed -= _jumpPerformed;
                _inputAction.canceled -= _jumpCanceled;
                _inputAction.Disable();
                _inputAction = null;
            }
        }
    }
}
