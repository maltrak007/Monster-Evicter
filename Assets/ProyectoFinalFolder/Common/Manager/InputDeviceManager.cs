using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public enum InputDeviceType { KeyboardMouse, Gamepad }

namespace ProyectoFinalFolder.Common.Manager
{
    public class InputDeviceManager : MonoBehaviour
    {
        public static InputDeviceManager Instance { get; private set; }

        public InputDeviceType CurrentDeviceType { get; private set; } = InputDeviceType.KeyboardMouse;

        public event Action<InputDeviceType> OnInputDeviceChanged;
        
        private Action<InputControl> inputCallback;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            
            inputCallback = OnAnyInput;
        }

        private void OnEnable()
        {
            InputSystem.onAnyButtonPress.Call(OnAnyInput);
        }
        
        private void OnAnyInput(InputControl control)
        {
            InputDeviceType newDevice;

            if (control.device is Gamepad)
                newDevice = InputDeviceType.Gamepad;
            else if (control.device is Keyboard || control.device is Mouse)
                newDevice = InputDeviceType.KeyboardMouse;
            else
                return;

            if (newDevice != CurrentDeviceType)
            {
                CurrentDeviceType = newDevice;
                OnInputDeviceChanged?.Invoke(CurrentDeviceType);
            }
        }
    }
}