using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SAS.StateMachineCharacterController
{
    [CreateAssetMenu(menuName = "SAS/State Machine Character Controller/Input")]
    public class InputConfig : ScriptableObject
    {
        [System.Serializable]
        class Input
        {
            [SerializeField] private string m_Key;
            [SerializeField] private InputActionReference m_InputActionReference; // ? Use InputActionReference

            public string Key => m_Key;
            public InputActionReference ActionReference => m_InputActionReference;
        }

        [SerializeField] private Input[] m_Inputs;
        private Dictionary<string, InputActionReference> _inputMap = new();
        private PlayerInput _playerInput;

        public void Initialize(PlayerInput playerInput)
        {
            _playerInput = playerInput ?? throw new ArgumentNullException(nameof(playerInput));

            _inputMap.Clear();
            foreach (var input in m_Inputs)
            {
                if (input.ActionReference != null)
                {
                    _inputMap[input.Key] = input.ActionReference;
                }
            }
        }

        public InputAction GetInputAction(string key)
        {
            if (_playerInput == null)
            {
                Debug.LogError("InputConfig is not initialized with PlayerInput.");
                return null;
            }

            if (_inputMap.TryGetValue(key, out var actionReference) && actionReference != null)
            {
                var action = _playerInput.actions.FindAction(actionReference.action.id); // ? Ensure it's fetched from PlayerInput
                if (action == null)
                {
                    Debug.LogError($"Input action '{actionReference.action.name}' not found in PlayerInput.");
                }
                return action;
            }

            // Allow commands to use an action's real name without requiring a
            // redundant alias entry in every InputConfig asset.
            var actionByName = _playerInput.actions.FindAction(key, false);
            if (actionByName != null)
                return actionByName;

            Debug.LogError($"Key '{key}' not found in InputConfig.");
            return null;
        }
    }
}
