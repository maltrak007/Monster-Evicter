using ProyectoFinalFolder.Components.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace ProyectoFinalFolder.Common.Components.BaseComponent
{
    public abstract class InteractableComponent : MonoBehaviour , IInteractable
    {
        public UnityEvent onInteract;
        
        protected bool canInteract { get; private set; }
        
        [SerializeField] protected Canvas interactionCanvas;

        public bool needInteractTriggerStay;
        
        private void OnEnable()
        {
            interactionCanvas.enabled = false;
        }

        public virtual void Interact()
        {
            onInteract?.Invoke();
        }

        public void SetCanInteract(bool interactBoolean)
        {
            canInteract = interactBoolean;
        }
        
        public void ActivateCanvas()
        {
            if (interactionCanvas != null)
            {
                interactionCanvas.enabled = true;
            }
        }
        
        public void DeactivateCanvas()
        {
            if (interactionCanvas != null)
            {
                interactionCanvas.enabled = false;
            }
        }
        
        public bool HasCanvas()
        {
            return interactionCanvas != null;
        }

        public virtual bool CheckConditionToActivateCanvas()
        {
            return false;
        }
    }
}