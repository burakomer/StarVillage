// GENERATED AUTOMATICALLY FROM 'Assets/DwarfEngine/Actions/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""f4d37d02-4396-4813-92b5-6b63839b79dd"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""00885a3a-27a9-44eb-bda5-8cc781e39201"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7e7c2660-12e5-4337-bcbe-7765bfbcca03"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""1efd6504-eedd-448e-8cf3-229b8d61a78b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c70a0d1b-2b05-4775-ba6c-ea0c8b5430d1"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PadStyle1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""9edcf031-4900-444c-9114-13800c8cb38c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4fef948a-34f0-4571-8dea-1118701355da"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PadStyle1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""101804d3-e51c-4488-949a-51a46b178f7e"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PadStyle1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""dd3c83a9-7b91-4af7-b8ed-87bd77298784"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PadStyle1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0089f1f8-23c9-477d-9f70-cce5004bd762"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PadStyle1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3905cfa5-c03a-4d14-a7f7-a94b8f791342"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PadStyle1"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e82e0b9-2238-4136-9fe1-22bd555cc4fd"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PadStyle1"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PadStyle1"",
            ""bindingGroup"": ""PadStyle1"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_Move = m_Controls.FindAction("Move", throwIfNotFound: true);
        m_Controls_Rotate = m_Controls.FindAction("Rotate", throwIfNotFound: true);
        m_Controls_Attack = m_Controls.FindAction("Attack", throwIfNotFound: true);
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

    // Controls
    private readonly InputActionMap m_Controls;
    private IControlsActions m_ControlsActionsCallbackInterface;
    private readonly InputAction m_Controls_Move;
    private readonly InputAction m_Controls_Rotate;
    private readonly InputAction m_Controls_Attack;
    public struct ControlsActions
    {
        private @PlayerControls m_Wrapper;
        public ControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Controls_Move;
        public InputAction @Rotate => m_Wrapper.m_Controls_Rotate;
        public InputAction @Attack => m_Wrapper.m_Controls_Attack;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void SetCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMove;
                @Rotate.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRotate;
                @Attack.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAttack;
            }
            m_Wrapper.m_ControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
            }
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);
    private int m_PadStyle1SchemeIndex = -1;
    public InputControlScheme PadStyle1Scheme
    {
        get
        {
            if (m_PadStyle1SchemeIndex == -1) m_PadStyle1SchemeIndex = asset.FindControlSchemeIndex("PadStyle1");
            return asset.controlSchemes[m_PadStyle1SchemeIndex];
        }
    }
    public interface IControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
    }
}
