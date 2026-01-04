using ProyectoFinalFolder.Common.Components.BaseComponent;
using UnityEngine;

namespace ProyectoFinalFolder.Enemies.CommonEnemy
{
    public class MonsterColliderScript : MonoBehaviour
    {
        public float attackDamage = 10f;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HealthComponent playerHealthComponent = other.GetComponent<HealthComponent>();
                if (playerHealthComponent != null)
                {
                    playerHealthComponent.TakeDamage(attackDamage);
                }
            }
        }
    }
}