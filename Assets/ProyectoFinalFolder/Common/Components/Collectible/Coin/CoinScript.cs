using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Player.Scripts.PlayerInventory;
using UnityEngine;

namespace ProyectoFinalFolder.Components.Collectible.Coin
{
    public class CoinScript : CollectibleComponent
    {
        [SerializeField] private int coinValue = 1;
        private PlayerInventoryScript playerInventoryScript;
        
        private void Awake()
        {
            pickUpCollider = GetComponent<Collider>();
            if (pickUpCollider == null)
                Debug.LogError("No collider found on the collectible object.");
        }
        
        protected override void CollectInteraction()
        {
            playerInventoryScript.AddCoinAmount(coinValue);
            //TODO:Insert sound and effect here
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