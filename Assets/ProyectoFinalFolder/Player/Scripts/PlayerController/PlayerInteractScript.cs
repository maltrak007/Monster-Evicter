using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Components;
using ProyectoFinalFolder.Components.Interfaces;
using ProyectoFinalFolder.Enemies;
using ProyectoFinalFolder.Enemies.CommonEnemy;
using ProyectoFinalFolder.Enemies.EnemyComponents;
using ProyectoFinalFolder.Player.Scripts.PlayerController;
using UnityEngine;
using UnityEngine.Events;

namespace ProyectoFinalFolder.Player.Scripts
{
    public class PlayerInteractScript : MonoBehaviour
    {
        public bool isInTrigger;
        public GameObject interactableGameObject;
        public float detectionRadius = 2f;
        private PlayerLocomotionInput playerLocomotionInput;
        private void Awake()
        {
            playerLocomotionInput = GetComponentInParent<PlayerLocomotionInput>();
        }
        void Update()
        {
            CheckInteractable();
            if (isInTrigger && playerLocomotionInput.InteractPressed && IsLookingAtInteractable())
            {
                HandleInteraction();
            }
        }
        
        void CheckInteractable()
        {
            LayerMask interactableLayerMask = LayerMask.GetMask("DetectionCollider");
            Vector3 checkOrigin = transform.position + transform.forward * (detectionRadius / 2f);
            Collider[] hits = Physics.OverlapSphere(checkOrigin, detectionRadius, interactableLayerMask);

            interactableGameObject = null;

            foreach (Collider hit in hits)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactableGameObject = hit.gameObject;
                    break;
                }
            }
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Vector3 checkOrigin = transform.position + transform.forward * (detectionRadius / 2f);
            Gizmos.DrawWireSphere(checkOrigin, detectionRadius);
        }

        void HandleInteraction()
        {
            IInteractable interactable = interactableGameObject?.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
                
                if (interactableGameObject.TryGetComponent(out EnemyInteractableComponent enemyInteractable))
                {
                    if (enemyInteractable.GetComponentInParent<Monster>().isStunned)
                    {
                        //PLAY THE MINIGAME
                        //RETURNS TRUE OR FALSE IF IT WAS COMPLETED
                        //DO NOTHING OR SET EVICTED TO TRUE
                        enemyInteractable.SetPlayerEvicted(true);
                    }
                }
            }
        }

        bool IsLookingAtInteractable()
        {
            return interactableGameObject != null;
        }

        public void OnTriggerEnterProxy(Collider other)
        {
            if (other.GetComponent<InteractableComponent>() != null)
            {
                isInTrigger = true;
                if (other.GetComponent<InteractableComponent>().HasCanvas())
                {
                    other.GetComponent<InteractableComponent>().ActivateCanvas();
                }
            }
        }

        public void OnTriggerStayProxy(Collider other)
        {
            //I need to make dissapear the canvas if the player is not looking at the interactable or if it doesnt meet the required conditions to interact
            if (other.GetComponent<InteractableComponent>() != null && other.GetComponent<InteractableComponent>().needInteractTriggerStay)
            {
                isInTrigger = true;
                if (other.GetComponent<InteractableComponent>().CheckConditionToActivateCanvas())
                {
                    other.GetComponent<InteractableComponent>().ActivateCanvas();
                }
                else
                {
                    other.GetComponent<InteractableComponent>().DeactivateCanvas();
                }
            }
        }
        
        public void OnTriggerExitProxy(Collider other)
        {
            if (other.GetComponent<InteractableComponent>() != null)
            {
                isInTrigger = false;
                if (other.GetComponent<InteractableComponent>().HasCanvas())
                {
                    other.GetComponent<InteractableComponent>().DeactivateCanvas();
                }
            }
        }
    }
}