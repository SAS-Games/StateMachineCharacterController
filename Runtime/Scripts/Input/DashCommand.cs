using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SAS.StateMachineCharacterController
{
    public class DashCommand : IInputCommand, IInputCallbackRegistry
    {
        private InputAction _inputAction;
        private readonly HashSet<Action> _callbacks = new();

        private void OnDashPerformed(CallbackContext context)
        {
            foreach (var callback in _callbacks)
                callback.Invoke();
        }

        public DashCommand(FSMCharacterController controller)
        {
            _callbacks.Add(controller.OnDashInitiated);
        }

        public void SetActive(InputConfig inputConfig, bool active)
        {
            if (active)
            {
                if (_inputAction != null)
                {
                    _inputAction.performed -= OnDashPerformed;
                    _inputAction.Disable();
                }

                _inputAction = inputConfig.GetInputAction("Dash");
                if (_inputAction != null)
                {
                    _inputAction.performed += OnDashPerformed;
                    _inputAction.Enable();
                }
            }
            else if (_inputAction != null)
            {
                _inputAction.performed -= OnDashPerformed;
                _inputAction.Disable();
                _inputAction = null;
            }
        }

        void IInputCallbackRegistry.RegisterCallback(Action callback) => _callbacks.Add(callback);
        void IInputCallbackRegistry.UnregisterCallback(Action callback) => _callbacks.Remove(callback);
    }
}