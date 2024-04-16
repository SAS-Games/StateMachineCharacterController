//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Packages/com.sas.fsmcharactercontroller/Runtime/Configs/Input/CameraLookControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @CameraLookControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraLookControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraLookControls"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""15350843-dc06-440b-9c0a-8763f14e0d6e"",
            ""actions"": [
                {
                    ""name"": ""RotateCamera"",
                    ""type"": ""Value"",
                    ""id"": ""ccfa3621-9b1b-4ddb-a507-720d9dc87b94"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseControlCamera"",
                    ""type"": ""Button"",
                    ""id"": ""e376f921-fb2d-4257-827f-1309ec9171ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7a3c3f2a-2617-4b0c-b6cb-c54058d6ae6e"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=2,y=2)"",
                    ""groups"": ""keyboard control scheme"",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8693aacf-2825-4c9c-a9d3-b2ecf6670489"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard control scheme"",
                    ""action"": ""MouseControlCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""keyboard control scheme"",
            ""bindingGroup"": ""keyboard control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_RotateCamera = m_Mouse.FindAction("RotateCamera", throwIfNotFound: true);
        m_Mouse_MouseControlCamera = m_Mouse.FindAction("MouseControlCamera", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Mouse
    private readonly InputActionMap m_Mouse;
    private List<IMouseActions> m_MouseActionsCallbackInterfaces = new List<IMouseActions>();
    private readonly InputAction m_Mouse_RotateCamera;
    private readonly InputAction m_Mouse_MouseControlCamera;
    public struct MouseActions
    {
        private @CameraLookControls m_Wrapper;
        public MouseActions(@CameraLookControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @RotateCamera => m_Wrapper.m_Mouse_RotateCamera;
        public InputAction @MouseControlCamera => m_Wrapper.m_Mouse_MouseControlCamera;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void AddCallbacks(IMouseActions instance)
        {
            if (instance == null || m_Wrapper.m_MouseActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MouseActionsCallbackInterfaces.Add(instance);
            @RotateCamera.started += instance.OnRotateCamera;
            @RotateCamera.performed += instance.OnRotateCamera;
            @RotateCamera.canceled += instance.OnRotateCamera;
            @MouseControlCamera.started += instance.OnMouseControlCamera;
            @MouseControlCamera.performed += instance.OnMouseControlCamera;
            @MouseControlCamera.canceled += instance.OnMouseControlCamera;
        }

        private void UnregisterCallbacks(IMouseActions instance)
        {
            @RotateCamera.started -= instance.OnRotateCamera;
            @RotateCamera.performed -= instance.OnRotateCamera;
            @RotateCamera.canceled -= instance.OnRotateCamera;
            @MouseControlCamera.started -= instance.OnMouseControlCamera;
            @MouseControlCamera.performed -= instance.OnMouseControlCamera;
            @MouseControlCamera.canceled -= instance.OnMouseControlCamera;
        }

        public void RemoveCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMouseActions instance)
        {
            foreach (var item in m_Wrapper.m_MouseActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MouseActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    private int m_keyboardcontrolschemeSchemeIndex = -1;
    public InputControlScheme keyboardcontrolschemeScheme
    {
        get
        {
            if (m_keyboardcontrolschemeSchemeIndex == -1) m_keyboardcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("keyboard control scheme");
            return asset.controlSchemes[m_keyboardcontrolschemeSchemeIndex];
        }
    }
    public interface IMouseActions
    {
        void OnRotateCamera(InputAction.CallbackContext context);
        void OnMouseControlCamera(InputAction.CallbackContext context);
    }
}
