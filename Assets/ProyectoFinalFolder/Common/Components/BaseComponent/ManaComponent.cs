using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//I WAS PLANNING TO IMPLEMENT IT TO THE ENEMY BUT IT ONLY WILL BE USED FOR THE PLAYER
namespace ProyectoFinalFolder.Common.Components.BaseComponent
{
    public class ManaComponent : MonoBehaviour
    {
        [Header("Mana Settings")] 
        public float maxMana { get; set; } = 100f;
        public float currentMana { get; set; }
    
        [SerializeField] protected Slider manaBarSlider;
    
        protected Coroutine regenCoroutine;

        [Header("Mana Events")] [SerializeField]
        protected UnityEvent onManaChanged;
    
        protected virtual void Start()
        {
            currentMana = maxMana;
            InitManaSliderValues();
        }

        protected virtual void Update()
        {
            if (currentMana < maxMana && regenCoroutine == null)
            {
                regenCoroutine = StartCoroutine(ManaRegeneration());
            }
        }

        public virtual IEnumerator ManaRegeneration()
        {
            while (currentMana < maxMana)
            {
                currentMana += Time.deltaTime * 2;
                currentMana = Mathf.Min(currentMana, maxMana);
                onManaChanged.Invoke();
            
                yield return null;
            }

            regenCoroutine = null;
        }

        protected void InitManaSliderValues()
        {
            if (manaBarSlider == null) return;
            manaBarSlider.maxValue = maxMana;
            manaBarSlider.value = maxMana;
        }

        public void UpdateManaSliderCurrentValue()
        {
            if (manaBarSlider != null)
            {
                manaBarSlider.value = currentMana;
            }
        }
    
        public void UseMana(float amount)
        {
            currentMana -= amount;
            currentMana = Mathf.Max(currentMana, 0);
            onManaChanged.Invoke();
        }

        public void RestoreMana(float amount)
        {
            currentMana += amount;
            currentMana = Mathf.Min(currentMana, maxMana);
            onManaChanged.Invoke();
        }
    }
}