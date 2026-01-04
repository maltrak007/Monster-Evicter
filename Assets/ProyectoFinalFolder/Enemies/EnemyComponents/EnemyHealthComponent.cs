using System.Collections;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using UnityEngine;
using UnityEngine.Events;



namespace ProyectoFinalFolder.Enemies.EnemyComponents
{
    public class EnemyHealthComponent : HealthComponent
    {
        public UnityEvent onEnemyRevive;
        
        public bool isEvicted { get; private set; }
        
        public void EnemyRevive()
        {
            isDead = false;
            onEnemyRevive.Invoke();
        }

        protected override void Die()
        {
            isDead = true;
            onDeath.Invoke();
        }
        
        public override void Heal(float amount)
        {
            currentHealth = 0;

            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            onHealthChanged.Invoke();
        }
    }
}