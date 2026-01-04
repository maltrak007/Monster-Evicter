using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Common.Manager.SaveSystem;
using ProyectoFinalFolder.Player.Scripts.PlayerInventory;
using UnityEngine;

namespace ProyectoFinalFolder.Components.Collectible.KeyItem
{
    public class KeyItemScript : CollectibleComponent
    {
        [SerializeField] private string keyItemName;
        private PlayerInventoryScript playerInventoryScript;
        
        private void Awake()
        {
            pickUpCollider = GetComponent<Collider>();
            if (pickUpCollider == null)
                Debug.LogError("No collider found on the collectible object.");
        }
        
        protected override void CollectInteraction()
        {
            if (playerInventoryScript.MatchKeyItem(keyItemName))
            {
                Debug.Log($"Key item '{keyItemName}' already collected.");
                return;
            }
            playerInventoryScript.AddKeyItem(keyItemName);
            SaveSystem.Save();
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out playerInventoryScript))
                {
                    pickUpCollider.enabled = false;
                    CollectInteraction();
                }
            }
        }
    }
}