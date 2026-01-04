using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Common.Manager.SaveSystem;
using UnityEngine;

namespace ProyectoFinalFolder.Enviroment.Scripts.Obstacle
{
    public class ObstacleScript : InteractableComponent
    {
        [Header("Detection Settings")] public float detectionRadius = 6f;
        public LayerMask interactableLayerMask;

        public string keyItemToMatch = "KeyItem";
        public bool unlockedObstacle;

        private Animator obstacleAnimator;

        public void OnEnable()
        {
            interactionCanvas.enabled = false;
            gameObject.SetActive(!unlockedObstacle);
        }

        private void Start()
        {
            obstacleAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            DetectNearbyEnemies();
        }

        public override void Interact()
        {
            if (!canInteract) return;
            HandleObstacleInteraction();
            base.Interact();
        }

        private void HandleObstacleInteraction()
        {
            if (!GameManagerScript.Instance.playerInventory.MatchKeyItem(keyItemToMatch)) return;
            GameManagerScript.Instance.playerInventory.RemoveKeyItem(keyItemToMatch);
            unlockedObstacle = true;
            SaveSystem.Save();
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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Vector3 checkOrigin = transform.position;
            Gizmos.DrawWireSphere(checkOrigin, detectionRadius);
        }
    }
}