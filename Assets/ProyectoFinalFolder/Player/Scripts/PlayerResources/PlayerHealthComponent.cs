using ProyectoFinalFolder.Common.Components.BaseComponent;
using UnityEngine;

namespace ProyectoFinalFolder.Player.Scripts.PlayerResources
{
    public class PlayerHealthComponent : HealthComponent
    {
        private PlayerUpgrades _playerUpgrades;
        protected override void Start()
        {
            _playerUpgrades = GetComponent<PlayerUpgrades>();
            this.maxHealth = _playerUpgrades.TryGetBestFloatUpgrade("healthUpgrade", out float maxHealth) ? maxHealth : 100f;
            currentHealth = this.maxHealth;
            InitHealthSliderValues();
        }

        public void UpdateHealthValues()
        {
            this.maxHealth = _playerUpgrades.TryGetBestFloatUpgrade("healthUpgrade", out float maxHealth) ? maxHealth : 100f;
            currentHealth = this.maxHealth;
        }
    }
}