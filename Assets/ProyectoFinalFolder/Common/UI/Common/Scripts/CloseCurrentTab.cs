using System;
using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Player.Scripts.PlayerController;
using UnityEngine;

namespace ProyectoFinalFolder.Common.UI.Common.Scripts
{
    public class CloseCurrentTab : MonoBehaviour
    {
        [SerializeField] private GameObject tabToClose;
        
        [SerializeField] private PlayerLocomotionInput playerLocomotionInput;

        [SerializeField] private bool ReturnToGame;
        
        [SerializeField] private bool ReturnToTab;
        [SerializeField] private GameObject tabToOpen;
        private void OnEnable()
        {
            playerLocomotionInput.onMenuClosePressedEvent.AddListener(CloseTab);
        }

        private void OnDisable()
        {
            playerLocomotionInput.onMenuClosePressedEvent.RemoveListener(CloseTab);
        }
        
        public void CloseTab()
        {
            if (tabToClose == null) return;
            if (ReturnToGame && ReturnToTab == false)
            {
                GameManagerScript.Instance.SetGameState(GameState.InGame);
                tabToClose.SetActive(false);
            }
            else if (ReturnToTab && ReturnToGame == false)
            {
                if (tabToOpen != null)
                {
                    tabToClose.SetActive(false);
                    tabToOpen.SetActive(true);
                }
            }
        }
    }
}