using System.Collections.Generic;
using ProyectoFinalFolder.Player.Scripts.MiscScripts;
using UnityEngine;

namespace ProyectoFinalFolder.Player.Scripts.PlayerInventory
{
    public class PlayerInventoryScript : MonoBehaviour
    {
        public int coinAmount { get; private set;}
        
        private List<string> playerKeyItems = new(); 
        
        public void AddKeyItem(string keyItem)
        {
            if (playerKeyItems.Contains(keyItem))
            {
                return;
            }
            playerKeyItems.Add(keyItem);
        }
        
        public void RemoveKeyItem(string keyItem)
        {
            if (!playerKeyItems.Contains(keyItem))
            {
                return;
            }
            playerKeyItems.Remove(keyItem);
        }
        
        public bool MatchKeyItem(string keyItem)
        {
            if (playerKeyItems.Contains(keyItem))
            {
                return true;
            }
            //Add the HUD message to indicate that the player doesnt have the specific key item
            return false;
        }
        
        public void AddCoinAmount(int amount)
        {
            coinAmount += amount;
        }
        
        public void RemoveCoinAmount(int amount)
        {
            if (coinAmount < amount)
            {
                return;
            }
            coinAmount -= amount;
        }
        
        public void Save(ref PlayerInventorySaveData data)
        {
            data.coinSaveAmount = coinAmount;
            data.playerSaveKeyItems = playerKeyItems;
        }
        
        public void Load(PlayerInventorySaveData data)
        {
            coinAmount = data.coinSaveAmount;
            playerKeyItems = data.playerSaveKeyItems;
        }
    }
    
    [System.Serializable]
    public struct PlayerInventorySaveData
    {
        public int coinSaveAmount;
        public List<string> playerSaveKeyItems;
    }
}