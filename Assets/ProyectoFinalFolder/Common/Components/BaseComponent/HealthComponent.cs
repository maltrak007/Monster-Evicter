using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProyectoFinalFolder.Common.Components.BaseComponent
{
    public class HealthComponent : MonoBehaviour
    {
        [Header("Health Settings")]
        protected float maxHealth { get; set; } = 100f;
        protected float currentHealth { get; set; }
    
        [Header("Health States")]
        [SerializeField] protected bool isDead = false;
        public bool isInmune {get; set;} = false;
    
        public Coroutine healthRegenerationCoroutine;
    
        [Header("Health Events")]
        [SerializeField] public UnityEvent onHealthChanged;
        [SerializeField] public UnityEvent onTakeDamage;
        [SerializeField] public UnityEvent onDeath;
        [SerializeField] protected Slider healthBarSlider;
    
        protected virtual void Start()
        {
            currentHealth = maxHealth;
            InitHealthSliderValues();
        }
    
        public void TakeDamage(float damage)
        {
            if (isDead || isInmune) return;

            currentHealth -= damage;
        
            onHealthChanged.Invoke();
        
            onTakeDamage.Invoke();
        
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    
        public virtual void Heal(float amount)
        {
            if (isDead) return;

            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        
            onHealthChanged.Invoke();
        }
    
        protected virtual void Die()
        {
            isDead = true;
        
            onDeath.Invoke();
        
            gameObject.SetActive(false);
        }
    
        protected void InitHealthSliderValues()
        {
            if (healthBarSlider != null)
            {
                healthBarSlider.maxValue = maxHealth;
                healthBarSlider.value = maxHealth;
            }
        }

        public void UpdateHealthSliderCurrentValue()
        {
            if (healthBarSlider != null)
            {
                healthBarSlider.value = currentHealth;
            }
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }    
    
        public float GetCurrentHealth()
        {
            return currentHealth;
        }
    
        public IEnumerator HealthRegeneration()
        {
            while (currentHealth < maxHealth)
            {
                currentHealth += Time.deltaTime * 2;
                currentHealth = Mathf.Min(currentHealth, maxHealth);
                onHealthChanged.Invoke();
            
                yield return null;
            }
            healthRegenerationCoroutine = null;
        }
    }
}
