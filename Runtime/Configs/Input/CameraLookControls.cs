// GENERATED AUTOMATICALLY FROM 'Assets/StateMachineCharacterController/Configs/Input/CameraLookControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraLookControls : IInputActionCollection, IDisposable
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
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseControlCamera"",
                    ""type"": ""Button"",
                    ""id"": ""e376f921-fb2d-4257-827f-1309ec9171ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
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

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
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
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @RotateCamera.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnRotateCamera;
                @MouseControlCamera.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouseControlCamera;
                @MouseControlCamera.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouseControlCamera;
                @MouseControlCamera.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouseControlCamera;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RotateCamera.started += instance.OnRotateCamera;
                @RotateCamera.performed += instance.OnRotateCamera;
                @RotateCamera.canceled += instance.OnRotateCamera;
                @MouseControlCamera.started += instance.OnMouseControlCamera;
                @MouseControlCamera.performed += instance.OnMouseControlCamera;
                @MouseControlCamera.canceled += instance.OnMouseControlCamera;
            }
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
