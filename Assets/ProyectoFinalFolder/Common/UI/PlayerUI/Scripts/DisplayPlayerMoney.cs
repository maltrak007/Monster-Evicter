using System;
using ProyectoFinalFolder.Player.Scripts.PlayerInventory;
using TMPro;
using UnityEngine;

namespace ProyectoFinalFolder.Common.UI.PlayerUI
{
    public class DisplayPlayerMoney : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;
        public PlayerInventoryScript playerInventory;


        private void Start()
        {
            coinText = GetComponent<TextMeshProUGUI>(); 
        }

        private void OnEnable()
        {
            UpdateCoinText();
        }
        
        public void UpdateCoinText()
        {
            coinText.text = playerInventory.coinAmount.ToString();
        }
    }
}