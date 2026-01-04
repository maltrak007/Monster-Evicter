using System;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using UnityEngine;

namespace ProyectoFinalFolder.Player.Scripts.PlayerController
{
    public class PlayerSoundRelay : MonoBehaviour
    {
        PlayerSpells playerSpells;
        
        string[] stepSounds = new string[]
        {
            "PlayerStep1",
            "PlayerStep2",
            "PlayerStep3",
            "PlayerStep4"
        };
        
        private void Start()
        {
            playerSpells = GetComponent<PlayerSpells>();
        }

        public void PlaySFXFromAnimation(string soundName)
        {
            if (SoundManagerScript.Instance != null)
            {
                SoundManagerScript.Instance.PlaySound(soundName);
            }
        }

        public void PlaySFXElementalBall()
        {
            switch (playerSpells.currentAffinity)
            {
                case Affinities.Fire:
                    PlaySFXFromAnimation("PlayerFireBall");
                    break;

                case Affinities.Water:
                    PlaySFXFromAnimation("PlayerWaterBall");
                    break;

                case Affinities.Electric:
                    PlaySFXFromAnimation("PlayerElectricBall");
                    break;
            }
        }

        public void PlaySFXSteps()
        {
            if (SoundManagerScript.Instance != null && stepSounds.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, stepSounds.Length);
                SoundManagerScript.Instance.PlaySound(stepSounds[randomIndex]);
            }
        }
    }
}