using System;
using ProyectoFinalFolder.Enemies;
using ProyectoFinalFolder.Player.Scripts.PlayerResources;
using UnityEngine;

namespace ProyectoFinalFolder.Player.Scripts.MiscScripts
{
    public class ShieldScript : MonoBehaviour
    {
        public PlayerManaComponent playerManaComponent;
        public PlayerHealthComponent playerHealthComponent;
        private Animator playerAnimator;
        private void Start()
        {
            playerAnimator = GetComponentInParent<Animator>();
        }

        private void OnDisable()
        {
            playerAnimator.SetBool("canEShield", false);
            playerHealthComponent.isInmune = false;
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                playerAnimator.SetBool("canEShield", true);
                playerHealthComponent.isInmune = true;
                playerManaComponent.UseMana(8f * Time.deltaTime);
                if (playerManaComponent.currentMana <= 0)
                {
                    gameObject.SetActive(false);
                    playerAnimator.SetBool("canEShield", false);
                }
            }
        }
    }
}