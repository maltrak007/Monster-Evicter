using System;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Common.Manager.SaveSystem;
using ProyectoFinalFolder.Components;
using UnityEngine;

namespace ProyectoFinalFolder.Enviroment.Scripts.Bonfire
{
    public class BonfireScript : InteractableComponent
    {
        public Transform  playerRespawnPoint;
        public bool wasActivated;
        public Light bonfireLight;
        public ParticleSystem bonfireParticles;

        [Header("Detection Settings")] 
        public float detectionRadius = 6f;
        public LayerMask interactableLayerMask;

        public void OnEnable()
        {
            interactionCanvas.enabled = false;
            if (wasActivated)
            {
                bonfireLight.enabled = true;
                bonfireParticles.Play();
            }
            else
            {
                bonfireLight.enabled = false;
                bonfireParticles.Stop();
            }
        }
        
        private void Update()
        {
            DetectNearbyEnemies();
        }

        private static void SaveGameInBonfire()
        {
            SaveSystem.Save();
        }
        
        private void ActivateBonfire()
        {
            if (wasActivated) return;
            interactionCanvas.enabled = false;
            wasActivated = true;
            bonfireLight.enabled = true;
            bonfireParticles.Play();
        }

        private void BonfireUtilities()
        {
            GameManagerScript.Instance.SetGameState(GameState.Menu);
            SetPlayerNewRespawnPoint();
            SaveGameInBonfire();
        }
        
        public override void Interact()
        {
            if (!canInteract) return;
            ActivateBonfire();
            BonfireUtilities();
            base.Interact();
        }

        public void DetectNearbyEnemies()
        {
            Vector3 checkOrigin = transform.position;
            Collider[] hits = Physics.OverlapSphere(checkOrigin, detectionRadius, interactableLayerMask);
            bool enemyNearby = false;

            foreach (Collider hit in hits)
            {
                if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    enemyNearby = true;
                    break;
                }
            }

            SetCanInteract(!enemyNearby);
        }

        public override bool CheckConditionToActivateCanvas()
        {
            return HasCanvas() && canInteract;
        }

        public void SetPlayerNewRespawnPoint()
        {
            GameManagerScript.Instance.currentPlayerRespawnPoint = playerRespawnPoint.position;
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Vector3 checkOrigin = transform.position;
            Gizmos.DrawWireSphere(checkOrigin, detectionRadius);
        }
    }
}