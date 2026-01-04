using ProyectoFinalFolder.Common.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ProyectoFinalFolder.Player.Scripts.PlayerController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerInput.IPlayerLocomotionActions,
        PlayerInput.IPlayerUIActions
    {
        public PlayerInput PlayerInput { get; private set; }

        public PlayerSpells PlayerSpells { get; private set; }

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        public bool ElementalBallAttackPressed { get; private set; }
        public UnityEvent onElementalBallAttackEvent;

        public bool ElementalShieldPressed { get; set; }
        public UnityEvent onElementalShieldEvent;
        public UnityEvent onElementalShieldReleaseEvent;

        public bool ElementalAreaAttackPressed { get; private set; }
        public UnityEvent onElementalAreaAttackEvent;

        public bool ElementalChangePressed { get; private set; }
        public UnityEvent onChangeElement;

        public bool HealthAction { get; private set; }
        public UnityEvent onHealthAction;

        public bool InteractPressed { get; private set; }
        public UnityEvent onInteractEvent;
        
        public UnityEvent onMenuClosePressedEvent;

        private void OnEnable()
        {
            PlayerSpells = GetComponent<PlayerSpells>();

            PlayerInput = new PlayerInput();
            PlayerInput.Enable();

            PlayerInput.PlayerLocomotion.Enable();
            PlayerInput.PlayerLocomotion.SetCallbacks(this);
            
            PlayerInput.PlayerUI.Enable();
            PlayerInput.PlayerUI.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerInput.PlayerLocomotion.Disable();
            PlayerInput.PlayerLocomotion.RemoveCallbacks(this);
            
            PlayerInput.PlayerUI.Disable();
            PlayerInput.PlayerUI.RemoveCallbacks(this);
        }

        private void LateUpdate()
        {
            ElementalBallAttackPressed = false;
            ElementalAreaAttackPressed = false;
            HealthAction = false;
            InteractPressed = false;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnElementalBallAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            ElementalBallAttackPressed = true;
            onElementalBallAttackEvent?.Invoke();
        }

        public void OnElementalShieldAction(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ElementalShieldPressed = true;
                onElementalShieldEvent?.Invoke();
            }
            else if (context.canceled)
            {
                ElementalShieldPressed = false;
                onElementalShieldReleaseEvent?.Invoke();
            }
        }

        public void OnElementalAreaAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            ElementalAreaAttackPressed = true;
            onElementalAreaAttackEvent?.Invoke();
        }

        public void OnChangeElement(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            ElementalChangePressed = true;
            onChangeElement?.Invoke();
        }

        public void OnHeal(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            HealthAction = true;
            onHealthAction?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            InteractPressed = true;
        }

        public void OnPauseGame(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            GameManagerScript.Instance.SetGameState(GameState.Pause);
        }

        public void OnMenuClose(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            onMenuClosePressedEvent?.Invoke();
        }
    }
}