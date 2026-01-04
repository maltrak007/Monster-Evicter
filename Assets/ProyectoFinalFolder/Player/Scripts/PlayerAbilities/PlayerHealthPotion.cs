using System;
using ProyectoFinalFolder.Player.Scripts.PlayerResources;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ProyectoFinalFolder.Player.Scripts.MiscScripts
{
    public class PlayerHealthPotion : MonoBehaviour
    {
        public float restoreAmount { get; set; } = 30f;
        
        [Header("Potion Settings")]
        private int maxPotionUses = 3;
        
        public UnityEvent onPotionChanged;
        public int currentPotionUses { get; private set; }

        private Animator playerAnimator;
        
        private PlayerUpgrades _playerUpgrades;
        
        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();
            _playerUpgrades = GetComponent<PlayerUpgrades>();
        }

        private void Start()
        {
            UpdateHealthPotionParameters();
            SetCurrentPotionUses(maxPotionUses);
        }

        public void RestoreHealth(PlayerHealthComponent healthComponent)
        {
            if (!Mathf.Approximately(healthComponent.GetCurrentHealth(), healthComponent.GetMaxHealth()) && currentPotionUses > 0)
            {
                playerAnimator.SetBool("canHeal", true);
                healthComponent.Heal(restoreAmount);
                DecreasePotionUses();
                return;
            }
            //TODO: FEEDBACK TO INDICATE THAT HE CANNOT HEAL WHEN IT'S FULL OR DOESNT HAVE POTIONS
            Debug.Log("No more potions available");
        }

        private void DecreasePotionUses()
        {
            if (currentPotionUses > 0)
            {
                currentPotionUses--;
                onPotionChanged?.Invoke();
            }
        }
        
        public void ResetCurrentPotionUses()
        {
            currentPotionUses = maxPotionUses;
            onPotionChanged?.Invoke();
        }

        private void SetCurrentPotionUses(int _uses)
        {
            currentPotionUses = _uses;
            onPotionChanged?.Invoke();
        }

        public void UpdateHealthPotionParameters()
        {
            maxPotionUses = _playerUpgrades.TryGetBestIntUpgrade("healthPotionsUses", out int _maxPotionUses) ? _maxPotionUses : 3;
            restoreAmount = _playerUpgrades.TryGetBestFloatUpgrade("healthPotionRestore", out float _restoreAmount) ? restoreAmount : 30f;
        }
    }
}