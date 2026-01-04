using System;
using System.Collections.Generic;
using ProyectoFinalFolder.Common.Manager;
using UnityEngine;

namespace ProyectoFinalFolder.Common.UI.Common.Scripts
{
    public class PauseMenuScript : MonoBehaviour
    {
        public Canvas canvaToClose;

        public Canvas optionCanvas;
        
        private void OnEnable()
        {
            canvaToClose.gameObject.SetActive(false);
        }
        
        public void ResumeButtonClick()
        {
            GameManagerScript.Instance.SetGameState(GameState.InGame);
            canvaToClose.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OptionsButtonClick()
        {
            optionCanvas.gameObject.SetActive(true);
        }

        public void ExitButtonClick()
        {
            
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ReturnToMainTitle()
        {
            
        }
    }
}