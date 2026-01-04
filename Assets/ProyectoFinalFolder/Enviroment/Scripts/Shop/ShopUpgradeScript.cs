using System;
using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Common.Manager.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProyectoFinalFolder.Enviroment.Scripts.Shop
{
    public class ShopUpgradeScript : MonoBehaviour
    {
        [SerializeField] private string upgradeKeyName;
        [SerializeField] private int upgradeCost;
        [SerializeField] private Image upgradeIcon;
        
        [Header("ShopUpgradeEvents")] 
        public UnityEvent onPurchaseDenied;
        public UnityEvent onPurchaseUpgrade;

        public Color unlockedColor;

        private void OnEnable()
        {
            if (GameManagerScript.Instance.playerUpgrades.floatUpgrades.TryGetValue(upgradeKeyName, out var upgrade))
            {
                upgradeIcon.color = upgrade.unlocked ? unlockedColor : Color.white;
                Button button = GetComponent<Button>();
                if (upgrade.unlocked)
                {
                    button.interactable = false;
                }
            }
            else if (GameManagerScript.Instance.playerUpgrades.intUpgrades.TryGetValue(upgradeKeyName, out var intUpgrade))
            {
                upgradeIcon.color = intUpgrade.unlocked ? unlockedColor : Color.white;
                Button button = GetComponent<Button>();
                if (intUpgrade.unlocked)
                {
                    button.interactable = false;
                }
            }
        }

        public void PurchaseUpgrade()
        {
            if (GameManagerScript.Instance.playerUpgrades.floatUpgrades.TryGetValue(upgradeKeyName, out var upgrade))
            {
                if (!upgrade.unlocked && GameManagerScript.Instance.playerInventory.coinAmount >= upgradeCost)
                {
                    GameManagerScript.Instance.playerInventory.RemoveCoinAmount(upgradeCost);
                    upgrade.unlocked = true;
                    upgradeIcon.color = unlockedColor;
                    onPurchaseUpgrade?.Invoke();
                    SaveSystem.Save();
                }
                onPurchaseDenied?.Invoke();
            }
            else if(GameManagerScript.Instance.playerUpgrades.intUpgrades.TryGetValue(upgradeKeyName, out var intUpgrade))
            {
                if (!intUpgrade.unlocked && GameManagerScript.Instance.playerInventory.coinAmount >= upgradeCost)
                {
                    GameManagerScript.Instance.playerInventory.RemoveCoinAmount(upgradeCost);
                    intUpgrade.unlocked = true;
                    upgradeIcon.color = unlockedColor;
                    onPurchaseUpgrade?.Invoke();
                    SaveSystem.Save();
                }
                else
                {
                    onPurchaseDenied?.Invoke();
                }
            }
        }
    }
}