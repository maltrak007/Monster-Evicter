using System;
using System.Collections;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using UnityEngine;
using UnityEngine.UI;

namespace ProyectoFinalFolder.Player.Scripts.PlayerResources
{
    public class PlayerManaComponent : ManaComponent
    {
        private PlayerUpgrades _playerUpgrades;
        
        [SerializeField] private ParticleSystem spellManaParticleSystem;
        
        private PlayerSpells spells;
        
        protected override void Start()
        {
            _playerUpgrades = GetComponent<PlayerUpgrades>();
            this.maxMana = _playerUpgrades.TryGetBestFloatUpgrade("manaUpgrade", out float maxMana) ? maxMana : 100f;
            currentMana = this.maxMana;
            InitManaSliderValues();
            
            spells = GetComponent<PlayerSpells>();
            if (spells != null)
            {
                spells.onAffinityChanged.AddListener(ChangeColourByAffinity);
            }
        }
        
        public override IEnumerator ManaRegeneration()
        {
            while (currentMana < maxMana)
            {
                currentMana += Time.deltaTime * 2;
                currentMana = Mathf.Min(currentMana, maxMana);
                onManaChanged.Invoke();
                
                if (spellManaParticleSystem != null)
                {
                    float manaRatio = currentMana / maxMana;
                    float emissionRate = manaRatio * 20f;

                    var emission = spellManaParticleSystem.emission;
                    emission.rateOverTime = emissionRate;
                }
                yield return null;
            }

            regenCoroutine = null;
        }

        
        [Obsolete("Obsolete")]
        public void ChangeColourByAffinity(Affinities affinity)
        {
            switch (affinity)
            {
                case Affinities.Fire:
                    manaBarSlider.fillRect.GetComponent<Image>().color = Color.red;
                    spellManaParticleSystem.startColor = Color.red;
                    break;
                case Affinities.Water:
                    manaBarSlider.fillRect.GetComponent<Image>().color = Color.blue;
                    spellManaParticleSystem.startColor = Color.blue;
                    break;
                case Affinities.Electric:
                    manaBarSlider.fillRect.GetComponent<Image>().color = Color.magenta;
                    spellManaParticleSystem.startColor = Color.magenta;
                    break;
                default:
                    manaBarSlider.fillRect.GetComponent<Image>().color = Color.white;
                    break;
            }
        }

        public void UpdateManaValues()
        {
            this.maxMana = _playerUpgrades.TryGetBestFloatUpgrade("manaUpgrade", out float maxMana) ? maxMana : 100f;
            currentMana = this.maxMana;
        }
    }
}